using UnityEngine;

public class Background : MonoBehaviour
{
    [Header("The center of rotation")]
    public RectTransform centerPoint;

    public float speed = 10f;

    void Update()
    {
        if (centerPoint == null)
            return;

        Vector3 worldCenter = centerPoint.position;
        transform.RotateAround(worldCenter, Vector3.forward, speed * Time.deltaTime);
    }
}
