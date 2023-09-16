using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackData : MonoBehaviour
{

    [SerializeField] float damage;
    public enum Owner
    {
        Player,
        Enemy
    }
    [SerializeField] Owner owner;




    public struct Data
    {
        public float damage;
        public Owner owner;
    }

    [HideInInspector] public Data data;




    private void Start()
    {
        data.damage = damage;
        data.owner = owner;
    }
}