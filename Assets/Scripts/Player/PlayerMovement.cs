using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    Vector2 movement;
    InputAction moveAction;
    Vector3 moveDirection;
    Rigidbody rb;
    void OnEnable()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void OnDisable()
    {
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        moveDirection = Quaternion.Euler(0, orientation.eulerAngles.y, 0) * new Vector3(movement.x, 0, movement.y);

        Vector3 targetVelocity = Vector3.zero;
        if (moveDirection != Vector3.zero)
        {
            targetVelocity = moveDirection.normalized * moveSpeed;
        }

        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }
}
