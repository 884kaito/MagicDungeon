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
        //if exceed max time, destroy
        if (timer >= maxTime)
            Destroy(this.gameObject);

        timer += Time.deltaTime;
    }

    //if hit terrain or enemy, destroy
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameManager.inst.enemyTag) || other.gameObject.CompareTag(GameManager.inst.terrainTag))
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag(GameManager.inst.enemyTag) || other.gameObject.CompareTag(GameManager.inst.terrainTag))
        {
            Destroy(this.gameObject);
        }
    }
}
