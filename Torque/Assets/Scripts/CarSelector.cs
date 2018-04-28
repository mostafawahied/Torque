using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSelector : MonoBehaviour {
    public GameObject car1;
    public GameObject corvette;
    public GameObject camaro;
    public GameObject GTR;
    public int car_selected;

	void Start () {
        car1.SetActive(true);
        corvette.SetActive(false);
        camaro.SetActive(false);
        GTR.SetActive(false);

        car_selected= 1;
		
	}
    public void load_car1() {
        car1.SetActive(true);
        corvette.SetActive(false);
        camaro.SetActive(false);
        GTR.SetActive(false);

        car_selected = 1;
    }
    public void load_corvette() {
        car1.SetActive(false);
        corvette.SetActive(true);
        camaro.SetActive(false);
        GTR.SetActive(false);

        car_selected = 2;
    }
    public void load_camaro() {
        car1.SetActive(false);
        corvette.SetActive(false);
        camaro.SetActive(true);
        GTR.SetActive(false);

        car_selected = 3;
    }
    public void load_GTR() {
        car1.SetActive(false);
        corvette.SetActive(false);
        camaro.SetActive(false);
        GTR.SetActive(true);

        car_selected = 4;
    }


}
