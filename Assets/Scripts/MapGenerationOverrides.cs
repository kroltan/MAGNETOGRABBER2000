using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerationOverrides : MonoBehaviour {
	public float MapSize;
	public string GameScene;

	[UsedImplicitly]
	private void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	public void StartWithMapSize(float mapSize) {
		MapSize = mapSize;
		SceneManager.LoadScene(GameScene, LoadSceneMode.Single);
	}
}
