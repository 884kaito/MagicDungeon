using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float speed;
    public float scale;
    public float range;

    private float timer;
    private float maxTime;



    void Awake()
    {
        maxTime = range / speed;
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
