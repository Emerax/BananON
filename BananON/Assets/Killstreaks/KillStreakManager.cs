using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillStreakManager : MonoBehaviour
{
    private int killStreakAmount;
    private float killStreakTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(killStreakTime > 0) {
            killStreakTime -= Time.deltaTime;
        }
        if (killStreakTime <= 0) {
            killStreakAmount = 0;
        }
    }

    public void onKill() {
        killStreakAmount += 1;
        killStreakTime = 5f;
    }
}
