using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ParticleSystem))]
public class LoadSceneAfterParticle : MonoBehaviour {
	public string TargetScene;

	private ParticleSystem _particleSystem;

	[UsedImplicitly]
	private void OnEnable() {
		_particleSystem = GetComponent<ParticleSystem>();
	}

	[UsedImplicitly]
	private void Update() {
		if (!_particleSystem.IsAlive()) {
			SceneManager.LoadScene(TargetScene, LoadSceneMode.Single);
		}
	}
}