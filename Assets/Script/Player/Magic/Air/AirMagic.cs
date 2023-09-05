using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirMagic : MonoBehaviour
{
    [SerializeField] float appearTime;
    [SerializeField] float defaultScale;

    private PlayerData pData;

    private bool isAppear = true;
    private bool isDesappear = false;
    private bool isPlayerOff = false;
    private float timer = 0;




    void Start()
    {
        pData = FindObjectOfType<PlayerData>();

        StartCoroutine(AppearTimer());
    }


    void Update()
    {
        ExecuteAppear();

        ExecuteDesappear();

        timer += Time.deltaTime;
    }




    #region//Appear

    IEnumerator AppearTimer()
    {
        isAppear = true;

        yield return new WaitForSeconds(appearTime);

        this.transform.localScale = new Vector3(defaultScale, defaultScale, 1);
        isAppear = false;
    }

    void ExecuteAppear()
    {
        if(isAppear)
        {
            float scale = timer / appearTime * defaultScale;
            this.transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    #endregion


    #region //Desappear

    IEnumerator DesappearTimer()
    {
        isDesappear = true;
        timer = 0;

        yield return new WaitForSeconds(appearTime);

        isDesappear = false;
        Destroy(this.gameObject);
    }

    void ExecuteDesappear()
    {
        //start desappear
        if (timer >= appearTime && isPlayerOff)
            StartCoroutine(DesappearTimer());

        //execute desappear
        if (isDesappear)
        {
            float scale = (1 - (timer / appearTime)) * defaultScale;
            this.transform.localScale = new Vector3(scale, scale, 1);
        }

        //verify if player is exit
        if (pData.state != pData.airMagic)
            isPlayerOff = true;
    }

    #endregion
}
