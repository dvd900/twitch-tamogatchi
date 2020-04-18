using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

//[ExecuteInEditMode]
public class rotateHandler : MonoBehaviour {

	public bool up,down,left,right,forward,back = false;
	public float speed;
	float wizzytime;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame

#if UNITY_EDITOR
	void OnGUI() {
		if (down == true) {
			transform.Rotate (Vector3.down * (1 / 60.0f) * speed);
		} else if (up == true) {
			transform.Rotate (Vector3.up * (1 / 60.0f) * speed);
		}
		if (left == true) {
			transform.Rotate (Vector3.left * (1 / 60.0f) * speed);
		} else if (right == true) {
			transform.Rotate (Vector3.right * (1 / 60.0f) * speed);
		}
		if (forward == true) {
			transform.Rotate (Vector3.forward * (1 / 60.0f) * speed);
		} else if (back == true) {
			transform.Rotate (Vector3.back * (1 / 60.0f) * speed);
		}
	}

#else
	#if !UNITY_STANDALONE_LINUX || UNITY_EDITOR_OSX
	void Update () {
		if (down == true) {
			transform.Rotate (Vector3.down * WizTime.deltaTime * speed);
		} else if (up == true) {
			transform.Rotate (Vector3.up * WizTime.deltaTime * speed);
		}
		if (left == true) {
			transform.Rotate (Vector3.left * WizTime.deltaTime * speed);
		} else if (right == true) {
			transform.Rotate (Vector3.right * WizTime.deltaTime * speed);
		}
		if (forward == true) {
			transform.Rotate (Vector3.forward * WizTime.deltaTime * speed);
		} else if (back == true) {
			transform.Rotate (Vector3.back * WizTime.deltaTime * speed);
		}
	}
	#endif
#endif


}
