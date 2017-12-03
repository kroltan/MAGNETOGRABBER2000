public class HealthBoost : Consumable {
	public float Amount;

	protected override void OnConsume(Player player) {
		player.HealthDelta(Amount);
	}
}
