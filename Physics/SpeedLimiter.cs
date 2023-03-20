using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpeedLimiter : MonoBehaviour {

	private Rigidbody _Controller;

	public float maxSpeed = 12f;

	private void Awake() {
		_Controller = GetComponent<Rigidbody>();
	}

	private void FixedUpdate() {
		_Controller.velocity = Vector3.ClampMagnitude(_Controller.velocity, maxSpeed);
	}
}
