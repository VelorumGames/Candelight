using DG.Tweening;
using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Window
{
    public class InventoryWindow : AUIWindow
    {
        [SerializeField] RectTransform _unactiveContainer;
        [SerializeField] RectTransform _activeContainer;
        [SerializeField] ParticleSystem _activeParticles;

        [SerializeField] Image _fragmentSprite;
        [SerializeField] Color _unableColor;

        [SerializeField] ParticleSystem _frameParticles;

        Inventory _inv;
        UIFragments _frag;

        bool _shown;
        int m_frame = -1;
        public int EternalFrameMode
        {
            get => m_frame;
            set
            {
                m_frame = value;

                if (value == -1) _frameParticles.Stop();
                else _frameParticles.Play();
            }
        }

        [SerializeField] EternalFrame[] _eternalFrames;
        AItem[] _eternalItems = new AItem[3];

        private void Awake()
        {
            _inv = FindObjectOfType<Inventory>();
            _frag = FindObjectOfType<UIFragments>();
        }

        protected override void OnStart()
        {
            _inv.LoadItems();
            _frag.KeepFragments();
            _shown = true;
        }

        protected override void OnClose()
        {
            _inv.UnloadItems();
            _frag.HideFragments();
            _shown = false;
        }

        public void ManageItemPosition(GameObject item, Vector3 pos, bool isOff)
        {
            item.GetComponent<Image>().SetNativeSize();
            item.GetComponent<RectTransform>().SetParent(isOff ? _unactiveContainer : _activeContainer);
            item.GetComponent<RectTransform>().localPosition = pos;
        }

        public void ShowActivatedParticles(Vector3 position)
        {
            _activeParticles.GetComponent<RectTransform>().position = new Vector3(position.x, position.y, _activeParticles.GetComponent<RectTransform>().position.z);
            if (_shown) _activeParticles.Play();
        }

        public void ManageUnableFeedback(Image button)
        {
            button.color = Color.white;
            button.DOColor(_unableColor, 0.5f).SetUpdate(true).Play().OnComplete(() => button.DOColor(Color.white, 0.5f).SetUpdate(true).Play());

            _fragmentSprite.color = Color.white;
            _fragmentSprite.DOColor(_unableColor, 0.5f).SetUpdate(true).Play().OnComplete(() => _fragmentSprite.DOColor(Color.white, 0.5f).SetUpdate(true).Play());
        }

        public void ShowItemInFrame(AItem item)
        {
            _eternalItems[EternalFrameMode] = item;
            _eternalFrames[EternalFrameMode].ShowItem(item.Data.ItemSprite);

            EternalFrameMode = -1;
        }

        public void ShowItemInFrame(AItem item, int frameId)
        {
            _eternalItems[frameId] = item;
            _eternalFrames[frameId].ShowItem(item.Data.ItemSprite);

            EternalFrameMode = -1;
        }

        public void ReturnItemFromFrame(int frameId)
        {
            Debug.Log($"{frameId} dentro de {_eternalFrames.Length}");
            if (_eternalItems[frameId] != null) _inv.ResetMarkItem(_eternalItems[frameId].gameObject);

            EternalFrameMode = -1;
        }
    }
}
