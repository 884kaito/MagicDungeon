using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private string groundTag = "Ground";
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;

    public bool IsGround()
    {
        if (isGroundEnter || isGroundStay)
        {
            isGround = true;
        }
        else if (isGroundExit)
        {
            isGround = false;
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        return isGround;
    }

    public bool IsGroundEnter()
    {
        return isGroundEnter;
    }

    public bool IsGroundExit()
    {
        return isGroundExit;
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(groundTag))
        {
            isGroundEnter = true;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag(groundTag))
        {
            isGroundStay = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag(groundTag))
        {
            isGroundExit = true;
        }
    }
}