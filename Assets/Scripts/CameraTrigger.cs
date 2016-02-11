using UnityEngine;
using System.Collections;

public class CameraTrigger : MonoBehaviour {
	
	public bool stopMovementX = false;
	public bool stopMovementY = false;
	public Vector2 offset = Vector2.zero;
	
	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		Camera.main.GetComponent<CameraMovement> ().EnableMovement (!stopMovementX, !stopMovementY);
		Camera.main.GetComponent<CameraMovement> ().SetOffset (offset);
	}
	
	void OnDrawGizmos() {
		Gizmos.color = new Color (1.0f, 0, 0, 0.4f);
		Gizmos.DrawCube (transform.position, transform.localScale);
	}
}