using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    private float scale;
    private float growthRate;
    private MeshRenderer meshy;
    public bool isGrowing;
    public BananaSpawner spawnerObject;

    // Start is called before the first frame update
    void Start()
    {
        scale = 0;
        isGrowing = true;
        growthRate = Random.Range(0.02f, 0.05f);
        transform.localScale = new Vector3(0,0,0);
        meshy = GetComponentInChildren<MeshRenderer>();
        meshy.material.color = spawnerObject.omogenBanana;
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
                Debug.Log("THis is a fully grown banan");
            }
        }

    }
}
