using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace Cameras
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;

        public List<CinemachineVirtualCamera> Cameras = new List<CinemachineVirtualCamera>();
        public CinemachineVirtualCamera InitialCam;
        CinemachineVirtualCamera _activeCam;

        CinemachineBasicMultiChannelPerlin _noise;
        float _originalAmp;
        float _originalFrec;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;
        }

        private void Start()
        {
            SetActiveCamera(InitialCam);
        }

        public void SetActiveCamera(CinemachineVirtualCamera newCam)
        {
            if (newCam != _activeCam)
            {
                foreach (var c in Cameras)
                {
                    if (c == newCam)
                    {
                        c.Priority = 1;
                        _noise = c.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                        if (_noise != null)
                        {
                            _originalAmp = _noise.m_AmplitudeGain;
                            _originalFrec = _noise.m_FrequencyGain;
                        }
                        _activeCam = newCam;
                    }
                    else c.Priority = 0;
                }
            }
        }

        public void SetActiveCamera(int i) => SetActiveCamera(Cameras[i]);

        public CinemachineVirtualCamera GetActiveCam() => _activeCam;
        public int GetActiveCamIndex()
        {
            for (int i = 0; i < Cameras.Count; i++)
            {
                if (Cameras[i] != _activeCam) i++;
                else return i;
            }
            return -1;
        }

        public void AddCamera(CinemachineVirtualCamera cam) => Cameras.Add(cam);

        /// <summary>
        /// Agitacion temporal de la camara
        /// </summary>
        /// <param name="amp"></param>
        /// <param name="frec"></param>
        /// <param name="time"></param>
        public void Shake(float amp, float frec, float time)
        {
            if (_noise != null)
            {
                StopAllCoroutines();
                //Se guardan los parametros originales
                _noise.m_AmplitudeGain = _originalAmp;
                _noise.m_FrequencyGain = _originalFrec;
                //Se cambia el ruido a los nuevos parametros de forma temporal
                StartCoroutine(ManageShake(amp, frec, time));
            }
            else Debug.Log("ERROR: Se intenta hacer Shake pero no se ha encontrado ningun noise en la camara: " + _activeCam.gameObject.name);
        }

        /// <summary>
        /// Agitacion consntante de la camara
        /// </summary>
        /// <param name="amp"></param>
        /// <param name="frec"></param>
        public void ConstantShake(float amp, float frec)
        {
            if (_noise != null)
            {
                StopAllCoroutines();
                _noise.m_AmplitudeGain = amp;
                _noise.m_FrequencyGain = frec;
            }
            else Debug.Log("ERROR: Se intenta hacer Shake pero no se ha encontrado ningun noise en la camara: " + _activeCam.gameObject.name);
        }

        public void Impulse()
        {

        }

        IEnumerator ManageShake(float amp, float frec, float time)
        {
            float iAmp = _noise.m_AmplitudeGain;
            float iFrec = _noise.m_FrequencyGain;
            float h = 0;
            while (h < 1)
            {
                _noise.m_AmplitudeGain = Mathf.Lerp(amp, iAmp, h);
                _noise.m_FrequencyGain = Mathf.Lerp(frec, iFrec, h);

                h += Time.deltaTime / time;
                yield return null;
            }
            _noise.m_AmplitudeGain = iAmp;
            _noise.m_FrequencyGain = iFrec;
            yield return null;
        }
    }
}
