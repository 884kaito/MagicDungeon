using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controle core object
public class Core : MonoBehaviour
{
    Rigidbody body;
    [SerializeField] float gravity;
    [SerializeField] float inicialSpeed;
    [SerializeField] float shpHeal;
    [SerializeField] Color color;
    


    void Start()
    {
        body = GetComponent<Rigidbody>();
        GetComponent<MeshRenderer>().material.color = color;

        //set calculate and set inicial vector
        Vector2 inicialVector = new Vector3(
            Random.Range(-1f, 1f), Random.Range(0f, 1f), 0).normalized;
        body.AddForce(inicialSpeed * inicialVector);

        //change color
        GetComponent<MeshRenderer>().material.color = color;
    }

    void Update()
    {
        //add gravity
        body.AddForce(0, gravity, 0);
    }

    //if hit player, destroy and increase shp
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameManager.inst.playerTag))
        {
            FindObjectOfType<PlayerData>().ShpHeal(shpHeal);
            Destroy(this.gameObject);
        }
    }
}
