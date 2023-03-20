using UnityEngine;

public static class TransformExtensions {

	public static void LookAt2D(this Transform origin, Transform target) {
		origin.LookAt2D(target.position);
	}

	public static void LookAt2D(this Transform origin, Vector2 target) {
		origin.LookAt2D(new Vector3(target.x, target.y, origin.position.z));
	}

	public static void LookAt2D(this Transform origin, Vector3 targetPosition) {
		origin.right = targetPosition - origin.position;
	}

	public static void LookAt(this Transform origin, Vector3 targetWorldPosition, bool lockX = false, bool lockY = false, bool lockZ = false) {
		targetWorldPosition.x = lockX ? origin.position.x : targetWorldPosition.x;
		targetWorldPosition.y = lockY ? origin.position.y : targetWorldPosition.y;
		targetWorldPosition.z = lockZ ? origin.position.z : targetWorldPosition.z;
		origin.LookAt(targetWorldPosition);
	}
}
