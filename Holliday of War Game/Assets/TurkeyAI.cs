using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurkeyAI : MonoBehaviour {


    Queue<Transform> bullets;
    GameObject BasePool;
    bool Walking;
    public float ShootRange;

	// Use this for initialization
	void Start () {
        bullets = new Queue<Transform>();
        foreach (Transform t in transform)
        {
            bullets.Enqueue(t);
        }

        BasePool = GameObject.FindGameObjectWithTag("BasePool");

        StartCoroutine(WalkToRandomBaseRandomly());
        StartCoroutine(WaitThenShoot());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator WalkToRandomBaseRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3, 20));

            float angle = Random.Range(0, 2 * Mathf.PI);
            Vector3 randomOffsetLocation = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));

            StartCoroutine(WalkToBase(BasePool.transform.GetChild(Random.Range(0, BasePool.transform.childCount - 1)).position + randomOffsetLocation));
            yield return new WaitUntil(() => !Walking);
        }
    }

    IEnumerator WalkToBase(Vector3 Location)
    {
        Walking = true;

        while ((transform.position - Location).sqrMagnitude > 0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position, Location, 1 * Time.deltaTime);
            yield return null;
        }

        Walking = false;
    }
    IEnumerator WaitThenShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(.4f);
            Collider2D c = Physics2D.OverlapCircle(transform.position, ShootRange, LayerMask.GetMask("Units"));
            if (c != null)
            {
                HeatSeekShoot(c.transform);
            }
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
        //if(other.tag == "Units")
           // HeatSeekShoot(other.transform);
    //}
    public void HeatSeekShoot(Transform target)
    {
        Transform t = bullets.Dequeue();
        t.gameObject.SetActive(true);
        StartCoroutine(JOJOSexPistols(t, target));
    }

    IEnumerator JOJOSexPistols(Transform numberThreeBullet, Transform target)
    {
        while(target.gameObject.activeInHierarchy && (numberThreeBullet.position - target.position).sqrMagnitude > 0.01)
        {
            numberThreeBullet.position = Vector3.MoveTowards(numberThreeBullet.position, target.position, 7 * Time.deltaTime);
            yield return null;
        }
        target.gameObject.SetActive(false);
        numberThreeBullet.gameObject.SetActive(false);
        numberThreeBullet.localPosition = Vector3.zero;
        bullets.Enqueue(numberThreeBullet);
    }
}
