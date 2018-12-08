using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopContainer : MonoBehaviour {
    //This is the container that hold all the units
    //It allows not only compact operations on the many tiny units,
    //but also allows groups of units to interact in interesting ways as
    //one, if so desired later

    private LinkedList<Transform> troopUnits;

    private Pool troopPool;
    private Pool TCP;
    private GameObject unitTargetBase;

    private bool dispersed;

    private int circlingCount;

    private float tpi = Mathf.PI*2;

    private bool faraway;

    public bool readyToRecycle;

    private Base targetBase;

    private Team currentTeam;
    private int troopCount;
    public Team myTeam()
    {
        return currentTeam;
    }

    private Sprite merry;
    private Sprite spooky;
    private Sprite[] troops;

    Queue<Transform> movingUnits;
    Queue<Transform> unitsToRemove;

    private void Awake()
    {
        troops = Resources.LoadAll<Sprite>("troops");
        merry = troops[0];
        spooky = troops[3];
        //to make code less manually interdependent I give things tags and at the start, the tags
        //are searchable so dependencies are automatically resolved
        troopPool = GameObject.FindGameObjectWithTag("TroopPool").GetComponent<Pool>();
        TCP = GameObject.FindGameObjectWithTag("TroopContainerPool").GetComponent<Pool>();
        //AddTroops(8);
        //unitBase = GameObject.FindGameObjectWithTag("Base");
        //test();
    }
    private void makeCorrectTeam(Transform unit)
    {
        if (currentTeam.Equals(Team.Merry))
        {
            unit.gameObject.GetComponent<Animator>().SetBool("Merry", true);
            //unit.gameObject.GetComponent<SpriteRenderer>().sprite = merry;
        }
        if (currentTeam.Equals(Team.Spooky))
        {
            unit.gameObject.GetComponent<Animator>().SetBool("Merry", false);
            //unit.gameObject.GetComponent<SpriteRenderer>().sprite = spooky;
        }
    }

    public void GoToTargetBase(GameObject target, int numberOfTroops, Team team)
    {
        currentTeam = team;
        targetBase = target.GetComponent<Base>();
        //units are about to be on the field, recycling them is not allowed
        readyToRecycle = false;

        //add troups to this troopContainer
        AddTroops(numberOfTroops);

        //set the target for the units
        unitTargetBase = target;

        //need a coroutine to allow waiting
        StartCoroutine(DisperseThenTargetThenRecyle(numberOfTroops));
    }

    IEnumerator DisperseThenTargetThenRecyle(int numberOfTroops)
    {
        //Ive decided distance 1.5 from base in a circle is a nice way to start
        //and not too much work, other solutions that were more natural
        //were a bit complicated looking
        DisperseTroops(1.5f);

        //this waits til every last unit has reached their place in the starting circle
        //before continuing onto next operation
        yield return new WaitUntil(() => circlingCount == transform.childCount);

        //Time to start heading toward the target base
        StartCoroutine(headTowardsBase(unitTargetBase.transform)); ;

        //this waits til every last unit is really close to the target base
        //before continuing

        //after the units reach the other base, I set this public bool to true
        //SendTroops is waiting for the units to be ready to be recycled so
        //they can be put back into the pool neatly and discretely
        readyToRecycle = true;
    }

    public void AddTroops(int numberOfTroops)
    {
        //for loop to make sure get all desired numbers of Units
        for (int i = 0; i < numberOfTroops; i++)
        {

            //TroopPool keeps track of units that can be given
            //and has functions to dispense them and take them back
            Transform unit = troopPool.GiveOutUnit();

            /*
            if (currentTeam.Equals(Team.Spooky))
            {
                unit.gameObject.layer = LayerMask.NameToLayer("SpookyUnits");
            }
            else if (currentTeam.Equals(Team.Merry))
            { 
                unit.gameObject.layer = LayerMask.NameToLayer("MerryUnits");
            }
            */
            //set the unit as a child of this troopcontainer
            unit.SetParent(transform, false);

            //just for security purposes make sure the unit's starting position
            //is in fact exactly the troopcontainer's, another class assures the troopcontainer
            //has the same location as the starting base
            unit.localPosition = Vector3.zero;

            //alright everything is in order, the unit can come alive now
            unit.gameObject.SetActive(true);
            makeCorrectTeam(unit);
        }
    }

    //move units to a circle around starting base
    public void DisperseTroops(float radiusMax)
    {
        // start counting number of units who are successfully moved in the coroutine to be called
        circlingCount = 0;
        int i = 0;
        int total = transform.childCount;
        Vector3 goalLocation;

        //evenly spread units around circle, the angle will be fractions of 2pi
        //the actual moving will be done in the coroutine thats called though
       foreach(Transform unit in transform)
        {
            goalLocation = unit.transform.position + new Vector3(Mathf.Cos(tpi * i / total) * radiusMax, Mathf.Sin(tpi * i / total) * radiusMax);
            StartCoroutine(MoveUnit(unit, goalLocation));
            //Debug.Log(new Vector3(Mathf.Cos(tpi * (i / total)) * radiusMax, Mathf.Sin(tpi * (i / total))));
            i++;
        }
    }

    IEnumerator MoveUnit(Transform unit, Vector3 goalLocation)
    {
        //while a unit is not close to goalLocation keep moving it towards it at a constant rate
        while((unit.position - goalLocation).sqrMagnitude > 0.01f){
            unit.position = Vector3.MoveTowards(unit.position, goalLocation,  Time.deltaTime * 5f);
            yield return null;
        }
        //when it gets there, increment circlingCount so DisperseThenTargetThenRecyle() will know to continue on
        circlingCount++;

    }

    //get to the base

    private void bringSingleUnitCloserToLocation(Transform unit, Vector3 goalLocation)
    {
        if (unit.gameObject.activeInHierarchy)
        {
            if ((goalLocation - unit.position).sqrMagnitude > .01f)
            {
                unit.position = Vector3.MoveTowards(unit.position, goalLocation, Time.deltaTime * 7f);
            }
            else
            {
                unitsToRemove.Enqueue(unit);
                targetBase.affectPopulation(new popOp(1, currentTeam, popOpType.InvadingForce));
            }
        }
        else
        {
            unitsToRemove.Enqueue(unit);
        }
    }

    IEnumerator headTowardsBase(Transform targetBaseTransform)
    {
        //establish that no unit is close for DisperseThenTargetThenRecyle() to wait
        faraway = true;
        Vector3 targetPos = targetBaseTransform.position;
        movingUnits = new Queue<Transform>();
        unitsToRemove = new Queue<Transform>();
        //convenience
        foreach (Transform unit in transform)
        {
            if (targetPos.x < 0)
            {
                unit.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                unit.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        while (transform.childCount > 0)
        {
            foreach (Transform t in transform)
            {
                movingUnits.Enqueue(t);
            }
            foreach(Transform t in movingUnits)
            {
                bringSingleUnitCloserToLocation(t, targetPos);
            }
            for(int i = 0; i < unitsToRemove.Count; i++)
            {
                troopPool.AcceptBackUnit(unitsToRemove.Dequeue());
            }
            movingUnits.Clear();
            yield return null;
        }
        TCP.AcceptBackUnit(transform);
    }

}
