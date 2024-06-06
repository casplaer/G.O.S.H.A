using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaeyrSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        playerPrefab.transform.position = transform.position;

    }
}
