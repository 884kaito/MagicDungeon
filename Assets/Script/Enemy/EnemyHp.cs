using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField] float maxHp;
    [SerializeField] pAttackCheck pAttack;
    [SerializeField] float deathTime;
    [SerializeField] float uncolorTime;
    [SerializeField] Material deathMaterial;
    [SerializeField] GameObject deathParticlePrehub;


    NEnemyMov mov;
    Animator anime;
    Rigidbody body;
    Renderer[] renderers;
    Color[] normalColors;
    

    float hp;
    



    void Start()
    {
        mov = GetComponent<NEnemyMov>();
        anime = GetComponentInChildren<Animator>();
        renderers = GetComponentsInChildren<Renderer>();
        body = GetComponent<Rigidbody>();

        hp = maxHp; //set hp

        //get inicial colors
        normalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            normalColors[i] = renderers[i].material.color;
    }

    void Update()
    {

    }


    void FixedUpdate()
    {
        if (pAttack.IsEnter())
            Hit(pAttack.hitData);


        //execute die
        if(mov.state == mov.die)
        {
            ExecuteDieAnim();
        }
    }


    


    void Hit(AttackData.Data data)
    {
        if (MinusHp(data.damage))
        {
            mov.state = mov.fright;
            StartCoroutine(HitAnime());
        }
        else
        {
            StartCoroutine(Death());
        }
    }


    bool MinusHp(float damage)
    {
        //if not have hp enough, die
        if (hp - damage <= 0)
            return false;

        hp -= damage;
        return true;
    }


    [SerializeField] float wait;
    [SerializeField] float mult;
    IEnumerator HitAnime()
    {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = new Color(normalColors[i].r * mult,
                                            normalColors[i].g * mult,
                                            normalColors[i].b * mult);

        yield return new WaitForSeconds(wait);

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = normalColors[i];
    }


    float timer = 0;
    IEnumerator Death()
    {
        //update state
        mov.state = mov.die;

        //stop enemy
        body.velocity = new Vector2(0, 0);
        anime.enabled = false;

        //change material and set original color
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = deathMaterial;
            renderers[i].material.color = normalColors[i];
        }

        //wait for animation
        yield return new WaitForSeconds(deathTime);

        //create death particle
        GameObject particle = Instantiate(deathParticlePrehub);
        particle.transform.position = this.transform.position;

        //destroy
        Destroy(this.gameObject);
    }

    void ExecuteDieAnim()
    {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = new Color(
                normalColors[i].r * ((uncolorTime - timer) / uncolorTime),
                normalColors[i].g * ((uncolorTime - timer) / uncolorTime),
                normalColors[i].b * ((uncolorTime - timer) / uncolorTime));
            
        timer += Time.deltaTime;
    }


}
