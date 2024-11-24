using Interactuables;
using Music;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class PrisonerMonsterCompletion : MonoBehaviour
    {
        public Sprite _markSprite;
        public Material _spriteMat;

        MusicManager _music;

        private void Awake()
        {
            _music = FindObjectOfType<MusicManager>();
        }

        private void Start()
        {
            NpcInter[] npcs = FindObjectsOfType<NpcInter>();

            foreach (NpcInter npc in npcs)
            {
                npc.gameObject.SetActive(false);

                GameObject sprite = new GameObject("Marca de muerte");
                sprite.transform.position = npc.transform.position + new Vector3(0f, -1f, 0f);
                sprite.transform.rotation = Quaternion.Euler(90f, 0f, Random.Range(0f, 90f));

                SpriteRenderer rend = sprite.AddComponent<SpriteRenderer>();
                rend.sprite = _markSprite;
                rend.sharedMaterial = _spriteMat;
            }

            _music.StopMusic(3);
        }
    }
}