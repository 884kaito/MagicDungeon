using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] float speed;

    private float timer = 0f;

    void Start()
    {

    }


    void Update()
    {
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);

        if(timer >= time)
        {
            Destroy(this.gameObject);
        }

        timer += Time.deltaTime;
    }
}
