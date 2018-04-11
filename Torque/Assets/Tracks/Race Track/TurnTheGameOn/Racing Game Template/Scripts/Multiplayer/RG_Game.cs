using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UnityEngine.Networking{
	public class RG_Game : MonoBehaviour {

		public NetworkManager manager;
		public int gameNumber;
		public Button button;
		public Text roomName;
		public Text mode;
		public Text map;
		public Text size;
		Vector2 rectsize;


		void Start(){
			GetComponent<LayoutElement>().minHeight = Screen.height / 10.4f;

			GetComponent<RectTransform> ().localScale = new Vector3(1,1,1);
			//manager = GameObject.Find ("LobbyManager").GetComponent<NetworkManager> ();

		}

		public void JoinGame(){
			//RG_GarageManager garageManager = GameObject.Find ("Garage Manager").GetComponent<RG_GarageManager> ();
			RG_NetworkManagerHUD managerHUD = GameObject.Find ("LobbyManager").GetComponent<RG_NetworkManagerHUD> ();
			//garageManager.uI.loadingWindow.SetActive (true);
			managerHUD.JoinMatch (gameNumber);
		}
	}
}