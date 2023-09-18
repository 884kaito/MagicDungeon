using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHp : MonoBehaviour
{
    [Header("Hit")]
    [SerializeField] float maxHp;
    [SerializeField] Collider hitArea;
    [SerializeField] pAttackCheck pAttack;

    [Header("Death")]
    [SerializeField] float deathTime;
    [SerializeField] float uncolorTime;
    [SerializeField] Material deathMaterial;
    [SerializeField] GameObject deathParticlePrehub;

    [Header("Damage Text")]
    [SerializeField] GameObject damageTextCanvas;
    [SerializeField] Vector3 damageTextOffset;
    [SerializeField] Vector3 damageTextRand;


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
        if(mov.state != mov.die)
            StartCoroutine(CreateDamageText(data.damage));

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


    IEnumerator HitAnime()
    {
        float colorMult = 0.7f;

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = new Color(normalColors[i].r * colorMult,
                                            normalColors[i].g * colorMult,
                                            normalColors[i].b * colorMult);

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = normalColors[i];
    }


    IEnumerator CreateDamageText(float damage)
    {
        //create
        GameObject canvas = Instantiate(damageTextCanvas);
        TMP_Text damageText = canvas.GetComponentInChildren<TMP_Text>();

        //set text and inicial position
        damageText.text = damage.ToString();
        Vector3 rand = new Vector3(Random.Range(-damageTextRand.x, damageTextRand.x), 
                        Random.Range(-damageTextRand.y, damageTextRand.y),
                        Random.Range(-damageTextRand.z, damageTextRand.z));
        damageText.transform.position += rand;
        damageText.transform.position += damageTextOffset;
        damageText.transform.position += this.transform.position;

        //because sometimes text didnt changed
        yield return new WaitForEndOfFrame();
        damageText.text = damage.ToString();
    }



    float timer = 0;
    IEnumerator Death()
    {
        //update state
        mov.state = mov.die;

        //disable hit collider
        hitArea.enabled = false;

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
