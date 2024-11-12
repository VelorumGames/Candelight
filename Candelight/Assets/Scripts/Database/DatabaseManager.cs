using Proyecto26;
using Scoreboard;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    List<UserData> _players = new List<UserData>();
    UserData _currentUserData;
    UserNames _names;

    ScoreboardManager _scoreboard;

    private void Awake()
    {
        _scoreboard = FindObjectOfType<ScoreboardManager>();

        _names = new UserNames();
    }

    IEnumerator Start()
    {
        yield return StartCoroutine(GetAllUsersData());

        //Se ordenan los jugadores segun la puntuacion
        _players.Sort(ScoreCompare);

        _scoreboard.UpdatePlayers(_players.ToArray());
    }

    int ScoreCompare(UserData x, UserData y)
    {
        if (x.Score == y.Score) return 0;
        else if (x.Score > y.Score) return -1;
        else return 1;
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
        _players.Clear();

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
                if (_currentUserData != null)
                {
                    UserData newData = new UserData();
                    newData.Name = _currentUserData.Name;
                    newData.Score = _currentUserData.Score;
                    newData.posX = _currentUserData.posX;
                    newData.posY = _currentUserData.posY;

                    _players.Add(newData);
                }
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
        Debug.Log("AAAAAAAAAAAAAA ");
        _names.Names = previousNames.Names;
    }

    #endregion
}

[Serializable]
public class UserNames
{
    public string Names;
}
