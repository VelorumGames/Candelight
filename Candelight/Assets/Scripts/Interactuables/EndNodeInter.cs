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
            Debug.Log("Se completa el nodo");
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
            //try
            //{
            //    FindObjectOfType<SimpleRoomManager>().CurrentNodeInfo.Node.RegisterCompletedNode();
            //}
            //catch (System.NullReferenceException e)
            //{
            //    Debug.Log("ERROR: Asegurate de que el mundo esta presente en esta escena para que el nodo pueda ser registrado. " + e);
            //}


            FindObjectOfType<PlayerController>().World.Candle -= 5f * FindObjectOfType<PlayerController>().World.NodeCandleFactor;
            _ui.ShowState(EGameState.Loading);
            SceneManager.LoadScene("WorldScene");
        }
    }
}