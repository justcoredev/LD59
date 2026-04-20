using UnityEngine;

public class CameraStretch : MonoBehaviour
{
    public float maxOffset = 0.5f;
    public float smoothSpeed = 5f;
    public Vector3 targetPos = new Vector3(0, -2, 0);

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        Vector3 mouseWorld = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mouseWorld - targetPos;
        Vector3 offset = Vector3.ClampMagnitude(direction / 5.0f, maxOffset * 5);
        Vector3 desiredPosition = targetPos + offset;
        desiredPosition = new Vector3(0, Mathf.Clamp(desiredPosition.y, -5, 0), 0);

        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(desiredPosition.x, desiredPosition.y, transform.position.z),
            Time.deltaTime * smoothSpeed
        );
    }
}