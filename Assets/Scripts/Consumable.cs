using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(SpriteRenderer))]
public abstract class Consumable : MonoBehaviour {
	private bool _consumed;

	[UsedImplicitly]
	private void OnCollisionEnter2D(Collision2D collision) {
		if (_consumed) {
			return;
		}
		var player = collision.transform.GetComponent<Player>();
		if (player == null) {
			return;
		}

		StartCoroutine(Consume(player));
	}


	protected IEnumerator Consume(Player player) {
		_consumed = true;
		var sprite = GetComponent<SpriteRenderer>();
		sprite.color = sprite.color * new Color(1, 1, 1, 0.5f);
		var audio = GetComponent<AudioSource>();
		audio.Play();
		yield return new WaitForSecondsRealtime(audio.clip.length);
		OnConsume(player);
		Destroy(gameObject);
	}

	protected abstract void OnConsume(Player player);
}
