using Photon.Pun;
using UnityEngine;

public class BananaProjectile : MonoBehaviour, IPunInstantiateMagicCallback {

    private float lifeTime;
    private PhotonView photonView;
    public float startingLifeTime;
    public float absoluteVelocity;
    private Rigidbody rBody;

    void Awake() {
        rBody = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        lifeTime = startingLifeTime;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) {
    }

    private void OnTriggerEnter(Collider collision) {
        if (photonView.IsMine) {
            if(collision.CompareTag("Enemy")) {
                Enemy enemy = collision.GetComponent<Enemy>();
                if(enemy != null) {

                    enemy.GetHit(transform.forward, absoluteVelocity);
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if(photonView.IsMine) {
            lifeTime -= Time.deltaTime;
            if(lifeTime < 0) PhotonNetwork.Destroy(gameObject);
            rBody.MovePosition(transform.position += absoluteVelocity * Time.deltaTime * transform.forward);
        }
    }
}
