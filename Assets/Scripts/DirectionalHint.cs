using JetBrains.Annotations;
using UnityEngine;

public class DirectionalHint : MonoBehaviour {
	public Transform From;
	public Transform To;

	private Vector3 _initialScale;

	[UsedImplicitly]
	private void OnEnable() {
		_initialScale = transform.localScale / Camera.main.orthographicSize;
	}

	[UsedImplicitly]
	private void Update() {
		if (From == null || To == null) {
			return;
		}
		var direction = (To.position - From.position).normalized;
		transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
		transform.localScale = _initialScale * Camera.main.orthographicSize;
	}
}
