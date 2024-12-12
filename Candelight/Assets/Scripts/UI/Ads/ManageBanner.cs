using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ManageBanner : MonoBehaviour
    {
        Image _img;
        int _id;
        [SerializeField] Sprite[] _adSprites;
        [SerializeField] string[] _adUrls;

        private void Awake()
        {
            _img = GetComponent<Image>();
        }

        private void OnEnable()
        {
            LoadAd();
        }

        void LoadAd()
        {
            if (_adSprites.Length > 0)
            {
                _id = Random.Range(0, _adSprites.Length);
                _img.sprite = _adSprites[_id];
            }
        }

        public void GoToAd()
        {
            if (_adSprites.Length > 0 && _adSprites.Length == _adUrls.Length)
            {
                if (_adUrls[_id] != null) Application.OpenURL(_adUrls[_id]);
            }
        }
    }
}