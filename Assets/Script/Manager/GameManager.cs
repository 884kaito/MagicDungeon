using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//storage and manage datas that envolve all game
public class GameManager : MonoBehaviour
{
    //tag names
    [HideInInspector] public string terrainTag = "Ground";
    [HideInInspector] public string playerTag = "Player";
    [HideInInspector] public string enemyTag = "Enemy";
    [HideInInspector] public string pAttackTag = "PlayerAttack";
    [HideInInspector] public string eAttackTag = "EnemyAttack";

    //time
    [HideInInspector] public float timeScale = 1;

    public IEnumerator StopForSeconds(float stopTime)
    {
        timeScale = 0;
        Time.timeScale = timeScale;

        yield return new WaitForSecondsRealtime(stopTime);

        timeScale = 1;
        Time.timeScale = timeScale;
    }

    public void setTimeScale(float scale)
    {
        timeScale = scale;
        Time.timeScale = timeScale;
    }



    //command to erase other gameManeger in scene
    public static GameManager inst = null;
    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
