using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class AutomaticHealthBar : MonoBehaviour
    {
        bool _active;
        [SerializeField] Transform _mask;
        [SerializeField] float _duration;
        float _time;

        float _oPos;
        float _endPos = 0.342f;

        public event System.Action OnHealthBarCompletion;

        private void Update()
        {
            if (_active)
            {
                _time += Time.deltaTime;
                _mask.localPosition = new Vector3(Mathf.Lerp(_endPos, _oPos, _time/_duration), _mask.localPosition.y, _mask.localPosition.z);

                if (_time >= _duration)
                {
                    OnHealthBarCompletion();
                }
            }
        }

        public void StartCountdown() => _active = true;
        public void StopCountdown() => _active = false;
    }
}
