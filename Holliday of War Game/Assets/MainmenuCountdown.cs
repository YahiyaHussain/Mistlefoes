using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainmenuCountdown : MonoBehaviour {

    backgroundAudio bA;
	// Use this for initialization
	void Start () {
        bA = GameObject.FindGameObjectWithTag("BackgroundAudio").GetComponent<backgroundAudio>();
        if (bA != null)
        {
            bA.playCorrectFanfar();
        }
        StartCoroutine(WaitThenMainMenu());
	}
	
	IEnumerator WaitThenMainMenu()
    {
        yield return new WaitForSeconds(5);
        Destroy(bA.gameObject);
        SceneManager.LoadScene(0);
    }
}
