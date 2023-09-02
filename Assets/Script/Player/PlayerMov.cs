using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    ///////////////
    /// Variable
    ///////////////


    #region // Editable in Inspector
    [SerializeField] private float groundSpeed;
    [SerializeField] private float maxJumpTime;
    [SerializeField] private float maxYSpeed;
    [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] AnimationCurve fallCurve;
    #endregion


    #region //editable in other scripts
    //move bools
    [HideInInspector] public bool isGround = false;
    #endregion


    #region //extern components
    private PlayerData data;
    private PlayerData.State state;
    private Rigidbody body;
    private CheckInput input;

    [Header("Player Components")]
    [SerializeField] private GroundCheck ground;
    [SerializeField] private GroundCheck head;
    #endregion

    ///////////////
    /// Script
    ///////////////

    void Start()
    {
        data = GetComponent<PlayerData>();
        state = data.state;
        body = GetComponent<Rigidbody>();
        input = GetComponent<CheckInput>();
    }

    void FixedUpdate()
    {
        CheckGroundCollisions();

        //calculate velocity
        SetVelocity();
    }

    #region //CheckGroundCollisions

    private bool isHead = false;
    public void CheckGroundCollisions()
    {
        isGround = ground.IsGround();
        isHead = head.IsGround();
    }

    #endregion


    #region //SetVelocity


    private float xSpeedNow;
    private float ySpeedNow;
    private void SetVelocity()
    {
        xSpeedNow = body.velocity.x;
        ySpeedNow = body.velocity.y;

        CalcXVelocity();
        CalcYVelocity();

        body.velocity = new Vector2(xSpeedNow, ySpeedNow);
    }



    #region //XVelocity

    private float CalcXVelocity()
    {
       // if (isGround)
       // {
            xSpeedNow = GroundXSet();
        //}
        //else
        //{

        //}
        if(ground.IsGroundEnter()) state = data.idle;

        return xSpeedNow;
    }


    private float GroundXSet()
    {
        bool isBothKeyOn = input.left.on && input.right.on;
        if (isBothKeyOn)
            xSpeedNow = 0.0f;

        else if (input.right.on)
        {
            data.isRight = true;
            xSpeedNow = groundSpeed;
        }

        else if (input.left.on)
        {
            data.isRight = false;
            xSpeedNow = -groundSpeed;
        }
        else
            xSpeedNow = 0.0f;

        return xSpeedNow;
    }

    #endregion


    private float jumpTime = 0f;
    private float fallTime = 0f;
    private float CalcYVelocity()
    {
        //start jump
        if (input.up.down && isGround && state != data.jump)
        {
            state = data.jump;
            ResetYData();
            Debug.Log("jump");
        }

        //execute jump
        if (state == data.jump)
        {
            //stop jump
            if (jumpTime >= maxJumpTime || ground.IsGroundEnter() || isHead)
            {
                state = data.fall;
                ResetYData();
            }
            Debug.Log("jumping");

            //execute jump
            jumpTime += Time.deltaTime;
            ySpeedNow = maxYSpeed * jumpCurve.Evaluate(jumpTime / maxJumpTime);
        }

        //start fall
        if (!isGround && state != data.jump && state != data.fall)
        {
            state = data.fall;
            ResetYData();
        }

        //execute fall
        if (state == data.fall)
        {
            if (isGround)
            {
                state = data.idle;
            }
            else
            {
                fallTime += Time.deltaTime;
                ySpeedNow = -maxYSpeed * fallCurve.Evaluate(fallTime / maxJumpTime);
            }
        }
        return ySpeedNow;
    }

    private void ResetYData()
    {
        jumpTime = 0f;
        fallTime = 0f;
    }

    //
    public void StopPlayer()
    {
        body.velocity = new Vector2(0, 0);
    }


    #endregion

}
