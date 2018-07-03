using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RetunHomeTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    GetComponent<Button>().onClick.AddListener(KioTools.ResetReturnHomeClockStatic);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ReturnHome()
    {
        Debug.Log("Received return home Msg.");
    }
}
