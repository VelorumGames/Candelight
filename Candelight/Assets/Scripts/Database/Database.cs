using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

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
            Debug.LogWarning($"[DATABASE] ERROR: " + ex.Message + "\nSe devuelve un valor por defecto.");
            callback(default);

            _running = false;
            Completed = true;
        });

        yield return new WaitUntil(() => Completed);
    }

    public static IEnumerator SendUserData(UserData data)
    {
        if (_names == null) _names = new UserNames();

        //Mandamos la info
        Debug.Log("[DATABASE] SE MANDA INFO");
        yield return Send($"Players/{data.Name}", data);

        //Tomamos la lista de nombres anteriores registrados
        yield return Get<UserNames>("Names/", RecieveNames);
        //yield return new WaitUntil(() => Completed);

        bool valid = true;
        string[] names = _names.Names.Split('*');
        foreach (var n in names)
        {
            if (n == data.Name) valid = false;
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

    static void RecieveNames(UserNames previousNames)
    {
        //Debug.Log("AAAAAAAAAAAAAA ");
        _names.Names = previousNames.Names;
    }
}

[Serializable]
public class UserData
{
    public string Name;
    public int Score;
    public float posX;
    public float posY;

    public UserData(string n, int sc, float pX, float pY)
    {
        Name = n;
        Score = sc;
        posX = pX;
        posY = pY;
    }
}

