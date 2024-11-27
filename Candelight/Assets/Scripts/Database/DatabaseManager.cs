using Proyecto26;
using Scoreboard;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DatabaseManager : MonoBehaviour
{
    List<ScoreData> _players = new List<ScoreData>();
    ScoreData _currentUserData;
    UserNames _names;

    ScoreboardManager _scoreboard;

    private void Awake()
    {
        if (!GameSettings.Online) SceneManager.LoadScene("MenuScene");
        
        _scoreboard = FindObjectOfType<ScoreboardManager>();

        _names = new UserNames();
    }

    IEnumerator Start()
    {
        FindObjectOfType<UIManager>().ShowState(EGameState.Database);
        yield return StartCoroutine(GetAllUsersData());

        Debug.Log("Hay players: " + _players.Count);

        //Se ordenan los jugadores segun la puntuacion
        _players.Sort(ScoreCompare);

        _scoreboard.UpdatePlayers(_players.ToArray());
        FindObjectOfType<UIManager>().HideState();
    }

    int ScoreCompare(ScoreData x, ScoreData y)
    {
        if (x.Score == y.Score) return 0;
        else if (x.Score > y.Score) return -1;
        else return 1;
    }

    public IEnumerator SendUserData(ScoreData data)
    {
        //Mandamos la info
        if (data.Name !=null)
        {
            yield return Database.Send($"Players/{data.Name}", data);

            //Tomamos la lista de nombres anteriores registrados
            yield return Database.Get<UserNames>("Names/", RecieveNames);
            //yield return new WaitUntil(() => Database.Completed);

            //Actualizamos la lista y la devolvemos a la base de datos
            Debug.Log("Se registra un nuevo nombre en " + _names.Names);
            _names.Names += $"{data.Name}*";
            Debug.Log("Nueva lista: " + _names.Names);
            yield return Database.Send("Names/", _names);
        }
    }

    public IEnumerator GetAllUsersData()
    {
        Debug.Log("Se busca la info de todos los jugadores");
        _players.Clear();

        //Recibo todos los nombres que hay en la base de datos
        yield return Database.Get<UserNames>("Names/", RecieveNames);
        //yield return new WaitUntil(() => Database.Completed);

        //Debug.Log("BBB");

        //Los separo en un array
        string[] names = _names.Names.Split('*');
        foreach(var name in names)
        {
            if (name != "")
            {
                Debug.Log("Se busca la info del jugador: " + name);

                //Tomo los datos de cada jugador
                yield return Database.Get<ScoreData>($"Players/{name}", RecieveData);
                //yield return new WaitUntil(() => Database.Completed);

                //Creo una copia de los datos y lo registro en la lista
                if (_currentUserData != null)
                {
                    ScoreData newData = new ScoreData(_currentUserData.Name, _currentUserData.Score, _currentUserData.posX, _currentUserData.posY);

                    _players.Add(newData);
                }
            }
            yield return null;
        }
    }

    #region Recieve Functions

    void RecieveData(ScoreData result)
    {
        _currentUserData = result;
    }

    void RecieveNames(UserNames previousNames)
    {
        //Debug.Log("AAAAAAAAAAAAAA ");
        _names.Names = previousNames.Names;
    }

    #endregion
}

[Serializable]
public class UserNames
{
    public string Names;
}
