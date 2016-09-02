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
    public float gravity = 0.25f;
    public float platformAssistHeight;
    public float platformAssistSensitivity;


    float cameraYAngle;
    bool grounded;
    bool onSlope;
    float bobEvening;
    float xMovement;
    Vector3 yMovement;
    float zMovement;
    bool changingCamera;
    Vector3 previousUp;
    Vector3 previousRight;
    Vector3 previousForward;
    Vector3 newUp;
    Vector3 tempRight;

    public Vector3 gravityDirection = new Vector3(0, -1, 0);
    Transform cameraTransform;
    Rigidbody rb;

	void Start ()
    {
        Cursor.visible = showCursor;
        if (!showCursor) Cursor.lockState = CursorLockMode.Locked;
        changingCamera = false;

        //gravityDirection = new Vector3(0, 0, 1);
        transform.up = -gravityDirection;
        //transform.rotation = Quaternion.Euler(gravityDirection * 90);
        cameraTransform = this.gameObject.transform.GetChild(0);
        cameraTransform.forward = transform.forward;

        rb = GetComponent<Rigidbody>();
	}
	
	void Update ()
    {
        if (!changingCamera)
        {
            GroundCheck();
            CameraMovement();
            PlayerMovement();
        }
        else
        {
            FixCamera();
        }
        

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
                if ((Input.GetButton("Sprint") || Input.GetAxis("Sprint") != 0) && grounded)
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
        //Check if on ground
        float distToGround = 1.2f;
        grounded = Physics.Raycast(transform.position, gravityDirection, distToGround);

        //Move towards platforms
        if (!grounded)
        {
            RaycastHit[] all = Physics.RaycastAll(transform.position, gravityDirection, platformAssistHeight);

            foreach(RaycastHit h in all)
            {
                if (h.transform.tag == "Platform Assist" && rb.velocity.y < 0)
                {
                    Debug.Log(DoubleAbs(gravityDirection.y));
                    Vector3 diff = transform.position - h.transform.position;
                    transform.position = Vector3.MoveTowards(transform.position,
                        new Vector3(transform.position.x - (diff.x * DoubleAbs(gravityDirection.x)), 
                        transform.position.y - (diff.y * DoubleAbs(gravityDirection.y)), 
                        transform.position.z - (diff.z * DoubleAbs(gravityDirection.z))), 
                        platformAssistSensitivity);
                }
            }
        }
        
    }

    void PlayerMovement()
    {
        //Jumping

        /*if (grounded && !Input.GetButton("Jump")) yMovement = gravityDirection * 1.0f;
        else */if (grounded && Input.GetButtonDown("Jump")) yMovement = transform.up * jumpPower;
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
        if ((Input.GetButton("Sprint") || Input.GetAxis("Sprint") != 0) && !(!sprintInAir && !grounded)) //Sprinting
        {
            xMovement *= sprintMultiplier;
            zMovement *= sprintMultiplier;
        }
        ApplyVelocity();

        //Slope checking
        //Vector3 knee = transform.position - MultiplyByGravity(0.7f); //height at which to raycast
        //RaycastHit hit;
        //if (Physics.Raycast(knee, rb.velocity, out hit, 1.0f)) //cast in direction of movement
        //{
        //    if (Vector3.Dot(transform.up, hit.normal) < 1.0f && Vector3.Dot(transform.up, hit.normal) >= 0.1f) //check angle of object hit by raycast
        //    {
        //        rb.velocity = new Vector3(rb.velocity.x * DoubleAbs(gravityDirection.x), rb.velocity.y * DoubleAbs(gravityDirection.y), rb.velocity.z * DoubleAbs(gravityDirection.z)); //Stop moving if angle is too steep
        //    }
        //}
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
        transform.up = Vector3.MoveTowards(transform.up, -gravityDirection, 1.0f);

        cameraTransform.localRotation = Quaternion.Euler(cameraTransform.localRotation.eulerAngles.x + mouseY * mouseYSensitivity,
            cameraTransform.localRotation.eulerAngles.y + mouseX * mouseXSensitivity, 0);
    }

    void ApplyVelocity()
    {
        Vector3 sideMovement;
        Vector3 forwardMovement;

        Vector3 forward = Quaternion.AngleAxis(-90, transform.up) * cameraTransform.right; //Alternative to camera's forward
        //Move based on camera's angle and gravity direction
        sideMovement = new Vector3(cameraTransform.right.x * DoubleAbs(gravityDirection.x), 
            cameraTransform.right.y * DoubleAbs(gravityDirection.y), 
            cameraTransform.right.z * DoubleAbs(gravityDirection.z)) * xMovement * movementSpeed;
        forwardMovement = new Vector3(forward.x * DoubleAbs(gravityDirection.x),
            forward.y * DoubleAbs(gravityDirection.y),
            forward.z * DoubleAbs(gravityDirection.z)) * zMovement * movementSpeed;

        rb.velocity = sideMovement + forwardMovement + yMovement + (gravityDirection * gravity); 
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

    float DoubleAbs(float num)
    {
        //0 will equal 1
        //1 and -1 will equal 0
        //Good for movement at different perspectives/angles
        return Mathf.Abs(Mathf.Abs(num) - 1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Gravity Wall" && !changingCamera && transform.up != other.transform.up)
        {
            previousUp = transform.up;
            previousRight = cameraTransform.right;
            gravityDirection = -other.transform.up;
            

            changingCamera = true;
        }
    }

    void FixCamera()
    {

        transform.up = Vector3.MoveTowards(transform.up, -gravityDirection, 0.1f);
        if (transform.up == -gravityDirection) changingCamera = false;

    }
}
