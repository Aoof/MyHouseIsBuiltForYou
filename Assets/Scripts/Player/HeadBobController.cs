using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    public bool Enabled = true;

    [SerializeField, Range(0, 0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float frequency = 10.0f;
    [SerializeField] private Transform camera;
    [SerializeField] private float resetSpeed = 10.0f;
    private Vector3 cameraStartPosition;
    private PlayerMovement playerMovement;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        cameraStartPosition = camera.localPosition;
    }

    private Vector3 FootStepMotion()
    {
        Vector3 position = Vector3.zero;

        position.y += Mathf.Sin(Time.time * frequency) * amplitude;
        position.x += Mathf.Sin(Time.time * frequency / 2) * amplitude * 2;

        return position;
    }

    private void ResetPosition()
    {
        if (camera.localPosition == cameraStartPosition)
            return;
        
        camera.localPosition = Vector3.Lerp(camera.localPosition, cameraStartPosition, resetSpeed * Time.deltaTime);
    }

    private void Update()
    {
        if (!Enabled)
            return;

        if (playerMovement.Movement != Vector2.zero) {
            camera.localPosition = cameraStartPosition + FootStepMotion();
        } else {
            ResetPosition();
        }
    }
}
