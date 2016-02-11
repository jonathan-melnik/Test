using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	private bool isMovementEnabledX = true;
	private bool isMovementEnabledY = true;
	private Vector2 offset = Vector2.zero;
	private Vector3 lastPlayerPos;
	private const float playerOffsetX = 3.0f;
	private const float playerOffsetY = 1.5f;
	private FixedValue fixedX = new FixedValue();
	private FixedValue fixedY = new FixedValue();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPos = transform.position;
		if (fixedX.isFixed) {
			targetPos.x = fixedX.value;
		} else {
			targetPos.x = lastPlayerPos.x + playerOffsetX + offset.x;
		}
		if (fixedY.isFixed) {
			targetPos.y = fixedY.value;
		} else {
			targetPos.y = lastPlayerPos.y + playerOffsetY + offset.y;
		}

		float ease = Time.smoothDeltaTime * 5.0f;
		if (ease > 1.0f) {
			ease = 1.0f;
		}

		transform.position = (targetPos - transform.position) * ease + transform.position; 
	}

	public void SetPlayerPosition(Vector3 playerPosition) {
		if (isMovementEnabledX) {
			lastPlayerPos.x = playerPosition.x;
		}
		if (isMovementEnabledY) {
			lastPlayerPos.y = playerPosition.y;
		}
	}

	public void EnableMovement(bool enableX, bool enableY) {
		isMovementEnabledX = enableX;
		isMovementEnabledY = enableY;
	}

	public void SetOffset(Vector2 offset) {
		this.offset = offset;
	}

	public void Reset(Vector3 playerPosition) {
		Vector3 targetPos = transform.position;
		targetPos.x = playerPosition.x + playerOffsetX;
		targetPos.y = playerPosition.y + playerOffsetY;
		transform.position = targetPos;
		lastPlayerPos = playerPosition;
		offset = Vector2.zero;
		isMovementEnabledX = true;
		isMovementEnabledY = true;
		fixedX.isFixed = false;
		fixedY.isFixed = false;
	}

	public void SetFixedX(float value, bool isFixed) {
		fixedX.value = value;
		fixedX.isFixed = isFixed;
	}

	public void SetFixedY(float value, bool isFixed) {
		fixedY.value = value;
		fixedY.isFixed = isFixed;
	}
}

class FixedValue
{
	public bool isFixed = false;
	public float value = 0;
}