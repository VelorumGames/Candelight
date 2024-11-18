using Cameras;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        private void Awake()
        {
            //SceneManager.sceneLoaded += RegisterBook;
        }

        private void Start()
        {
            ResetText();
            //_prevCam = CameraManager.Instance.GetActiveCam();
            //CameraManager.Instance.SetActiveCamera(_bookCam);
        }

        //void RegisterBook(Scene scene, LoadSceneMode mode)
        //{
        //    _camMan = FindObjectOfType<CameraManager>();
        //
        //    if (_camMan != null) _camMan.AddCamera(_bookCam);
        //
        //    gameObject.SetActive(false);
        //}

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
            _text.text += str;
        }

        public void ResetText() => _text.text = "";

        public IEnumerator ShowResult(ARune rune)
        {
            _result.text = rune.Name;
            yield return new WaitForSeconds(1.5f);
            ResetText();
            _result.text = "";
        }

        private void OnDisable()
        {
            if (_camMan != null) _camMan.SetActiveCamera(_prevCam, 1f);
        }
    }
}