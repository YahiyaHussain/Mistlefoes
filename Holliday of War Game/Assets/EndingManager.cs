using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour {

    PlayerSelection PS;
    AudioManager AM;
	// Use this for initialization
	void Start () {
        PS = GameObject.FindGameObjectWithTag("PlayerSelection").GetComponent<PlayerSelection>();
        AM = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
	}
	public void EndGame(bool PlayerWon)
    {
        AM.stopAnyMusic();
        StartCoroutine(revealTextWaitThenQuitToMenu());
        if (PlayerWon)
        {
            GetComponent<TextMeshProUGUI>().text = "You Win!";
            if (PS.myTeam().Equals(Team.Spooky))
            {
                AM.PlaySound("Hallowin");
            }
            else
            {
                AM.PlaySound("ChristmasWin");
            }
            saveData.recordLevelComplete(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            GetComponent<TextMeshProUGUI>().text = "You Lose!";
            if (PS.myTeam().Equals(Team.Merry))
            {
                AM.PlaySound("ChristmasLose");
            }
            else
            {
                AM.PlaySound("Hallolose");
            }
        }

    }
    IEnumerator revealTextWaitThenQuitToMenu()
    {
        for (int i = 0; i < 20; i++)
        {
            transform.localScale = ((i / 20.0f) * Vector3.one);
            yield return null;
        }
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(0);
    }
	
}
