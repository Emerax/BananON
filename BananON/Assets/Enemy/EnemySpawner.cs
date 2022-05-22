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

    private bool hasStarted;

    private void Awake() {
        spawnTimer = spawnCooldown;
        hasStarted = false;
    }

    private void Update() {
        if(!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) return;

        if(!hasStarted) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0) {
            spawnTimer = spawnCooldown;

            Vector3 spawnPos = transform.position + Vector3.forward * spawnRadius;
            spawnPos = Quaternion.Euler(0, Random.Range(0, 360), 0) * spawnPos;
            PhotonNetwork.Instantiate("Enemy Cylinder", spawnPos, Quaternion.identity);
        }
    }

    public void ResetGame() {
        Enemy[] instantiated = FindObjectsOfType<Enemy>();
        for(int i = 0; i < instantiated.Length; i++) {
            if(!instantiated[i]) continue;

            PhotonNetwork.Destroy(instantiated[i].gameObject);
        }

        spawnTimer = spawnCooldown;

        hasStarted = true;
    }

    public void EndGame() {
        hasStarted = false;
    }
}
