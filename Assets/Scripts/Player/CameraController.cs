using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Vector2 sensitivity;
    public Transform orientation;
    Vector2 rotation;

    private InputAction lookAction;

    public void Start()
    {
        orientation.rotation = transform.rotation;
    }

    void OnEnable()
    {
        lookAction = InputSystem.actions.FindAction("Look");
    }

    void OnDisable()
    {
    }

    public void Update()
    {
        Vector2 lookDelta = lookAction.ReadValue<Vector2>();

        rotation.x += lookDelta.x * sensitivity.x * Time.deltaTime;
        rotation.y -= lookDelta.y * sensitivity.y * Time.deltaTime;

        rotation.y = Mathf.Clamp(rotation.y, -90f, 90f);
        orientation.rotation = Quaternion.Euler(rotation.y, rotation.x, 0f);
        transform.rotation = orientation.rotation;
    }
}
