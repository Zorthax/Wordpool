using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour {

    [Header("Camera Variables")]
    public bool showCursor = true;
    public float yOrigin = 0;
    public float mouseXSensitivity = 4;
    public float mouseYSensitivity = 2;
    public float mouseYMax = 320;
    public float mouseYMin = 30;
    public bool invertMouseX;
    public bool invertMouseY;
    public bool headBobbing;
    public float bobbingHeight = 0.5f;
    public float bobbingSpeed = 5;

    [Space(5)]
    [Header("Movement Variables")]
    public float movementSpeed = 20;
    public float sprintMultiplier = 2;
    public float jumpPower = 10;
    public float speedInAirMultiplier = 1.0f;
    public bool sprintInAir;
    public float gravity = 9.18f;


    float cameraYAngle;
    bool grounded;
    bool onSlope;
    float bobEvening;
    float xMovement;
    Vector3 yMovement;
    float zMovement;

    Vector3 gravityDirection;
    Transform cameraTransform;
    Rigidbody rb;

	void Start ()
    {
        Cursor.visible = showCursor;
        if (!showCursor) Cursor.lockState = CursorLockMode.Locked;

        gravityDirection = new Vector3(0, -1, 0);
        cameraTransform = this.gameObject.transform.GetChild(0);
        cameraTransform.forward = transform.forward;

        rb = GetComponent<Rigidbody>();
	}
	
	void Update ()
    {

        GroundCheck();
        CameraMovement();
        PlayerMovement();

        //Keep camera at player position
        cameraTransform.localPosition = CameraBobHeight();
        
    }

    public Vector3 CameraBobHeight()
    {
        if (headBobbing && grounded)
        {
            if (xMovement != 0 || zMovement != 0)
            {
                bobEvening = Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * bobbingSpeed));
                if (Input.GetButton("Sprint") && grounded)
                    return new Vector3(0, Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * bobbingSpeed * sprintMultiplier)) * bobbingHeight, 0);
                else
                    return new Vector3(0, Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * bobbingSpeed)) * bobbingHeight, 0);
            }
            else if (bobEvening > 0.1f)
            {
                //bobEvening *= 0.98f;
                return new Vector3(0, bobEvening * bobbingHeight, 0);
            }
        }

        return Vector3.zero;
    }

    void GroundCheck()
    {
        //Raycast downwards
        float distToGround = 1.0f;
        grounded = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.2f);
        
    }

    void PlayerMovement()
    {
        //Jumping
        yMovement = CurrentUpVelocity();
        if (grounded && Input.GetButtonDown("Jump")) yMovement = -gravityDirection * jumpPower;

        //Walking
        xMovement = Input.GetAxis("Horizontal");
        zMovement = Input.GetAxis("Vertical");
        if (!grounded)                        //Slow down in the air
        {
            xMovement *= speedInAirMultiplier;
            zMovement *= speedInAirMultiplier;
        }
        if (xMovement != 0 && zMovement != 0) //Prevent diagonal movement being too fast
        {
            xMovement /= 2;
            zMovement /= 2;
        }
        if (Input.GetButton("Sprint") && !(!sprintInAir && !grounded)) //Sprinting
        {
            xMovement *= sprintMultiplier;
            zMovement *= sprintMultiplier;
        }
        ApplyVelocity();

        //Slope checking
        Vector3 knee = transform.position - new Vector3(0, 0.7f, 0); //height at which to raycast
        RaycastHit hit;
        if (Physics.Raycast(knee, new Vector3(rb.velocity.x, 0, rb.velocity.z), out hit, 1.0f)) //cast in direction of movement
        {
            if (Vector3.Dot(Vector3.up, hit.normal) < 0.7f && Vector3.Dot(Vector3.up, hit.normal) >= 0.2f) //check angle of object hit by raycast
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0); //Stop moving if angle is too steep
            }
        }
    }

    void CameraMovement()
    {
        //Mouse and camera movement
        float mouseY;
        if (invertMouseY) mouseY = Input.GetAxis("Camera Y");
        else mouseY = -Input.GetAxis("Camera Y");
        float mouseX;
        if (invertMouseX) mouseX = -Input.GetAxis("Camera X");
        else mouseX = Input.GetAxis("Camera X");

        //Prevent camera from looking up too high or too low
        cameraYAngle = cameraTransform.localRotation.eulerAngles.x;
        if (mouseY > 0 && cameraYAngle > mouseYMin && cameraYAngle < mouseYMin + 10 || (mouseY < 0 && cameraYAngle < mouseYMax && cameraYAngle > mouseYMax - 10))
        {
            mouseY = 0;
        }

        //Rotate with mouse
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + mouseX * mouseXSensitivity, 0);
        cameraTransform.localRotation = Quaternion.Euler(cameraTransform.localRotation.eulerAngles.x + mouseY * mouseYSensitivity,
            cameraTransform.localRotation.eulerAngles.y, 0);
    }

    void ApplyVelocity()
    {
        Vector3 forwardMovement = new Vector3(transform.forward.x, 0, transform.forward.z) * zMovement * movementSpeed;
        Vector3 sideMovement = new Vector3(transform.right.x, 0, transform.right.z) * xMovement * movementSpeed;
        rb.velocity = forwardMovement + sideMovement + yMovement + (gravityDirection * gravity); 
    }

    Vector3 CurrentUpVelocity()
    {
        return new Vector3(rb.velocity.x * -gravityDirection.x, rb.velocity.y * -gravityDirection.y, rb.velocity.z * -gravityDirection.z);
    }
}
