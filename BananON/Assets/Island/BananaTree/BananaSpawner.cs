using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaSpawner : MonoBehaviour {
    public GameObject bananaPrefab;
    public List<GameObject> bananaPositions;
    public int bananaLimit;
    private List<Banana> spawnedBananas;
    public Color omogenBanana;
    public Color sexyBanana;


    // Start is called before the first frame update
    void Start() {
        spawnedBananas = new List<Banana>();
    }

    // Update is called once per frame
    void Update() {
        if(!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) return;

        if(spawnedBananas.Count < bananaLimit) {
            SpawnBanana();
        }
    }

    public bool UnregisterBanana(Banana banana) {
        return spawnedBananas.Remove(banana);
    }

    private void SpawnBanana() {
        int location = Random.Range(0, 8);
        Vector3 spawnPos = bananaPositions[location].transform.position;
        spawnPos += new Vector3(Random.Range(0f, 1f), -1, Random.Range(0f, 1f));
        Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0f, 360f), -90f);
        object[] spawnParams = { Random.Range(0.02f, 0.05f) };
        GameObject bananaObj = PhotonNetwork.Instantiate("Banana", spawnPos, spawnRotation, data:spawnParams);
        Banana banana = bananaObj.GetComponent<Banana>();
        banana.spawnerObject = this;
        spawnedBananas.Add(banana);
    }
}
