using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelection : MonoBehaviour {
    private EnemyAI E;
    private PlayerSelection P;
    //public Team PlayerTeam;
    private MenuManager M;

    // Use this for initialization
    void Awake () {
        M = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>();
        E = GameObject.FindGameObjectWithTag("EnemyAI").GetComponent<EnemyAI>();
        P = GameObject.FindGameObjectWithTag("PlayerSelection").GetComponent<PlayerSelection>();


        if (M.selectedTeam().Equals(Team.Merry))
        {
            P.currenteam = Team.Merry;
            E.myTeam = Team.Spooky;
        }
        else
        {
            P.currenteam = Team.Spooky;
            E.myTeam = Team.Merry;
        }
        E.algToUse = M.chosenEnemyAI();
	}

}
