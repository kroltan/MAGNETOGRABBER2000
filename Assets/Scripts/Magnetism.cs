using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

public class Magnetism : MonoBehaviour {
	public float MagneticConstant = 1;
	public float CutoffSquareMagnitude = 0.05f;
	public Transform CenterOfActivity;
	public float ActivityRadius;

	private Magnetic[] _magnets;

	[UsedImplicitly]
	private void OnEnable() {
		_magnets = FindObjectsOfType<Magnetic>();
	}

	private bool Eligible(Behaviour magnet) {
		if (magnet == null || !magnet.enabled || CenterOfActivity == null) {
			return false;
		}
		return (magnet.transform.position - CenterOfActivity.position).sqrMagnitude <= ActivityRadius;
	}

	[UsedImplicitly]
	private void Update() {
		foreach (var a in _magnets) {
			if (!Eligible(a)) {
				continue;
			}
			foreach (var b in _magnets) {
				if (!Eligible(b) || a == b) {
					continue;
				}

				var force = Utils.CoulombForce(
					MagneticConstant,
					a.transform.position, a.Charge,
					b.transform.position, b.Charge
				);
				if (force.sqrMagnitude < CutoffSquareMagnitude) {
					continue;
				}
				a.Rigidbody.AddForce(force);
			}
		}
	}
}
