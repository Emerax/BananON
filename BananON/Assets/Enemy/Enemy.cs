using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Enemy hunts down nearest player.
/// When they reach a player... THEY ATTACK!
/// </summary>
public class Enemy : MonoBehaviour, IOnPhotonViewPreNetDestroy {
    [SerializeField]
    private float attackDistance = 2;
    [SerializeField]
    private float attackCooldown = 2;
    private float attackTimer;

    private NavMeshAgent agent;
    private Animator anim;

    private VRPlayer target;

    public Vector2 volumeVariation = new Vector2(0.5f, 0.75f);
    public AudioClip[] capyBaraShot;
    public AudioClip[] capyBaraAttack;
    public AudioClip[] capyBaraIdle;

    private AudioSource audioSource;

    private float idleTime;
    private Vector2 maxMinIdle = new Vector2(4f, 7f);

    private bool noPlayersLeft;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        UpdateTargets();

        attackTimer = 0;
        idleTime = Random.Range(maxMinIdle.x, maxMinIdle.y);
        noPlayersLeft = false;
    }

    private void Update() {
        idleTime -= Time.deltaTime;
        if(idleTime < 0) {
            audioSource.PlayOneShot(capyBaraIdle[Random.Range(0, capyBaraIdle.Length)], Random.Range(volumeVariation.x, volumeVariation.y));
            idleTime = Random.Range(maxMinIdle.x, maxMinIdle.y);
        }
        if(!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) return;

        //Hunt player
        if(target) {
            agent.SetDestination(target.transform.position);

            //Handle attack-behaviour and animation
            if(attackTimer >= 0)
                attackTimer -= Time.deltaTime;
            Vector3 toTarget = target.transform.position - transform.position;
            float distance = toTarget.magnitude;

            if(attackTimer <= 0 && distance < attackDistance) {
                attackTimer = attackCooldown;
                anim.SetTrigger("Attack");
                //Only plays for host?
                audioSource.PlayOneShot(capyBaraAttack[Random.Range(0, capyBaraAttack.Length)], Random.Range(0.1f, 0.13f));
            }
        }
        else {
            UpdateTargets();
            if(!target) {
                Debug.LogWarning("No Players for enemy to target");
            }
        }

    }

    public void GetHit(Vector3 launchDir, float launchStrength) {
        audioSource.PlayOneShot(capyBaraShot[Random.Range(0,capyBaraShot.Length)], Random.Range(volumeVariation.x, volumeVariation.y));
        if(!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) return;

        //When enemies are hit, they die.
        object[] spawnParams = { launchDir * launchStrength };
        PhotonNetwork.Instantiate("Capybara Ded", transform.position, transform.rotation, data: spawnParams);

        PhotonNetwork.Destroy(gameObject);
    }

   

    public void UpdateTargets() {
        if(!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) return;

        //Enemy targets closest player
        VRPlayer[] players = FindObjectsOfType<VRPlayer>();
        float minDist = float.MaxValue;
        for(int i = 0; i < players.Length; i++) {
            if(!players[i]) continue;
            if(players[i].IsDead) continue;

            float currentDist = Vector3.Distance(
                transform.position, players[i].transform.position);
            if(currentDist < minDist) {
                minDist = currentDist;
                target = players[i];
            }
        }

        if(!target) {
            noPlayersLeft = true;
            //TODO: Celebrate
        }
    }

    public void OnPreNetDestroy(PhotonView rootView) {
        //Play sound alternative. Not in use currently
        //if(audioSource && audioDeath) {
        //    audioSource.PlayOneShot(audioDeath);
        //}
        //else {
        //    Debug.LogWarning("Enemy " + name + " does not have audio clip or audio source set up.");
        //}

    }

    public void ResetGame() {
        PhotonNetwork.Destroy(gameObject);
    }
}
