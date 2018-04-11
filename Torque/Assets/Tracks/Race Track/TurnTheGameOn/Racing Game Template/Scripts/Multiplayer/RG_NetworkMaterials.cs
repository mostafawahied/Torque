using UnityEngine;
using System.Collections;

public class RG_NetworkMaterials : MonoBehaviour {


	public Renderer objectRef;
	public Material[] instancedMaterials;
	public string[] playerConfigurable;
	public RG_NetPlayer playerReference;
	public int slot;

	void Awake () {
		playerReference = transform.root.GetComponent<RG_NetPlayer> ();
		objectRef = GetComponent<MeshRenderer> ();
		System.Array.Resize (ref instancedMaterials, objectRef.materials.Length);
		slot = playerReference.slot;
		for(int i = 0; i < objectRef.materials.Length; i++){
			
			instancedMaterials [i] = new Material (objectRef.materials[i]);
		}

	}

	public void UpdateMaterialColors(){


			for (int i = 0; i < playerConfigurable.Length; i++) {
				if (playerConfigurable [i] == "CARBODY") {					
					objectRef.materials [i].color = playerReference.carBodyColor;
				} else if (playerConfigurable [i] == "GLASS") {
					objectRef.materials [i].color = playerReference.carGlassColor;
				} else if (playerConfigurable [i] == "BRAKE") {
					objectRef.materials [i].color = playerReference.carBrakeColor;
				} else if (playerConfigurable [i] == "RIM") {
					objectRef.materials [i].color = playerReference.carRimColor;
				}
			}


	}


}
