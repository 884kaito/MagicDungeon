using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour
{
    private string playerTag;
    private bool isHit = false;
    private bool isEnter, isStay, isExit;

    private void Start()
    {
        playerTag = GameManager.inst.playerTag;
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

        isEnter = false;
        isStay = false;
        isExit = false;
        return isHit;
    }

    public bool IsEnter()
    {
        return isEnter;
    }

    public bool IsExit()
    {
        return isExit;
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(playerTag))
        {
            isEnter = true;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag(playerTag))
        {
            isStay = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag(playerTag))
        {
            isExit = true;
        }
    }
}
