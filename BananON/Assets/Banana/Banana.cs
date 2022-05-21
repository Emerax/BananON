using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour, IPunInstantiateMagicCallback
{
    private float scale;
    private float growthRate;
    private MeshRenderer meshy;
    public bool isGrowing;
    public BananaSpawner spawnerObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        scale = 0;
        isGrowing = true;
        transform.localScale = new Vector3(0, 0, 0);
        meshy = GetComponentInChildren<MeshRenderer>();
        meshy.material.color = spawnerObject.omogenBanana;
        growthRate = (float)info.photonView.InstantiationData[0];
    }

    private void OnDestroy() {
        spawnerObject.UnregisterBanana(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrowing) {
            scale += growthRate * Time.deltaTime;
            transform.localScale = new Vector3(scale, scale, scale);
            meshy.material.color = Color.Lerp(spawnerObject.omogenBanana, spawnerObject.sexyBanana, scale);
            if(scale > 1) {
                GetComponent<Rigidbody>().useGravity = true;
                meshy.material.color = spawnerObject.sexyBanana;
                isGrowing = false;
            }
        }

    }
}
