using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class BuffFeedback : MonoBehaviour
    {
        [SerializeField] MeshRenderer[] _rends;
        [SerializeField] Material[] _buffMats;

        bool _active;

        public void LoadBuff()
        {
            if (!_active)
            {
                _active = true;
                int count = 0;
                foreach (var el in ARune.MageManager.GetActiveElements())
                {
                    _rends[count].gameObject.SetActive(true);
                    switch (el.Name)
                    {
                        case "Fire":
                            _rends[count].material = _buffMats[0];
                            break;
                        case "Electric":
                            _rends[count].material = _buffMats[1];
                            break;
                        case "Cosmic":
                            _rends[count].material = _buffMats[2];
                            break;
                        case "Phantom":
                            _rends[count].material = _buffMats[3];
                            break;
                    }

                    count++;
                }
            }
        }

        public void ResetBuff()
        {
            _active = false;
            foreach (var rend in _rends) rend.gameObject.SetActive(false);
        }
    }
}