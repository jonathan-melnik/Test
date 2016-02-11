using UnityEngine;
using System.Collections;

public class FixedCameraTrigger : MonoBehaviour {

	public bool isFixedX = false;
	public float x;
	public bool isFixedY = false;
	public float y;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		Camera.main.GetComponent<CameraMovement> ().SetFixedX (x, isFixedX);	
		Camera.main.GetComponent<CameraMovement> ().SetFixedY (y, isFixedY);	
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color (1.0f, 0, 0, 0.4f);
		Gizmos.DrawCube (transform.position, transform.localScale);
	}
}
