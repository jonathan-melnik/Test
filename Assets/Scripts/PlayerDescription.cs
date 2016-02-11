using UnityEngine;
using System.Collections;

public class PlayerDescription : MonoBehaviour {

	public float gravity;
	public float runSpeed;
	public float jumpSpeed;
	public bool canJump;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public new Collider2D collider {
		get {
			return GetComponent<Collider2D> ();
		}
	}
}
