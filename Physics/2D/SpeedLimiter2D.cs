using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpeedLimiter2D : MonoBehaviour {

	private Rigidbody2D _Controller;

	public float maxSpeed = 12f;

	private void Awake() {
		_Controller = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate() {
		_Controller.velocity = Vector2.ClampMagnitude(_Controller.velocity, maxSpeed);
	}
}
