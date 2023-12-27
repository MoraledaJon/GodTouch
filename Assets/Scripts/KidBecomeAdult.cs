using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidBecomeAdult : MonoBehaviour
{
    private float time;
    public float timeToAdult;
    private bool isAdult;
    public ParticleSystem particleEffectPrefa;
    void Start()
    {
        time = 0.0f;
    }
    void Update()
    {
        time += Time.deltaTime;



        if (time > timeToAdult && !isAdult)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.3f * Time.deltaTime,
            transform.localScale.y + 0.3f * Time.deltaTime,
            transform.localScale.z + 0.3f * Time.deltaTime);
            if (transform.localScale.x >= 1)
            {
                ParticleSystem particleEffect;
                particleEffect = Instantiate(particleEffectPrefa, transform.position + new Vector3(0, 5f, 0), particleEffectPrefa.transform.rotation, transform);

                particleEffect.Play();
                isAdult = true;
            }
        }
    }
}
