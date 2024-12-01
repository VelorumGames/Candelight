using Proyecto26;
using System;
using System.Collections;
using UnityEngine;

public static class Database
{
    static string _url = "https://candelightfirebase-default-rtdb.europe-west1.firebasedatabase.app";
    public static bool Completed;
    static bool _running;

    static UserNames _names;

    public static IEnumerator Send(string header, object data)
    {
        //Debug.Log($"Condicion de espera: {!_running}");
        yield return new WaitUntil(() => !_running);

        Completed = false;
        _running = true;

        RestClient.Put($"{_url}/{header}.json", JsonUtility.ToJson(data)).Then(response =>
        {
            Completed = true;
            _running = false;
        });
        yield return new WaitUntil(() => Completed);
        Debug.Log($"[DATABASE] INFORMACION ENVIADA");
    }

    public static IEnumerator Get<T>(string header, Action<T> callback)
    {
        yield return new WaitUntil(() => !_running);

        Completed = false;
        _running = true;

        Debug.Log($"[DATABASE] Buscando informacion en {header}");
        RestClient.Get<T>($"{_url}/{header}.json").Then(response =>
        {
            Debug.Log($"[DATABASE] Info de tipo {response} encontrada!");
            callback(response);

            _running = false;
            Completed = true;
        }).Catch(ex =>
        {
            _running = false;
            Completed = true;

            Debug.LogWarning($"[DATABASE] ERROR: " + ex.Message + "\nSe devuelve un valor por defecto.");
            callback(default);
        });

        yield return new WaitUntil(() => Completed);
    }

    public static IEnumerator SendUserData(ScoreData data)
    {
        if (_names == null) _names = new UserNames();

        //Mandamos la info
        Debug.Log("[DATABASE] SE MANDA INFO");
        if (data.Name != "")
        {
            yield return Send($"Players/{data.Name}", data);

            //Tomamos la lista de nombres anteriores registrados
            yield return Get<UserNames>("Names/", RecieveNames);
            //yield return new WaitUntil(() => Completed);

            bool valid = true;
            if (_names.Names != null)
            {
                string[] names = _names.Names.Split('*');
                foreach (var n in names)
                {
                    if (n == data.Name) valid = false;
                }
            }

            //Actualizamos la lista y la devolvemos a la base de datos
            if (valid)
            {
                Debug.Log("[DATABASE] Se registra un nuevo nombre en " + _names.Names);
                _names.Names += $"{data.Name}*";
                Debug.Log("[DATABASE] Nueva lista: " + _names.Names);
                yield return Send("Names/", _names);
            }
        }
    }

    static void RecieveNames(UserNames previousNames)
    {
        //Debug.Log("AAAAAAAAAAAAAA ");
        _names.Names = previousNames.Names;
    }
}

[Serializable]
public class ScoreData
{
    public string Name;
    public int Score;
    public float posX;
    public float posY;

    public bool CanRevive;
    public int CurrentNode = -1;
    public int[] CompletedNodes;
    public int[] ActiveItems;
    public int[] UnactiveItems;
    public int[] MarkedItems;
    public int Fragments;
    public float Candle;
    public string Runes;

    public ScoreData(string n, int sc, float pX, float pY)
    {
        Name = n;
        Score = sc;
        posX = pX;
        posY = pY;
    }

    public void ImplementGameData(SaveData data)
    {
        CanRevive = data.CanRevive;
        CurrentNode = data.CurrentNode;
        CompletedNodes = data.CompletedNodes;
        ActiveItems = data.ActiveItems;
        UnactiveItems = data.UnactiveItems;
        MarkedItems = data.MarkedItems;
        Fragments = data.Fragments;
        Candle = data.Candle;
        Runes = data.Runes;
    }

    public SaveData GetGameData()
    {
        SaveData data = new SaveData();
        data.CanRevive = CanRevive;
        data.CurrentNode = CurrentNode;
        data.CompletedNodes = CompletedNodes;
        data.ActiveItems = ActiveItems;
        data.UnactiveItems = UnactiveItems;
        data.MarkedItems = MarkedItems;
        data.Fragments = Fragments;
        data.Candle = Candle;
        data.Runes = Runes;
        return data;
    }
}

[Serializable]
public class UserData
{
    public string Name;
    public string Password;

    public UserData(string name, string pass)
    {
        Name = name;
        Password = pass;
    }
}