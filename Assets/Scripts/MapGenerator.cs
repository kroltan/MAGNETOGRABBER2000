using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour {
	[Serializable]
	public class CollectableVariant {
		public Color Tint;
		public int Value;
	}

	public float MapSize;

	[Header("Loading")]
	public GameObject LoadingScreen;
	public GameObject Timer;
	public Magnetism MagnetismSystem;
	public Text StatusText;

	[Header("Collectable Placement")]
	public Collectable Collectable;
	public CollectableVariant[] Variants;
	public int ObstaclesPerCollectable;

	[Header("Obstacle Placement")]
	public float ObstacleDensity;
	public float AverageObstacleDistance;
	public Transform[] Obstacles;

	[Header("Smoothing")]
	public int SmoothingRounds;
	public float SmoothingFactor;

	[UsedImplicitly]
	private void OnDrawGizmos() {
		var previousColor = Gizmos.color;
		Gizmos.color = Color.green;

		Gizmos.DrawWireSphere(transform.position, MapSize);

		Gizmos.color = previousColor;
	}

	private IEnumerator<int> Place(IList<Transform> obstacles, IList<Transform> choices, Action<Transform> adjustment = null) {
		for (var i = obstacles.Count - 1; i >= 0; i--) {
			var obstacleIndex = Mathf.FloorToInt(Random.value * choices.Count);
			obstacles[i] = Instantiate(
				choices[obstacleIndex],
				Random.insideUnitCircle * MapSize,
				Quaternion.AngleAxis(Random.value * 360, Vector3.forward)
			);
			adjustment?.Invoke(obstacles[i]);
			if (i % 10 == 0) {
				yield return i;
			}
		}
	}

	private IEnumerator<int> Relax(IList<Transform> obstacles) {
		var forces = new Vector3[obstacles.Count];
		for (var i = 0; i < SmoothingRounds; i++) {
			for (var j = obstacles.Count - 1; j >= 0; j--) {
				var obstacle = obstacles[j];
				forces[j] = obstacles
					.Where(o => o != obstacle)
					.Select(o => Utils.CoulombForce(
						SmoothingFactor,
						obstacle.position, 1,
						o.position, 1
					))
					.Aggregate((acc, f) => acc + f);
			}
			for (var j = obstacles.Count - 1; j >= 0; j--) {
				var obstacle = obstacles[j];
				obstacle.position += forces[j];
				var magnitude = Mathf.Clamp(obstacle.position.magnitude, AverageObstacleDistance, MapSize);
				obstacle.position = obstacle.position.normalized * magnitude;
			}
			yield return i;
		}
	}

	private IEnumerator GenerateMap() {
		LoadingScreen.SetActive(true);
		Timer.SetActive(false);
		MagnetismSystem.enabled = false;

		var virtualMapArea = Utils.CircleArea(MapSize) * ObstacleDensity;
		var obstacleArea = Utils.CircleArea(AverageObstacleDistance);

		var obstacles = new Transform[(int) (virtualMapArea / obstacleArea)];
		yield return Utils.WithCallback(Place(obstacles, Obstacles), lastPlaced => {
			StatusText.text = $"Placing obstacles, {lastPlaced} remaining";
		});
		yield return Utils.WithCallback(Relax(obstacles), step => {
			StatusText.text = $"Relaxing: step {step} of {SmoothingRounds}";
		});

		var collectables = new Transform[obstacles.Length / ObstaclesPerCollectable];
		var collectablePlacer = Place(collectables, new[] {Collectable.transform}, t => {
			var variant = Variants[Mathf.FloorToInt(Random.value * Variants.Length)];
			t.GetComponent<Collectable>().Value = variant.Value;
			t.GetComponent<SpriteRenderer>().color = variant.Tint;
		});
		yield return Utils.WithCallback(collectablePlacer, lastPlaced => {
			StatusText.text = $"Placing collectables, {lastPlaced} remaining";
		});

		LoadingScreen.SetActive(false);
		Timer.SetActive(true);
		MagnetismSystem.enabled = true;
	}

	[UsedImplicitly]
	private void OnEnable() {
		var overrides = FindObjectOfType<MapGenerationOverrides>();
		if (overrides != null) {
			MapSize = overrides.MapSize;
		}
		StartCoroutine(GenerateMap());
	}
}
