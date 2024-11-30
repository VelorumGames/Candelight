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

namespace Interactuables
{
    public class EndNodeInter : AInteractuables
    {
        public GameObject Fires;
        public ParticleSystem FireParticles;
        public NodeInfo CurrentNodeInfo;

        UIManager _ui;

        private void Awake()
        {
            _ui = FindObjectOfType<UIManager>();
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

            yield return new WaitForSeconds(0.5f);

            FindObjectOfType<CameraManager>().Shake(20f, 0.1f, 2.5f);

            yield return new WaitForSeconds(0.5f);

            _ui.FadeToWhite(2.5f, FinishScene);
            DOTween.To(() => CameraManager.Instance.GetActiveCam().m_Lens.FieldOfView, x => CameraManager.Instance.GetActiveCam().m_Lens.FieldOfView = x, 90f, 2.5f);
        }

        void FinishScene()
        {
            FindObjectOfType<PlayerController>().World.Candle -= 5f * FindObjectOfType<PlayerController>().World.NodeCandleFactor;
            _ui.ShowState(EGameState.Loading);
            SceneManager.LoadScene("WorldScene");
        }
    }
}