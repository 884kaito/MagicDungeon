using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider shpBar;
    [SerializeField] private Slider mpBar;
    private PlayerData data;

    void Start()
    {
        data = FindObjectOfType<PlayerData>();

        StartCoroutine(LateStart());
    }

    //wait 1 frame to atualize maxHp in PlayerData
    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();

        UpdateBarMax();
        MaximizeBar();
    }

    void Update()
    {
        UpdateBarValue();

        moneyText.text = GameManager.inst.money.ToString();
    }

    public void UpdateBarMax()
    {
        hpBar.maxValue = data.maxHp;
        mpBar.maxValue = data.maxMp;
        shpBar.maxValue = data.maxShp;
    }

    public void MaximizeBar()
    {
        data.hp = data.maxHp;
        data.mp = data.maxMp;
        data.shp = data.maxShp;
        UpdateBarValue();
    }

    public void UpdateBarValue()
    {
        hpBar.value = data.hp;
        mpBar.value = data.mp;
        shpBar.value = data.shp;
    }
}
