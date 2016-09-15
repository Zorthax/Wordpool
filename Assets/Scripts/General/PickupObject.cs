using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PickupObject : MonoBehaviour {

	GameObject mainCamera;
	bool carrying;
	public GameObject carriedObject;
    public Vector3 position;
	public float smooth;
	public float speed;
    public float throwForce;
    public float throwHeight;
	public float regrabDelay=0.5f;
	public float grabDistance=5f;
	public AudioSource oneTimeSoundSource;
	public AudioSource soundSource;
	float grabDelay=0f;
	Quaternion rot;


	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetButtonDown("Throw")) {
			carrying = false;
			carriedObject.GetComponent<Rigidbody>().useGravity = true;
			carriedObject.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * throwForce + Camera.main.transform.up * throwHeight;
            carriedObject = null;
        }

		if (carrying) {
			Carry (carriedObject);
			CheckDrop();
		} else {
			Pickup ();
		}
		if (grabDelay > 0) {
			grabDelay-=Time.deltaTime;
		}



	}
	void RotateObject() {
		carriedObject.transform.Rotate (5, 10, 5);
	}

	void Carry(GameObject obj) {
		obj.GetComponent<Rigidbody>().velocity=Vector3.zero;
        //obj.transform.position = Vector3.Lerp(obj.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime * smooth);

        Vector3 positionOnScreen = Camera.main.transform.forward * position.z + Camera.main.transform.right * position.x + Camera.main.transform.up * position.y;

        Vector3 direct = (mainCamera.transform.position + positionOnScreen) - obj.transform.position;
		direct.Normalize ();
		obj.GetComponent<Rigidbody>().velocity = direct * smooth * Vector3.Distance(mainCamera.transform.position + positionOnScreen, obj.transform.position);                                                  
		// stop picked up object rotating 
		obj.transform.rotation = Quaternion.identity; 

	}

	void Pickup() {
		if (CrossPlatformInputManager.GetButtonDown("Pick Up")&& grabDelay <= 0) {
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x,y));
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, grabDistance)) {
				Pickupable p = hit.collider.GetComponent<Pickupable>();
				if( p != null) {
					/*if (p.oneTimeSound!=null&&p.pickedUp==false){
						oneTimeSoundSource.clip=p.oneTimeSound;
						oneTimeSoundSource.Play();
						hit.collider.GetComponent<Pickupable>().pickedUp=true;
					}
					if (p.pickUpSound!=null){
						soundSource.clip=p.pickUpSound;
						soundSource.Play();

					}*/
					carrying = true;
					carriedObject = p.gameObject;
					p.gameObject.GetComponent<Rigidbody>().useGravity = false;
					p.gameObject.GetComponent<Rigidbody>().velocity=Vector3.zero;
					rot = p.gameObject.transform.rotation;
				}
			}

		}
	}

	void CheckDrop() {
		if (CrossPlatformInputManager.GetButtonDown("Pick Up")) {
			DropObject();
		}
	}

	public void DropObject() {
		carrying = false;
		carriedObject.GetComponent<Rigidbody>().useGravity = true;
		carriedObject = null;
		grabDelay = regrabDelay;
	}
}
