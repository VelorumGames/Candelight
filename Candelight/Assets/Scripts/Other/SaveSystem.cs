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
    public static ScoreData ScoreboardData;
    public static SaveData GameData;

    public static string PlayerName;

    public static float StarRange = 10f;

    public static bool ScoreboardIntro;

    public static IEnumerator Save(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();

        //Subir info a la base de datos
        Debug.Log("Se guardarán los datos de: " + PlayerName);
        //Database.Send($"Players/{PlayerData.Name}", PlayerData);
        yield return Database.SendUserData(ScoreboardData);
    }

    public static IEnumerator Load()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData save = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            //Coger info de la base de datos
            yield return Database.Get<ScoreData>($"Players/{PlayerName}", GetPlayerData);

            GameData = save;
        }
        else
        {
            Debug.Log("ERROR: No se ha encontrado archivo de guardado");
            GameData = null;
        }
    }

    static void GetPlayerData(ScoreData data)
    {
        if (data != null)
        {
            ScoreboardData = data; //Si se ha encontrado
        }
        else //Si no se ha encontrado
        {
            Debug.LogWarning($"ERROR: No se ha encontrado datos del jugador \"{PlayerName}\" en la base de datos");
        }
    }

    public static void GenerateNewPlayerData(WorldInfo world)
    {
        ScoreboardData.Score = world.CompletedNodes;
        ScoreboardData.posX = Random.Range(-StarRange, StarRange);
        ScoreboardData.posY = Random.Range(-StarRange, StarRange);
    }

    public static bool RemovePreviousGameData()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            return true;
        }

        return false;
    }

    public static void RestartDataOnDeath()
    {
        GameData.RestartOnDeath();
    }

    public static bool ExistsPreviousGame() => File.Exists(path);
}

[System.Serializable]
public class SaveData
{
    public int[] CompletedNodes;
    public int[] ActiveItems;
    public int[] UnactiveItems;
    public int Fragments;
    public float Candle;
    public string Runes;

    public SaveData(WorldInfo world, Inventory inventory)
    {
        Debug.Log("GUARDANDO DATOS");

        CompletedNodes = world.CompletedIds.ToArray();
        //Debug.Log("Nodos completados: " + CompletedNodes.Length);

        int[][] invData = GetInventoryData(inventory);
        ActiveItems = invData[0];
        UnactiveItems = invData[1];
        Fragments = invData[2][0];

        //string s = "ITEMS ACTIVOS: " + ActiveItems.Length;
        //foreach (var id in ActiveItems) s += $"\n- {id}";
        //s += "\nITEMS INACTIVOS: " + UnactiveItems.Length;
        //foreach (var id in UnactiveItems)
        //{
        //    Debug.Log("Meto item: " + id);
        //    s += $"\n- {id}";
        //}
        //s += $"\nFRAGMENTOS: {Fragments}";
        //Debug.Log(s);

        Candle = world.Candle;

        Runes = "";
        foreach(var r in ARune.Spells.Values)
        {
            if (r.IsActivated()) Runes += $"{r.Name},";
        }
        Debug.Log("Runas guardadas: " + Runes);
    }

    int[][] GetInventoryData(Inventory inv)
    {
        int[][] invData = new int[3][];

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

    public void RestartOnDeath()
    {
        ActiveItems = new int[1];
        ActiveItems[0] = -1;

        UnactiveItems= new int[1];
        UnactiveItems[0] = -1;

        Fragments = 0;
        Candle = 100f;
        Runes = "";
    }
}
