using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manage player movement
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

    [Header("Heal")]
    [SerializeField] private float healSpeed;
    [SerializeField] private float healStartTime;
    [SerializeField] private float shpUseSpeed;
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
        body = GetComponent<Rigidbody>();
        input = GetComponent<CheckInput>();
    }

    void FixedUpdate()
    {
        CheckGroundCollisions();

        if (!data.canControl)
            return;

        //calculate velocity
        SetVelocity();

        AirMagic();

        Heal();
    }





    #region //CheckGroundCollisions

    private bool isHead = false;
    private bool groundCollide = false;
    public void CheckGroundCollisions()
    {
        isGround = ground.IsGround();
        isHead = head.IsGround();
        groundCollide = pCol.IsGround();
    }

    #endregion




    #region //SetVelocity

    float ySpeedNow;
    private void SetVelocity()
    {
        if (data.state == data.airMagic || data.state == data.heal)
        {
            StopPlayer();
            return;
        }

        Crouch();

        if (data.state == data.crouch)
            return;

        xSpeedNow = body.velocity.x;
        ySpeedNow = body.velocity.y;
        
        CalcXVelocity();
        CalcYVelocity();

        body.velocity = new Vector2(xSpeedNow, ySpeedNow);
    }



    #region //XVelocity

    private float CalcXVelocity()
    {
        if (data.state == data.idle || data.state == data.run)
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
        data.state = data.idle;

        float speed = 0;
        if (input.left.on) speed--;
        if (input.right.on) speed++;

        xSpeedNow = groundSpeed * speed; //set speed

        //set states
        if(speed != 0)
        {
            data.state = data.run;
            if(speed > 0) data.isRight = true;
            else data.isRight = false;
        }

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
        if (input.up.down && isGround && data.state != data.jump)
        {
            data.state = data.jump;
            ResetYData();
        }

        //execute jump
        if (data.state == data.jump)
        {
            //stop jump
            if (jumpTime >= maxJumpTime || ground.IsGroundEnter() || isHead)
            {
                data.state = data.fall;
                ResetYData();
            }

            //execute jump
            jumpTime += Time.deltaTime;
            ySpeedNow = maxYSpeed * jumpCurve.Evaluate(jumpTime / maxJumpTime);
        }

        //start fall
        if (!isGround && data.state != data.jump && data.state != data.fall)
        {
            data.state = data.fall;
            ResetYData();
        }

        //execute fall
        if (data.state == data.fall)
        {
            //stop fall
            if (isGround)
            {
                data.state = data.idle;
            }

            //execute jump
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



    #endregion
    


    void Crouch()
    {
        //start crouch
        if(input.crouch.on && isGround)
        {
            data.state = data.crouch;
            StopPlayer();
        }

        //stop crouch
        else
        {
            if (data.state == data.crouch)
                data.state = data.idle;
        }
    }



    public void StopPlayer()
    {
        body.velocity = new Vector2(0, 0);
    }

    #endregion




    #region//AirMagic
    private 

    void AirMagic()
    {
        //start air magic and stay
        if (input.airMagic.down && !isGround && data.state != data.airMagic)
        {
            StartAirMagic();
            return;
        }

        //start air magic and jump
        if (input.up.down && !isGround && data.state != data.airMagic)
        {
            if(StartAirMagic())
                StopAirMagic();
            return;
        }

        //stop air magic
        if (data.state == data.airMagic && (input.airMagic.down || input.up.down))
        {
            StopAirMagic();
        }

        //turn
        if (data.state == data.airMagic)
        {
            if (input.right.on) data.isRight = true;
            else if (input.left.on) data.isRight = false;
        }
    }

    bool StartAirMagic()
    {
        //view if have mana enough
        bool canShoot = data.UseMp(airMagicMp);
        if (!canShoot) return false;

        //set states
        data.state = data.airMagic;
        StopPlayer();

        //create air magic circle
        GameObject airMagic = Instantiate(airMagicPrehub);
        airMagic.transform.position = this.transform.position;
        airMagic.transform.position += new Vector3(0, -airMagicDesalloc, 0);

        return true;
    }

    void StopAirMagic()
    {
        //stop air magic and jump
        if (input.up.down)
        {
            data.state = data.jump;

            //set x velocity
            float direction = 0;
            if (input.right.on) direction += 1;
            if (input.left.on) direction -= 1;
            body.velocity = new Vector2(direction * maxXAirSpeed, body.velocity.y);
        }

        //stop air magic and fall
        if (input.airMagic.down)
            data.state = data.fall;

        ResetYData();
    }

    #endregion




    #region
    float chargeTimer = 0f;

    void Heal()
    {
        //start heal charging
        if(input.heal.down && isGround && data.state != data.heal && data.CanHpHeal())
        {
            StartCharge();
        }

        //execute heal
        if (data.state == data.heal && chargeTimer > healStartTime)
            if(!data.HpHeal(healSpeed, shpUseSpeed)) //heal and verify if can continue healing
                StopHeal();

        //stop heal
        if (data.state == data.heal && !input.heal.on)
        {
            StopHeal();
        }

        chargeTimer += Time.deltaTime;
    }


    void StartCharge()
    {
        //start heal state
        data.state = data.heal;
        chargeTimer = 0;
        StopPlayer();
    }

    void StopHeal()
    {
        //update state
        data.state = data.idle;
    }

    #endregion
}
