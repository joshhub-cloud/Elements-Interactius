using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerv02 : MonoBehaviour
{

    [SerializeField] private Transform _cameraTransform;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;
    public bool isGrounded;

    public float _vertical;
    public float _horizontal;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;

    [SerializeField] private Vector3 _moveDirection = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GroundCheck();
        Move();
        Animate();
        

          Vector3 cameraForward = Vector3.Scale(_cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveDirection = _vertical * cameraForward + _horizontal * _cameraTransform.right;
        _moveDirection.x = moveDirection.x * _moveSpeed; _moveDirection.z = moveDirection.z * _moveSpeed;
    }

    private void GroundCheck()
    {
        // Check if the player is on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reset the velocity if the player is grounded and not falling
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Slightly negative to keep grounded
            animator.SetBool("isFalling", false);
        }
    }

   private void Move()
    {
        // Get input for movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Move the player
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Jump only if the player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("isJumping", true);
        }
    }

    private void Animate()
    {
        // Determine if the player is moving
        if (controller.velocity.magnitude > 0.1f && isGrounded == true)
        {
            animator.SetBool("isRunning", true);
        }

        // Set idle animation if not moving
        if (controller.velocity.magnitude > 0f && isGrounded)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        // Set jump and falling animations
        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
        }
        
        if (!isGrounded && velocity.y < 0)
        {
            animator.SetBool("isFalling", true);
        }

        // Set drop animation when touching ground after falling
        if (isGrounded && animator.GetBool("isFalling"))
        {
            animator.SetTrigger("Drop");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a sphere to visualize the ground check radius
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}
