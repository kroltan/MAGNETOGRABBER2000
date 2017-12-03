public class BonusTime : Consumable {
	public float ExtraSeconds;

	protected override void OnConsume(Player player) {
		FindObjectOfType<Timer>().Duration += ExtraSeconds;
	}
}
