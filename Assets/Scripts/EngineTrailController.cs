using UnityEngine;

public class EngineTrailController : MonoBehaviour
{
    [Header("Trail Settings")]
    [SerializeField] private float minSpeedThreshold = 0.1f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float maxLifetime = 1f;
    [SerializeField] private float fadeSpeed = 5f;

    [Header("References")]
    public DroneController agent;
    public ParticleSystem[] engineTrails;

    private void Start()
    {
        agent = GetComponent<DroneController>();
    }

    private void Update()
    {
        float speed = agent.CurrentVelocity;
        float normalizedSpeed = Mathf.Clamp01(speed / maxSpeed);
        float targetLifetime = normalizedSpeed > minSpeedThreshold ? normalizedSpeed * maxLifetime : 0f;

        foreach (var trail in engineTrails)
        {
            var main = trail.main;
            float currentLifetime = main.startLifetime.constant;
            float newLifetime = Mathf.Lerp(currentLifetime, targetLifetime, Time.deltaTime * fadeSpeed);
            main.startLifetime = new ParticleSystem.MinMaxCurve(newLifetime);
        }
    }
}