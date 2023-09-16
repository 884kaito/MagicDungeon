using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField] float maxHp;
    [SerializeField] pAttackCheck pAttack;

    NEnemyMov mov;
    NEnemyMov.State state;
    


    float hp;

    void Start()
    {
        mov = GetComponent<NEnemyMov>();
        hp = maxHp;
    }

    void Update()
    {
        mov.state = state;
    }


    void FixedUpdate()
    {
        if (pAttack.IsEnter())
            Hit(pAttack.hitData);
    }



    void Hit(AttackData.Data data)
    {
        Debug.Log("hit");
        if (!MinusHp(data.damage))
        {
            Death();
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

    void Death()
    {
        state = mov.die;
        Debug.Log("death");
        Destroy(this.gameObject);
    }
}
