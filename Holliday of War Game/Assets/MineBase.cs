using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBase : Base {

    HolidaySpiritBar spiritBar;

    Team playerTeam;
    private void Start()
    {
        playerTeam = GameObject.FindGameObjectWithTag("PlayerSelection").GetComponent<PlayerSelection>().myTeam();
        startClone();
        spiritBar = GameObject.FindGameObjectWithTag("SpiritBar").GetComponent<HolidaySpiritBar>();
        StartCoroutine(waitThenGiveSupplies());
    }

    IEnumerator waitThenGiveSupplies()
    {
        while (true)
        {
            if (myTeam() == playerTeam)
            {
                if (myPopulation() > 1)
                {
                    spiritBar.addSupplies(myPopulation());
                }
            }
            yield return new WaitForSeconds(0.4f);
        }
    }
}
