using UnityEngine;
using System.Collections;

public class PlayerSpeedTrigger : MonoBehaviour {

	public bool setSpeedX = false;
	public float speedX = 0;
	public bool setSpeedY = false;
	public float speedY = 0;
	public bool setMaxSpeedX = false;
	public float maxSpeedX = 0;
	public bool setMaxSpeedY = false;
	public float maxSpeedY = 0;
	
	private PlayerController player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindObjectOfType<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnDrawGizmos() {
		Gizmos.color = new Color (1.0f, 0, 0, 0.4f);
		Gizmos.DrawCube (transform.position, transform.localScale);
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (setSpeedX) {
			player.SetSpeedX (speedX);
		}
		if (setSpeedY) {
			player.SetSpeedY (speedY);
		}
		if (setMaxSpeedX) {
			player.SetMaxSpeedX (setMaxSpeedX, maxSpeedX);
		}
		if (setMaxSpeedY) {
			player.SetMaxSpeedY (setMaxSpeedY, maxSpeedY);
		}
	}
}
