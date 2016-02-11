using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private float speedX;
	private float speedY;
	public Camera mainCamera;
	private float gravity;
	public LayerMask obstaclesLayerMask;
	public LayerMask wallsLayerMask;
	private bool isDead = false;
	private bool isTouchingFloor = false;
	private float feetHeight = 0.05f;
	private bool collidedWithBlocker = false;
	private bool _canJump = true;
	private int isTouchingWall = 0;
	private const float wallGravityModifier = 0.3f;
	private bool isHorizontalMovementEnabled = false;
	private bool isAutoRunningEnabled = true;
	private bool isJumpingEnabled = true;
	private bool isWallDragEnabled = true;
	private FixedValue fixedMaxSpeedX = new FixedValue();
	private FixedValue fixedMaxSpeedY = new FixedValue();
	public Transform respawn;
	public PlayerDescription cubePlayer;
	public PlayerDescription submarinePlayer;
	public PlayerDescription currentPlayerDescription;
	private bool isInSurface = false;

	void Start () {
		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		float deltaTime = Time.fixedDeltaTime;	
		if (isTouchingWall != 0 && isWallDragEnabled) {
			speedY += gravity * wallGravityModifier * deltaTime;
		} else {
			speedY += gravity * deltaTime;
		}

		if(fixedMaxSpeedY.isFixed) {
			speedY = Mathf.Clamp(speedY, -fixedMaxSpeedY.value, fixedMaxSpeedY.value);
		}

		if (isJumpingEnabled) {
			if ((Input.GetMouseButtonDown (0) || Input.GetKeyDown (KeyCode.Space)) && !collidedWithBlocker && canJump && (isTouchingFloor || isTouchingWall != 0)) {
				speedY = jumpSpeed;
				if(isTouchingFloor) {
					currentPlayerDescription.GetComponent<Animator>().SetTrigger("Jump");
				}
				isTouchingFloor = false;
				canJump = false;
				if (isTouchingWall != 0) {
					speedX = isTouchingWall * runSpeed;
					isTouchingWall = 0;
				}
			}
		} 

		if (currentPlayerDescription == submarinePlayer) {
			if(isInSurface) {
				gravity = -currentPlayerDescription.gravity;
			} else {
				if (Input.GetMouseButton (0) || Input.GetKey(KeyCode.Space)) {
					gravity = -currentPlayerDescription.gravity * 1.2f;
				} else {
					gravity = currentPlayerDescription.gravity;
				}
			}
		}

		if (isHorizontalMovementEnabled) {
			if(Input.GetKey(KeyCode.RightArrow)) {
				speedX = 10.0f;
			} else if(Input.GetKey (KeyCode.LeftArrow)) {
				speedX = -10.0f;
			} else {
				speedX = Input.acceleration.x * 25.0f;
				if(speedX > 10.0f) {
					speedX = 10.0f;
				} else if(speedX < -10.0f) {
					speedX = -10.0f;
				}
			}
		}

		Vector3 newPos = transform.position;

		newPos.x += speedX * deltaTime;	
		newPos.y += speedY * deltaTime + 0.5f * gravity * deltaTime * deltaTime;
		
		// Tengo que detectar la colision luego de setear la nueva posicion al colisionar con el suelo,
		// pero antes de mover la camara para que no tiemble la camara
		// La deteccion de colision de unity no se actualiza con el transform automaticamente, sino que 
		// eso pasa en otro thread y a veces falla, asi que detecto yo la colision con el suelo


		Collider2D floorCollider = HitFloor (newPos);
		if (floorCollider != null) {
			float floorY = floorCollider.bounds.center.y + floorCollider.bounds.size.y / 2;
			float playerFeet = newPos.y - playerCollider.bounds.size.y / 2;
			if (playerFeet < floorY) {
				newPos.y = floorY + playerCollider.bounds.size.y / 2;
				speedY = 0;
				isTouchingFloor = true;
				canJump = true;
				if (isTouchingWall == 0 && isAutoRunningEnabled) {
					speedX = runSpeed;
				}
			}
		} else {
			isTouchingFloor = false;
		}

		// wall collision
		if (speedX >= 0) {
			RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.right, playerCollider.bounds.size.x, wallsLayerMask);
			if (hit.collider && hit.collider.tag == "Wall") {
				float wallX = hit.collider.bounds.center.x - hit.collider.bounds.size.x / 2;
				float playerSide = newPos.x + playerCollider.bounds.size.x / 2;
				if (playerSide > wallX) {
					newPos.x = wallX - playerCollider.bounds.size.x / 2;
				}							
			}
		} else {
			RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.left, playerCollider.bounds.size.x, wallsLayerMask);
			if (hit.collider && hit.collider.tag == "Wall") {
				float wallX = hit.collider.bounds.center.x + hit.collider.bounds.size.x / 2;
				float playerSide = newPos.x - playerCollider.bounds.size.x / 2;
				if(playerSide < wallX) {
					newPos.x = wallX + playerCollider.bounds.size.x / 2;
				}
			}
		}

		if (isHorizontalMovementEnabled) {
			speedX = 0;
		}
		
		if (!isDead) {		
			transform.position = newPos;
		}

		Camera.main.GetComponent<CameraMovement> ().SetPlayerPosition (transform.position);
	}
	
	void Reset() {
		isDead = false;
		speedY = 0;
		speedX = runSpeed;
		transform.position = respawn.position;
		collidedWithBlocker = false;
		Camera.main.GetComponent<CameraMovement> ().Reset (transform.position);
		isAutoRunningEnabled = true;
		isWallDragEnabled = true;
		isJumpingEnabled = true;
		isHorizontalMovementEnabled = false;

		SetPlayerDescription (cubePlayer);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "DeadZone" || other.tag == "Obstacle") {
			isDead = true;
			//Reset ();
			Application.LoadLevel(0);
		} else if (other.tag == "Submarine") {
			SetPlayerDescription(submarinePlayer);
			speedY *= 0.75f;
			other.gameObject.SetActive(false);
		}
		OnTriggerStay2D (other);
	}
	
	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Blocker" && !collidedWithBlocker) {
			speedX = 0;
			if (speedY > 0) {
				speedY = 0;
			}
			collidedWithBlocker = true;
		} else if (other.tag == "Wall") {
			float wallX = other.bounds.center.x - other.bounds.size.x / 2;
			Vector3 pos = transform.position;
			int wasTouchingWall = isTouchingWall;
			if (speedX >= 0 && wallX >= pos.x) {
				isTouchingWall = -1;
			} else if (speedX < 0 && wallX <= pos.x) {
				isTouchingWall = 1;
			}

			if (isTouchingWall != 0) {
				speedX = 0;
				if (isWallDragEnabled) {
					if (wasTouchingWall != isTouchingWall) { // it is the first frame touching the wall then zeroed speedY
						speedY = 0;
					} else if (speedY > 0) {
						speedY = 0;
					}
					canJump = true;
				}
			}
		} else if (other.tag == "Surface") {
			isInSurface = true;
			speedY *= 0.9f;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Wall") {
			isTouchingWall = 0;
		} else if (other.tag == "Surface") {
			isInSurface = false;
		}
	}

	public void EnableHorizontalMovement(bool value) {
		isHorizontalMovementEnabled = value;
		EnableAutoRunning (!value);
	}

	public void EnableAutoRunning(bool value) {
		isAutoRunningEnabled = value;
		if (!isAutoRunningEnabled) {
			//speedX = 0;
		}
	}

	public void EnableJumping(bool value) {
		isJumpingEnabled = value;
	}

	public void EnableWallDrag(bool value) {
		isWallDragEnabled = value;
	}

	public void SetSpeedX(float value) {
		speedX = value;
	}

	public void SetSpeedY(float value) {
		speedY = value;
	}

	public void SetMaxSpeedX(bool isFixed, float value) {
		fixedMaxSpeedX.isFixed = isFixed;
		fixedMaxSpeedX.value = value;
	}

	public void SetMaxSpeedY(bool isFixed, float value) {
		fixedMaxSpeedY.isFixed = isFixed;
		fixedMaxSpeedY.value = value;
	}

	private void SetPlayerDescription(PlayerDescription desc) {
		if (currentPlayerDescription != null) {
			currentPlayerDescription.gameObject.SetActive(false);
		}
		currentPlayerDescription = desc;
		desc.gameObject.SetActive (true);
		gravity = desc.gravity;
		speedX = runSpeed;
	}

	private float runSpeed {
		get {
			return currentPlayerDescription.runSpeed;
		}
	}

	private float jumpSpeed {
		get {
			return currentPlayerDescription.jumpSpeed;
		}
	}

	private Collider2D playerCollider {
		get {
			return currentPlayerDescription.collider;
		}
	}

	private bool canJump {
		get {
			return _canJump && currentPlayerDescription.canJump;
		}
		set {
			_canJump = value;
		}
	}

	private Collider2D HitFloor(Vector3 playerPos) {
		RaycastHit2D hitLeft = Physics2D.Raycast (new Vector2 (playerPos.x - playerCollider.bounds.size.x/2, playerPos.y - playerCollider.bounds.size.y / 2 + feetHeight),
		                                          Vector2.down, feetHeight, obstaclesLayerMask);
		RaycastHit2D hitRight = Physics2D.Raycast (new Vector2 (playerPos.x + playerCollider.bounds.size.x/2, playerPos.y - playerCollider.bounds.size.y / 2 + feetHeight),
		                                           Vector2.down, feetHeight, obstaclesLayerMask);
		if (hitLeft.collider != null && hitLeft.collider.tag == "Floor") {
			return hitLeft.collider;
		}
		if (hitRight.collider != null && hitRight.collider.tag == "Floor") {
			return hitRight.collider;
		}
		return null;
	}
}