using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Magnetic : MonoBehaviour {
	public float Charge;

	public Rigidbody2D Rigidbody { get; private set; }

	[UsedImplicitly]
	private void Start() {
		Rigidbody = GetComponent<Rigidbody2D>();
	}

	[UsedImplicitly]
	private void OnBecameVisible() {
		enabled = true;
		Rigidbody.simulated = true;
	}

	[UsedImplicitly]
	private void OnBecameInvisible() {
		enabled = false;
		Rigidbody.simulated = false;
	}
}
