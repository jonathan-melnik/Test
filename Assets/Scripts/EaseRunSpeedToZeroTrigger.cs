using UnityEngine;
using System.Collections;

public class EaseRunSpeedToZeroTrigger : MonoBehaviour {

	//public bool easeRunSpeedToZero = false;

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
		//player.EaseRunSpeedToZero ();
	}
}
