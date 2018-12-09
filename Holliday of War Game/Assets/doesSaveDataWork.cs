using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doesSaveDataWork : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (saveData.isLevelCompleted(1))
        {
            Debug.Log("You did the first level before!");
        }
	}
	
}
