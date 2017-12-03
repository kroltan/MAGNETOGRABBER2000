using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
	public static Vector2 CoulombForce(float k, Vector2 posA, float qA, Vector2 posB, float qB) {
		var delta = posA - posB;
		var magnitude = k * (qA * qB) / Mathf.Pow(delta.magnitude, 2);
		return delta.normalized * magnitude;
	}

	public static float CircleArea(float radius) => Mathf.PI * Mathf.Pow(radius, 2);
	
	public static IEnumerator<T> WithCallback<T>(IEnumerator<T> enumerator, Action<T> callback) {
		while (enumerator.MoveNext()) {
			yield return enumerator.Current;
			callback(enumerator.Current);
		}
	}
}