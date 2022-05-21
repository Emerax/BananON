using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Banana")) {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
