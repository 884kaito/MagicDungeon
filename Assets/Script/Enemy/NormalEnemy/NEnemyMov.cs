using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//do movement and animation
public class NEnemyMov : EnemyState
{
    [Header("Movement Parameters")]
    [SerializeField] float xSpeed;
    [SerializeField] float gravity;

    [Header("Animation Parameters")]
    [SerializeField] float exitGroundTime;
    [SerializeField] float enterGroundTime;



    [Header("Slime Components")]
    [SerializeField] GroundCheck ground;
    [SerializeField] GroundCheck front;
    [SerializeField] PlayerCheck searchArea;
    [SerializeField] PlayerCheck battleArea;
    [SerializeField] AnimationClip moveAnimation;
    Rigidbody body;
    Animator anime;
    AnimatorClipInfo animeInfo;
    




    void Start()
    {
        body = GetComponent<Rigidbody>();
        anime = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        Move();

        Animation();
    }





    void Move()
    {
        //dead
        if (state == die) return;

        //flip
        if (front.IsGround() || !ground.IsGround())
        {
            xSpeed = -xSpeed;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }



        //start Fright
        if ((searchArea.IsHit() && (state == move || state == search))
             || (battleArea.IsHit() && state == search))
        {
            state = State.Fright;
        }
        
        //stop Fright
        if (!battleArea.IsHit() && state == fright)
        {
            state = State.Search;
        }




        //set velocity in normal mode
        if (state == State.Move)
        {
            float animationTime = (moveAnimation.length * anime.GetCurrentAnimatorStateInfo(0).normalizedTime) % moveAnimation.length;

            //if jumping, move
            if (animationTime >= exitGroundTime && animationTime <= enterGroundTime)
                body.velocity = new Vector2(xSpeed, -gravity);
            //if in ground, stop
            else
                body.velocity = new Vector2(0, 0);
        }

        //set velocity in fright mode
        if (state == fright)
        {
            body.velocity = new Vector2(0, 0);
        }




    }



    private readonly string moveAnim = "Move";
    private readonly string frightAnim = "Fright";
    private readonly string searchAnim = "Search";

    void Animation()
    {
        animeInfo = anime.GetCurrentAnimatorClipInfo(0)[0];


        //move
        if (state == State.Move && animeInfo.clip.name != moveAnim)
        {
            anime.Play(moveAnim);
        }

        //fright
        if (state == State.Fright && animeInfo.clip.name != frightAnim)
        {
            anime.Play(frightAnim);
        }

        //search
        if(state == State.Search)
        {
            //if search finish, change to move
            if (animeInfo.clip.name == searchAnim && anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                state = State.Move;
            }
                

            if (animeInfo.clip.name != searchAnim)
                anime.Play(searchAnim);
        }
    }

}
