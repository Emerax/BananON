using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    private float scale;
    private MeshRenderer meshy;
    public BananaSpawner spawnerObject;

    // Start is called before the first frame update
    void Start()
    {
        scale = 0;
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
        if(scale < 1) {
            scale += 0.5f * Time.deltaTime;
            transform.localScale = new Vector3(scale, scale, scale);
            meshy.material.color = Color.Lerp(spawnerObject.omogenBanana, spawnerObject.sexyBanana, scale);
        }
        else {
            spawnerObject.UnregisterBanana(this);
            GetComponent<Rigidbody>().useGravity = true;
            meshy.material.color = spawnerObject.sexyBanana;
            Debug.Log("THis is a fully grown banan");
        }

    }
}
