using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField] float maxHp;
    [SerializeField] pAttackCheck pAttack;
    [SerializeField] float deathTime;
    [SerializeField] Material deathMaterial;


    NEnemyMov mov;
    NEnemyMov.State state;
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
        if(state == mov.die)
        {
            ExecuteDieAnim();
        }
    }


    


    void Hit(AttackData.Data data)
    {
        if (!MinusHp(data.damage))
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





    readonly string deathAnim = "Die";
    float timer = 0;
    IEnumerator Death()
    {
        state = mov.die;
        mov.state = state;
        body.velocity = new Vector2(0, 0);
        anime.enabled = false;
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = deathMaterial;
            renderers[i].material.color = normalColors[i];
        }

            yield return new WaitForSeconds(deathTime);

        Destroy(this.gameObject);
    }

    void ExecuteDieAnim()
    {
        for(int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = new Color(
                normalColors[i].r * ((deathTime - timer) / deathTime),
                normalColors[i].g * ((deathTime - timer) / deathTime),
                normalColors[i].b * ((deathTime - timer) / deathTime));
            
        timer += Time.deltaTime;
    }


}
