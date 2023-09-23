using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCheck : MonoBehaviour
{
    [SerializeField] AttackData.Owner checkAttack;

    private string enemyAttackTag, enemyTag, playerAttackTag;
    private bool isHit = false;
    private bool isEnter, isStay, isExit;

    [HideInInspector] public AttackData.Data hitData;





    private void Start()
    {
        enemyAttackTag = GameManager.inst.eAttackTag;
        enemyTag = GameManager.inst.enemyTag;
        playerAttackTag = GameManager.inst.pAttackTag;
    }

    //verify if isEnter or isStay
    public bool IsHit()
    {
        if (isEnter || isStay)
        {
            isHit = true;
        }
        else if (isExit)
        {
            isHit = false;
        }

        ResetValue();
        return isHit;
    }


    public bool IsEnter()
    {
        bool returnValue = isEnter;
        ResetValue();

        return returnValue;
    }

    public bool IsExit()
    {
        bool returnValue = isExit;
        ResetValue();

        return returnValue;
    }

    private void ResetValue()
    {
        isEnter = false;
        isStay = false;
        isExit = false;
    }






    private void OnTriggerEnter(Collider collision)
    {
        //if find searching tag
        if (SearchForTag(collision))
        {
            isEnter = true;
            //set attack data
            if (collision.GetComponent<AttackData>())
                hitData = collision.GetComponent<AttackData>().data;
            else
                Debug.LogError("Hit object dont have AttackData script");
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        //if find searching tag
        if (SearchForTag(collision))
        {
            isStay = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        //if find searching tag
        if (SearchForTag(collision))
        {
            isExit = true;
        }
    }

    bool SearchForTag(Collider collision)
    {
        return ((collision.CompareTag(enemyAttackTag) || collision.CompareTag(enemyTag)) && checkAttack == AttackData.Owner.Enemy)
            || (collision.CompareTag(playerAttackTag) && checkAttack == AttackData.Owner.Player);
    }
}
