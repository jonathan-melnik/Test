using UnityEngine;
using System.Collections;

public class PlayerTrigger : MonoBehaviour {

	public bool allowHorizontalMovement = false;
	public bool allowAutoRunning = false;
	public bool allowJumping = false;
	public bool allowWallDrag = false;

	private PlayerController player;


	// Use this for initialization
	void Start () {
		player = GameObject.FindObjectOfType<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		player.EnableHorizontalMovement (allowHorizontalMovement);
		if (!allowHorizontalMovement) {
			player.EnableAutoRunning (allowAutoRunning);
		}
		player.EnableJumping (allowJumping);
		player.EnableWallDrag (allowWallDrag);
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color (1.0f, 0, 0, 0.4f);
		Gizmos.DrawCube (transform.position, transform.localScale);
	}
}
