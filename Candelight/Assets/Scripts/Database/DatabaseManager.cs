using Proyecto26;
using Scoreboard;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    List<UserData> players = new List<UserData>();
    UserData _currentUserData;
    UserNames _names;

    private void Awake()
    {
        _names = new UserNames();
    }

    IEnumerator Start()
    {
        ////Testeo
        //UserData data1 = new UserData();
        //data1.Name = "Mauro";
        //data1.Score = 10;
        //data1.posX = 2f;
        //data1.posY = 2f;
        //
        //UserData data2 = new UserData();
        //data2.Name = "Patricio";
        //data2.Score = 20;
        //data2.posX = -2f;
        //data2.posY = -2f;
        
        //_names = new UserNames();
        //_names.Names = "";
        //Database.Send("Names/", _names);
        
        //yield return StartCoroutine(SendUserData(data1));
        //yield return new WaitForSeconds(0.25f);
        //yield return StartCoroutine(SendUserData(data2));
        //yield return new WaitForSeconds(0.25f);

        yield return StartCoroutine(GetAllUsersData());

        FindObjectOfType<ScoreboardManager>().SpawnStars(players.ToArray());
    }

    public IEnumerator SendUserData(UserData data)
    {
        //Mandamos la info
        Database.Send($"Players/{data.Name}", data);

        //Tomamos la lista de nombres anteriores registrados
        Database.Get<UserNames>("Names/", RecieveNames);
        yield return new WaitUntil(() => Database.Completed);

        //Actualizamos la lista y la devolvemos a la base de datos
        Debug.Log("Se registra un nuevo nombre en " + _names.Names);
        _names.Names += $"{data.Name}*";
        Debug.Log("Nueva lista: " + _names.Names);
        Database.Send("Names/", _names);
    }

    public IEnumerator GetAllUsersData()
    {
        Debug.Log("Se busca la info de todos los jugadores");
        players.Clear();

        //Recibo todos los nombres que hay en la base de datos
        Database.Get<UserNames>("Names/", RecieveNames);
        yield return new WaitUntil(() => Database.Completed);

        Debug.Log("BBB");

        //Los separo en un array
        string[] names = _names.Names.Split('*');
        foreach(var name in names)
        {
            if (name != "")
            {
                Debug.Log("Se busca la info del jugador: " + name);

                //Tomo los datos de cada jugador
                Database.Get<UserData>($"Players/{name}", RecieveData);
                yield return new WaitUntil(() => Database.Completed);

                //Creo una copia de los datos y lo registro en la lista
                UserData newData = new UserData();
                newData.Name = _currentUserData.Name;
                newData.Score = _currentUserData.Score;
                newData.posX = _currentUserData.posX;
                newData.posY = _currentUserData.posY;

                players.Add(newData);
            }
            yield return null;
        }
    }

    #region Recieve Functions

    void RecieveData(UserData result)
    {
        _currentUserData = result;
    }

    void RecieveNames(UserNames previousNames)
    {
        _names.Names = previousNames.Names;
    }

    #endregion
}

[Serializable]
public class UserNames
{
    public string Names;
}
