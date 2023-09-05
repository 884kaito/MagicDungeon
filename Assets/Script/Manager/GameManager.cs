using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public string terrainTag = "Ground";
    [HideInInspector] public string playerTag = "Player";
    [HideInInspector] public string enemyTag = "Enemy";


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
