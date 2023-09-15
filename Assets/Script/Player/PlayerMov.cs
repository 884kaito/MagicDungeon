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
    [SerializeField] private float groundYSpeed;
    public float maxJumpTime;
    [SerializeField] float maxYSpeed;
    [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] AnimationCurve fallCurve;

    [Header("AirMagic")]
    [SerializeField] private float airMagicDesalloc; //air Magic deallocation
    [SerializeField] private float airMagicMp;
    #endregion


    #region //editable in other scripts

    //move
    [HideInInspector] public bool isGround = false;
    [HideInInspector] public float jumpTime = 0f;
    [HideInInspector] public float fallTime = 0f;
    [HideInInspector] public float xSpeedNow;

    #endregion


    #region //extern components
    [Header("Player Components")]
    private PlayerData data;
    private PlayerData.State state;
    private Rigidbody body;
    private CheckInput input;
    [SerializeField] private GroundCheck ground;
    [SerializeField] private GroundCheck head;
    [SerializeField] private GroundCheck pCol;

    [Header("Extern Components")]
    [SerializeField] private GameObject airMagicPrehub;
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

        AirMagic();

        data.state = state;
    }





    #region //CheckGroundCollisions

    private bool isHead = false;
    private bool groundCollide = false;
    public void CheckGroundCollisions()
    {
        isGround = ground.IsGround();
        isHead = head.IsGround();
        groundCollide = pCol.IsGround();
        Debug.Log(groundCollide);
    }

    #endregion




    #region //SetVelocity

    float ySpeedNow;
    private void SetVelocity()
    {
        if (state == data.airMagic)
        {
            StopPlayer();
            return;
        }
        Debug.Log("body.velocity1 " + body.velocity);
        xSpeedNow = body.velocity.x;
        ySpeedNow = body.velocity.y;
        
        CalcXVelocity();
        CalcYVelocity();
        Debug.Log("ySpeed " + ySpeedNow);
        Debug.Log("body.velocity2 " + body.velocity);
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


    #region//YVelocity

    private float CalcYVelocity()
    {
        //if is on the ground 
        if (isGround && !groundCollide) ySpeedNow = -groundYSpeed;
        else ySpeedNow = 0;

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
                Debug.Log(fallTime);
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

    public void StopPlayer()
    {
        body.velocity = new Vector2(0, 0);
    }

    #endregion

    #endregion




    #region//AirMagic
    private 

    void AirMagic()
    {
        if (input.airMagic.down && !isGround && state != data.airMagic)
        {
            StartAirMagic();
            return;
        }

        if (input.up.down && !isGround && state != data.airMagic)
        {
            if(StartAirMagic())
                StopAirMagic();
            return;
        }

        if (state == data.airMagic && (input.airMagic.down || input.up.down))
        {
            StopAirMagic();
        }

            
    }

    bool StartAirMagic()
    {
        bool canShoot = data.UseMp(airMagicMp);
        if (!canShoot) return false;

        state = data.airMagic;
        StopPlayer();

        GameObject airMagic = Instantiate(airMagicPrehub);
        airMagic.transform.position = this.transform.position;
        airMagic.transform.position += new Vector3(0, -airMagicDesalloc, 0);
        airMagic.SetActive(true);

        return true;
    }

    void StopAirMagic()
    {
        if (input.up.down)
        {
            state = data.jump;

            //set x velocity
            float direction = 0;
            if (input.right.on) direction += 1;
            if (input.left.on) direction -= 1;
            body.velocity = new Vector2(direction * maxXAirSpeed, body.velocity.y);
        }   
        if (input.airMagic.down)
            state = data.fall;
        ResetYData();
    }

    #endregion
}
