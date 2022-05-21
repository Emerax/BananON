using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls hand poses.
/// Squeeze floats control different fingers on a 0-1 scale.
/// 0 means closed, 1 means open.
/// </summary>
public class Hand : MonoBehaviour{
    public float ThumbSqueeze {
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
        handAnim.SetFloat("Thumb Squeeze", ThumbSqueeze);
        handAnim.SetFloat("Pointer Squeeze", PointerSqueeze);
        handAnim.SetFloat("Fingers Squeeze", FingerSqueeze);
    }
}
