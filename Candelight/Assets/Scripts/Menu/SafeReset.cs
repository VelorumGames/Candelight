using Controls;
using Hechizos;
using Items;
using Music;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

public class SafeReset : MonoBehaviour
{
    [SerializeField] WorldInfo _world;

    void Start()
    {
        ResetPermanentGameObjects();
    }

    void ResetPermanentGameObjects()
    {
        if (_world.World) Destroy(_world.World);

        PlayerController cont = FindObjectOfType<PlayerController>();
        if (cont) Destroy(cont.gameObject);

        MusicManager music = FindObjectOfType<MusicManager>();
        if (music) Destroy(music.gameObject);

        InputManager input = FindObjectOfType<InputManager>();
        if (input) Destroy(input.gameObject);

        Mage mage = FindObjectOfType<Mage>();
        if (mage) Destroy(mage.gameObject);

        Inventory inv = FindObjectOfType<Inventory>();
        if (inv) Destroy(inv.gameObject);

        LuciernagaController[] luciernagas = FindObjectsOfType<LuciernagaController>();
        foreach (var l in luciernagas)
        {
            if (l != null) Destroy(l.gameObject);
        }
    }
}
