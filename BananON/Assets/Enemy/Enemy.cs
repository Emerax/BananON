using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform goal;
    private NavMeshAgent agent;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if(goal) {
            agent.destination = goal.position;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Banana")) {
            Destroy(gameObject);
        }
    }
}
