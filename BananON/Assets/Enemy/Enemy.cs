using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    private VRPlayer target;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        UpdateTargets();
    }

    private void Update() {
        if(!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) return;

        if(target)
            agent.SetDestination(target.transform.position);
        else
            Debug.LogWarning("No Players for enemy to target");
    }

    private void OnTriggerEnter(Collider other) {
        if(!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) return;

        if (other.CompareTag("Banana")) {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void UpdateTargets() {
        if(!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) return;

        //Enemy targets closest player
        VRPlayer[] players = FindObjectsOfType<VRPlayer>();
        float minDist = float.MaxValue;
        for(int i = 0; i < players.Length; i++) {
            if(!players[i]) continue;

            float currentDist = Vector3.Distance(
                transform.position, players[i].transform.position);
            if(currentDist < minDist) {
                minDist = currentDist;
                target = players[i];
            }
        }
    }   
}
