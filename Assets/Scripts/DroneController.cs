using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DroneController : MonoBehaviour
{
    public enum Faction { Red, Blue }
    public Faction faction;
    public float speed = 5f;
    public Transform homeSlot;
    public Resource targetResource;

    private DroneState state = DroneState.Idle;
    private NavMeshAgent agent;

    private enum DroneState { Idle, MovingToResource, Gathering, Returning }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
    }

    void Update()
    {
        UpdatePathTarget();
        Debug.Log($"Drone {faction} State: {state}");

        switch (state)
        {
            case DroneState.Idle:
                TryFindResource();
                break;

            case DroneState.MovingToResource:
                if (targetResource == null)
                {
                    state = DroneState.Idle;
                    break;
                }

                float distToResource = Vector3.Distance(transform.position, targetResource.transform.position);
                MoveTo(targetResource.transform.position);

                if (distToResource < 1f && state != DroneState.Gathering)
                {
                    state = DroneState.Gathering;
                    StartCoroutine(GatherResource());
                }
                break;

            case DroneState.Gathering:
                break;

            case DroneState.Returning:
                float distToHome = Vector3.Distance(transform.position, homeSlot.position);

                MoveTo(homeSlot.position);

                if (distToHome < 0.8f)
                {
                    ResourceManager.Instance.AddResource(faction);
                    state = DroneState.Idle;
                    TryFindResource();
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
        agent.SetDestination(destination);

        Vector3 direction = destination - transform.position;
        if (direction.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
    private void TryFindResource()
    {
        if (targetResource == null)
        {
            targetResource = ResourceManager.Instance.GetNearestAvailable(transform.position);
            if (targetResource != null)
            {
                targetResource.Claim();
                state = DroneState.MovingToResource;
            }
        }
    }

    private IEnumerator GatherResource()
    {
        yield return new WaitForSeconds(2f);
        if (targetResource != null)
        {
            targetResource.Collect(); // Вместо прямого Destroy
            targetResource = null;
            state = DroneState.Returning;
        }
        else
        {
            state = DroneState.Idle;
        }
    }
}