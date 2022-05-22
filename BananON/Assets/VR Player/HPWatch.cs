using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPWatch : MonoBehaviour
{
    public Image hpImage;
    public TMP_Text text;

    public VRPlayer vrPlayerToTrack;

    private void Awake() {
        if(!vrPlayerToTrack)
            vrPlayerToTrack = GetComponentInParent<VRPlayer>();
    }

    private void Update() {
        hpImage.fillAmount = vrPlayerToTrack.GetNormalizedHP();
        text.text = vrPlayerToTrack.GetCurrentHP() + "/" + vrPlayerToTrack.maxHitPoints;
    }
}
