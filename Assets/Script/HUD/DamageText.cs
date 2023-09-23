using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//move damage text and destroy
public class DamageText : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField] float speed;

    private float timer = 0f;


    void Update()
    {
        //up text
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);

        timer += Time.deltaTime;

        //destroy after few time
        if (timer >= time)
        {
            Destroy(this.gameObject);
        }
    }
}
