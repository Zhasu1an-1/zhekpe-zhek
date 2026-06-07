using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    [Header("Dash")]
    public float dashSpeed = 18f;
    public float dashTime = 0.18f;
    public float dashCooldown = 1f;

    private CharacterController controller;
    private PlayerHealth playerHealth;
    private Animator animator;

    private Vector3 moveDirection;
    private Vector3 dashDirection;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private float lastDashTime = -10f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerHealth = GetComponent<PlayerHealth>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleMovementInput();
        HandleDashInput();
        MovePlayer();
        RotatePlayer();
        UpdateRunAnimation();
    }

    void HandleMovementInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * z + right * x).normalized;
    }

    void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time - lastDashTime >= dashCooldown)
        {
            StartDash();
        }
    }

    void MovePlayer()
    {
        if (controller == null) return;

        if (isDashing)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);

            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0)
                EndDash();
        }
        else
        {
            controller.Move(moveDirection * speed * Time.deltaTime);
        }
    }

    void RotatePlayer()
    {
        if (moveDirection.magnitude > 0.1f && !isDashing)
            transform.forward = moveDirection;
    }

    void UpdateRunAnimation()
    {
        if (animator == null) return;

        bool isMoving = moveDirection.magnitude > 0.1f;
        animator.SetBool("isRunning", isMoving && !isDashing);
    }

    void StartDash()
    {
        dashDirection = moveDirection == Vector3.zero ? transform.forward : moveDirection;

        isDashing = true;
        dashTimer = dashTime;
        lastDashTime = Time.time;

        if (playerHealth != null)
            playerHealth.SetInvincible(true);

        Debug.Log("Dash Dodge!");
    }

    void EndDash()
    {
        isDashing = false;

        if (playerHealth != null)
            playerHealth.SetInvincible(false);
    }
}