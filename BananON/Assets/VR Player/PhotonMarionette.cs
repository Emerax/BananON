using Photon.Pun;
using Unity.XR.Oculus;
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
    private Hand hand;

    public ControllerType Type {
        get => type;
    }

    void Start() {
        XRNode node = XRNode.CenterEye;
        switch(type) {
            case ControllerType.HEAD:
                break;
            case ControllerType.RIGHT:
                hand = GetComponentInChildren<Hand>();
                node = XRNode.RightHand;
                break;
            case ControllerType.LEFT:
                hand = GetComponentInChildren<Hand>();
                node = XRNode.LeftHand;
                break;
        }

        if(node != XRNode.CenterEye) {
            device = InputDevices.GetDeviceAtXRNode(node);
            if(device == null) {
                Debug.LogError("RUH ROW SCOOBS.");
            }
        }
    }

    void Update() {
        if(device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue)) {
            hand.PointerSqueeze = 1 - triggerValue;
        }
        if(device.TryGetFeatureValue(CommonUsages.grip, out float gripValue)) {
            hand.FingerSqueeze = 1 - gripValue;
        }
        device.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bool primaryAxisTouched);
        device.TryGetFeatureValue(CommonUsages.secondary2DAxisTouch, out bool secondaryAxisTouched);
        device.TryGetFeatureValue(CommonUsages.primaryTouch, out bool primaryTouched);
        device.TryGetFeatureValue(CommonUsages.secondaryTouch, out bool secondaryTouched);
        hand.ThumbSqueeze = primaryAxisTouched || secondaryAxisTouched || primaryTouched || secondaryTouched;
    }

    public void Init(int ownerActorNumber) {
        if(ownerActorNumber != PhotonNetwork.LocalPlayer.ActorNumber) {
            Destroy(GetComponent<TrackedPoseDriver>());
            if(type == ControllerType.HEAD) {
                Destroy(GetComponent<Camera>());
                Destroy(GetComponent<TrackedPoseDriver>());
            }
        }
        else {
            if(type == ControllerType.HEAD) {
                Destroy(transform.Find("HeadVisuals").gameObject);
            }
        }
    }
}
