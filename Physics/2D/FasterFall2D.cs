using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FasterFall2D : MonoBehaviour {

	private Rigidbody2D _Controller;
	private float _InitialGravity;

	public float fallingGravity = 2f;

	private void Awake() {
		_Controller = GetComponent<Rigidbody2D>();
		_InitialGravity = _Controller.gravityScale;
	}

	private void FixedUpdate() {
		_Controller.gravityScale = _Controller.velocity.y < 0 ? fallingGravity : _InitialGravity;
	}
}
