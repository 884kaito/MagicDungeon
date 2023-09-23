using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    //state
    [HideInInspector]
    public enum State
    {
        Move,
        Fright,
        Search,
        Die
    }
    [HideInInspector] public State state = State.Move;
    [HideInInspector] public State move = State.Move;
    [HideInInspector] public State fright = State.Fright;
    [HideInInspector] public State search = State.Search;
    [HideInInspector] public State die = State.Die;
}
