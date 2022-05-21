using Photon.Pun;
using System.Collections;
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

    private PhotonView photonView;

    private InputDevice device;
    private Hand hand;
    public ControllerType Type {
        get => type;
    }

    void Awake() {
        photonView = GetComponent<PhotonView>();
    }

    void Start() {
        XRNode node = XRNode.Head;
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
        if(!photonView.IsMine) {
            return;
        }
        if(type != ControllerType.HEAD) {
            UpdateDeviceLocalPose();
            UpdateHandInput();
        }
    }

    private void OnPreRender() {
        if (type == ControllerType.HEAD) {
            UpdateDeviceLocalPose();
        }
    }

    public void Init(bool localObject) {
        if(localObject) {
            //This object owned by this client.
            if(type == ControllerType.HEAD) {
                //Destroy local head so it does not obscure camera.
                Destroy(transform.Find("HeadVisuals").gameObject);
            }
        }
        else {
            //This object someone else's
            if(type == ControllerType.HEAD) {
                Destroy(GetComponent<Camera>());
            }
        }
    }

    private void UpdateDeviceLocalPose() {
        if (!photonView.IsMine) {
            return;
        }
        bool posOK = device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
        bool rotOK = device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);
        if (posOK && rotOK) {
            transform.localPosition = position;
            transform.localRotation = rotation;
            //transform.SetPositionAndRotation(position, rotation);
        }
        else {
            Debug.LogError($"Failed to get pose for device {name}. Got Position: {posOK}, rotation: {rotOK}");
        }
    }

    private void UpdateHandInput() {
        if(!photonView.IsMine) {
            return;
        }

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
}
