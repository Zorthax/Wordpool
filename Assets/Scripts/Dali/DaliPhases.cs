using UnityEngine;
using System.Collections;

public class DaliPhases : MonoBehaviour {

    //public GameObject rosesPhase2;
    //public GameObject rosesPhase3Butter;
    //public GameObject rosesPhase3Faces;
    //public GameObject clocksPhase2;
    //public GameObject clocksPhase3Butter;
    //public GameObject clocksPhase3Faces;
    //public GameObject finalFormTrees;
    //public GameObject finalFormScissors;

    public GameObject[] changingObjects;

    // i am a stinky pants

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public bool[] SavePhases()
    {
        bool[] phases;

        phases = new bool[changingObjects.Length];
        for (int i = 0; i < changingObjects.Length; i++)
        {
            phases[i] = changingObjects[i].activeSelf;
        }

        return phases;
    }

    public void LoadPhases(bool[,] phases, int index)
    {
        for (int i = 0; i < changingObjects.Length; i++)
        {
            changingObjects[i].SetActive(phases[index, i]);
        }
    }
}
