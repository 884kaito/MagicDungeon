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



    void Start()
    {
        mov = GetComponent<PlayerMov>();
        data = GetComponent<PlayerData>();
        body = GetComponent<Rigidbody>();
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

    void Animate()
    {
        //abbreviate
        state = data.state;
        animeInfo = anime.GetCurrentAnimatorClipInfo(0)[0];
        
        //turn
        if(data.isRight)
            body.transform.localScale = new Vector3(1f, 1f, 1f);
        else
            body.transform.localScale = new Vector3(-1f, 1f, 1f);

        //idle
        if (state == data.idle && animeInfo.clip.name != idle)
            anime.Play(idle);

        //run
        if (state == data.run && animeInfo.clip.name != run)
            anime.Play(run);
            
    }

    #endregion
}
