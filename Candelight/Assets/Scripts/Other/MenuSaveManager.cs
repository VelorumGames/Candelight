using Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using World;

public class MenuSaveManager : MonoBehaviour
{
    public WorldInfo World;
    public string PlayerName;

    private void Start()
    {
        World.LoadedInfo = true;

        SaveSystem.PlayerData = new UserData(PlayerName, 0, 0f, 0f);
    }

    /// <summary>
    /// Funcion a la que llamar desde el menu principal si queremos cargar datos del juego
    /// </summary>
    public void LoadData()
    {
        SaveData data = SaveSystem.Load();

        FindObjectOfType<Inventory>().LoadInventory(data.ActiveItems, data.UnactiveItems, data.Fragments);
        World.Candle = data.Candle;
        foreach (var node in data.CompletedNodes) World.CompletedIds.Add(node);
        World.CompletedNodes = World.CompletedIds.Count;
        SaveSystem.PlayerData.Score = World.CompletedNodes;

        World.LoadedInfo = false;
    }
}
