using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls hand poses.
/// Squeeze floats control different fingers on a 0-1 scale.
/// 0 means closed, 1 means open.
/// </summary>
public class Hand : MonoBehaviour, IPunObservable {
    public bool ThumbSqueeze {
        get;
        set;
    }
    public float PointerSqueeze {
        get;
        set;
    }
    public float FingerSqueeze {
        get;
        set;
    }

    public Animator handAnim;

    private float thumbValue = 0;

    private void Awake() {
        //handAnim should be set up manually to be sure it links to correct child with animator.
        if (!handAnim) {
            Debug.LogWarning("No handAnim set up. GetComponentinChildren was used.");
            handAnim = GetComponentInChildren<Animator>();

            if(!handAnim) {
                Debug.LogError("No handAnim found.");
            }
        }
    }

    private void Update() {
        thumbValue = Mathf.MoveTowards(thumbValue, !ThumbSqueeze ? 1 : 0, 0.3f);
        handAnim.SetFloat("Thumb Squeeze", thumbValue);
        handAnim.SetFloat("Pointer Squeeze", PointerSqueeze);
        handAnim.SetFloat("Fingers Squeeze", FingerSqueeze);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        Debug.LogError("Sending hand animation data");
        if (stream.IsWriting) {
            stream.SendNext(ThumbSqueeze);
            stream.SendNext(PointerSqueeze);
            stream.SendNext(FingerSqueeze);
        }
        else {
            ThumbSqueeze = (bool)stream.ReceiveNext();
            PointerSqueeze = (float)stream.ReceiveNext();
            FingerSqueeze = (float)stream.ReceiveNext();
        }
    }
}
