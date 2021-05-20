using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    [System.Serializable]
    public class MouseInput
    {
        public Vector2 Damping;
        public Vector2 Sensitivity;
        public bool lockMouse;

    }
    [SerializeField] MouseInput MouseControl;
    Vector2 mouseInput;

    private TouchField touchField;
    


    [SerializeField]
    private Vector2 default_Look_Limits;
    public Transform playerAim;
    private float current_Roll_Angle;
    [SerializeField]
    private float roll_Angle = 10f;
    [SerializeField]
    private float roll_Speed = 3f;
    [SerializeField]
    private bool invert;
    private Vector2 current_Mouse_Look;




    private VirtualJoystick joyStick;
    private CharacterController charController;
    private Vector3 moveDirection;
    public float speed;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float proneSpeed;
    [SerializeField] float walkAimSpeed;
    [SerializeField] float crouchAimSpeed;
    [SerializeField] float proneAimSpeed;
    [SerializeField] float gravity = 20f;
    [SerializeField] float jumpForce = 10f;
    public float verticalVelocity;

    [SerializeField] Transform groundPlane;
    public MouseLook mouseLook;
    // Start is called before the first frame update
    void Start()
    {

        //groundPlane = GameObject.FindGameObjectWithTag("ImageTarget").transform;
        //this.transform.parent = groundPlane;
        speed = walkSpeed;
        charController = GetComponent<CharacterController>();
        touchField = GameObject.FindGameObjectWithTag("TouchField").GetComponent<TouchField>();
        joyStick = GameObject.FindGameObjectWithTag("VirtualJoystick").GetComponent<VirtualJoystick>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        LookAroundAndCameraPosition();
    }



    /// <summary>
    /// --------------------------PLAYER MOVEMENT ------------------------------------------------
    /// </summary>
    void Move()
    {
        moveDirection = new Vector3(joyStick.Horizontal(), 0f, joyStick.Vertical());
        moveDirection = transform.TransformDirection(moveDirection);

       

        moveDirection *= speed * Time.deltaTime;

        ApplyGravity();
        charController.Move(moveDirection);
    }
    void ApplyGravity()
    {
        if (charController.isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;

            PlayerJump();
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity * Time.deltaTime;
    }


    //-------------------------------FOR PLAYER JUMP-----------------------------------------------------------
    void PlayerJump()
    {
        if (charController.isGrounded && Input.GetKeyDown(KeyCode.Space) /*InputController.Instance.IsJump == true*/)
        {
            verticalVelocity = jumpForce;
            //InputController.Instance.IsJump = false;
        }

    }



    //---------THIS FUNCTION IS FOR CAMERA AND PLAYER LOOKING AND ITS POSITION SET ACCRODING TO PLAYER MOVEMENT----
    void LookAroundAndCameraPosition()
    {
        current_Mouse_Look = new Vector2(touchField.mouseY(), touchField.mouseX());

        mouseInput.x = Mathf.Lerp(mouseInput.x, touchField.mouseX(), 1f / MouseControl.Damping.x);
        //mouseInput.y += current_Mouse_Look.x * MouseControl.Sensitivity.y * (invert ? 1f : -1f);


       
        //--------------------------------THIS SETS THE MAXIMUM VERTICLE ANGLE OF ROTATION FOR PLAYER CAMERA -----------
        //mouseInput.y = Mathf.Clamp(mouseInput.y, default_Look_Limits.x, default_Look_Limits.y);


        current_Roll_Angle = Mathf.Lerp(current_Roll_Angle, touchField.mouseX() * roll_Angle, Time.deltaTime * roll_Speed);

        //playerAim.localRotation = Quaternion.Euler(mouseInput.y, 0f, current_Roll_Angle);
        
        transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);
    }





}
