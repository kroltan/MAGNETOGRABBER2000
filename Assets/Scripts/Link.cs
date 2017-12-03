using UnityEngine;

public class Link : MonoBehaviour {
	public string Url;

	public void Visit() {
		Application.OpenURL(Url);
	}
}
