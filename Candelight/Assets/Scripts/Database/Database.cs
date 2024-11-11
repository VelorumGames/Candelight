using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

public static class Database
{
    static string _url = "https://candelightfirebase-default-rtdb.europe-west1.firebasedatabase.app";
    public static bool Completed = true;

    public static void Send(string header, object data)
    {
        if (Completed)
        {
            Completed = false;
            RestClient.Put($"{_url}/{header}.json", JsonUtility.ToJson(data)).Then(response => Completed = true);
        }
    }

    public static void Get<T>(string header, Action<T> callback)
    {
        if (Completed)
        {
            Completed = false;
            Debug.Log($"Buscando informacion en {header}");
            RestClient.Get<T>($"{_url}/{header}.json").Then(response =>
            {
                Debug.Log($"Info de tipo {response} encontrada!");
                callback(response);
                Completed = true;
            });
        }
    }
}

[Serializable]
public class UserData
{
    public string Name;
    public int Score;
    public float posX;
    public float posY;
}

