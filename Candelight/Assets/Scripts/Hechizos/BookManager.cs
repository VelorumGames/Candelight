using Cameras;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hechizos
{
    public class BookManager : MonoBehaviour
    {
        [SerializeField] TextMeshPro _text;
        [SerializeField] TextMeshPro _result;
        [SerializeField] CinemachineVirtualCamera _bookCam;
        CinemachineVirtualCamera _prevCam;

        CameraManager _camMan;

        private void Start()
        {
            ResetText();
        }

        private void OnEnable()
        {
            _camMan = FindObjectOfType<CameraManager>();
            _camMan.AddCamera(_bookCam);

            if (_camMan != null)
            {
                ResetText();
                _prevCam = _camMan.GetActiveCam();
                _camMan.SetActiveCamera(_bookCam, 0.5f);
            }
        }

        public void AddNewString(string str)
        {
            if (_text.text.Length > 30) _text.text = "";

            _text.text += str;
        }

        public void ResetText() => _text.text = "";

        public void ShowResult(ARune rune)
        {
            FindObjectOfType<ManageBookSpell>().Show(rune);
            ResetText();
        }

        private void OnDisable()
        {
            if (_camMan != null) _camMan.SetActiveCamera(_prevCam, 1f);
        }
    }
}