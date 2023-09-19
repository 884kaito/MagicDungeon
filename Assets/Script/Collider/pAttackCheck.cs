using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pAttackCheck : MonoBehaviour
{
    private string checkTag;
    private bool isHit = false;
    private bool isEnter, isStay, isExit;

    [HideInInspector] public AttackData.Data hitData;



    private void Start()
    {
        checkTag = GameManager.inst.pAttackTag;
    }

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
        if (collision.CompareTag(checkTag))
        {
            isEnter = true;
            if(collision.GetComponent<AttackData>())
                hitData = collision.GetComponent<AttackData>().data;
            else
                Debug.LogError("Hit object dont have AttackData script");
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag(checkTag))
        {
            isStay = true;
            if (collision.GetComponent<AttackData>())
                hitData = collision.GetComponent<AttackData>().data;
            else
                Debug.LogError("Hit object dont have AttackData script");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag(checkTag))
        {
            isExit = true;
            if (collision.GetComponent<AttackData>())
                hitData = collision.GetComponent<AttackData>().data;
            else
                Debug.LogError("Hit object dont have AttackData script");

        }
    }
}
