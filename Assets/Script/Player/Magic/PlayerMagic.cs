using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    ///////////////
    /// Variable
    ///////////////

    #region // Variables

    [SerializeField] private float emitRadius;


    [Header("Magic Circles Variables")]

    //fireMagic
    [SerializeField] MagicCircleData[] magics;


    //extern Conponents
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
        bool canShoot = data.UseMp(magics[0].mp);
        if (!canShoot) return;


        //Instantiate
        GameObject magic = Instantiate(magics[0].gameObject);


        //Calculate position

        //get mouse and player position
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = this.transform.position.z;
        Vector3 playerPos = Camera.main.WorldToScreenPoint(this.transform.position);


        //calculate degree
        Vector3 dif = (mousePos - playerPos).normalized;  
        float degree = Mathf.Atan2(dif.x, dif.y) * Mathf.Rad2Deg;
        degree += 90;
        if (degree < 0) degree += 360;
        float radDegree = degree * Mathf.Deg2Rad;


        //set position
        magic.transform.position = this.transform.position;
        magic.transform.position += new Vector3(-Mathf.Cos(radDegree) * emitRadius, Mathf.Sin(radDegree) * emitRadius, magic.transform.position.z);

        //set rotation
        magic.transform.eulerAngles = new Vector3(degree, magic.transform.eulerAngles.y, magic.transform.eulerAngles.z);
        magic.GetComponent<MagicCircleData>().degree = radDegree;


        //set active
        magic.SetActive(true);


    }
}
