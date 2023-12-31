using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manage player animation
public class PlayerAnime : MonoBehaviour
{
    //editable in other scripts

    //extern componentes
    [Header("Player Component")]
    [SerializeField] private Animator anime;
    private PlayerMov mov;
    private PlayerData data;
    private Rigidbody body;
    
    //private components
    private Vector3 playerScale;

    void Start()
    {
        mov = GetComponent<PlayerMov>();
        data = GetComponent<PlayerData>();
        body = GetComponent<Rigidbody>();
        playerScale = this.transform.localScale; //set player default scale
    }

    void Update()
    {
        Animate();
    }


    #region Animate

    //animation variables
    PlayerData.State state;
    AnimatorClipInfo animeInfo;

    //animation names
    readonly string idle = "Idle";
    readonly string run = "Run";
    readonly string jump = "Jump";
    readonly string fall = "Fall";
    readonly string hit = "Hit";
    readonly string crouch = "Crouch";
    readonly string heal = "Heal";

    //animation parameters
    readonly string jumpFallTime = "jumpFallTime";
    readonly string isRun = "isRun";
    readonly string isGround = "isGround";


    void Animate()
    {
        //abbreviate
        state = data.state;
        animeInfo = anime.GetCurrentAnimatorClipInfo(0)[0];

        //set parameters
        anime.SetBool(isRun, mov.xSpeedNow != 0);
        anime.SetBool(isGround, mov.isGround);

        //turn
        if (data.isRight)
            body.transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z);
        else
            body.transform.localScale = new Vector3(-playerScale.x, playerScale.y, playerScale.z);



        //link player state with animation

        //reset all triggers
        ResetTriggers();

        //idle
        if (state == data.idle)
            anime.SetTrigger(idle);

        //run
        else if (state == data.run)
            anime.SetTrigger(run);

        //jump
        else if (state == data.jump)
        {
            anime.SetFloat(jumpFallTime, (mov.jumpTime / mov.maxJumpTime));
            anime.SetTrigger(jump);
        }
        //fall
        else if (state == data.fall)
        {
            anime.SetFloat(jumpFallTime, (mov.fallTime / mov.maxJumpTime));
            anime.SetTrigger(fall);
        }

        //hit
        else if ((state == data.hit || state == data.die) && animeInfo.clip.name != hit)
            anime.Play(hit);

        //crouch
        else if (state == data.crouch || state == data.airMagic)
            anime.SetTrigger(crouch);

        //heal
        else if (state == data.heal)
            anime.SetTrigger(heal);
    }


    void ResetTriggers()
    {
        anime.ResetTrigger(idle);
        anime.ResetTrigger(run);
        anime.ResetTrigger(jump);
        anime.ResetTrigger(fall);
        anime.ResetTrigger(crouch);
        anime.ResetTrigger(heal);
    }

    #endregion
}
