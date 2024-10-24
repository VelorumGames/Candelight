using World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Map;
using DG.Tweening;
using Cameras;
using UI;
using UnityEditor.PackageManager;

namespace Interactuables
{
    public class EndNodeInter : AInteractuables
    {
        public override void Interaction()
        {
            Debug.Log("Se completa el nodo");
            UIManager.Instance.FadeToWhite(2f);
            DOTween.To(() => CameraManager.Instance.GetActiveCam().m_Lens.FieldOfView, x => CameraManager.Instance.GetActiveCam().m_Lens.FieldOfView = x, 90f, 2f).OnComplete(FinishScene);            
        }

        void FinishScene()
        {
            try
            {
                FindObjectOfType<SimpleRoomManager>().CurrentNodeInfo.Node.RegisterCompletedNode();
            }
            catch (System.NullReferenceException e)
            {
                Debug.Log("ERROR: Asegurate de que el mundo esta presente en esta escena para que el nodo pueda ser registrado. " + e);
            }
            
            SceneManager.LoadScene("WorldScene");
        }
    }
}