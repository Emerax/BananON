using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaSpawner : MonoBehaviour {
    public static BananaSpawner Instance;
    public GameObject bananaPrefab;
    public List<GameObject> bananaPositions;
    public int bananaLimit;
    public float maxGrowthRate;
    public float minGrowthRate;
    private List<Banana> spawnedBananas;
    public Color unripeBanana;
    public Color ripeBanana;

    void Awake() {
        if(Instance == null) Instance = this;
    }

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
        float bananaRotation = Random.Range(0f, 360f);
        float xShift = Mathf.Cos(bananaRotation)*0.3f;
        float yShift = Mathf.Sin(bananaRotation)*0.3f;
        spawnPos += new Vector3(xShift, 0, yShift);
        Quaternion spawnRotation = Quaternion.Euler(0, bananaRotation, -90f);
        object[] spawnParams = { Random.Range(minGrowthRate, maxGrowthRate)};
        GameObject bananaObj = PhotonNetwork.Instantiate("Banana", spawnPos, spawnRotation, data:spawnParams);
        Banana banana = bananaObj.GetComponent<Banana>();
        spawnedBananas.Add(banana);
    }
}
