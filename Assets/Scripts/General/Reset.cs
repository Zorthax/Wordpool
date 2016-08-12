using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour {

    NotepadScript note;

	// Use this for initialization
	void Start () {
        note = GameObject.FindGameObjectWithTag("Player").GetComponent<NotepadScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider col)
	{
        if (col.tag == "reset")
        {
            Debug.Log("reset");
            col.gameObject.SetActive(false);
            SceneManager.LoadScene("Dali_Preproduction_001");
            note.sentence = "In the sky";
        }
    }
}
