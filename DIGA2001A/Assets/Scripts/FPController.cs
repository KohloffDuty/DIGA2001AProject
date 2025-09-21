using UnityEngine;
using UnityEngine.InputSystem;

public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Look Settings")]
    public Transform playerCamera;     // Camera assigned in Inspector
    public float lookSpeed = 2f;       // Same as lookSensitivity in old script
    private float verticalLookRotation = 0f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform gunPoint;

    [Header("Crouch Settings")]
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchSpeed = 2.5f;
    private float originalMoveSpeed;

    [Header("Pickup Setting")]
    public float pickupRange = 3f;
    public Transform holdPoint;
    private PickUpObject heldObject;

    [Header("Interaction Settings")]
    public float interactRange = 3f;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;

    private PlayerControls playerInput;   

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = new PlayerControls();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        originalMoveSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();

        playerInput.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero;

        playerInput.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Look.canceled += ctx => lookInput = Vector2.zero;


        playerInput.Player.Jump.performed += ctx => OnJump();

        playerInput.Player.Shoot.performed += ctx => OnShoot();

     
        playerInput.Player.PickUp.performed += ctx => OnPickUp();

       
        playerInput.Player.Crouch.performed += ctx => OnCrouch(true);
        playerInput.Player.Crouch.canceled += ctx => OnCrouch(false);
    }

    private void OnDisable()
    {
        playerInput.Player.Disable();
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();

        if (heldObject != null)
        {
            heldObject.MoveToHoldPoint(holdPoint.position);
        }
    }

    private void OnJump()
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    private void OnShoot()
    {
        Shoot();
    }
    private void Shoot()
    {
        if (bulletPrefab != null && gunPoint != null)
        {
          
            GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(playerCamera.forward * 1000f); 
            }

            Destroy(bullet, 3f); 
        }
    }


    private void OnCrouch(bool crouching)
    {
        if (crouching)
        {
            controller.height = crouchHeight;
            moveSpeed = crouchSpeed;
        }
        else
        {
            controller.height = standHeight;
            moveSpeed = originalMoveSpeed;
        }
    }

    private void OnPickUp()
    {
        if (heldObject == null)
        {
           
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            Debug.DrawRay(ray.origin, ray.direction * pickupRange, Color.green, 1f);

            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                PickUpObject pickup = hit.collider.GetComponent<PickUpObject>();
                if (pickup != null)
                {
                    pickup.PickUp(holdPoint);
                    heldObject = pickup;
                }
            }
        }
        else
        {
            heldObject.Drop();
            heldObject = null;
        }
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.CompareTag("Switchable"))
            {
                var switcher = hit.collider.GetComponent<MaterialSwitcher>();
                if (switcher != null)
                {
                    switcher.ToggleMaterial();
                }
            }
        }
    }

    public void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void HandleLook()
    {
        float lookX = lookInput.x * lookSpeed;
        float lookY = lookInput.y * lookSpeed;

        transform.Rotate(Vector3.up * lookX);

        
        verticalLookRotation -= lookY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0f, 0f);
    }
}
