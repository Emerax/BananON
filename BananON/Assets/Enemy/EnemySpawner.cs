using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float spawnCooldown = 3;
    private float spawnTimer;

    [SerializeField]
    private float spawnRadius = 20;

    private void Awake() {
        spawnTimer = spawnCooldown;
    }

    private void Update() {
        if(!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0) {
            spawnTimer = spawnCooldown;

            Vector3 spawnPos = transform.position + Vector3.forward * spawnRadius;
            spawnPos = Quaternion.Euler(0, Random.Range(0, 360), 0) * spawnPos;
            PhotonNetwork.Instantiate("Enemy Cylinder", spawnPos, Quaternion.identity);
        }
    }
}
