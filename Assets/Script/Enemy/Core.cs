using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        Vector2 inicialVector = new Vector3(
            Random.Range(-1f, 1f), Random.Range(0f, 1f), 0).normalized;

        body.AddForce(inicialSpeed * inicialVector);
    }

    void Update()
    {
        GetComponent<MeshRenderer>().material.color = color;
        body.AddForce(0, gravity, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameManager.inst.playerTag))
        {
            FindObjectOfType<PlayerData>().ShpHeal(shpHeal);
            Destroy(this.gameObject);
        }
    }
}
