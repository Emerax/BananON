using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peel : MonoBehaviour
{
    private PhotonView view;


    private void Awake() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision) {
        Debug.Log("Collide");
        if(!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) return;

        if(collision.collider.tag == "Enemy" ){
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            if(enemy != null) {
                enemy.GetHit();
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
