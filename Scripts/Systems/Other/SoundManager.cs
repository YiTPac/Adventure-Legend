using Godot;
using System;

enum SoundBus
{
	Master,
	SFX,
	BackgroundMusic
}

public partial class SoundManager : Node
{
	[Export] private Node sfx;
	[Export] private AudioStreamPlayer backgroundMusicPlayer;
	public static SoundManager Instance { get; private set; }
	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			QueueFree();
		}
	}
	public void PlaySfx(string name)
	{
		var player = sfx.GetNode(name) as AudioStreamPlayer;
		player.Play();
	}
	
	public void PlayMusic(AudioStream stream)
	{
		if (backgroundMusicPlayer.Stream == stream && backgroundMusicPlayer.IsPlaying())
		{
			return;
		}
		backgroundMusicPlayer.Stream = stream;
		backgroundMusicPlayer.Play();
	}
	
	public void SetupUiSounds(Node node)
	{
		//GD.Print(GetPathTo(node) + node.Name);
		if (node is Button button)
		{
			button.Pressed += () => PlaySfx("UIPress");
			button.FocusEntered += () => PlaySfx("UIFocus");
			button.MouseEntered += button.GrabFocus;
		}

		if (node is Slider slider)
		{
			slider.ValueChanged += (value) => PlaySfx("UIPress");
			slider.FocusEntered += () => PlaySfx("UIFocus");
			slider.MouseEntered += slider.GrabFocus;
		}
		foreach(Node child in node.GetChildren())
		{
			SetupUiSounds(child);
		}
	}

	public float GetVolume(int busIndex)
	{
		return AudioServer.GetBusVolumeLinear(busIndex);
	}

	public void SetVolume(int busIndex, float volume)
	{
		//GD.Print(volume);
		AudioServer.SetBusVolumeLinear(busIndex, volume);
	}
}
