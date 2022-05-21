using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peel : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 5;
    private float lifeTimeTimer;

    private PhotonView view;
    [SerializeField]
    private float launchStrength = 30;
    [SerializeField]
    private float launchUpAngle = 25;

    private void Awake() {
        lifeTimeTimer = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision) {
        if(!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) return;

        if(collision.collider.tag == "Enemy" ){
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            if(enemy != null) {

                Vector3 launchDir = Quaternion.Euler(launchUpAngle, Random.Range(0, 360), 0) * Vector3.forward;
                enemy.GetHit(launchDir, launchStrength);
                //PhotonNetwork.Destroy(gameObject);
                GetComponent<Rigidbody>().AddForce(
                    Quaternion.Euler(0,180,0) * launchDir * launchStrength, ForceMode.Impulse);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if(!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) return;
        lifeTimeTimer += Time.deltaTime;
        if(lifeTimeTimer > lifeTime) {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
