using Hechizos;
using Items;
using Menu;
using Player;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

public class MenuSaveManager : MonoBehaviour
{
    public WorldInfo World;
    public NodeInfo CurrentNodeInfo;

    [SerializeField] GameObject _play;
    [SerializeField] GameObject _loadSave;

    ScoreData _previousData;

    UIManager _ui;

    private void Awake()
    {
        //FindObjectOfType<PlayerController>().transform.position = new Vector3(999f, 999f, 999f); 
        GameSettings.LoadedControls = false;
        _ui = FindObjectOfType<UIManager>();
    }

    private IEnumerator Start()
    {
        GameSettings.ExistsPreviousGame = false;

        GameSettings.LoadedWorld = false;

        World.LoadedPreviousGame = false;
        World.CompletedNodes = 0;

        CurrentNodeInfo.Node = null;

        if (SaveSystem.PlayerName == null) SaveSystem.PlayerName = $"NombreAleatorio{Random.Range(0, 1000)}"; //Medida de seguridad
        Debug.Log("NOMBRE JUGADOR: " + SaveSystem.PlayerName);

        SaveSystem.ScoreboardData = new ScoreData(SaveSystem.PlayerName, -1, 0f, 0f);
        SaveSystem.ScoreboardIntro = false;

        if (GameSettings.Online)
        {
            _ui.ShowState(EGameState.Database);

            yield return Database.Get<ScoreData>($"Players/{SaveSystem.PlayerName}", GetScoreData);

            _ui.HideState();

            
        }
        else
        {
            _ui.ShowTutorial("Tu partida no se guardará en el modo Sin Conexión");
        }
    }

    void GetScoreData(ScoreData data)
    {
        _previousData = data;

        if (_previousData != null) //Si ha encontrado una previa partida guardada
        {
            Debug.Log("PARTIDA ENCONTRADA");
            GameSettings.ExistsPreviousGame = true;

            Vector3 pos = _play.GetComponent<RectTransform>().position;
            _play.GetComponent<RectTransform>().position = _loadSave.GetComponent<RectTransform>().position;
            _loadSave.GetComponent<RectTransform>().position = pos;

            _loadSave.SetActive(true);
        }
    }

    /// <summary>
    /// Funcion a la que llamar desde el menu principal si queremos cargar datos del juego
    /// </summary>
    public void LoadData()
    {
        StartCoroutine(ManageLoadData());
    }

    IEnumerator ManageLoadData()
    {
        yield return StartCoroutine(SaveSystem.Load());
        SaveData data = SaveSystem.GameData;
        
        ARune.CreateAllRunes(FindObjectOfType<Mage>());
        
        GameSettings.CanRevive = data.CanRevive;
        
        FindObjectOfType<Inventory>().LoadInventory(data.ActiveItems, data.UnactiveItems, data.MarkedItems, data.Fragments);
        World.Candle = data.Candle;
        World.CompletedIds.Clear();
        foreach (var node in data.CompletedNodes) World.CompletedIds.Add(node);
        //World.CompletedNodes = World.CompletedIds.Count;
        SaveSystem.ScoreboardData.Score = World.CompletedNodes;
        World.World = null;
        
        World.LoadedPreviousGame = true;
        
        //Debug.Log("Datos de runas: " + data.Runes);
        string[] runeNames = data.Runes.Split(",");
        int count = 0;
        foreach(var rune in ARune.Spells.Values)
        {
            if (rune.Name == runeNames[count])
            {
                count++;
                rune.Activate(true);
            }
        }
        data.Runes = "";

        FindObjectOfType<MenuEffectManager>().StartGame("WorldScene");
    }
}
