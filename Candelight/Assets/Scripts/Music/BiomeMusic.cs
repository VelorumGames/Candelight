using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

namespace Music
{
    public class BiomeMusic : MonoBehaviour
    {
        [SerializeField] AudioClip[] _durniaMusic;
        [SerializeField] float[] _dStartVolumes;
        [Space(10)]
        [SerializeField] AudioClip[] _temeriaMusic;
        [SerializeField] float[] _tStartVolumes;
        [Space(10)]
        [SerializeField] AudioClip[] _idriaMusic;
        [SerializeField] float[] _iStartVolumes;

        MusicManager _music;

        public NodeInfo CurrentNodeInfo;

        private void Awake()
        {
            _music = FindObjectOfType<MusicManager>();
        }

        private void Start()
        {
            LoadMusic(CurrentNodeInfo.Biome);
        }

        public void LoadMusic(EBiome biome)
        {
            if (_music != null)
            {
                switch (biome)
                {
                    case EBiome.Durnia:
                        _music.LoadClips(_durniaMusic, _dStartVolumes);
                        break;
                    case EBiome.Temeria:
                        _music.LoadClips(_temeriaMusic, _tStartVolumes);
                        break;
                    case EBiome.Idria:
                        _music.LoadClips(_idriaMusic, _iStartVolumes);
                        break;
                    default:
                        Debug.LogWarning("ERROR: No se ha cargado correctamente la musica. No se ha procesado el bioma " + biome.ToString());
                        break;
                }
                switch(SceneManager.GetActiveScene().name)
                {
                    case "LevelScene":
                        _music.PlayMusic(0);
                        _music.PlayMusicAtRandom(10f, 20f);
                        break;
                    case "CalmScene":
                        _music.PlayMusic(0);
                        _music.ChangeVolumeFrom(3, 0f, 0.3f, 10f);
                        _music.PlayMusic(3);
                        break;
                }

            }
        }
    }
}