using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBase : Base {

    Collider2D[] Attackables;
    Pool ProjectilePool;

    public float range;

    private void Start()
    {
        startClone();
        ProjectilePool = GameObject.FindGameObjectWithTag("ProjectilePool").GetComponent<Pool>();

        StartCoroutine(waitThenStrike());
    }


   
    IEnumerator waitThenStrike()
    {
        while (true)
        {
            Team before = myTeam();
            yield return new WaitUntil(() => Physics2D.OverlapCircle(transform.position, range, LayerMask.GetMask("Units")));
            Transform projectile = ProjectilePool.GiveOutUnit();
            projectile.transform.position = transform.position;

            Attackables = Physics2D.OverlapCircleAll(transform.position, 3, LayerMask.GetMask("Units"));

            Transform unitToAttack = null;
            foreach (Collider2D c in Attackables)
            {
                if (!c.gameObject.GetComponent<BasicUnit>().myTeam().Equals(myTeam()))
                {
                    unitToAttack = c.transform;
                    break;
                }
            }

            
            if (before == myTeam() && unitToAttack != null)
            {
                
                
                projectile.gameObject.SetActive(true);
                projectile.position = projectile.position + new Vector3(0, 1, 0);
                Transform unit = unitToAttack.transform;

                StartCoroutine(ShootTowardsEnemy(projectile, unit.position));
            }
            else
            {
                ProjectilePool.AcceptBackUnit(projectile);
            }
            yield return new WaitForSeconds(.8f);
        }
    }

    IEnumerator ShootTowardsEnemy(Transform prj, Vector3 Pos)
    {
        smush();
        //Debug.Log("got here");
        Vector3 unitPos = prj.transform.position;
        while ((Pos - unitPos).sqrMagnitude > .01f)
        {
            unitPos = prj.transform.position;
            prj.position = Vector3.MoveTowards(prj.position, Pos, Time.deltaTime * 8f);
            yield return null;
        }
        prj.localPosition = Vector3.zero;
        ProjectilePool.AcceptBackUnit(prj);
    }   


}
