using Godot;
using System;

public partial class VolumeSlider : HSlider
{
    [Export] private StringName bus = "Master";
    [Export] private AudioStream backgroundMusic;
    private int busIndex;
    public override async void _Ready()
    {
        busIndex = AudioServer.GetBusIndex(bus);
        
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        Value = SoundManager.Instance.GetVolume(busIndex);
        ValueChanged += OnValueChanged;
        //SoundManager.Instance.PlayMusic(backgroundMusic);
    }

    private void OnValueChanged(double value)
    {
        SoundManager.Instance.SetVolume(busIndex, (float)value);
        Game.Instance.SaveConfig();
    }
}
