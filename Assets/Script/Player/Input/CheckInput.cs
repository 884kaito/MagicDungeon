using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Check inputs linked with fixedUpdate time
/// </summary>
public class CheckInput : MonoBehaviour
{
    //Editable in inspector
    [Header("Keys")]
    [SerializeField] private int keyNumber;
    [SerializeField] private string rightKey;
    [SerializeField] private string leftKey;
    [SerializeField] private string upKey;
    [SerializeField] private string downKey;
    [SerializeField] private string changeKey;
    [SerializeField] private string healKey;
    [SerializeField] private string dashKey;
    [SerializeField] private string basicAttackKey;


    //Editable in other scripts
    [HideInInspector] public KeyData down;
    [HideInInspector] public KeyData up;
    [HideInInspector] public KeyData right;
    [HideInInspector] public KeyData left;
    [HideInInspector] public KeyData change;
    [HideInInspector] public KeyData heal;
    [HideInInspector] public KeyData dash;
    [HideInInspector] public KeyData basicAttack;


    //local variable
    private KeyData[] keys;






    private void Start()
    {
        //inicialize keys

        keys = new KeyData[keyNumber];

        down = new KeyData();
        right = new KeyData();
        up = new KeyData();
        left = new KeyData();
        change = new KeyData();
        heal = new KeyData();
        dash = new KeyData();
        basicAttack = new KeyData();
        
        keys[0] = down;
        keys[1] = left;
        keys[2] = right;
        keys[3] = up;
        keys[4] = change;
        keys[5] = heal;
        keys[6] = dash;
        keys[7] = basicAttack;

        SetKeyNames();
    }



    void FixedUpdate()
    {
        CheckKey();
        AtualizeBeforeKey();
    }





    //atualiza o nome dos keys
    public void SetKeyNames()
    {
        down.code = downKey;
        up.code = upKey;
        right.code = rightKey;
        left.code = leftKey;
        change.code = changeKey;
        heal.code = healKey;
        dash.code = dashKey;
        basicAttack.code = basicAttackKey;
    }


    #region //CheckKeys

    private void CheckKey()
    {
        InicializeKey();

        VerifityKey();
    }


    //reseta os variaveis do key
    private void InicializeKey()
    {
        for(int i = 0; i < keyNumber; i++)
        {
            keys[i].down = false;
            keys[i].on = false;
        }
    }



    bool rightBottonNow;
    bool beforeRightBotton = false;

    //atualiza os variaveis dos keys
    private void VerifityKey()
    {
        //keyboard key
        for(int i = 0; i < keyNumber; i++)
        {
            bool keyNow = Input.GetKey(keys[i].code) || Input.GetKeyDown(keys[i].code);
            if (keyNow && keys[i].before)
            {
                keys[i].on = true;
            }
            else if  (keyNow && !keys[i].before)
            {
                keys[i].down = true;
            }
            keys[i].on = keys[i].on || keys[i].down;
        }
    }


    //armazena o key no frame anterior
    private void AtualizeBeforeKey()
    {
        //keyboard key
        for (int i = 0; i < keyNumber; i++)
        {
            keys[i].before = keys[i].on;
        }

        beforeRightBotton = rightBottonNow;

    }

    #endregion
}
