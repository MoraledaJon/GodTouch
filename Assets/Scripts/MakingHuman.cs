using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakingHuman : MonoBehaviour
{
    private float time;
    public float spawnTime;
    public GameObject kidGirl;
    public GameObject kidBoy;

    void Start()
    {
        time = 0.0f;
    }


    void Update()
    {
        if (transform.GetChild(0) == null)
            return;
        
        time += Time.deltaTime;
        if(time > spawnTime)
        { 
            GameObject kid;
            int rnd50 = Random.Range(0, 2); //50% of boy or girl
            if (rnd50 == 0)
                kid = kidGirl;
            else
                kid = kidBoy;

            int rnd = Random.Range(0,transform.childCount);
            GameObject parent = transform.GetChild(rnd).gameObject;
           Instantiate(kid, new Vector3(parent.transform.position.x,
                                             parent.transform.transform.position.y,
                                             parent.transform.transform.position.z),
                                             transform.rotation,parent.transform.parent.transform);

            time = 0;
        }
    }
}
