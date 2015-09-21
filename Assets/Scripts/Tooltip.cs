using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tooltip : MonoBehaviour {

	public GameObject tooltipPanel;
	public GameObject tooltip;

	private Text tooltipText;
	private BoxCollider2D collider;
	private LabItem labItem;

	// Use this for initialization
	void Start () {
	
//		collider = gameObject.GetComponent<BoxCollider2D> ();
		tooltipText = tooltip.GetComponent<Text> ();
		labItem = gameObject.GetComponent<LabItem> ();

		tooltipText.text = gameObject.name;
		tooltipPanel.SetActive (false);

	}

	void OnMouseEnter() {
		if (labItem.getIsPlaced ()) {
			tooltipPanel.SetActive (true);
		}
	}

	void OnMouseExit() {
		if (labItem.getIsPlaced ()) {
			tooltipPanel.SetActive (false);
		}
	}

}
