using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Collector : MonoBehaviour {
	public string Format = "{0}";
	public Text ScoreCounter;
	public int Score;

	[UsedImplicitly]
	private void OnEnable() {
		if (!PlayerPrefs.HasKey("HighScore")) {
			PlayerPrefs.SetInt("HighScore", 0);
		}
		UpdateScore();
	}

	[UsedImplicitly]
	private void OnTriggerEnter2D(Collider2D other) {
		var collectable = other.GetComponent<Collectable>();
		if (collectable == null) {
			return;
		}

		Score += collectable.Value;
		UpdateScore();
		collectable.Collect();
	}

	private void UpdateScore() {
		ScoreCounter.text = string.Format(Format, Score);
		PlayerPrefs.SetInt("HighScore", Math.Max(
			PlayerPrefs.GetInt("HighScore"),
			Score
		));
		PlayerPrefs.Save();
	}
}
