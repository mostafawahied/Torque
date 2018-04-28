using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStable : MonoBehaviour {
    public GameObject TheCar;
    public float CarY;

    void Update()
    {
        CarY = TheCar.transform.eulerAngles.y;

        transform.eulerAngles = new Vector3(0f, CarY, 0f);

    }
}
