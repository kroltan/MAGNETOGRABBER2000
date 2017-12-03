using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HighscoreDisplay : MonoBehaviour {
	public string Format = "{}";
	
	[UsedImplicitly]
	private void OnEnable() {
		if (PlayerPrefs.HasKey("HighScore")) {
			var highscore = PlayerPrefs.GetInt("HighScore");
			GetComponent<Text>().text = string.Format(Format, highscore);
		} else {
			gameObject.SetActive(false);
		}
	}
}
