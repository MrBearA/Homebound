
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class DogMovement : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    public float turnSpeed = 10f;
    public float gravity = -20f;
    public Transform cameraTransform;
    public float speedMultiplier = 1f;
    public float wobbleAmount = 0f;

    private float verticalVelocity = 0f;
    private CharacterController controller;
    private InputAction moveAction;
    private InputAction sprintAction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();
        Vector3 direction = camForward * input.y + camRight * input.x;

        if(wobbleAmount > 0f)
        {
            float angle = Mathf.Sin(Time.time * 5f) * wobbleAmount;
            direction = Quaternion.Euler(0f, angle, 0f) * direction;
        }

        float speed = sprintAction.IsPressed() ? runSpeed : walkSpeed;
        
        if(controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
        verticalVelocity += gravity * Time.deltaTime;
        Vector3 move = direction * speed * speedMultiplier;
        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }
}
