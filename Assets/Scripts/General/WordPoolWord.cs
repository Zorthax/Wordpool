using UnityEngine;
using System.Collections;

public class WordPoolWord : MonoBehaviour {

    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;
    public bool backToHub = false;
    //public string triggerPhaseWord;
    public bool replaceSentence = false;
    public string sentence;
    //public string otherWordOff;
    public bool respawnAfterWordpool = false;
    public bool changeMaterial = false;
    public Material newMaterial;

    Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    public void Respawn()
    {
        transform.position = originalPosition;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
    }

}
