using UnityEngine;
using System.Collections;

public class RG_MainMenu : MonoBehaviour {

	public GameObject netmanager;

	void Start(){
		if(GameObject.Find("Network Manager") == null){
			Instantiate(netmanager, transform.position, transform.rotation);
		}
	}

}