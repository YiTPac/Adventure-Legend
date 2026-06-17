using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class StatusPanel : HBoxContainer
{
	[Export] public Stats stats;
	private TextureProgressBar healthBar;
	private TextureProgressBar easedHealthBar;
	private TextureProgressBar energyBar;

	public override void _Ready()
	{
		stats ??= Game.Instance.playerStats;
		energyBar = GetNode<TextureProgressBar>("V/EnergyBar");
		healthBar = GetNode<TextureProgressBar>("V/HealthBar");
		easedHealthBar = GetNode<TextureProgressBar>("V/HealthBar/EasedHealthBar");
		stats.HealthChanged += UpdateHealthBar;
		stats.EnergyChanged += UpdateEnergyBar;
		UpdateHealthBar(true);
		UpdateEnergyBar();
	}
	public override void _ExitTree()
	{
		if (stats != null)
		{
			stats.HealthChanged -= UpdateHealthBar;
			stats.EnergyChanged -= UpdateEnergyBar;
		}

	}
	public void UpdateHealthBar(bool skipAnimation)
	{
		if (stats == null || healthBar == null || easedHealthBar == null)
		{
			return;
		}
		float percentage = (float)stats.CurrentHealth / stats.MaxHealth;
		healthBar.Value = percentage;
		if (skipAnimation)
		{
			easedHealthBar.Value = percentage;
		}
		else
		{
			CreateTween().TweenProperty(easedHealthBar, "value", percentage, 0.3f);
		}
	}
	public void UpdateHealthBar()
	{
		UpdateHealthBar(false);
	}
	public void UpdateEnergyBar()
	{
		float percentage = (float)stats.CurrentEnergy / stats.MaxEnergy;
		energyBar.Value = percentage;
	}
}
