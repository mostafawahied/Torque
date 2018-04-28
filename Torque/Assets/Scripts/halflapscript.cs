using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class halflapscript : MonoBehaviour {

    public GameObject LapCompleteTrig;
    public GameObject HalfLapTrig;
    public void OnTriggerEnter(Collider other)
    {
        
            LapCompleteTrig.SetActive(true);
            HalfLapTrig.SetActive(false);
    }
}
