using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;

    
    private Sliding slide;
    private WallRun wallrun;
    private GunSystem gunsystem;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    public float movementMultiplier = 10f;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;


    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;
    [SerializeField] float gravityMultiplier;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;
    bool isGrounded;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;


    Rigidbody rb;

    RaycastHit slopeHit;

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if(slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private IEnumerable SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime;
            yield return null;
        }
        moveSpeed = desiredMoveSpeed;
    }

    private void Start()
    {
        slide = GetComponent<Sliding>();
        wallrun = GetComponent<WallRun>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }


    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();
        ControlDrag();
        ControlSpeed();
        
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        if (OnSlope() && slide.sliding)
        {
            rb.AddForce(orientation.forward * jumpForce, ForceMode.Impulse);
        }
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection(slopeMoveDirection) * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (wallrun.wallrunning)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
            rb.AddForce(-orientation.up * gravityMultiplier * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //HARD LEVEL TELEPORTERS---------------------------------------------------------
        if (collider.gameObject.name == "Plane0")
        {
            rb.transform.position = new Vector3(1.0f, 2.47f, 0.25f);
        }
        if (collider.gameObject.name == "Plane1")
        {
            rb.transform.position = new Vector3(2.07f, -6.73f, 139.4f);
        }
        if (collider.gameObject.name == "Plane2")
        {
            rb.transform.position = new Vector3(195.1f, -19.24f, 139.4f);
        }
        if (collider.gameObject.name == "Plane3")
        {
            rb.transform.position = new Vector3(195.1f, -19.24f, 139.4f);
        }
        if (collider.gameObject.name == "Plane4")
        {
            rb.transform.position = new Vector3(195.1f, -19.24f, 139.4f);
        }
        if (collider.gameObject.name == "Plane5")
        {
            rb.transform.position = new Vector3(219.2f, 42.01f, 2.85f);
        }
        if (collider.gameObject.name == "Plane6")
        {
            rb.transform.position = new Vector3(219.2f, 42.01f, 2.85f);
        }
        if (collider.gameObject.name == "Plane7")
        {
            rb.transform.position = new Vector3(76.53f, 8.29f, -87.96f);
        }
        //-------------------------------------------------------------------------------

        //MEDIUM LEVEL TELEPORTERS-------------------------------------------------------
        if (collider.gameObject.name == "PlaneMid0")
        {
            rb.transform.position = new Vector3(1.5f, 1.5f, 1.5f);
        }
        if (collider.gameObject.name == "PlaneMid1")
        {
            rb.transform.position = new Vector3(-17.43f, 4.4f, 96.24f);
        }
        if (collider.gameObject.name == "PlaneMid2")
        {
            rb.transform.position = new Vector3(-17.43f, 4.4f, 96.24f);
        }
        if (collider.gameObject.name == "PlaneMid3")
        {
            rb.transform.position = new Vector3(-17.43f, 4.4f, 96.24f);
        }
        if (collider.gameObject.name == "PlaneMid4")
        {
            rb.transform.position = new Vector3(-15.23f, 11.23f, 199.776f);
        }
        if (collider.gameObject.name == "PlaneMid5")
        {
            rb.transform.position = new Vector3(103.62f, 11.23f, 171.04f);
        }
        if (collider.gameObject.name == "PlaneMid6")
        {
            rb.transform.position = new Vector3(215.79f, 6.21f, 200.99f);
        }
        //-------------------------------------------------------------------------------

        //EASY LEVEL TELEPORTERS-------------------------------------------------------
        if (collider.gameObject.name == "Plane0_easy")
        {
            rb.transform.position = new Vector3(7.8f, 15.47f, -44.2f);
        }
        if (collider.gameObject.name == "Plane1_easy")
        {
            rb.transform.position = new Vector3(7.8f, -12.37f, 181.59f);
        }
        if (collider.gameObject.name == "Plane2_easy")
        {
            rb.transform.position = new Vector3(7.8f, -22.2f, 332.6f);
        }


    }
}
