using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployBomb : MonoBehaviour {
    Queue<Transform> Bombs;
    GameObject crosshair;
    Camera camera;
    SpriteRenderer SR;
    Team playerTeam;
    AudioManager AM;

    public float bombRange;
    // Use this for initialization
    void Start() {
        AM = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        Bombs = new Queue<Transform>();
        foreach (Transform t in transform)
        {
            Bombs.Enqueue(t);
        }


        playerTeam = GameObject.FindGameObjectWithTag("PlayerSelection").GetComponent<PlayerSelection>().myTeam();
        crosshair = GameObject.FindGameObjectWithTag("Crosshair");
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        SR = crosshair.GetComponent<SpriteRenderer>();
    }
    public void playerSend()
    {
        SR.enabled = true;
        StartCoroutine(waitForSelectionThenBombAndReset(playerTeam));
    }

    IEnumerator waitForSelectionThenBombAndReset(Team senderTeam)
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        AM.PlaySound("Falling");
        sendBomb(camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2)), senderTeam); 
        SR.enabled = false;
    }

    public void sendBomb(Vector3 Pos, Team senderTeam)
    {
        Transform bomb = Bombs.Dequeue();
        bomb.transform.position = new Vector3(Pos.x, transform.position.y);

        bomb.gameObject.SetActive(true);
        if (senderTeam.Equals(Team.Merry))
        {
            bomb.GetComponent<Animator>().SetBool("IsMerry", true);
        }
        else
        {
            bomb.GetComponent<Animator>().SetBool("IsMerry", false);
        }
        StartCoroutine(moveToTarget(Pos, bomb, senderTeam));
    }

    IEnumerator moveToTarget(Vector3 Pos, Transform bomb, Team senderTeam)
    {
        while ((Pos - bomb.transform.position).sqrMagnitude > 0.1f)
        {
            bomb.transform.position = Vector3.MoveTowards(bomb.transform.position, Pos, 16 * Time.deltaTime);
            yield return null;
        }
        AM.StopSound("Falling");
        yield return new WaitForSeconds(0.1f);
        AM.PlaySound("Boom");
        bomb.GetComponent<Animator>().SetBool("TimeToExplode", true);
        killEnemiesAndHurtBuildings(senderTeam, Pos);
        yield return new WaitForSeconds(1);

        bomb.gameObject.SetActive(false);
        bomb.localPosition = Vector3.zero;
        Bombs.Enqueue(bomb);
    }

    private void killEnemiesAndHurtBuildings(Team senderTeam, Vector3 Pos)
    {
        Collider2D[] colliders;

        //GetComponent<BoxCollider2D>().OverlapCollider((new ContactFilter2D().NoFilter()), colliders);

        colliders = Physics2D.OverlapBoxAll(Pos, new Vector2(bombRange, bombRange), 90);
        foreach (Collider2D c in colliders)
        {
            Debug.Log(c.name);
            if (c != null)
            {
                if (c.tag == "BasicUnit")
                {
                    c.gameObject.SetActive(false);
                }
                else if (c.tag == "Base")
                {
                    c.gameObject.GetComponent<Base>().affectPopulation(new popOp(15, senderTeam, popOpType.BombstrikeOnBase));
                    c.gameObject.GetComponent<Base>().bounce();
                }
            }
        }
    }
}
