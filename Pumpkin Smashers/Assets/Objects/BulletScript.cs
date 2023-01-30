using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 30;
    private bool canDamage;

    private void Awake()
    {
        Destroy(this.gameObject, 3f);
        canDamage = false;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerExit(Collider other)
    {
        canDamage = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canDamage)
        {
            other.GetComponent<PlayerHealth>().TakeDamage(2);
            Destroy(this.gameObject);
        }
    }
}
