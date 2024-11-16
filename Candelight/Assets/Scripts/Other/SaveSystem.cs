using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using World;
using Items;
using Hechizos;

public static class SaveSystem
{
    public static string path = Application.persistentDataPath + "/player.save";
    public static UserData PlayerData;

    public static void Save(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();

        //Subir info a la base de datos
        Database.Send($"Players/{PlayerData.Name}", PlayerData);
    }

    public static SaveData Load()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData save = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return save;
        }
        else
        {
            Debug.Log("ERROR: No se ha encontrado archivo de guardado");
            return null;
        }
    }
}

[System.Serializable]
public class SaveData
{
    //La seed no hace falta guardarla

    //Tengo que tener:
    // Nodos completados: Array con ids [DONE]
    // Los objetos (cuáles activados y cuáles desactivados): Dos arrays con int [DONE]
    // Los fragmentos: int
    // La vida restante: float
    // Las runas desbloqueadas: Array con strings

    public int[] CompletedNodes;
    public int[] ActiveItems;
    public int[] UnactiveItems;
    public int Fragments;
    public float Candle;
    public string[] Runes;

    public SaveData(WorldInfo world, Inventory inventory)
    {
        CompletedNodes = world.CompletedIds.ToArray();

        int[][] invData = GetInventoryData(inventory);
        ActiveItems = invData[0];
        UnactiveItems = invData[1];
        Fragments = invData[2][0];

        Candle = world.Candle;

        List<string> runes = new List<string>();

        foreach(var r in ARune.Spells.Values)
        {
            if (r.IsActivated()) runes.Add(r.Name);
        }
        Runes = runes.ToArray();
    }

    int[][] GetInventoryData(Inventory inv)
    {
        int[][] invData = new int[2][];

        invData[0] = new int[inv.ActiveItems.Count];
        for (int i = 0; i < inv.ActiveItems.Count; i++)
        {
            invData[0][i] = inv.ActiveItems[i].GetComponent<AItem>().Data.Id;
        }

        invData[1] = new int[inv.UnactiveItems.Count];
        for (int i = 0; i < inv.UnactiveItems.Count; i++)
        {
            invData[1][i] = inv.UnactiveItems[i].GetComponent<AItem>().Data.Id;
        }

        invData[2] = new int[1];
        invData[2][0] = inv.GetFragments();

        return invData;
    }
}
