using Cinemachine;
using Controls;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

namespace Cameras
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;

        CinemachineBrain _brain;

        public List<CinemachineVirtualCamera> Cameras = new List<CinemachineVirtualCamera>();
        public CinemachineVirtualCamera InitialCam;
        CinemachineVirtualCamera _activeCam;

        CinemachineBasicMultiChannelPerlin _noise;
        float _originalAmp;
        float _originalFrec;

        Tween _spellTween;
        Tween _resetSpellTween;

        InputManager _input;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            _input = FindObjectOfType<InputManager>();

            DOTween.defaultAutoPlay = AutoPlay.None;
            _spellTween = DOTween.To(() => GetActiveCam().m_Lens.FieldOfView, x => GetActiveCam().m_Lens.FieldOfView = x, 55f, 0.3f).SetAutoKill(false);
            _resetSpellTween = DOTween.To(() => GetActiveCam().m_Lens.FieldOfView, x => GetActiveCam().m_Lens.FieldOfView = x, 60f, 0.1f).SetAutoKill(false);
            DOTween.defaultAutoPlay = AutoPlay.All;

            _brain = FindObjectOfType<CinemachineBrain>();
            _brain.m_DefaultBlend.m_Time = 0f;
        }

        private void Start()
        {
            SetActiveCamera(InitialCam, 0f);

            _noise = InitialCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (_noise != null)
            {
                _noise.m_AmplitudeGain = 0f;
                _noise.m_FrequencyGain = 0f;
            }
        }

        private void OnEnable()
        {
            if (_input != null)
            {
                _input.OnStartElementMode += EnterSpellMode;
                _input.OnExitElementMode += ExitSpellMode;
                _input.OnStartShapeMode += EnterSpellMode;
                _input.OnExitShapeMode += ExitSpellMode;
            }
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

        public void SetActiveCamera(CinemachineVirtualCamera newCam, float blendTime)
        {
            if (newCam != _activeCam)
            {
                foreach (var c in Cameras)
                {
                    if (c == newCam)
                    {
                        _brain.m_DefaultBlend.m_Time = blendTime;

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
        public void SetActiveCamera(int i, float blendTime) => SetActiveCamera(Cameras[i], blendTime);

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

        public void AddCamera(CinemachineVirtualCamera cam)
        {
            if (!Cameras.Contains(cam)) Cameras.Add(cam);
        }

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

        public void EnterSpellMode()
        {
            _resetSpellTween.Pause();

            _spellTween.Restart();
            _spellTween.Play();
        }

        public void ExitSpellMode()
        {
            _spellTween.Pause();

            _resetSpellTween.Restart();
            _resetSpellTween.Play();
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

        public float GetCurrentBlendTime() => _brain.m_DefaultBlend.m_Time;

        private void OnDisable()
        {
            if (_input != null)
            {
                _input.OnStartElementMode -= EnterSpellMode;
                _input.OnExitElementMode -= ExitSpellMode;
                _input.OnStartShapeMode -= EnterSpellMode;
                _input.OnExitShapeMode -= ExitSpellMode;
            }
        }
    }
}
