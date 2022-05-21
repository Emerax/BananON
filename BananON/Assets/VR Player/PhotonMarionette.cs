using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PhotonMarionette : MonoBehaviour {
    public enum ControllerType {
        HEAD,
        RIGHT,
        LEFT
    }

    [SerializeField]
    private ControllerType type;

    public ControllerType Type {
        get => type;
    }

    public void Init(int ownerActorNumber) {
        if(ownerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber) {
            Destroy(GetComponent<TrackedPoseDriver>());
            if (type == ControllerType.HEAD) {
                Destroy(transform.Find("Camera Offset").gameObject);
            }
        }
        else {
            if (type == ControllerType.HEAD) {
                Destroy(transform.Find("HeadVisuals").gameObject);
            }
        }
    }
}
