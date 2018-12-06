using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour {

    Queue<Transform> ObjectQueue;
    
    public float PercentAvailable;
    private float totalObjects;

    private void Awake()
    {
        ObjectQueue = new Queue<Transform>();
        foreach (Transform unit in transform)
        {
            ObjectQueue.Enqueue(unit);
        }
        totalObjects = ObjectQueue.Count;
        if (ObjectQueue.Count == 0)
        {
            Debug.Log("Error " + name + " has 0 units");
        }
    }
    private void Update()
    {
        PercentAvailable = ObjectQueue.Count*100 / totalObjects;
    }
    public Transform GiveOutUnit()
    {
        if (ObjectQueue.Count == 0)
        {
            Debug.Log("Error " + name + " Out of Units");
            return null;
        }
        return ObjectQueue.Dequeue();
    }
    public void AcceptBackUnit(Transform t)
    {

        if (!ObjectQueue.Contains(t))
        {
            t.gameObject.SetActive(false);
            t.SetParent(transform, false);
            ObjectQueue.Enqueue(t);
        }
    }
}
