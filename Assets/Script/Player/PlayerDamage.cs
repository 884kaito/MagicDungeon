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

    [Header("Death Parameters")]
    [SerializeField] float deathTime;
    [SerializeField] float uncolorTime;
    [SerializeField] float deathTimeScale;
    [SerializeField] GameObject deathParticlePrehub;
    [SerializeField] GameObject wandPrehub;
    [SerializeField] Material deathMaterial;





    //player component
    PlayerData data;
    Renderer[] renderers;
    Rigidbody body;
    Animator anime;

    //extern component
    CameraController cameraSc;

    //private 
    private Vector2 hitAnimeVelocity;
    Color[] normalColors;






    void Start()
    {
        //get component
        data = FindObjectOfType<PlayerData>();
        renderers = GetComponentsInChildren<Renderer>();
        body = GetComponent<Rigidbody>();
        anime = GetComponentInChildren<Animator>();
        cameraSc = FindObjectOfType<CameraController>();

        //calculate hit animation velocity
        hitAnimeVelocity = HitAnimeVelocity();

        //get inicial colors
        normalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            normalColors[i] = renderers[i].material.color;
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = new Color(
                normalColors[i].r,
                normalColors[i].g,
                normalColors[i].b);
        }
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

        //execute die
        if (data.state == data.die)
        {
            //uncolor
            ExecuteDieAnim();
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
            StartCoroutine(Death());
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









    float timer = 0;
    IEnumerator Death()
    {
        StartDeath();

        //wait 2 frames
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        anime.enabled = false;

        //wait for animation
        yield return new WaitForSeconds(deathTime);

        //create death particle
        GameObject particle = Instantiate(deathParticlePrehub);
        particle.transform.position = this.transform.position;

        //drop core
        GameObject wand = Instantiate(wandPrehub);
        wand.transform.position += this.transform.position;

        //destroy
        Destroy(this.gameObject);
    }


    void StartDeath()
    {
        //update state
        data.state = data.die;

        //set hp
        data.hp = 0;

        //change time scale
        GameManager.inst.setTimeScale(deathTimeScale);

        //disable hit collider
        hitArea.enabled = false;

        //stop enemy
        body.velocity = new Vector2(0, 0);

        //change material and set original color
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = normalColors[i];
    }

    void ExecuteDieAnim()
    {
        //uncolor
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = new Color(
                normalColors[i].r * ((uncolorTime - timer) / uncolorTime),
                normalColors[i].g * ((uncolorTime - timer) / uncolorTime),
                normalColors[i].b * ((uncolorTime - timer) / uncolorTime));

        timer += Time.deltaTime;
    }

}
