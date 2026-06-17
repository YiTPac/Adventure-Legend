using System.Collections.Generic;

public struct GameData
{
    public Dictionary<string, WorldStateData> worldStates;
    public PlayerData playerData;
    public StatsData playerStatsData;
    public string scene;
    public GameData() { }
}

