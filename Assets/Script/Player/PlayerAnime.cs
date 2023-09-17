using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnime : MonoBehaviour
{
    //editable in other scripts

    //extern componentes
    [Header("Player Component")]
    [SerializeField] private Animator anime;
    private PlayerMov mov;
    private PlayerData data;
    private Rigidbody body;

    private Vector3 playerScale;

    void Start()
    {
        mov = GetComponent<PlayerMov>();
        data = GetComponent<PlayerData>();
        body = GetComponent<Rigidbody>();
        playerScale = this.transform.localScale;
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
    readonly string land = "Land";
    readonly string hit = "Hit";

    //animation variables
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

        //idle
        if ((state == data.idle) 
            && (animeInfo.clip.name != idle && animeInfo.clip.name != fall && animeInfo.clip.name != land))
            anime.Play(idle);

        //run
        if (state == data.run && (animeInfo.clip.name != run && animeInfo.clip.name != fall && animeInfo.clip.name != land))
            anime.Play(run);

        //jump&fall&land
        if (state == data.jump)
        {
            anime.SetFloat(jumpFallTime, (mov.jumpTime / mov.maxJumpTime));
            if (animeInfo.clip.name != jump)
                anime.Play(jump);

        }
        if(state == data.fall)
        {
            anime.SetFloat(jumpFallTime, (mov.fallTime / mov.maxJumpTime));
            if (animeInfo.clip.name != fall)
                anime.Play(fall);
        }

        //hit
        if (state == data.hit && animeInfo.clip.name != hit)
            anime.Play(hit);



    }

    #endregion
}
