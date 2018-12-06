using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUnit : MonoBehaviour {

    private Team currentTeam;



    private void OnEnable()
    {
        currentTeam = GetComponentInParent<TroopContainer>().myTeam();
    }

    public Team myTeam()
    {
        return currentTeam;
    }
    

    public Transform returnParent()
    {
        return transform.parent;
    }



}
