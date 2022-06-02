using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    void Start()
    {
        GameObject instance = Instantiate(player);
        instance.name = "Player";
    }
}
