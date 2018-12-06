using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendTroops : MonoBehaviour {

    

    private Pool TCP;


    Transform ActiveTroopContainers;

    private void Awake()
    {
        TCP = GameObject.FindGameObjectWithTag("TroopContainerPool").GetComponent<Pool>();
    }
    public void main(HashSet<Base> sources, Base target, Team senderTeam)
    {
        foreach(Base b in sources)
        {
            //not allowed to empty a building fully
            if (b.myPopulation() > 1)
            {
                b.StartCoroutine(b.SendTroopsFromBaseToTargetBase(target, senderTeam));
            }
        }
    }
    

}
