using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class RG_NetworkMigrationManager : MonoBehaviour {

	public NetworkMigrationManager networkMigrationManager;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(networkMigrationManager.hostWasShutdown){

		}
		if(networkMigrationManager.disconnectedFromHost){
		//	if(){
				
		//	}
		}
	}
}
