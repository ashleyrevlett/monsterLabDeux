using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildButtonManager : MonoBehaviour {

	/* Setup build button w/ price, text.
	 * Handle button press callback.
	 */

	public string title;
	public float price;
	public Text titleText;
	public Text priceText;

	private BoardManager board;
	private Button button;
	private GameStateStore gss;


	void Start () {	

		gss = GameObject.Find ("GameManager").GetComponent<GameStateStore> ();
		board = GameObject.Find ("LabScene").GetComponent<BoardManager> ();

		// setup click handler to change active build button in game state
		button = gameObject.GetComponent<Button> ();
		button.onClick.AddListener(delegate { updateActiveBuildButton(); });

		// update button text w/ inspector attributes
		titleText.text = title;
		priceText.text = "$" + price.ToString ("F2");

	}

	// button press callback
	public void updateActiveBuildButton(){
		LabItem item = gss.getLabItem (this.title);
		if (item != null) {
			board.HoldPiece (item.gameObject);
		}
	}

}
