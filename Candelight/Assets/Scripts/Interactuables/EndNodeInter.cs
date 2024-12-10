using World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Map;
using DG.Tweening;
using Cameras;
using UI;
using Player;
using Items;

namespace Interactuables
{
    public class EndNodeInter : AInteractuables
    {
        public GameObject Fires;
        public ParticleSystem FireParticles;
        public NodeInfo CurrentNodeInfo;
        [SerializeField] WorldInfo _world;

        AudioSource _audio;

        UIManager _ui;

        private void Awake()
        {
            _ui = FindObjectOfType<UIManager>();
            _audio = GetComponent<AudioSource>();
        }

        public override void Interaction()
        {
            _ui.ShowWarning(() => StartCoroutine(StartTransition()), "Entrega parte de tu alma y prende la Gran Pira.");
        }

        IEnumerator StartTransition()
        {
            Fires.SetActive(true);
            FireParticles.Play();

            FindObjectOfType<PlayerController>().SetMove(false);

            CurrentNodeInfo.Node.RegisterCompletedNode();

            _ui.Back();

            _audio.Play();

            yield return new WaitForSeconds(0.5f);

            FindObjectOfType<CameraManager>().Shake(20f, 0.1f, 2.5f);

            yield return new WaitForSeconds(0.5f);

            _ui.FadeToWhite(2.5f, () => StartCoroutine(FinishScene()));
            DOTween.To(() => CameraManager.Instance.GetActiveCam().m_Lens.FieldOfView, x => CameraManager.Instance.GetActiveCam().m_Lens.FieldOfView = x, 90f, 2.5f);
        }

        IEnumerator FinishScene()
        {
            _world.Candle -= 5f * _world.NodeCandleFactor;

            if (GameSettings.AutoSave)
            {
                _ui.ShowState(EGameState.Saving);
                yield return StartCoroutine(SaveSystem.Save(new SaveData(CurrentNodeInfo, _world, FindObjectOfType<Inventory>())));
            }

            _ui.ShowState(EGameState.Loading);
            SceneManager.LoadScene("WorldScene");
        }
    }
}