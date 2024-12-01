using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UI;
using UI.Window;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Items
{
    public class EternalFrame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        InventoryWindow _invWin;
        public int Id;

        [SerializeField] bool _active;
        bool _itemMarked;
        [SerializeField] Image _itemImg;

        [SerializeField] ParticleSystem _particles;

        float _oScale;

        private void Awake()
        {
            _oScale = GetComponent<RectTransform>().localScale.x;
        }

        private void OnEnable()
        {
            if (!_invWin) _invWin = FindObjectOfType<InventoryWindow>();
        }

        public void ActivateFrame()
        {
            if (_active)
            {
                if (!_invWin) _invWin = FindObjectOfType<InventoryWindow>();

                if (_itemMarked) //Quitar el item seleccionado
                {
                    _invWin.ReturnItemFromFrame(Id);

                    HideItem();

                    _itemMarked = false;
                }
                else //Preparar para seleccionar un item
                {
                    _invWin.EternalFrameMode = Id;

                    _itemMarked = true;
                }

                _particles.Play();
            }
            else //Si todavia esta bloqueado
            {
                GetComponent<Image>().DOColor(Color.red, 0.5f).Play().OnComplete(() => GetComponent<Image>().DOColor(Color.white, 0.5f).Play());
            }
        }

        public void ShowItem(Sprite itemSprite)
        {
            _itemImg.color = Color.white;
            _itemImg.sprite = itemSprite;
            _itemImg.SetNativeSize();
        }

        void HideItem()
        {
            _itemImg.color = new Color(1f, 1f, 1f, 0f);
        }

        public void OnPointerEnter(PointerEventData _)
        {
            GetComponent<RectTransform>().DOScale(_oScale * 1.1f, 0.2f);

            if (GameSettings.FrameTutorial)
            {
                FindObjectOfType<UIManager>().ShowTutorial("Los artefactos que protejas en un marco eterno perdurar�n tras la muerte, pero no podr�s usarlos mientras tanto.", 8f);
                GameSettings.FrameTutorial = false;
            }
        }

        public void OnPointerExit(PointerEventData _)
        {
            GetComponent<RectTransform>().DOScale(_oScale, 0.2f);
        }
    }
}