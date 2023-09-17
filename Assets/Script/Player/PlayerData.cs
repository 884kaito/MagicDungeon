using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// storage player frequently used parameters
/// </summary>
public class PlayerData : MonoBehaviour
{
    #region // Editable in Inspector
    public float maxHp;
    public float maxMp;
    public float maxShp;
    [SerializeField] float mpHealCooldown;
    [SerializeField] float mpHealSpeed;
    #endregion


    #region //editable in other scripts

    //player state
    [HideInInspector] public bool isRight = true;
    [HideInInspector] public bool canHit = true;
    [HideInInspector] public bool canControl = true;

    public enum State {
        Idle,
        Run,
        Jump,
        Fall,
        AirMagic,
        Hit,
        Die
    };
    [HideInInspector] public State state = State.Idle;
    [HideInInspector] public State idle = State.Idle;
    [HideInInspector] public State run = State.Run;
    [HideInInspector] public State jump = State.Jump;
    [HideInInspector] public State fall = State.Fall;
    [HideInInspector] public State airMagic = State.AirMagic;
    [HideInInspector] public State hit = State.Hit;
    [HideInInspector] public State die = State.Die;

    //hp & mp
    [HideInInspector] public float hp;
    [HideInInspector] public float mp;
    [HideInInspector] public float shp;
    #endregion


    #region //private
    
    private float mpHealTimer;

    #endregion




    void Start()
    {

    }
    private void Update()
    {
        MpHeal();
    }




    #region //Mp

    public bool UseMp(float useMp)
    {
        //if not have mp enough, cant use mp
        if (mp - useMp < 0)
            return false;

        mp -= useMp;
        mpHealTimer = 0;

        return true;
    }

    void MpHeal()
    {
        mpHealTimer += Time.deltaTime;
        if (mpHealTimer >= mpHealCooldown && mp < maxMp)
            mp += Time.deltaTime * maxMp * mpHealSpeed; 
    }
    #endregion





    #region //Hp

    public bool MinusHp(float damage)
    {
        //if not have hp enough, die
        if (hp - damage <= 0)
            return false;

        hp -= damage;
        return true;
    }

    #endregion
}
