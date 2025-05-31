using System.Collections;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public enum Faction { Red, Blue }
    public Faction faction;
    public float speed = 5f;
    public Transform homeSlot;
    public Resource targetResource;

    private DroneState state = DroneState.Idle;

    private enum DroneState { Idle, MovingToResource, Gathering, Returning }

    void Update()
    {
        UpdatePathTarget();

        switch (state)
        {
            case DroneState.Idle:
                targetResource = ResourceManager.Instance.GetNearestAvailable(transform.position);
                Debug.Log($"[Drone] Searching... found: {targetResource}");
                if (targetResource != null)
                {
                    targetResource.Claim();
                    state = DroneState.MovingToResource;
                }
                break;

            case DroneState.MovingToResource:
                if (targetResource == null)
                {
                    Debug.LogWarning("[Drone] Target resource lost! Returning to Idle.");
                    state = DroneState.Idle;
                    break;
                }

                float distToResource = Vector3.Distance(transform.position, targetResource.transform.position);
                Debug.Log($"[Drone] Moving. Distance to resource: {distToResource}");

                MoveTo(targetResource.transform.position);

                if (distToResource < 1f && state != DroneState.Gathering)
                {
                    Debug.Log("[Drone] Start gathering resource");
                    state = DroneState.Gathering;
                    StartCoroutine(GatherResource());
                }
                break;

            case DroneState.Gathering:
                // Корутин выполняет задержку, здесь ничего делать не нужно
                break;

            case DroneState.Returning:
                float distToHome = Vector3.Distance(transform.position, homeSlot.position);
                Debug.Log($"[Drone] Returning. Distance to home: {distToHome}");

                MoveTo(homeSlot.position);

                if (distToHome < 0.6f)
                {
                    ResourceManager.Instance.AddResource(faction);
                    Debug.Log("[Drone] Resource delivered. Returning to Idle.");
                    state = DroneState.Idle;
                }
                break;
        }
    }

    private void UpdatePathTarget()
    {
        var pathRenderer = GetComponent<PathRenderer>();
        if (pathRenderer != null)
        {
            if (state == DroneState.MovingToResource && targetResource != null)
                pathRenderer.target = targetResource.transform;
            else if (state == DroneState.Returning)
                pathRenderer.target = homeSlot;
            else
                pathRenderer.target = null;
        }
    }

    private void MoveTo(Vector3 destination)
    {
        Vector3 dir = (destination - transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        Vector3 oldPos = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        Debug.Log($"[Drone] Moving from {oldPos} to {transform.position} towards {destination}");
    }

    private IEnumerator GatherResource()
    {
        Debug.Log("[Drone] Gathering resource started...");
        yield return new WaitForSeconds(2f);
        if (targetResource != null)
        {
            Debug.Log("[Drone] Resource gathered, destroying resource object.");
            Destroy(targetResource.gameObject);
            targetResource = null;
            state = DroneState.Returning;
        }
        else
        {
            Debug.LogWarning("[Drone] Resource disappeared during gathering.");
            state = DroneState.Idle;
        }
    }
}