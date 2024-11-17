using Items;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

public class MenuSaveManager : MonoBehaviour
{
    public WorldInfo World;
    public string PlayerName;

    [SerializeField] GameObject _play;
    [SerializeField] GameObject _loadSave;

    private void Awake()
    {
        //FindObjectOfType<PlayerController>().transform.position = new Vector3(999f, 999f, 999f); 
    }

    private void Start()
    {
        World.LoadedInfo = true;

        SaveSystem.PlayerData = new UserData(PlayerName, -1, 0f, 0f);

        SaveSystem.ScoreboardIntro = false;

        if (SaveSystem.ExistsPreviousGame())
        {
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

        FindObjectOfType<Inventory>().LoadInventory(data.ActiveItems, data.UnactiveItems, data.Fragments);
        World.Candle = data.Candle;
        foreach (var node in data.CompletedNodes) World.CompletedIds.Add(node);
        World.CompletedNodes = World.CompletedIds.Count;
        SaveSystem.PlayerData.Score = World.CompletedNodes;

        World.LoadedInfo = false;
    }
}
