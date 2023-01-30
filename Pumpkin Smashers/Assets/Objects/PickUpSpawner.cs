using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pickUp;

    void Awake()
    {
        InvokeRepeating("SpawnPickUp", 5f, 10f);
    }

    private void SpawnPickUp ()
    {
        var position = new Vector3(Random.Range(-70, 70), Random.Range(2, 7), Random.Range(-40, 40));
        Instantiate(pickUp, position, Quaternion.identity);
    }
}
