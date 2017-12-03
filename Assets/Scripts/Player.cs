using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Magnetic), typeof(AudioSource))]
public class Player : MonoBehaviour {
	public float PropulsionSpeed;
	public float RotationSpeed;

	[Header("Visuals")]
	public float PropulsionIndicatorThreshold = 0.1f;
	public Transform PropulsionIndicator;

	[Header("Health")]
	public float Health;
	public float MaxHealth;
	public RectTransform LifeBar;
	public float ForceDamageFactor;
	public float DamageThreshold;
	public Transform DeathEffect;
	public AudioClip DeathClip;

	[Header("Magnetron")]
	public float MagnetronIntensity;
	public float EnergyCost;
	public float RechargeRate;
	public float MaxEnergy;
	public RectTransform EnergyBar;
	public Transform MagnetronIndicator;

	private Rigidbody2D _rigidbody;
	private Magnetic _magnetic;
	private AudioSource _audio;
	private Vector2 _previousVelocty;
	private float _usualCharge;
	private float _energy;

	[UsedImplicitly]
	private void OnEnable() {
		_rigidbody = GetComponent<Rigidbody2D>();
		_magnetic = GetComponent<Magnetic>();
		_audio = GetComponent<AudioSource>();
		_usualCharge = _magnetic.Charge;
		Health = MaxHealth;
	}

	private void UpdateHealth() {
		var damage = (_previousVelocty - _rigidbody.velocity).magnitude * ForceDamageFactor;
		if (damage > DamageThreshold) {
			HealthDelta(-damage);
		}
	}

	private void Move() {
		var propulsion = Input.GetAxis("Vertical") * PropulsionSpeed;
		var rotation = Input.GetAxis("Horizontal") * RotationSpeed;

		PropulsionIndicator.gameObject.SetActive(propulsion > PropulsionIndicatorThreshold);
		_audio.mute = Mathf.Abs(propulsion) < PropulsionIndicatorThreshold;

		_rigidbody.AddForce(propulsion * transform.up);
		_rigidbody.AddTorque(-rotation);
	}

	private void Magnetron() {
		var extraCharge = 0f;
		if (Input.GetButton("Magnetron")) {
			extraCharge = MagnetronIntensity;
			_energy -= EnergyCost * Time.deltaTime;
		} else {
			_energy += RechargeRate * Time.deltaTime;
		}
		_energy = Mathf.Clamp(_energy, 0, MaxEnergy);
		if (_energy < MaxEnergy / 100) {
			extraCharge = 0;
		}

		MagnetronIndicator.gameObject.SetActive(Mathf.Abs(extraCharge) > 0.01f);
		_magnetic.Charge = _usualCharge + extraCharge;

		var barScale = EnergyBar.localScale;
		barScale.x = _energy / MaxEnergy;
		EnergyBar.localScale = barScale;
	}

	[UsedImplicitly]
	private void Update() {
		UpdateHealth();
		Move();
		Magnetron();
		_previousVelocty = _rigidbody.velocity;
	}

	public void HealthDelta(float amount) {
		Health += amount;
		Health = Mathf.Clamp(Health, 0, MaxHealth);
		var barScale = LifeBar.localScale;
		barScale.x = Health / MaxHealth;
		LifeBar.localScale = barScale;

		if (Health > 0) {
			return;
		}

		Instantiate(
			DeathEffect,
			transform.position,
			transform.rotation
		);
		Destroy(gameObject);
		var go = new GameObject();
		go.transform.position = transform.position;
		var audio = go.AddComponent<AudioSource>();
		audio.clip = DeathClip;
		audio.Play();
	}
}
