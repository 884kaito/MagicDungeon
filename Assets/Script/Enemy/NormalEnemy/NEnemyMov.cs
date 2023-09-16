using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEnemyMov : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] float xSpeed;
    [SerializeField] float gravity;

    [Header("Animation Parameters")]
    [SerializeField] float exitGroundTime;
    [SerializeField] float enterGroundTime;



    [Header("Slime Components")]
    Rigidbody body;
    PlayerData pData;
    Animator anime;
    [SerializeField] GroundCheck ground;
    [SerializeField] GroundCheck front;
    [SerializeField] PlayerCheck searchArea;
    [SerializeField] PlayerCheck battleArea;
    [SerializeField] AnimationClip moveAnimation;



    enum State
    {
        Move,
        Fright,
        Search,
        Die
    }
    State state = State.Move;
    AnimatorClipInfo animeInfo;




    void Start()
    {
        body = GetComponent<Rigidbody>();
        pData = FindObjectOfType<PlayerData>();
        anime = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        Move();

        Animation();
    }





    void Move()
    {
        //flip
        if (front.IsGround() || !ground.IsGround())
        {
            xSpeed = -xSpeed;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        //start Fright
        if ((searchArea.IsHit() && (state == State.Move || state == State.Search))
             || (battleArea.IsHit() && state == State.Search))
        {
            state = State.Fright;
        }

        //stop Fright
        if (!battleArea.IsHit() && state == State.Fright)
        {
            state = State.Search;
        }


        //set velocity in normal mode
        if (state == State.Move)
        {
            float animationTime = (moveAnimation.length * anime.GetCurrentAnimatorStateInfo(0).normalizedTime) % moveAnimation.length;

            if (animationTime >= exitGroundTime && animationTime <= enterGroundTime)
                body.velocity = new Vector2(xSpeed, -gravity);
            else
                body.velocity = new Vector2(0, 0);


        }

        //set velocity in attack mode
        if (state == State.Fright)
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

        if(state == State.Search)
        {
            //search finish, change to move
            if (animeInfo.clip.name == searchAnim && anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                state = State.Move;

            if (animeInfo.clip.name != searchAnim)
                anime.Play(searchAnim);
        }
    }
}
