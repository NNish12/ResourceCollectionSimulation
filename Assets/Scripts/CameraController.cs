using UnityEngine;

public class CameraController : MonoBehaviour
{
    private readonly string Horizontal = "Horizontal";
    private readonly string Vertical = "Vertical";

    [Header("Camera Settings")]
    public float zoomFactor = 2f;
    public float zoomSpeed = 5f;
    public float moveSpeed = 5f;
    public float returnSpeed = 2f;

    private Vector3 initialPosition;
    private float initialZoom;
    private float targetZoom;
    private Vector3 targetPosition;

    private bool isZoomedIn = false;
    private bool isReturning = false;

    private Transform followTarget = null;
    private float doubleClickTime = 0.3f;
    private float lastClickTime;

    private void Start()
    {
        initialPosition = transform.position;
        initialZoom = Camera.main.orthographicSize;

        targetZoom = initialZoom;
        targetPosition = initialPosition;
    }

    private void Update()
    {
        HandleDoubleClick();

        if (followTarget != null)
        {
            Vector3 followPos = new Vector3(followTarget.position.x, followTarget.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, followPos, Time.deltaTime * moveSpeed);
        }
        else if (isReturning)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * returnSpeed);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

            float distance = Vector3.Distance(transform.position, targetPosition);
            float zoomDiff = Mathf.Abs(Camera.main.orthographicSize - targetZoom);

            if (distance < 0.05f && zoomDiff < 0.05f)
            {
                transform.position = targetPosition;
                Camera.main.orthographicSize = targetZoom;
                isReturning = false;
            }
        }
        else
        {
            HandleCameraMovement();
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
        }
        targetZoom = Mathf.Clamp(targetZoom, 2f, 20f);
    }

    private void HandleDoubleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastClickTime < doubleClickTime) //двойной клик
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

                if (hit.collider != null && hit.collider.CompareTag("Drone"))
                {
                    followTarget = hit.collider.transform;
                    isZoomedIn = true;
                    isReturning = false;
                    targetZoom = initialZoom / zoomFactor;
                }
                else
                {
                    followTarget = null;
                    isZoomedIn = false;
                    isReturning = true;
                    targetPosition = initialPosition;
                    targetZoom = initialZoom;
                }
            }

            lastClickTime = Time.time;
        }
    }

    private void HandleCameraMovement()
    {
        float moveX = Input.GetAxis(Horizontal);
        float moveY = Input.GetAxis(Vertical);

        if (moveX != 0 || moveY != 0)
        {
            Vector3 move = new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;
            transform.position += move;
        }
    }
}