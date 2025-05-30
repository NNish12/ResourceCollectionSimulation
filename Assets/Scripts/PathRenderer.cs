using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    public bool ShowPath = false;
    public Transform target;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color(0.5f, 0.5f, 0.5f, 1f);
        lineRenderer.endColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    void Update()
    {
        if (ShowPath && target != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, target.position);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}