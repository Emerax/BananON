using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class VRPlayer : MonoBehaviour, IPunInstantiateMagicCallback {
    private Dictionary<PhotonMarionette.ControllerType, PhotonMarionette> controllers = new Dictionary<PhotonMarionette.ControllerType, PhotonMarionette>();

    // Update is called once per frame
    void Update() {

    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        object[] spawnParams = info.photonView.InstantiationData;
        foreach(PhotonMarionette marionette in GetComponentsInChildren<PhotonMarionette>()) {
            controllers[marionette.Type] = marionette;
            marionette.Init((int)spawnParams[0]);

        }
    }
}
