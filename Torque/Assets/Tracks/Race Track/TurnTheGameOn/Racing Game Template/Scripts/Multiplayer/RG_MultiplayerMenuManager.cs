namespace UnityEngine.Networking{

	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public class RG_MultiplayerMenuManager : MonoBehaviour {

		public RG_NetworkManager rg_networkManager;
		public NetworkManager manager;


		[Header("Menu Windows")]
		public GameObject gameModeWindow;
		public GameObject matchmakingWindow;
		public GameObject connectIPWindow;


		public void Awake(){
			manager = GameObject.Find("Network Manager").GetComponent<NetworkManager>();
			rg_networkManager = GameObject.Find("Network Manager").GetComponent<RG_NetworkManager>();
		}

		public void MatchmakingWindow(){
			if (matchmakingWindow.activeInHierarchy) {
				matchmakingWindow.SetActive (false);
				//rg_networkManager.StopMatchMaker();
				//rg_networkManager.StopMatch();
				//StopMatchMaker();
				manager.StopMatchMaker();
				gameModeWindow.SetActive(true);
			} else {
				gameModeWindow.SetActive(false);
				matchmakingWindow.SetActive(true);
				rg_networkManager.SetupMatchmakingButtons();
				//rg_networkManager.StartMatchMaker ();
				//rg_networkManager.StartMatch();
				manager.StartMatchMaker();
			}
		}

		public void ConnectIPWindow(){
			if (connectIPWindow.activeInHierarchy) {
				connectIPWindow.SetActive(false);
				gameModeWindow.SetActive(true);
			} else {
				gameModeWindow.SetActive(false);
				connectIPWindow.SetActive(true);
				rg_networkManager.SetupConnectIPButtons();
			}
		}

	}

}