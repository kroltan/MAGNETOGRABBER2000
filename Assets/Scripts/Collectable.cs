using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Collectable : MonoBehaviour {
	public int Value;
	public ParticleSystem Explosion;

	public void Collect() {
		if (Explosion == null) {
			return;
		}
		var rotation = Quaternion.FromToRotation(Vector3.up, GetComponent<Rigidbody2D>().velocity.normalized);
		var ps = Instantiate(Explosion, transform.position, rotation);

		var main = ps.main;
		main.startColor = GetComponent<SpriteRenderer>().color;
		StartCoroutine(DestroyParticleSystem(ps));
	}

	private IEnumerator DestroyParticleSystem(ParticleSystem target) {
		gameObject.SetActive(false);
		yield return new WaitWhile(target.IsAlive);
		Destroy(target);
		Destroy(gameObject);
	}
}
