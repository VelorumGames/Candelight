using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using World;

namespace Map
{
    public class ShowLevelName : MonoBehaviour
    {
        public NodeInfo CurrentNodeInfo;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(3f);
            FindObjectOfType<UIManager>().ShowLevelName(LoadName());
        }

        string LoadName()
        {
            string s = "";

            if (SceneManager.GetActiveScene().name == "NodeEndScene")
            {
                s = $"Gran Pira de {CurrentNodeInfo.Name}";
            }
            else if (SceneManager.GetActiveScene().name == "CalmScene")
            {
                s = CurrentNodeInfo.Name;
            }
            else
            {
                switch (CurrentNodeInfo.CurrentLevel)
                {
                    case 0:
                        s = $"Alrededores de {CurrentNodeInfo.Name}";
                        break;
                    case 1:
                        s = $"Afueras de {CurrentNodeInfo.Name}";
                        break;
                    case 2:
                        s = $"Senderos de {CurrentNodeInfo.Name}";
                        break;
                    case 3:
                        s = $"Profundidades de {CurrentNodeInfo.Name}";
                        break;
                    default:
                        s = CurrentNodeInfo.Name;
                        break;
                }
            }

            return s;
        }
    }
}