using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manage player attack magic
public class PlayerMagic : MonoBehaviour
{
    ///////////////
    /// Variable
    ///////////////

    #region // Variables

    //editable in inspector
    [Header("Magic Circles Variables")]
    [SerializeField] float emitRadius;
    [SerializeField] Vector3 emitOffset;
    [SerializeField] MagicCircleData[] magics;


    //extern Components
    private PlayerData data;
    private CheckInput input;

    #endregion




    ///////////////
    /// Script
    ///////////////

    void Start()
    {
        data = GetComponent<PlayerData>();
        input = GetComponent<CheckInput>();
    }


    int magicNumber = 0;
    void FixedUpdate()
    {
        if(input.attack.down) //when add more magic add last number key pressed
        {
            StartMagic(magicNumber);
        }
    }



    //n = shoot magic number
    void StartMagic(int n)
    {
        //check mp and if have, shoot, if not return
        bool canShoot = data.UseMp(magics[n].mp);
        if (!canShoot) return;


        //create magic circle
        GameObject magic = Instantiate(magics[n].gameObject);


        //Calculate position and rotation

        //get mouse and player position
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = this.transform.position.z;
        Vector3 playerPos = Camera.main.WorldToScreenPoint(this.transform.position);


        //calculate degree
        Vector3 dif = (mousePos - playerPos).normalized;
        float radian = Mathf.Atan2(dif.y, dif.x); //radian

        //set position
        magic.transform.position = this.transform.position;
        magic.transform.position += new Vector3(Mathf.Cos(radian) * emitRadius, Mathf.Sin(radian) * emitRadius, 0);
        magic.transform.position += emitOffset;
        Debug.Log(radian / Mathf.PI);
        Debug.Log(radian * Mathf.Rad2Deg);
        //set rotation
        magic.transform.eulerAngles = new Vector3(0, 0, radian * Mathf.Rad2Deg);
    }
}
