using Cameras;
using DG.Tweening;
using Map;
using Player;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interactuables
{
    public class EndInter : AInteractuables
    {
        public GameObject Fires;
        public ParticleSystem FireParticles;

        //AudioSource _audio;

        public override void Interaction()
        {
            Debug.Log("Se pasa a la siguiente zona");
            FindObjectOfType<PlayerController>().SetMove(false);
            Fires.SetActive(true);
            GetComponent<AudioSource>().Play();
            FireParticles.Play();
            DOTween.To(() => CameraManager.Instance.GetActiveCam().m_Lens.FieldOfView, x => CameraManager.Instance.GetActiveCam().m_Lens.FieldOfView = x, 20f, 2f).OnComplete(TryEndLevel);
        }

        void TryEndLevel()
        {
            if (SceneManager.GetActiveScene().name == "TutorialScene")
            {
                GameSettings.Tutorial = false;

                FindObjectOfType<UIManager>().FadeToBlack(3f, () =>
                {
                    SceneManager.LoadScene("WorldScene");
                    FindObjectOfType<UIManager>().ShowState(EGameState.Loading);
                });
            }
            else
            {   
                MapManager map = FindObjectOfType<MapManager>();
                if (map != null)
                {
                    FindObjectOfType<UIManager>().FadeToBlack(1f, map.EndLevel);
                }
                else
                {
                    SimpleRoomManager simpleMap = FindObjectOfType<SimpleRoomManager>();
                    FindObjectOfType<UIManager>().FadeToBlack(1f, simpleMap.EndLevel);
                }
            }
        }
    }
}