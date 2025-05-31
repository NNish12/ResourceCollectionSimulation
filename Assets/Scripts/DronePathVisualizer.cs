using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LineRenderer))]
public class DronePathVisualizer : MonoBehaviour
{
    public bool showPath = true;

    private NavMeshAgent agent;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // простой материал
        lineRenderer.widthMultiplier = 0.03f;
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.green;
    }

    private void Update()
    {
        if (!showPath || agent == null || agent.path == null || agent.path.corners.Length < 2)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        var corners = agent.path.corners;
        lineRenderer.positionCount = corners.Length;
        lineRenderer.SetPositions(corners);
    }

    public void SetShowPath(bool value)
    {
        showPath = value;
        if (!showPath)
            lineRenderer.positionCount = 0;
    }
}