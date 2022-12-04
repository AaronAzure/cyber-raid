using UnityEngine;
using Rewired;

public class PreviewControls : MonoBehaviour
{
	public Rewired.Player player;
	public int playerId;
	public PreviewOverlay manager;
	[SerializeField] GameObject readyObj;


    // Start is called before the first frame update
    void Start()
    {
		player = ReInput.players.GetPlayer(playerId);
		readyObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("Start"))
		{
			manager.ReadyUp();
			readyObj.SetActive(true);
			this.enabled = false;
		}
    }
}
