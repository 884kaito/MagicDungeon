using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// storage player frequently used parameters
/// </summary>
public class PlayerData : MonoBehaviour
{

    [HideInInspector] public bool isRight = true;
    public enum State {
        Idle,
        Run,
        Jump,
        Fall
    };
    [HideInInspector] public State state = State.Idle;
    [HideInInspector] public State idle = State.Idle;
    [HideInInspector] public State run = State.Run;
    [HideInInspector] public State jump = State.Jump;
    [HideInInspector] public State fall = State.Fall;


    void Start()
    {
        
    }
}
