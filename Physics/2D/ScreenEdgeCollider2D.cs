using UnityEngine;

public class ScreenEdgeCollider2D : MonoBehaviour {

	enum UpdateMode {
		Update,
		LateUpdate,
		FixedUpdate
	}

	private Vector2[] _EdgePoints = new Vector2[5];

	[SerializeField] private Camera m_ReferenceCamera;
	[SerializeField] private EdgeCollider2D m_EdgeCollider;
	[SerializeField] private UpdateMode m_UpdateMode;

	private void Awake() {
		InitCamera();
		InitCollider();
	}

	private void InitCamera() {
		if (m_ReferenceCamera == null)
			m_ReferenceCamera = Camera.main;

		if (m_ReferenceCamera == null) {
			Debug.LogError("Reference camera not found, failed to create screen edge collider.");

		} else if (!m_ReferenceCamera.orthographic) {
			Debug.LogError("Reference camera is not orthographic, failed to create screen edge collider.");
		}
	}

	private void InitCollider() {
		if (m_EdgeCollider != null)
			return;

		m_EdgeCollider = GetComponent<EdgeCollider2D>();

		if (m_EdgeCollider == null)
			m_EdgeCollider = gameObject.AddComponent<EdgeCollider2D>();

		UpdateBounds();
	}

	private void Update() {
		if (m_UpdateMode == UpdateMode.Update)
			UpdateBounds();
	}

	private void LateUpdate() {
		if (m_UpdateMode == UpdateMode.LateUpdate)
			UpdateBounds();
	}

	private void FixedUpdate() {
		if (m_UpdateMode == UpdateMode.FixedUpdate)
			UpdateBounds();
	}

	private void UpdateBounds() {
		Vector2 bottomLeft = m_ReferenceCamera.ScreenToWorldPoint(new Vector3(0, 0, m_ReferenceCamera.nearClipPlane));
		Vector2 topRight = m_ReferenceCamera.ScreenToWorldPoint(new Vector3(m_ReferenceCamera.pixelWidth, m_ReferenceCamera.pixelHeight));

		_EdgePoints[0] = bottomLeft;
		_EdgePoints[1] = new Vector2(bottomLeft.x, topRight.y);
		_EdgePoints[2] = topRight;
		_EdgePoints[3] = new Vector2(topRight.x, bottomLeft.y);
		_EdgePoints[4] = bottomLeft;

		m_EdgeCollider.points = _EdgePoints;
	}
}
