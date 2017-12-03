using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	public float Duration;
	public Gradient ColorOverTime;
	public Text Display;
	public string TargetScene;

	private float _startTime;

	[UsedImplicitly]
	private void OnEnable() {
		_startTime = Time.timeSinceLevelLoad;
	}

	[UsedImplicitly]
	private void Update() {
		var elapsed = Time.timeSinceLevelLoad - _startTime;
		var progression = elapsed / Duration;
		Display.text = TimeSpan.FromSeconds(Duration - elapsed).ToString("mm':'ss");
		Display.color = ColorOverTime.Evaluate(progression);
		if (progression >= 1) {
			SceneManager.LoadScene(TargetScene, LoadSceneMode.Single);
		}
	}
}
