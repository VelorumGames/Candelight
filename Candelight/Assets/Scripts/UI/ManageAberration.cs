using Controls;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UI
{
    public class ManageAberration : MonoBehaviour
    {
        ChromaticAberration _chroma;
        InputManager _input;

        private void Awake()
        {
            _input = FindObjectOfType<InputManager>();
        }

        private void OnEnable()
        {
            if (GetComponent<Volume>().sharedProfile.TryGet(out _chroma))
            {
                _input.OnStartElementMode += Show;
                _input.OnStartShapeMode += Show;
                _input.OnExitElementMode += Hide;
                _input.OnExitShapeMode += Hide;
            }
        }

        void Show()
        {
            _chroma.intensity.Override(0.5f);
        }

        void Hide()
        {
            _chroma.intensity.Override(0f);
        }

        private void OnDisable()
        {
            if (GetComponent<Volume>().sharedProfile.TryGet(out _chroma))
            {
                _input.OnStartElementMode -= Show;
                _input.OnStartShapeMode -= Show;
                _input.OnExitElementMode -= Hide;
                _input.OnExitShapeMode -= Hide;
            }
        }
    }
}