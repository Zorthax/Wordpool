using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    Vector3 start;
    public Vector3 end;
    GameObject player;
    public float timeToTravel;

    //Unused function for previous moving platform attempts
    //void movePlatform(Vector3 pos, Vector3 movePosition, float speed)
    //{
    //    //function that calculates the position of the platform and then runs 
    //    //MovePosition to translate the platform at a set speed
    //    Vector3 direction = (movePosition - pos).normalized;
    //    GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + direction * speed * Time.deltaTime);
    //}

    // Use this for initialization

    void Start () {
        //transform.position = start;
        start = transform.position;
        player = GameObject.Find("Capsule");
	}
	
	//Update is called once per frame
	void Update () {
        float loops = 2 * timeToTravel;
        float t = Time.time;
        int numLoops = (int)(t / loops);
        float remainder = t - loops * numLoops;

        float lerpValue = remainder / timeToTravel;
        if (lerpValue > 1)
            lerpValue = 2.0f - lerpValue;

        transform.position = Vector3.Lerp(start, end, lerpValue);
        /*
        //old moving platform code that works at a basic level but has numerical inaccuracies overtime with other platforms
        //code above is improved to stay consistent in its movement using lerp to move
        //the platforms.
        if (!reached)
        {
            //finds distance between where the platform is to its end point
            float distance = Vector3.Distance(transform.position, end);
            //the platform will continue to move until it is really close to its
            //end point where it then changes direction
            if (distance > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, end, speed * Time.deltaTime);
            }
            else
            {
                reached = true;
            }
        }
        else
        {
            //the code above is reversed here
            float distance = Vector3.Distance(transform.position, start);
            if (distance > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, start, speed * Time.deltaTime);
            }
            else
            {
                reached = false;
            }
        }
        */
        /*
        //code using MovePosition above to translate the position of the platform between two positions
        //player doesnt stick to the platform when it is moving so the player has to keep up with the platform
        if (!reached)
        {
            //finds distance between where the platform is to its end point
            float distance = Vector3.Distance(transform.position, end);
            //the platform will continue to move until it is really close to its
            //end point where it then changes direction
            if (distance > 0.1f)
            {
                movePlatform(transform.position, end, platformSpeed);
            }
            else
            {
                reached = true;
            }
        }
        else
        {
            //the code above is reversed here
            float distance = Vector3.Distance(transform.position, start);
            if (distance > 0.1f)
            {
                movePlatform(transform.position, start, platformSpeed);
            }
            else
            {
                reached = false;
            }
        }*/
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.parent = this.transform;
            //other.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.25f, this.transform.position.z);
            //other.transform.localScale = new Vector3(1, 1, 1);
            //Vector3 position = this.transform.position - other.transform.position;
            //player.transform.position = player.transform.position + position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.parent = null;
            //other.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.transform.tag == "Player")
    //    {
    //        other.transform.parent = this.transform;
    //        //other.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.25f, this.transform.position.z);
    //        //other.transform.localScale = new Vector3(1, 1, 1);
    //        //Vector3 position = this.transform.position - other.transform.position;
    //        //player.transform.position = player.transform.position + position;
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.transform.tag == "Player")
    //    {
    //        other.transform.parent = null;
    //        //other.transform.localScale = new Vector3(1, 1, 1);
    //    }
    //}
}
