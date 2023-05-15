using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AI : MonoBehaviour
{
    public bool roaming = true;
    public float movesp;
    bool reachdestination = false;
    public bool updatePath;
    public float nextwpdis;
    public Seeker seeker;
    public SpriteRenderer renderer;
    Path path;
    Coroutine moveCoroutine;
    //fire
    public bool isShoot = false;
    public GameObject bullet;
    public float bulletspeed;
    public float shootdelay;
    private float firecooldown;
    private void Start()
    {
        InvokeRepeating("CalculatePath",0f,0.5f);
        reachdestination = true;
        
    }
    private void Update()
    {
        firecooldown -= Time.deltaTime;
        if (firecooldown  < 0 && isShoot ==true)
        {
            firecooldown = shootdelay;
            enemyFire();
        }
    }
    void enemyFire()
    {
        var bulletTmp =Instantiate(bullet,transform.position,Quaternion.identity);
        Rigidbody2D rb = bulletTmp.GetComponent<Rigidbody2D>();
        Vector3 playerpos =FindObjectOfType<PlayerController>().transform.position;
        Vector3 direction = playerpos - transform.position;
        rb.AddForce(direction.normalized * bulletspeed, ForceMode2D.Impulse);
        Destroy(bulletTmp, 1f);
    }
    void CalculatePath()
    {
        Vector2 target = Findtarget();
        if (seeker.IsDone()&& (reachdestination|| updatePath))
        {
            seeker.StartPath(transform.position, target,OnPathComplete);
        }
    }
    void OnPathComplete(Path p)
    {
        if (p.error) return;
        path = p;
        MovetoTarget();
    }
    void MovetoTarget()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MovetoTargetCoroutine());
    }
    IEnumerator MovetoTargetCoroutine()
    {
        int currentWP = 0;
        reachdestination = false;
        while (currentWP<path.vectorPath.Count)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWP]-(Vector2)transform.position).normalized;
            Vector2 force =direction*movesp*Time.deltaTime;
            transform.position += (Vector3) force;
            float distance =Vector2.Distance(transform.position, path.vectorPath[currentWP]);
            if (distance <nextwpdis) currentWP++;
            if (force.x != 0)
            {
                if (force.x < 0)
                {
                    renderer.transform.localScale = new Vector3(1, 1, 0);
                }
                else
                {
                    renderer.transform.localScale = new Vector3(-1, 1, 0);
                }
            }
            yield return null;
        }
        reachdestination = true;
    }
    Vector2 Findtarget()
    {
        Vector3 playerpos = FindObjectOfType<PlayerController>().transform.position;
        if (roaming==true)
        {
            return (Vector2)playerpos + (Random.Range(5f, 5f) * new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized);
        }
        else
        {
            return playerpos;
        }
    }
}
