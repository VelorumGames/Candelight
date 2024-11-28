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
        public NodeInfo CurrentNodeInfo;

        UIManager _ui;

        private void Awake()
        {
            _ui = FindObjectOfType<UIManager>();
        }

        public override void Interaction()
        {
            FindObjectOfType<PlayerController>().SetMove(false);

            CurrentNodeInfo.Node.RegisterCompletedNode();

            _ui.ShowWarning(StartTransition, "Entrega parte de tu alma y prende la Gran Pira.");
        }

        void StartTransition()
        {
            _ui.Back();
            _ui.FadeToWhite(2f, FinishScene);
            DOTween.To(() => CameraManager.Instance.GetActiveCam().m_Lens.FieldOfView, x => CameraManager.Instance.GetActiveCam().m_Lens.FieldOfView = x, 90f, 2f);
        }

        void FinishScene()
        {
            FindObjectOfType<PlayerController>().World.Candle -= 5f * FindObjectOfType<PlayerController>().World.NodeCandleFactor;
            _ui.ShowState(EGameState.Loading);
            SceneManager.LoadScene("WorldScene");
        }
    }
}