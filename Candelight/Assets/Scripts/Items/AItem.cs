using DG.Tweening;
using Items.ConcreteItems;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UI;
using UI.Window;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Items 
{

    public abstract class AItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public ItemInfo Data;

        [SerializeField] protected Sprite[] _buttonSprites;

        Image _img;
        float _oScale;

        public bool IsNew = true;
        protected bool IsActivated = false;

        InventoryWindow _invWin;
        Inventory _inv;

        UISoundManager _sound;

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneChanged;
            DontDestroyOnLoad(this);

            _sound = FindObjectOfType<UISoundManager>();

            _inv = FindObjectOfType<Inventory>();
        }

        private void Start()
        {
            _img = GetComponent<Image>();
            GetComponentInChildren<TextMeshProUGUI>().text = Data.Name;
        }

        void OnSceneChanged(Scene scene, LoadSceneMode mode)
        {
            _sound = FindObjectOfType<UISoundManager>();
        }

        protected abstract void ApplyProperty();
        protected abstract void ResetProperty();

        public void ApplyItem()
        {
            if (IsActivated)
                ApplyProperty();
        }

        public void SetActivation()  // Funcion preparada para llamarse con un boton/clic dependiendo de la interfaz
        {
            if (!_inv) _inv = FindObjectOfType<Inventory>();
            if (!_invWin) _invWin = FindObjectOfType<InventoryWindow>();

            if (IsActivated) //Se desactiva si estaba activado
            {
                IsActivated = false;
                ResetProperty();
                _inv.DeactivateItem(this);

                _img.sprite = _buttonSprites[0];

                _sound.PlayDeactivateItem();
            }
            else
            {
                if (_invWin.EternalFrameMode != -1)
                {
                    _inv.MarkItem(gameObject);

                    _sound.PlayMarkItem();
                }
                else if (_inv.GetFragments() >= (int)Data.Category) //Se activa si estaba activado y hay fragmentos suficientes
                {
                    IsActivated = true;
                    ApplyProperty();
                    _inv.ActivateItem(this);

                    _img.sprite = _buttonSprites[1];

                    if (!_invWin) _invWin = FindObjectOfType<InventoryWindow>();
                    _invWin.ShowActivatedParticles(GetComponent<RectTransform>().position);

                    _sound.PlayActivateItem();
                }
                else
                {
                    Debug.Log("[INFO] NO SE HA PODIDO ACTIVAR EL ITEM");
                    _invWin.ManageUnableFeedback(GetComponent<Image>());

                    _sound.PlayCantButtonSound();
                }
            }
        }

        public bool IsActive() => IsActivated;

        public void OnPointerEnter(PointerEventData _)
        {
            _oScale = GetComponent<RectTransform>().localScale.x;
            GetComponent<RectTransform>().DOScale(_oScale * 1.03f, 0.2f);

            if (GameSettings.ItemTutorial)
            {
                FindObjectOfType<UIManager>().ShowTutorial("Cada artefacto tiene unas propiedades determinadas, pero solo se les puede sacar partido con la energía de un determinado número de fragmentos.", 10f);

                GameSettings.ItemTutorial = false;
            }

            _sound.PlayHoverItem();
        }

        public void OnPointerExit(PointerEventData _)
        {
            GetComponent<RectTransform>().DOScale(_oScale, 0.2f);
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneChanged;
        }
    }

    public enum EItemCategory
    {
        Common = 4,
        Rare = 6,
        Epic = 8,
        Legendary = 10
    }
}
