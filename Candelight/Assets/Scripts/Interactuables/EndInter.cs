using Cameras;
using DG.Tweening;
using Map;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactuables
{
    public class EndInter : AInteractuables
    {
        public override void Interaction()
        {
            Debug.Log("Se pasa a la siguiente zona");
            FindObjectOfType<PlayerController>().SetMove(false);
            DOTween.To(() => CameraManager.Instance.GetActiveCam().m_Lens.FieldOfView, x => CameraManager.Instance.GetActiveCam().m_Lens.FieldOfView = x, 20f, 2f).OnComplete(TryEndLevel);
        }

        void TryEndLevel()
        {
            MapManager map = FindObjectOfType<MapManager>();
            if (map != null)
            {
                map.EndLevel();
            }
            else
            {
                SimpleRoomManager simpleMap = FindObjectOfType<SimpleRoomManager>();
                simpleMap.EndLevel();
            }
        }
    }
}