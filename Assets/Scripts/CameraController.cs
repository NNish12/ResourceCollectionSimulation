using UnityEngine;

public class CameraController : MonoBehaviour
{
    // private readonly string Horizontal = "Horizontal";
    // private readonly string Vertical = "Vertical";

    public float followSpeed = 5f;

    public float defaultZoom;
    public float zoomOnTarget = 3f;
    public float zoomSpeed = 5f;

    private Transform currentTarget = null;
    private Vector3 defaultPosition;
    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        defaultPosition = transform.position;
        defaultZoom = cam.orthographicSize;
    }

    void Update()
    {
        HandleMouseClick();
        FollowTarget();
        HandleZoom();
    }

    void FollowTarget()
    {
        Vector3 targetPos;

        if (currentTarget != null)
        {
            targetPos = new Vector3(
                currentTarget.position.x,
                currentTarget.position.y,
                transform.position.z
            );
        }
        else
        {
            targetPos = new Vector3(
                defaultPosition.x,
                defaultPosition.y,
                transform.position.z
            );
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }

    void HandleZoom()
    {
        float targetZoom = currentTarget != null ? zoomOnTarget : defaultZoom;
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);
    }

    void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastClickTime < doubleClickThreshold)
            {
                Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider != null && hit.collider.CompareTag("Drone"))
                {
                    currentTarget = hit.transform;
                }
                else
                {
                    currentTarget = null;
                }
            }

            lastClickTime = Time.time;
        }
    }
}