using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;

public class PhotonMarionette : MonoBehaviour {
    public enum ControllerType {
        HEAD,
        RIGHT,
        LEFT
    }

    [SerializeField]
    private ControllerType type;

    private InputDevice device;

    public ControllerType Type {
        get => type;
    }

    void Start() {
        XRNode node = XRNode.CenterEye;
        switch(type) {
            case ControllerType.HEAD:
                break;
            case ControllerType.RIGHT:
                node = XRNode.RightHand;
                break;
            case ControllerType.LEFT:
                node = XRNode.LeftHand;
                break;
            default:
                break;
        }

        if (node != XRNode.CenterEye) {
            device = InputDevices.GetDeviceAtXRNode(node);
            if (device == null) {
                Debug.LogError("RUH ROW SCOOBS.");
            }
        } 
    }

    public void Init(int ownerActorNumber) {
        if(ownerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber) {
            Destroy(GetComponent<TrackedPoseDriver>());
            if (type == ControllerType.HEAD) {
                Destroy(GetComponent<Camera>());
                Destroy(GetComponent<TrackedPoseDriver>());
            }
        }
        else {
            if (type == ControllerType.HEAD) {
                Destroy(transform.Find("HeadVisuals").gameObject);
            }
        }
    }
}
