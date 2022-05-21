using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDed : MonoBehaviour, IPunInstantiateMagicCallback
{
    [SerializeField]
    private float lifeTime = 5;
    private float lifeTimeTimer;

    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        Vector3 launch = (Vector3)info.photonView.InstantiationData[0];
        GetComponent<Rigidbody>().AddForce(launch, ForceMode.Impulse);
    }

    private void Awake() {
        lifeTimeTimer = 0;
    }

    private void Update() {
        if(!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) return;
        lifeTimeTimer += Time.deltaTime;

        if (lifeTimeTimer > lifeTime) {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
