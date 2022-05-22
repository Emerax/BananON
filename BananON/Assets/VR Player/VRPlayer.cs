using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class VRPlayer : MonoBehaviour, IPunInstantiateMagicCallback {
    private Dictionary<PhotonMarionette.ControllerType, PhotonMarionette> controllers = new Dictionary<PhotonMarionette.ControllerType, PhotonMarionette>();

    private float hitPoints;
    public float maxHitPoints = 10;
    private bool isDead;

    public GameObject ghostHead;
    public GameObject livingBody;
    public Transform head;
    public Transform hurtBox;
    private Vector3 startOffset;

    private void Awake() {
        hitPoints = maxHitPoints;
        isDead = false;

        startOffset = hurtBox.position - head.position;
        ghostHead.SetActive(false);
    }

    void Update() {

    }

    private void FixedUpdate() {
        if(isDead) return;

        Vector3 bodyPos = hurtBox.position;
        Vector3 headPos = head.position;

        bodyPos.x = headPos.x;
        bodyPos.z = headPos.z;

        hurtBox.position = bodyPos;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        object[] spawnParams = info.photonView.InstantiationData;
        int ownerNumber = (int)spawnParams[0];
        foreach(PhotonMarionette marionette in GetComponentsInChildren<PhotonMarionette>()) {
            controllers[marionette.Type] = marionette;
            marionette.Init(ownerNumber == PhotonNetwork.LocalPlayer.ActorNumber);
        }

        if (PhotonNetwork.IsMasterClient) {
            FindObjectOfType<StartResetButton>().RegisterHost(controllers[PhotonMarionette.ControllerType.HEAD]);
        }
    }

    public void TakeDamage(float dmg) {
        hitPoints -= dmg;
        if(hitPoints <= 0) {
            isDead = true;
            if(ghostHead)
               ghostHead.SetActive(true);
            if(livingBody)
                livingBody.SetActive(false);
        }
    }

    public bool IsDead() {
        return isDead;
    }

    public float GetNormalizedHP() {
        return hitPoints / maxHitPoints;
    }
    public float GetCurrentHP() {
        return hitPoints;
    }
}
