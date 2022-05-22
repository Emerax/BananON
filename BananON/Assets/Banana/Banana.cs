using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Banana : MonoBehaviour, IPunInstantiateMagicCallback, IOnPhotonViewOwnerChange {
    private float scale;
    private float growthRate;
    private MeshRenderer meshy;
    public bool isGrowing;
    public BananaSpawner spawnerObject;
    private float delay = 2f;
    private Rigidbody rigBody;
    private PhotonView view;
    private PhotonMarionette localHolder;

    void Awake() {
        meshy = GetComponentInChildren<MeshRenderer>();
        spawnerObject = BananaSpawner.Instance;
        rigBody = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
    }


    // Start is called before the first frame update
    void Start() {
    }

    private void OnEnable() {
        view.AddCallbackTarget(this);
    }

    private void OnDisable() {
        view.RemoveCallbackTarget(this);
    }

    private void OnDestroy() {
        spawnerObject.UnregisterBanana(this);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        scale = 0;
        growthRate = (float)info.photonView.InstantiationData[0];
        isGrowing = true;
        transform.localScale = new Vector3(0, 0, 0);
        meshy.material.color = spawnerObject.unripeBanana;
    }

    public void OnOwnerChange(Player newOwner, Player previousOwner) {
        if (localHolder != null && newOwner != PhotonNetwork.LocalPlayer) {

        }
    }

    public void FireBanana() {
        Vector3 spawnPos = transform.position;
        Quaternion spawnRotation = transform.rotation;
        PhotonNetwork.Instantiate("BananaProjectile", spawnPos, spawnRotation);
        if(view.IsMine) {
            PhotonNetwork.Destroy(gameObject);
            SpawnPeel();
        }
    }

    public void OnGraspBegin(PhotonMarionette hand) {
        view.TransferOwnership(PhotonNetwork.LocalPlayer);
        if (localHolder != null) {
            localHolder.OnBananaStolen();
        }
        localHolder = hand;
        rigBody.isKinematic = true;
    }

    public void OnGraspEnd(Vector3 releaseVelocity) {
        rigBody.isKinematic = false;
        rigBody.velocity = releaseVelocity;
        localHolder = null;
    }

    public bool SqueezeBanana(float yourBoat) {
        if(yourBoat > 0.9f) {
            FireBanana();
            return true;
        }
        return false;
    }

    private void SpawnPeel() {
        Vector3 spawnPos = transform.position;
        spawnPos += Vector3.up * 0.5f;
        Quaternion spawnRotation = transform.rotation;
        PhotonNetwork.Instantiate("Peel", spawnPos, spawnRotation);
    }

    // Update is called once per frame
    void Update() {
        if(isGrowing) {
            scale += growthRate * Time.deltaTime;
            transform.localScale = new Vector3(scale, scale, scale);
            meshy.material.color = Color.Lerp(spawnerObject.unripeBanana, spawnerObject.ripeBanana, scale);
            if(scale > 1) {
                rigBody.isKinematic = false;
                rigBody.useGravity = true;
                meshy.material.color = spawnerObject.ripeBanana;
                isGrowing = false;
            }
        }
    }
}
