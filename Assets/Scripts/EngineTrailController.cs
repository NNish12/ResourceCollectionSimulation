using UnityEngine;

public class EngineTrailController : MonoBehaviour
{
    [Header("Trail Settings")]
    [SerializeField] public float maxEmissionRate = 100f;
    [SerializeField] public float maxSpeed = 10f;
    [SerializeField] private float maxLifeTime = 1f; //длина шлейфа
    [SerializeField] private float minSpeedThreshold = 0.1f; //минимальная скорость, при которой шлейф будет отображаться
    [SerializeField] private float timeToFade = 0.5f; //время затухания шлейфа

    [Header("References")]
    public DroneController agent;
    public ParticleSystem engineTrail;

    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.MainModule mainModule;
    private void Start()
    {
        agent = GetComponent<DroneController>();
        emissionModule = engineTrail.emission; //это структура, которая управляет эмиссией частиц в ParticleSystem
        mainModule = engineTrail.main;
    }

    private float smoothEmission;
    private float smoothLifetime;

    private void Update()
    {
        float speed = agent.CurrentVelocity;
        float normalizeSpeed = Mathf.Clamp01(speed / maxSpeed);

        float targetEmission = normalizeSpeed * maxEmissionRate;
        float targetLifetime = normalizeSpeed * maxLifeTime;

        if (speed < minSpeedThreshold)
        {
            smoothEmission = Mathf.Lerp(smoothEmission, 0f, Time.deltaTime / timeToFade);
            smoothLifetime = Mathf.Lerp(smoothLifetime, 0f, Time.deltaTime / timeToFade);
        }
        else
        {
            emissionModule.rateOverTime = Mathf.Max(0f, smoothEmission / 2f);
            smoothEmission = Mathf.Lerp(smoothEmission, targetEmission, Time.deltaTime / timeToFade);
            smoothLifetime = Mathf.Lerp(smoothLifetime, targetLifetime, Time.deltaTime / timeToFade);
        }
        emissionModule.rateOverTime = Mathf.Max(0f, smoothEmission);
        mainModule.startLifetime = Mathf.Max(0f, smoothLifetime);
    }


}
