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
        spawnPos += new Vector3(Random.Range(0f,0.2f), 0, Random.Range(0f, 0.2f));
        Quaternion spawnRotation = Quaternion.Euler(0, bananaRotation, 0);
        object[] spawnParams = { Random.Range(minGrowthRate, maxGrowthRate)};
        GameObject bananaObj = PhotonNetwork.Instantiate("Banana", spawnPos, spawnRotation, data:spawnParams);
        Banana banana = bananaObj.GetComponent<Banana>();
        spawnedBananas.Add(banana);
    }

    public void ResetGame() {
        for(int i= 0; i < spawnedBananas.Count; i++) {
            Destroy(spawnedBananas[i].gameObject);
        }
        spawnedBananas.Clear();
    }
}
