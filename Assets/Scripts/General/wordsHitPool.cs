using UnityEngine;
using System.Collections;

public class wordsHitPool : MonoBehaviour 
{
	public GameObject roses;
	public GameObject clocks;
	public GameObject faces;
	public GameObject butter;
    GameObject player;
    public GameObject defaultRosePosition;
    public GameObject defaultClockPosition;
    public GameObject defaultFacePosition;
    public GameObject defaultButterPosition;
    public GameObject defaultResetPosition;
    GameObject roseWord;
    GameObject clockWord;
    GameObject faceWord;
    GameObject butterWord;

    //All note related stuff done by Adam
    NotepadScript note;


    // Use this for initialization
    void Start () 
	{
        roseWord = GameObject.FindGameObjectWithTag("roseword");
        clockWord = GameObject.FindGameObjectWithTag("clockword");
        faceWord = GameObject.FindGameObjectWithTag("faceword");
        butterWord = GameObject.FindGameObjectWithTag("butterword");
        player = GameObject.FindGameObjectWithTag("Player");

        note = GameObject.FindGameObjectWithTag("Player").GetComponent<NotepadScript>();
    }
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.tag == "roseword") {
			Debug.Log ("Roses Active");
			roses.SetActive (true);
            note.sentence += " are roses. Around me";
            //col.transform.position = defaultRosePosition.transform.position;
            col.transform.position = GameObject.FindGameObjectWithTag("wordTeleport").transform.position;
            if (player.GetComponent<PickupObject>().carriedObject != null)
                player.GetComponent<PickupObject>().DropObject();
            clockWord.SetActive(false);
			//GameObject clockword = GameObject.FindWithTag ("clockword");
			//Destroy (obj: clockword);
		} else if (col.tag == "clockword") {
			Debug.Log ("Clocks Active");
			clocks.SetActive (true);
            note.sentence += " are clocks. Around me";
            col.transform.position = GameObject.FindGameObjectWithTag("wordTeleport").transform.position;
            if (player.GetComponent<PickupObject>().carriedObject != null)
                player.GetComponent<PickupObject>().DropObject();
            roseWord.SetActive(false);
			//GameObject clockword = GameObject.FindWithTag ("roseword");
			//Destroy (obj: clockword);
		} else if (col.tag == "butterword") {
			Debug.Log ("Butterflies Active");
			butter.SetActive (true);
            note.sentence += " are butterflies.";
            col.transform.position = GameObject.FindGameObjectWithTag("wordTeleport").transform.position;
            if (player.GetComponent<PickupObject>().carriedObject != null)
                player.GetComponent<PickupObject>().DropObject();
            faceWord.SetActive(false);
            //GameObject clockword = GameObject.FindWithTag ("faceword");
			//Destroy (obj: clockword);
		} else if (col.tag == "faceword") {
			Debug.Log ("Faces Active");
			faces.SetActive (true);
            note.sentence += " are faces.";
            col.transform.position = GameObject.FindGameObjectWithTag("wordTeleport").transform.position;
            if (player.GetComponent<PickupObject>().carriedObject != null)
                player.GetComponent<PickupObject>().DropObject();
            butterWord.SetActive(false);
            //GameObject clockword = GameObject.FindWithTag ("butterword");
			//Destroy (obj: clockword);
        }
        //WIP reset function
        //else if (col.tag == "reset")
        //{
        //    //change pool to set the active elements in scene to what the level was originally
        //    Debug.Log("Reset");
        //    roseWord.transform.position = defaultRosePosition.transform.position;
        //    clockWord.transform.position = defaultClockPosition.transform.position;
        //    if (roses.activeInHierarchy == true || clocks.activeInHierarchy == true)
        //    {
        //        faceWord.transform.position = defaultFacePosition.transform.position;
        //        butterWord.transform.position = defaultButterPosition.transform.position;
        //    }
        //    col.transform.position = defaultResetPosition.transform.position;
        //    butter.SetActive(false);
        //    faces.SetActive(false);
        //    roses.SetActive(false);
        //    clocks.SetActive(false);
        //}
        else if (col.tag == "quit")
        {
            //---ADAM TO BRYAN---//
            //---The following two lines makes the game quit if the player touches the wordpool
            col.gameObject.SetActive(false);
            Application.Quit();
        }
    }
}