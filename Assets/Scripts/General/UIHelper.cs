using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHelper : MonoBehaviour {

    //change onGUI to start using Canvas instead
    GameObject cameraPosition;
    public float jumpDelayReset;
    public float moveDelayReset;
    public float pickupDisplayDelay;
    public float pickupDelayReset;
    public float sprintDelayReset;
    int testMask = 1 << 8;
    Image testImage;

    public class HUDTimer
    {
        public float delay;
        public float reset;

        public GameObject obj;

        public HUDTimer(GameObject o, float r)
        {
            obj = o;
            reset = r;
            delay = reset;
        }

        public void Update(bool condition)
        {
            //jump display
            if (condition)
            {
                delay = reset;
            }
            else
            {
                delay -= Time.deltaTime;
            }

            if (delay <= 0)
            {
                UIHelper.FadeIn(obj);
            }
            else
            {
                UIHelper.FadeOut(obj);
            }

        }
    }

    HUDTimer jumpTimer;
    HUDTimer moveTimer;
    HUDTimer sprintTimer;

    // Use this for initialization
    void Start () {
        cameraPosition = GameObject.FindGameObjectWithTag("MainCamera");

        jumpTimer = new HUDTimer(GameObject.Find("Canvas/UICanvas/Jump"), jumpDelayReset);
        moveTimer = new HUDTimer(GameObject.Find("Canvas/UICanvas/Move"), moveDelayReset);
        sprintTimer = new HUDTimer(GameObject.Find("Canvas/UICanvas/Sprint"), sprintDelayReset);
    }
	
	// Update is called once per frame
	void Update () {

        //pickup display
        if (Physics.Raycast(cameraPosition.transform.position, cameraPosition.transform.forward, Mathf.Infinity, testMask) && GetComponent<PickupObject>().carriedObject == null)
        {
            pickupDisplayDelay -= Time.deltaTime;
        }
        else
        {
            pickupDisplayDelay = pickupDelayReset;
        }

        if (pickupDisplayDelay <= 0)
        {
            //find a way to use tags and layers to trigger the raycast on the objective
            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(cameraPosition.transform.position, cameraPosition.transform.forward, out hitInfo, Mathf.Infinity, testMask))
            {
                FadeIn(GameObject.Find("Canvas/UICanvas/Pickup"));//PickupUi
            }
        }
        else
        {
            FadeOut(GameObject.Find("Canvas/UICanvas/Pickup"));
        }

        //jump display
        jumpTimer.Update(Input.GetKey(KeyCode.Space));
        /*if (Input.GetKey(KeyCode.Space))
        {
            jumpDisplayDelay = jumpDelayReset;
        }
        else
        {
            jumpDisplayDelay -= Time.deltaTime;
        }

        if (jumpDisplayDelay <= 0)
        {
            FadeIn(GameObject.Find("UICanvas/Jump"));
        }
        else
        {
            FadeOut(GameObject.Find("UICanvas/Jump"));
        }*/

        //move display
        moveTimer.Update(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S));
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
        //{
        //    moveDisplayDelay = moveDelayReset;
        //}
        //else
        //{
        //    moveDisplayDelay -= Time.deltaTime;
        //}

        //if (moveDisplayDelay <= 0)
        //{
        //    FadeIn(GameObject.Find("UICanvas/Move"));
        //}
        //else
        //{
        //    FadeOut(GameObject.Find("UICanvas/Move"));
        //}

        //camera(?) display

        //sprint display
        sprintTimer.Update(Input.GetKey(KeyCode.LeftShift));
        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    sprintDisplayDelay = sprintDelayReset;
        //}
        //else
        //{
        //    sprintDisplayDelay -= Time.deltaTime;
        //}

        //if (sprintDisplayDelay <= 0)
        //{
        //    FadeIn(GameObject.Find("UICanvas/Sprint"));
        //}
        //else
        //{
        //    FadeOut(GameObject.Find("UICanvas/Sprint"));
        //}
    }

    static void FadeOut(GameObject uiElement)
    {
        if (uiElement.GetComponentInChildren<RawImage>().color.a >= 0 && uiElement.GetComponentInChildren<Text>().color.a >= 0)
        {
            Color boxColor = uiElement.GetComponentInChildren<RawImage>().color;
            Color imageColor = uiElement.GetComponentInChildren<Image>().color;
            Color textColor = uiElement.GetComponentInChildren<Text>().color;
            boxColor.a -= 0.007f;
            imageColor.a -= 0.007f;
            textColor.a -= 0.007f;
            uiElement.GetComponentInChildren<RawImage>().color = boxColor;
            uiElement.GetComponentInChildren<Image>().color = imageColor;
            uiElement.GetComponentInChildren<Text>().color = textColor;
        }
    }
    static void FadeIn(GameObject uiElement)
    {
        if (uiElement.GetComponentInChildren<RawImage>().color.a <= 1 && uiElement.GetComponentInChildren<Text>().color.a <= 1)
        {
            Color boxColor = uiElement.GetComponentInChildren<RawImage>().color;
            Color imageColor = uiElement.GetComponentInChildren<Image>().color;
            Color textColor = uiElement.GetComponentInChildren<Text>().color;
            boxColor.a += 0.007f;
            imageColor.a += 0.007f;
            textColor.a += 0.007f;
            uiElement.GetComponentInChildren<RawImage>().color = boxColor;
            uiElement.GetComponentInChildren<Image>().color = imageColor;
            uiElement.GetComponentInChildren<Text>().color = textColor;
        }
    }

    void OnGUI()
    {
        //make the input disappear when object is picked up
        //add more GUI instructions for other buttons over larger periods of time
    }
}
