using UnityEngine;
using System.Collections;

public class ScientistController : MonoBehaviour {

	void Start () {
		// camera should be centered over scientist
		Vector3 camPos = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, Camera.main.transform.position.z);
		Camera.main.transform.position = camPos;	
	}
	
}
