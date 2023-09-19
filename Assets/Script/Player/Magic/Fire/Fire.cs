using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : AttackData
{

    public float speed;
    public float scale;
    public float range;

    private float timer;
    private float maxTime;

    CapsuleCollider col;



    void Start()
    {
        maxTime = range / speed;
        data.damage = damage;
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        col = GetComponent<CapsuleCollider>();
        col.enabled = false;

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        col.enabled = true;
    }




    void FixedUpdate()
    {
        CountTime();
    }



    //if hit terrain or enemy, destroy
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameManager.inst.enemyTag) || other.gameObject.CompareTag(GameManager.inst.terrainTag))
        {
            DestroyMagic();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag(GameManager.inst.enemyTag) || other.gameObject.CompareTag(GameManager.inst.terrainTag))
        {
            DestroyMagic();
        }
    }


    void CountTime()
    {
        //if exceed max time, destroy
        if (timer >= maxTime)
            DestroyMagic();

        timer += Time.deltaTime;
    }


    void DestroyMagic()
    {
        Destroy(this.gameObject);
    }
}
