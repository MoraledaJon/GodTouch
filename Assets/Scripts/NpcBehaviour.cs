using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    IDLE,
    WALKING
}
public class NpcBehaviour : MonoBehaviour
{ 
    public float speed;
    public State state;
    private Animator anim;
    [Range(1f, 100f)]
    public float distanceWalk;
    [Range(1f, 10f)]
    public float TimeIdle;
    private Vector3 postowalkto;
    private float timerIdle;
    public bool isbeingDragged = false;
    private bool dead = false;

    void Start()
    {
        timerIdle = 0;
        state = State.IDLE;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
       
        if(state == State.IDLE)
        {
            timerIdle += Time.deltaTime;
            if(timerIdle > TimeIdle)
            {
                timerIdle = 0;
                postowalkto = GetRandomPosition();
                StartCoroutine(WalkTo());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case ("Sea"):
                if(!isbeingDragged && !dead)
                StartCoroutine(Die());
                break;
        }
    }

    IEnumerator  Die()
    {
        dead = true;
        anim.SetTrigger("death");
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }

    #region WalkAI

    private bool CanWalkTo(Vector3 desiderPos)
    {
        float radius = 1f;
        bool NotOccupied = true;
        Collider[]hitColliders = Physics.OverlapSphere(new Vector3(desiderPos.x,5, desiderPos.z), radius);
        foreach(Collider coll in hitColliders)
        {
            switch(coll.gameObject.tag)
            {
                case ("Sea"):
                    bool isThereTerrain = false;
                    foreach (Collider coll2 in hitColliders)
                    {
                        if (coll2.gameObject.tag == "Terrain")
                        {
                            isThereTerrain = true;
                        }
                    }
                    if (!isThereTerrain)
                        NotOccupied = false;
                    break;
                case ("Obstacle"):
                    NotOccupied = false;
                    break;
            }
        }


        return NotOccupied;

    }

    Vector3  GetRandomPosition()
    {
        Vector3 randPos = new Vector3
            (transform.position.x + Random.Range(-distanceWalk,distanceWalk),
            transform.position.y,
            transform.position.z + Random.Range(-distanceWalk,distanceWalk));

        return randPos;
    }
    private IEnumerator WalkTo()
    {
        bool doneWalking = false;
        state = State.WALKING;
        while (!doneWalking)
        {
            if (CanWalkTo(postowalkto))
            {
                anim.SetBool("Walking", true);
               
                while (Vector3.Distance(transform.position, postowalkto) > 1)
                {
                    transform.LookAt(postowalkto);
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    yield return 0;
                }
                state = State.IDLE;
                anim.SetBool("Walking", false);
                doneWalking = true;
            }
            else
            {
                postowalkto = GetRandomPosition();
                print("Invalid Pos");
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
    #endregion

}
