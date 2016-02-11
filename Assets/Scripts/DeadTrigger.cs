using UnityEngine;
using System.Collections;

public class DeadTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color (0, 0, 0, 0.5f);
		Gizmos.DrawCube (transform.position, transform.localScale);
	}
}
