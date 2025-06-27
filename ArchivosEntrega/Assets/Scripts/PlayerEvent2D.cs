using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerEvent2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("References")]
    public Animator animator;
    public Transform swordHitbox;

    public float attackInterval;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float rotationVelocity;
    private float turnSmoothTime = 0.1f;
    private float lastAttack = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAttack();
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float h = Input.GetAxis("Horizontal");
        Vector3 input = new Vector3(h, 0, 0).normalized;

        animator.SetFloat("VELOCITY", input.magnitude);
        
        if (input.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("JUMP");
        }
    }

    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var currentTime = Time.time;
            if (attackInterval < currentTime - lastAttack)
            {
                int attackId = Random.Range(0, 10) % 2;

                string trigger = $"ATTACK_0{attackId}";

                animator.SetTrigger(trigger);
                lastAttack = currentTime;
            }
        }
    }

    // Called via Animation Event (during swing)
    public void EnableSwordHitbox() => swordHitbox.gameObject.SetActive(true);
    public void DisableSwordHitbox() => swordHitbox.gameObject.SetActive(false);
}