using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manage fire magic circle
//criate fire magic
public class FireMagic : MagicCircleData
{
    [SerializeField] GameObject firePrehub;

    void Awake()
    {
        StartCoroutine(DeathTimer());
    }


    private bool isShoted = false;
    private void FixedUpdate()
    {
        //create magic on middle
        if (isMiddle && !isShoted)
            CreateFire();
    }

    
    void CreateFire()
    {
        isShoted = true;

        //create
        GameObject fire = Instantiate(firePrehub);

        //position & rotation
        fire.transform.position = transform.position;
        fire.transform.rotation = transform.rotation;

        //velocity
        Rigidbody body = fire.GetComponent<Rigidbody>();
        Fire fireSc = fire.GetComponent<Fire>();
        Vector3 velocity = new Vector3(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.eulerAngles.z * Mathf.Deg2Rad), 0);
        body.velocity = velocity.normalized * fireSc.speed;
    }
}
