using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagCheck : MonoBehaviour
{
    [SerializeField] private string checkTag;
    private bool isHit = false;
    private bool isEnter, isStay, isExit;

    [HideInInspector] public Collider col;
    [HideInInspector] public bool IsHit()
    {
        if (isEnter || isStay)
        {
            isHit = true;
        }
        else if (isExit)
        {
            isHit = false;
        }

        isEnter = false;
        isStay = false;
        isExit = false;
        return isHit;
    }

    [HideInInspector] public bool IsEnter()
    {
        return isEnter;
    }

    [HideInInspector] public bool IsExit()
    {
        return isExit;
    }
    

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(checkTag))
        {
            isEnter = true;
            col = collision;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag(checkTag))
        {
            isStay = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag(checkTag))
        {
            isExit = true;
        }
    }
}
