using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildButtonManager : MonoBehaviour {

	public string title;
	public float price;
	public Text titleText;
	public Text priceText;

	private GameStateStore gss;
	private Button button;


	void Start () {	

		// remember game state ferences
		GameObject gameManager = GameObject.Find ("GameManager");
		gss = gameManager.GetComponent<GameStateStore> ();

		// setup click handler to change active build button in game state
		button = gameObject.GetComponent<Button> ();
		button.onClick.AddListener(delegate { updateActiveBuildButton(); });

		// update button text w/ inspector attributes
		titleText.text = title;
		priceText.text = "$" + price.ToString ("F2");

	}


	public void updateActiveBuildButton(){
		// TODO send message to game state mgr for active build button
		Debug.Log (this.title + " is now the active build button");

		LabItem item = gss.getLabItem (this.title);
		if (item != null) {
			gss.setActiveBuildItem(item);
		}
	}



}
