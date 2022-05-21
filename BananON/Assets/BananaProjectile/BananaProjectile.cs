using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaProjectile : MonoBehaviour, IPunInstantiateMagicCallback {

    private float lifeTime;
    private PhotonView photonView;
    public float startingLifeTime;
    public float absoluteVelocity;

    void Awake() {
        photonView = GetComponent<PhotonView>();
        lifeTime = startingLifeTime;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) {

    }

    private 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine) {
            lifeTime -= Time.deltaTime;
            if(lifeTime < 0) PhotonNetwork.Destroy(gameObject);

            transform.Translate(transform.forward * absoluteVelocity * Time.deltaTime);
        }
    }
}
