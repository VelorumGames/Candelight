using Cameras;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Hechizos
{
    public class BookManager : MonoBehaviour
    {
        TextMeshPro _text;
        [SerializeField] CinemachineVirtualCamera _bookCam;
        CinemachineVirtualCamera _prevCam;

        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshPro>();
        }

        private void Start()
        {
            CameraManager.Instance.AddCamera(_bookCam);
            ResetText();
            _prevCam = CameraManager.Instance.GetActiveCam();
            CameraManager.Instance.SetActiveCamera(_bookCam);
        }

        private void OnEnable()
        {
            ResetText();
            _prevCam = CameraManager.Instance.GetActiveCam();
            CameraManager.Instance.SetActiveCamera(_bookCam);
        }

        public void AddNewString(string str)
        {
            _text.text += str;
        }

        public void ResetText() => _text.text = "";

        private void OnDisable()
        {
            if (CameraManager.Instance != null) CameraManager.Instance.SetActiveCamera(_prevCam);
        }
    }
}