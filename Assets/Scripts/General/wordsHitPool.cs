using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class wordsHitPool : MonoBehaviour 
{
    GameObject player;
    //All note related stuff done by Adam
    NotepadScript note;

    // Use this for initialization
    void Start () 
	{
        player = GameObject.FindGameObjectWithTag("Player");
        note = GameObject.FindGameObjectWithTag("Player").GetComponent<NotepadScript>();
    }
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnTriggerEnter (Collider col)
	{
        //check if there is a WordPoolWord component on the other object
        WordPoolWord obj = col.gameObject.GetComponent<WordPoolWord>();
        if (obj == null)
            return;
        //figure out what is going to happen with quit object
        if (obj.backToHub)
        {
            obj.gameObject.SetActive(false);
            SceneManager.LoadSceneAsync(0);
            return;
        }

        if (obj.PhaseTrigger != null)
            obj.PhaseTrigger.SetActive(true);
        if (obj.OppositeWordOff != null)
            obj.OppositeWordOff.SetActive(false);

        if (obj.changeMaterial)
        {
            SwappableMaterial[] allObj;
            allObj = Resources.FindObjectsOfTypeAll<SwappableMaterial>();
            foreach (SwappableMaterial swap in allObj)
                swap.ChangeMaterial(obj.newMaterial);
        }

        if (obj.replaceSentence)
            note.sentence = obj.sentence;
        else
            note.sentence += obj.sentence;

        col.gameObject.SetActive(false);

        if (player.GetComponent<PickupObject>().carriedObject != null)
            player.GetComponent<PickupObject>().DropObject();

        //for (int i = 0; i < phases.Length; i++)
        //{
        //    if (obj.triggerPhaseWord == phases[i].name)
        //    {
        //        phases[i].SetActive(true);
        //        note.sentence += obj.sentence;
        //    }
        //}

        //string onTag = obj.triggerPhaseWord; // string member
        //GameObject wordOn = GameObject.Find(onTag);
        //wordOn.SetActive(true);
        //note.sentence += obj.sentence; // string member of WordPoolWord

        //string offTag = obj.otherWordOff;
        //GameObject wordOff = GameObject.FindGameObjectWithTag(offTag);
        //wordOff.SetActive(false);

        //old wordpool script
        /*
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
        }*/
    }
}