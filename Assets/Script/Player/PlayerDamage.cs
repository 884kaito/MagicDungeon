using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manage player damage
public class PlayerDamage : MonoBehaviour
{
    //editable in inspector
    [Header("Hit Parameters")]
    [SerializeField] float hitTime;
    [SerializeField] float invincibleTime;
    [SerializeField] float hitAnimeAngle;
    [SerializeField] float hitAnimeSpeed;
    [SerializeField] float hitStopTime;
    [SerializeField] float shakeWight;
    [SerializeField] AttackCheck hitArea;

    
    //player component
    PlayerData data;
    Renderer[] renderers;
    Rigidbody body;

    //extern component
    CameraController cameraSc;

    //private 
    private Vector2 hitAnimeVelocity;





    void Start()
    {
        //get component
        data = FindObjectOfType<PlayerData>();
        renderers = GetComponentsInChildren<Renderer>();
        body = GetComponent<Rigidbody>();
        cameraSc = FindObjectOfType<CameraController>();

        //calculate hit animation velocity
        hitAnimeVelocity = HitAnimeVelocity();
    }

    //calculate velocity after hit
    Vector2 HitAnimeVelocity()
    {
        Vector2 velocity = new Vector2();

        float angle = hitAnimeAngle * Mathf.Deg2Rad;
        velocity.x = (hitAnimeSpeed * Mathf.Cos(angle));
        velocity.y = (hitAnimeSpeed * Mathf.Sin(angle));

        return velocity;
    }

    void FixedUpdate()
    {
        //execute damage
        if(hitArea.IsHit() && data.canHit)
        {
            Damage(hitArea.hitData);
        }
    }






    void Damage(AttackData.Data hit)
    {
        //set states
        data.canHit = false;
        data.canControl = false;

        //hit
        if(data.MinusHp(hit.damage))
        {
            StartCoroutine(Hit());
        }
        //death
        else
        {
            ///change to death animation
            data.hp = 0;
            Destroy(this.gameObject);
        }

    }


    IEnumerator Hit()
    {
        //inicialize hit
        data.state = data.hit;

        //blink
        StartCoroutine(Blink());

        //hit stop effect
        cameraSc.StartShake(shakeWight, hitStopTime);
        StartCoroutine(GameManager.inst.StopForSeconds(hitStopTime));

        //set velocity
        if (data.isRight)
            body.velocity = new Vector2(hitAnimeVelocity.x, hitAnimeVelocity.y);
        else
            body.velocity = new Vector2(-hitAnimeVelocity.x, hitAnimeVelocity.y);


        //wait hit
        yield return new WaitForSeconds(hitTime);


        //end hit move and player can move
        data.state = data.fall;
        body.velocity = new Vector2(0, 0);
        data.canControl = true;


        //wait invencible time
        yield return new WaitForSeconds(invincibleTime - hitTime);


        //end invencible time
        data.canHit = true;
    }

    



    IEnumerator Blink()
    {
        //execute blink when invencible
        while (!data.canHit)
        {
            AppearRenderer(true);
            yield return new WaitForSeconds(0.1f);
            AppearRenderer(false);
            yield return new WaitForSeconds(0.1f);
        }
        AppearRenderer(true);
    }

    //if isAppear true, erase player
    void AppearRenderer(bool isAppear)
    {
        foreach (Renderer renderer in renderers)
            renderer.enabled = isAppear;
    }
}
