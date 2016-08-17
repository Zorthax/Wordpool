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

    public Vector3 gravityDirection = new Vector3(0, -1, 0);
    Transform cameraTransform;
    Rigidbody rb;

	void Start ()
    {
        Cursor.visible = showCursor;
        if (!showCursor) Cursor.lockState = CursorLockMode.Locked;

        //gravityDirection = new Vector3(0, 0, 1);
        transform.up = gravityDirection;
        //transform.rotation = Quaternion.Euler(gravityDirection * 90);
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
        cameraTransform.localPosition = Vector3.MoveTowards(cameraTransform.localPosition, CameraBobHeight(), 0.1f);
        
    }

    public Vector3 CameraBobHeight()
    {
        if (headBobbing && grounded)
        {
            if (xMovement != 0 || zMovement != 0)
            {
                bobEvening = Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * bobbingSpeed));
                if (Input.GetButton("Sprint") && grounded)
                    return MultiplyByGravity(Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * bobbingSpeed * sprintMultiplier)) * bobbingHeight);
                else
                    return MultiplyByGravity(Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * bobbingSpeed)) * bobbingHeight);
            }
            else if (bobEvening > 0.1f)
            {
                //bobEvening *= 0.98f;
                return MultiplyByGravity(bobEvening * bobbingHeight);
            }
        }

        return Vector3.zero;
    }

    void GroundCheck()
    {
        //Raycast downwards
        float distToGround = 1.2f;
        grounded = Physics.Raycast(transform.position, gravityDirection, distToGround);
        
    }

    void PlayerMovement()
    {
        //Jumping
        if (grounded && Input.GetButtonDown("Jump")) yMovement = transform.up * jumpPower;
        else yMovement = CurrentUpVelocity();
        

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
        Vector3 knee = transform.position - MultiplyByGravity(0.7f); //height at which to raycast
        RaycastHit hit;
        if (Physics.Raycast(knee, new Vector3(rb.velocity.x, 0, rb.velocity.z), out hit, 1.0f)) //cast in direction of movement
        {
            if (Vector3.Dot(Vector3.up, hit.normal) < 0.7f && Vector3.Dot(Vector3.up, hit.normal) >= 0.2f) //check angle of object hit by raycast
            {
                //rb.velocity = new Vector3(0, rb.velocity.y, 0); //Stop moving if angle is too steep
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
        transform.up = -gravityDirection;

        cameraTransform.localRotation = Quaternion.Euler(cameraTransform.localRotation.eulerAngles.x + mouseY * mouseYSensitivity,
            cameraTransform.localRotation.eulerAngles.y + mouseX * mouseXSensitivity, 0);
        

        

    }

    void ApplyVelocity()
    {

        Vector3 sideMovement;
        Vector3 forwardMovement;
            sideMovement = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z) * xMovement * movementSpeed;
            forwardMovement = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z) * zMovement * movementSpeed;
        //Vector3 sideMovement = new Vector3(transform.right.x, 0, transform.right.z) * xMovement * movementSpeed;
        rb.velocity = forwardMovement + sideMovement + yMovement + (gravityDirection * gravity); 
    }

    Vector3 CurrentUpVelocity()
    {
        //return new Vector3(Mathf.Abs(rb.velocity.x) * (gravityDirection.x), Mathf.Abs(rb.velocity.y) * (gravityDirection.y), Mathf.Abs(rb.velocity.z) * (gravityDirection.z));
        float x, y, z;
        if (gravityDirection.x >= 0) x = rb.velocity.x * (gravityDirection.x);
        else x = -rb.velocity.x * (gravityDirection.x);
        if (gravityDirection.y >= 0) y = rb.velocity.y * (gravityDirection.y);
        else y = -rb.velocity.y * (gravityDirection.y);
        if (gravityDirection.z >= 0) z = rb.velocity.z * (gravityDirection.z);
        else z = -rb.velocity.z * (gravityDirection.z);
        return new Vector3(x, y, z);
    }

    Vector3 MultiplyByGravity(float direction)
    {
        return new Vector3(direction * -gravityDirection.x, direction * -gravityDirection.y, direction * -gravityDirection.z);
    }
}
