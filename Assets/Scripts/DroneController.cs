using System.Collections;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public enum Faction { Red, Blue }
    public Faction faction;
    public float speed = 5f;
    public Transform homeSlot;
    public Resource targetResource;

    private bool carryingResource = false;
    private DroneState state = DroneState.Idle;

    private enum DroneState { Idle, MovingToResource, Gathering, Returning }

    void Update()
    {
        UpdatePathTarget();

        switch (state)
        {
            case DroneState.Idle:
                targetResource = ResourceManager.Instance.GetNearestAvailable(transform.position);
                if (targetResource != null)
                {
                    targetResource.Claim();
                    state = DroneState.MovingToResource;
                }
                break;

            case DroneState.MovingToResource:
                MoveTo(targetResource.transform.position);
                if (Vector3.Distance(transform.position, targetResource.transform.position) < 0.5f)
                {
                    StartCoroutine(GatherResource());
                    state = DroneState.Gathering;
                }
                break;

            case DroneState.Returning:
                MoveTo(homeSlot.position);
                if (Vector3.Distance(transform.position, homeSlot.position) < 0.1f)
                {
                    carryingResource = false;
                    ResourceManager.Instance.AddResource(faction);
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
        Vector3 dir = (destination - transform.position).normalized;
        transform.up = dir;
        transform.position += dir * speed * Time.deltaTime;
    }

    private IEnumerator GatherResource()
    {
        yield return new WaitForSeconds(2f);
        if (targetResource != null)
        {
            Destroy(targetResource.gameObject);
            carryingResource = true;
            state = DroneState.Returning;
        }
    }
}