using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float attackDistance = 2;
    [SerializeField]
    private float attackCooldown = 2;
    private float attackTimer;
    private NavMeshAgent agent;
    private Animator anim;

    private VRPlayer target;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        UpdateTargets();

        attackTimer = 0;
    }

    private void Update() {
        if(!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) return;

        if(target) {
            agent.SetDestination(target.transform.position);

            if(attackTimer >= 0)
                attackTimer -= Time.deltaTime;
            float distance = Vector3.Distance(
                transform.position, target.transform.position);
            if(attackTimer <= 0 && distance < attackDistance) {
                attackTimer = attackCooldown;
                anim.SetTrigger("Attack");
            }
        }
        else {
            Debug.LogWarning("No Players for enemy to target");
        }
    }

    public void GetHit() {
        if(!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) return;

        PhotonNetwork.Destroy(gameObject);
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
