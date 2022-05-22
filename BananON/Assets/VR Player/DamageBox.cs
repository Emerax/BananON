using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour {
    public VRPlayer vrPlayer;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Hitbox")) {
            vrPlayer.TakeDamage(1);
        }
    }
}
