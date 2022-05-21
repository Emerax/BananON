using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaSpawner : MonoBehaviour
{
    public GameObject bananaPrefab;
    public List<GameObject> bananaPositions;
    public int bananaLimit;
    private List<Banana> spawnedBananas;
    public Color omogenBanana;
    public Color sexyBanana;

    
    // Start is called before the first frame update
    void Start()
    {
        spawnedBananas = new List<Banana>();
    }

    // Update is called once per frame
    void Update()
    {
        //Every now and then rather than always pretty much
        if(spawnedBananas.Count < bananaLimit) {
            SpawnBanana();
        }
    }

    public bool UnregisterBanana(Banana banana) {
        return spawnedBananas.Remove(banana);
    }

    private void SpawnBanana() {
        Debug.Log("Spawning a banana!");
        int location = Random.Range(0, 8);
        GameObject bananaObj = Instantiate(bananaPrefab, bananaPositions[location].transform);
        bananaObj.transform.Translate(Random.Range(0f, 1f), -1, Random.Range(0f, 1f),Space.World);
        bananaObj.transform.Rotate(0,Random.Range(0f,360f),-90f) ;
        Banana banana = bananaObj.GetComponent<Banana>();
        banana.spawnerObject = this;
        spawnedBananas.Add(banana);
    }
}
