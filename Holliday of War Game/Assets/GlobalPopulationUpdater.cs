using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPopulationUpdater : MonoBehaviour {

    LinkedList<Transform> allBases;
    LinkedList<Transform> normalBases; //population growers
    LinkedList<Transform> specialBases; // not population growing bases

    private GameObject BasePool;
	// Use this for initialization
	void Start () {
        allBases = new LinkedList<Transform>();
        specialBases = new LinkedList<Transform>();
        normalBases = new LinkedList<Transform>();
        BasePool = GameObject.FindGameObjectWithTag("BasePool");
        initializeBases();
        StartCoroutine(UpdatePopulation());
	}
	
    private void initializeBases()
    {
        foreach (Transform t in BasePool.transform)
        {
            allBases.AddFirst(t);
        }
        foreach (Transform t in BasePool.transform)
        {

            if (t.GetComponent<ArcherBase>() != null)
            {
                specialBases.AddFirst(t);
            }
            else if (t.GetComponent<MineBase>() != null)
            {
                specialBases.AddFirst(t);
            }
            else if (t.GetComponent<Base>() != null)
            {
                normalBases.AddFirst(t);
            }
        }
        if (allBases.Count == 0)
        {
            Debug.Log("ERROR NO BASES");
        }
    }


    IEnumerator UpdatePopulation()
    {
        while (true)
        {
            Base b;
            foreach (Transform t in allBases)
            {
                b = t.GetComponent<Base>();
                if (b.myPopulation() < b.myMaxPopulation())
                    if (!b.myTeam().Equals(Team.Neutral))
                    {
                        b.affectPopulation(new popOp(1, b.myTeam(), popOpType.NewSoldierBorn));
                    }
            }
            foreach (Transform t in allBases)
            {
                b = t.GetComponent<Base>();
                if (b.myPopulation() > b.myMaxPopulation())
                    if (!b.myTeam().Equals(Team.Neutral))
                    {
                        b.affectPopulation(new popOp(1, b.myTeam(), popOpType.SoldierStarvedToDeath));
                    }
            }
            yield return new WaitForSeconds(1);
        }
    }
}
