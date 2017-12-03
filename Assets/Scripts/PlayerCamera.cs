using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour {
	[Range(0, 1)]
	public float Convergence;
	[Range(0, 1)]
	public float ZoomConvergence;
	public float BaseZoom;
	public float SpeedZoomRatio;
	public Rigidbody2D Target;

	private Camera _camera;

	[UsedImplicitly]
	private void OnEnable() {

		_camera = GetComponent<Camera>();
	}
	
	[UsedImplicitly]
	private void Update() {
		var targetZoomLevel = BaseZoom + Target.velocity.magnitude * SpeedZoomRatio;
		_camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetZoomLevel, ZoomConvergence);
		Vector3 targetPosition = Target.position + Target.velocity;

		targetPosition.z = transform.position.z;
		transform.position = Vector3.Lerp(transform.position, targetPosition, Convergence);
	}
}
