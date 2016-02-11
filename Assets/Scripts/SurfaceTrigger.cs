using UnityEngine;
using System.Collections;

public class SurfaceTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color (1.0f, 0, 0, 0.4f);
		Gizmos.DrawCube (transform.position, transform.localScale);
	}
}
