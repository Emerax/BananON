using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartResetButton : MonoBehaviourPunCallbacks {
    public float treeDist = 1.5f;
    private Vector3 spawnerPos;
    private PhotonMarionette hostHead;

    void Awake() {
        Vector3 treePos = FindObjectOfType<BananaSpawner>().transform.position;
        spawnerPos = new Vector3(treePos.x, 0 , treePos.z);
    }

    public override void OnJoinedRoom() {
        if (!PhotonNetwork.IsMasterClient) {
            Destroy(gameObject);
            return;
        }
    }

    private void Update() {
        if (hostHead != null) {
            Vector3 direction = (hostHead.transform.position - spawnerPos).normalized;
            Vector3 position = spawnerPos + direction * treeDist;
            position.y = hostHead.transform.position.y;
            transform.SetPositionAndRotation(position, Quaternion.LookRotation(hostHead.transform.position - position));
        }
    }

    public void RegisterHost(PhotonMarionette playerHead) {
        hostHead = playerHead;
    }

    public void StartGame() {
        Debug.Log("Starting game!");
    }
}
