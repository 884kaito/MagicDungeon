using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMagic : MonoBehaviour
{
    [SerializeField] GameObject firePrehub;

    private MagicCircleData mData;
    private PlayerData pData;

    void Awake()
    {
        mData = GetComponent<MagicCircleData>();
        pData = FindAnyObjectByType<PlayerData>();

        mData.StartAppear();
    }


    private bool isShoted = false;
    private void FixedUpdate()
    {
        if (mData.isMiddle && !isShoted)
            CreateFire();
    }

    
    void CreateFire()
    {
        isShoted = true;

        //create
        GameObject fire = Instantiate(firePrehub);
        fire.SetActive(true);

        //position & rotation
        fire.transform.position = transform.position;
        fire.transform.eulerAngles = new Vector3(fire.transform.eulerAngles.x, fire.transform.eulerAngles.y, this.transform.eulerAngles.x);

        //velocity
        Rigidbody body = fire.GetComponent<Rigidbody>();
        Fire fireSc = fire.GetComponent<Fire>();
        Vector3 velocity = new Vector3();
        velocity.x = -Mathf.Cos(mData.degree);
        velocity.y = Mathf.Sin(mData.degree);
        velocity = velocity.normalized * fireSc.speed;
        body.velocity = velocity;

        //active
        fire.SetActive(true);
    }
}
