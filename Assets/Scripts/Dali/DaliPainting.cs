using UnityEngine;
using System.Collections;

public class DaliPainting : MonoBehaviour {

    public float distance = 3;

    Collider col;

	// Use this for initialization
	void Start ()
    {
        col = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Pick Up"))
        {
            RaycastHit[] all = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, distance);

            foreach (RaycastHit h in all)
            {
                if (h.transform == transform)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(1);
                }
            }
        }
	}
}
