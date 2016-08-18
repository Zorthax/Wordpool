using UnityEngine;
using System.Collections;

public class NotepadScript : MonoBehaviour {

    public Transform notepad;
    public Vector3 position = new Vector3(-0.4f, -0.2f, 0.5f);
    public float moveSpeed;
    public string sentence;
    public UnityEngine.UI.Text text;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
        /*Vector3 point = transform.position + new Vector3(position.z * transform.forward.x, position.y, position.z * transform.forward.z)
            + new Vector3(position.x * transform.right.x, 0, position.x * transform.right.z);// + (GetComponent<FirstPersonController>().CameraBobHeight() / 3);
        if (Vector3.Distance(notepad.position, point) > 0.1f)
        {
            notepad.position = Vector3.MoveTowards(notepad.position, point, 1.0f);
        }
        else
        {
            notepad.position = Vector3.MoveTowards(notepad.position, point, 0.01f);
        }*/
        if (Input.GetButton("Notepad"))
            notepad.localPosition = Vector3.MoveTowards(notepad.localPosition, position, moveSpeed);
        else notepad.localPosition = Vector3.MoveTowards(notepad.localPosition, new Vector3(position.x, -1, position.z), moveSpeed);
        notepad.localRotation = Quaternion.Euler(0, 180, 0);

        text.text = sentence;
	}
}
