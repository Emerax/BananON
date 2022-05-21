using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour, IPunInstantiateMagicCallback {
    private float scale;
    private float growthRate;
    private MeshRenderer meshy;
    public bool isGrowing;
    public BananaSpawner spawnerObject;
    private float delay = 2f;
    private Rigidbody rigBody;
    private PhotonView view;

    void Awake() {
        meshy = GetComponentInChildren<MeshRenderer>();
        spawnerObject = BananaSpawner.Instance;
        rigBody = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
    }


    // Start is called before the first frame update
    void Start() {
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        scale = 0;
        growthRate = (float)info.photonView.InstantiationData[0];
        isGrowing = true;
        transform.localScale = new Vector3(0, 0, 0);
        meshy.material.color = spawnerObject.unripeBanana;
    }

    public void FireBanana() {
        Vector3 spawnPos = transform.position;
        Quaternion spawnRotation = transform.rotation;
        PhotonNetwork.Instantiate("BananaProjectile", spawnPos, spawnRotation);
        if(view.IsMine) {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void OnGraspBegin() {
        view.TransferOwnership(PhotonNetwork.LocalPlayer);
        rigBody.isKinematic = true;
    }

    public void OnGraspEnd() {
        rigBody.isKinematic = false;
    }

    private void OnDestroy() {
        spawnerObject.UnregisterBanana(this);
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
        ////Testing code for firing bananas
        //else {
        //    delay -= Time.deltaTime;
        //    if(delay < 0) {
        //        FireBanana();
        //    }
        //}
    }
}
