using Godot;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;

public interface ISaveable
{
    public string GetSaveKey(World world)
    {
        return world.GetPathTo(this as Node);
    }
    public string SaveState();
    public void LoadState(string jsonText);
}

public interface ISaveable<DataType> : ISaveable
{
    public DataType OnSaveState();
    public void OnLoadState(DataType data);

    string ISaveable.SaveState()
    {
        var options = new JsonSerializerOptions
        {
            IncludeFields = true,
            WriteIndented = true
        };
        return JsonSerializer.Serialize(OnSaveState(), options);
    }

    void ISaveable.LoadState(string jsonText)
    {
        var options = new JsonSerializerOptions
        {
            IncludeFields = true,
            WriteIndented = true,
        };
        OnLoadState(JsonSerializer.Deserialize<DataType>(jsonText, options)!);
    }
}

