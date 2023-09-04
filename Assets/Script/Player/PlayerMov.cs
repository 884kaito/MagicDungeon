using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    ///////////////
    /// Variable
    ///////////////


    #region // Editable in Inspector
    [Header("XMove")]
    [SerializeField] private float groundSpeed;
    [SerializeField] float maxXAirSpeed;
    [SerializeField] float airAcelerateSpeed;
    [SerializeField] float airDesacelerateSpeed;

    [Header("YMove")]
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
        state = data.state; //to abbreviate
        body = GetComponent<Rigidbody>();
        input = GetComponent<CheckInput>();
    }

    void FixedUpdate()
    {
        CheckGroundCollisions();

        //calculate velocity
        SetVelocity();

        data.state = state;
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
        if (state == data.idle || state == data.run)
        {
            xSpeedNow = GroundXSet();
        }
        else
        {
            xSpeedNow = NonGroundXSet();
        }

        return xSpeedNow;
    }

    private float GroundXSet()
    {
        state = data.idle;

        bool isBothKeyOn = input.left.on && input.right.on;
        if (isBothKeyOn)
            xSpeedNow = 0.0f;

        else if (input.right.on)
        {
            data.isRight = true;
            xSpeedNow = groundSpeed;
            state = data.run;
        }

        else if (input.left.on)
        {
            data.isRight = false;
            xSpeedNow = -groundSpeed;
            state = data.run;
        }
        else
            xSpeedNow = 0.0f;

        return xSpeedNow;
    }

    private float NonGroundXSet()
    {
        xSpeedNow = body.velocity.x;

        //desacelerate if press the two buttons or nothing
        bool isBothKeyOn = input.left.on && input.right.on;
        bool isBothKeyOff = !input.left.on && !input.right.on;
        if (isBothKeyOn || isBothKeyOff)
        {
            if (xSpeedNow > 0.1f)
            {
                xSpeedNow -= airDesacelerateSpeed;
            }
            else if (xSpeedNow < -0.1f)
            {
                xSpeedNow += airDesacelerateSpeed;
            }
            else
            {
                xSpeedNow = 0.0f;
            }
        }

        //acelerate
        else if (input.right.on)
        {
            data.isRight = true;
            xSpeedNow += airAcelerateSpeed;
        }

        else if (input.left.on)
        {
            data.isRight = false;
            xSpeedNow -= airAcelerateSpeed;
        }

        //set velocity to not exceed max/min
        if (Mathf.Abs(xSpeedNow) > maxXAirSpeed)
            xSpeedNow = Mathf.Sign(xSpeedNow) * maxXAirSpeed;

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
