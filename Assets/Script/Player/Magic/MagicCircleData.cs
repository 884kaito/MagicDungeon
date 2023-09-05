using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleData : MonoBehaviour
{
    public string type;
    public float duration;
    public float speed;
    public float mp;
    public float scale;

    [HideInInspector] public float degree;
    [HideInInspector] public bool isMiddle = false;

    private PlayerData pData;




    private void Awake()
    {
        pData = FindObjectOfType<PlayerData>();
    }

    void Update()
    {
        ExecuteAppear();

        ExecuteDesappear();

        timer += Time.deltaTime;
    }



    private bool isAppear;
    private bool isDesappear;
    private float timer;

    public void StartAppear()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        //start appear
        isAppear = true;
        timer = 0;

        //wait magic circle grow
        yield return new WaitForSeconds(speed);

        //end appear
        this.transform.localScale = new Vector3(scale, scale, 1);
        isAppear = false;

        //wait for some seconds
        yield return new WaitForSeconds(duration / 2);

        isMiddle = true;

        //wait for some seconds
        yield return new WaitForSeconds(duration / 2);


        //start desappear
        isDesappear = true;
        timer = 0;

        //wait desappear
        yield return new WaitForSeconds(speed);

        //end desappear
        isDesappear = false;
        Destroy(this.gameObject);
    }

    void ExecuteAppear()
    {
        if (isAppear)
        {
            float realScale = timer / speed * scale;
            this.transform.localScale = new Vector3(realScale, realScale, 1);
        }
    }

    void ExecuteDesappear()
    {
        if (isDesappear)
        {
            float realScale = (1 - (timer / speed)) * scale;
            this.transform.localScale = new Vector3(realScale, realScale, 1);
        }
    }



}
