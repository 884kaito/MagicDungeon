using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Armazena dados de um key
/// </summary>

[System.Serializable]
public class KeyData : MonoBehaviour
{
    public string code;
    public bool on;
    public bool down;
    public bool before;

    private void Awake()
    {
        code = "";
        on = false;
        down = false;
        before = false;
    }
}
