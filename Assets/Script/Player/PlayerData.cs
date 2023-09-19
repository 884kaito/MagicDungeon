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
        Die,
        Crouch,
        Heal
    };
    [HideInInspector] public State state = State.Idle;
    [HideInInspector] public State idle = State.Idle;
    [HideInInspector] public State run = State.Run;
    [HideInInspector] public State jump = State.Jump;
    [HideInInspector] public State fall = State.Fall;
    [HideInInspector] public State airMagic = State.AirMagic;
    [HideInInspector] public State hit = State.Hit;
    [HideInInspector] public State die = State.Die;
    [HideInInspector] public State crouch = State.Crouch;
    [HideInInspector] public State heal = State.Heal;

    //hp & mp
    [HideInInspector] public float hp;
    [HideInInspector] public float mp;
    [HideInInspector] public float shp;
    #endregion


    #region //private

    private float mpHealTimer;
    private float hpHealSpeed;
    private bool isHeal = false;

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

   public bool HpHeal(float hpHealSpeed, float shpUseSpeed)
    {
        if (CanHpHeal())
        {
            hp += Time.deltaTime * maxHp * hpHealSpeed;
            shp -= Time.deltaTime * shpUseSpeed;
            return true;
        }
        return false;
    }

    public bool CanHpHeal()
    {
        return shp > 0 && hp < maxHp;
    }

    public void StopHeal()
    {
        isHeal = false;
    }

    public void ShpHeal(float heal)
    {
        shp = Mathf.Clamp(shp + heal, 0, maxShp);
    }
    #endregion
}
