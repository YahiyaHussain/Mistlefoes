using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour {

    Transform BasePool;
    List<Base> bases;
    HashSet<Base> selectedBases;

    Base target;
    [HideInInspector]
    public Team currenteam;

    SendTroops TroopSender;

    //private backgroundAudio bA;
    private AudioManager AM;

	// Use this for initialization
    public Team myTeam()
    {
        return currenteam;
    }
	void Start () {
        AM = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        TroopSender = GameObject.FindGameObjectWithTag("TroopSender").GetComponent<SendTroops>();
        //myTeam = Team.Merry;
        BasePool = GameObject.FindGameObjectWithTag("BasePool").transform;
        bases = new List<Base>();
        selectedBases = new HashSet<Base>();
  
        foreach(Transform place in BasePool){
            bases.Add(place.GetComponent<Base>());
        }
        //bA = GameObject.FindGameObjectWithTag("BackgroundAudio").GetComponent<backgroundAudio>();
	}

    private IEnumerator bounceBaseToBeat(Base b)
    {
        while (true)
        {
            int t = AM.currentBeat;
            yield return new WaitUntil(() => AM.currentBeat == (t + 1) % AM.bps);
            b.bounce();
        }
    }

    private void Update()
    {
        //hey its simple to implement and less straining on the players fingers like
        //why not only depend on the mouse being clicked on and then seeing what is moused over
        if (Input.GetMouseButton(0))
        {
            foreach(Base b in bases)
            {
                if (b.HasMouseOverIt())
                {
                    if (!b.isHighlighted())
                    {
                        //you cant enlist units from an enemy team
                        if (b.myTeam().Equals(currenteam))
                        {
                            //add to selected bases and highlight for player feedback
                            selectedBases.Add(b);
                            b.highlight();
                            StartCoroutine(bounceBaseToBeat(b));
                        }
                        else
                        {
                            if(target != null)
                            {
                                target.unhighlight();
                            }
                            target = b;
                            target.highlight();
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            StopAllCoroutines();
            LinkedList<Base> toRemove = new LinkedList<Base>();
            foreach (Base b in selectedBases)
            {
                if (!b.myTeam().Equals(currenteam))
                {
                    b.unhighlight();
                    toRemove.AddFirst(b);
                }
            }
            foreach (Base b in toRemove)
            {
                selectedBases.Remove(b);
            }

            foreach (Base b in selectedBases)
            {
                b.unhighlight();
            }
            if (target != null)
                target.unhighlight();

            //when locking in choice with deselect lets make sure player
            //is mousing over a base, otherwise do nothing
            target = null;
            foreach(Base b in bases)
            {
                if (b.HasMouseOverIt())
                {
                    target = b;
                }
            }
            //Debug.Log(target);
            //target isn't a place for units to come from
            selectedBases.Remove(target);
            
            //pretty easy neat way to deselect, just dont mouse over anything when let go
            if (target != null && selectedBases.Count != 0)
                //troop sender does a lot to tie things together
                TroopSender.main(selectedBases, target, currenteam);
            //alright clear for next time
            selectedBases.Clear();
        }
    }
}
