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
    public DroneState State => state;
    public float CurrentVelocity => agent.velocity.magnitude; //текущая скорость дрона
    private NavMeshAgent agent;

    public enum DroneState { Idle, MovingToResource, Gathering, Returning }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
    }

    void Update()
    {
        UpdateLookDirection();

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

    private void MoveTo(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    private void UpdateLookDirection()
    {
        Vector3 velocity = agent.velocity;

        if (velocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
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
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        if (agent != null)
        {
            agent.speed = newSpeed;
        }
    }

    private IEnumerator GatherResource()
    {
        yield return new WaitForSeconds(2f);
        if (targetResource != null)
        {
            targetResource.Collect();
            targetResource = null;
            state = DroneState.Returning;
        }
        else
        {
            state = DroneState.Idle;
        }
    }
}