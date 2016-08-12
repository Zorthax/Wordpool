using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour 
{
	public GameObject teleportOutLocation;
    public GameObject boxTeleportLocation;

	// Use this for initialization
	void Start () 
    {
	
		//Check if Teleport out location has been set.
		if (teleportOutLocation == null) {
			Debug.LogWarning("No Teleporter Out Location has been set for " + transform.name + ", Please set it and restart");
		}

		//Checks to see if you have set the collider on this object to a trigger 
		if (!GetComponent<Collider> ().isTrigger)
        {
			Debug.LogWarning("Collider on " + transform.name + " is not set to \"Is Trigger\", please update and restart");
		}
	}
	
	// This function is called then any collider touches the collider on this object.
	void OnTriggerEnter(Collider collidedObject) 
    {
		Debug.Log ("Player Teleported");
		//Teleport GameObject that was collided with to the teleportOutLocation
		if (collidedObject.gameObject.tag == "Player") 
        {
			collidedObject.transform.position = teleportOutLocation.transform.position;
            collidedObject.GetComponent<PickupObject>().DropObject();
		}
        else
        {
            collidedObject.transform.position = boxTeleportLocation.transform.position;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PickupObject>().DropObject();
        }
	}

	//This section Draws a line from the teleporter in to the teleporter out within the editor
	void OnDrawGizmos(){
		Gizmos.DrawLine(transform.position,teleportOutLocation.transform.position);
	}
}
