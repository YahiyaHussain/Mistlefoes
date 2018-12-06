using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelection : MonoBehaviour {
    private EnemyAI E;
    private PlayerSelection P;
    public Team PlayerTeam;

	// Use this for initialization
	void Awake () {
        E = GameObject.FindGameObjectWithTag("EnemyAI").GetComponent<EnemyAI>();
        P = GameObject.FindGameObjectWithTag("PlayerSelection").GetComponent<PlayerSelection>();

        if (PlayerTeam.Equals(Team.Merry))
        {
            P.currenteam = Team.Merry;
            E.myTeam = Team.Spooky;
        }
        else
        {
            P.currenteam = Team.Spooky;
            E.myTeam = Team.Merry;
        }
	}

}
