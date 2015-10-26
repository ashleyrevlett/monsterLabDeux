using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildButtonManager : MonoBehaviour {

	public string title;
	public float price;
	public Text titleText;
	public Text priceText;

	private BuildMenuManager buildMenu;
	private Button button;


	void Start () {	

		// remember game references
		buildMenu = GameObject.Find ("LabGUIPrefab").GetComponent<BuildMenuManager> ();

		// setup click handler to change active build button in game state
		button = gameObject.GetComponent<Button> ();
		button.onClick.AddListener(delegate { updateActiveBuildButton(); });

		// update button text w/ inspector attributes
		titleText.text = title;
		priceText.text = "$" + price.ToString ("F2");

	}
	
	public void updateActiveBuildButton(){
		LabItem item = buildMenu.getLabItem (this.title);
		if (item != null) {
			buildMenu.setActiveBuildItem(item);
		}
	}

}
