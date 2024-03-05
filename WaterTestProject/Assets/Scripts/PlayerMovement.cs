using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    Transform t;
    public static bool inWater;
    public static bool isSwimming;
    //if not in water, walk
    //if in water and not swimming, float
    //if in water and swimming, swim

    public LayerMask waterMask;

    [Header("Attributes")]
    public float staticWalkSpeed = 3f;
    public float walkSpeed = 3f;
    public float swimSpeed = 6f;
    public float runSpeed = 12f;
    public float staticRunSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;
    CamMover CamMoverScript;
    public int swimstate = 0;

    private Vector3 moveDirection = Vector3.zero;
    //private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;
    [SerializeField] private GameObject slamera = null;

    void Start()
    {
        t = this.transform;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inWater = false;
        CamMoverScript = GetComponent<CamMover>();
    }

    private void FixedUpdate()
    {
        SwimmingOrFloating();
        Debug.Log(swimstate);
    }

    void SwitchMovement()
    {
        //toggle inWater
        inWater = !inWater;

    }

    private void OnTriggerEnter(Collider other)
    {
        SwitchMovement();
    }

    private void OnTriggerExit(Collider other)
    {
        SwitchMovement();
    }

    void SwimmingOrFloating()
    {
        Debug.DrawRay(new Vector3(t.position.x, t.position.y + 0.5f, t.position.z), Vector3.down * 3, Color.magenta, 1f, false);
        bool swimCheck = false;
        

        if (inWater)
        {
            RaycastHit hit;
            if(Physics.Raycast(new Vector3(t.position.x, t.position.y + 0.5f, t.position.z), Vector3.down*3, out hit, Mathf.Infinity, waterMask))
            {
                if(hit.distance < 0.1f)
                {
                    swimCheck = true;
                }
            }
            else
            {

                swimCheck = true;
            }
            //Debug.DrawRay(new Vector3(t.position.x, t.position.y + 0.5f, t.position.z), Vector3.down*3, Color.magenta, 1f, false);
        }
 
        isSwimming = swimCheck;
        Debug.Log("isSwimming = " + isSwimming);
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Forward") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float curSpeedZ = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftControl) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;

        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = staticWalkSpeed;
            runSpeed = staticRunSpeed;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            transform.rotation = CamMoverScript.newRotation;
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));
            // rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            // rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            // transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

            //float YRotation = CamMoverScript.newRotation.y;
            //YRotation += 1;
            //YRotation *= 180;
            //transform.eulerAngles = new Vector3(0, YRotation, 0);
            //Debug.Log(slamera.transform.rotation);
            //Debug.Log(YRotation);
            //Debug.Log(transform.rotation);
        }
    }
}