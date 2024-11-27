using Items.ConcreteItems;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items 
{

    public abstract class AItem : MonoBehaviour
    {
        public ItemInfo Data;

        [SerializeField] protected Sprite[] _buttonSprites;
        Image _img;

        public bool IsNew = true;
        protected bool IsActivated = false;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            _img = GetComponent<Image>();
            GetComponentInChildren<TextMeshProUGUI>().text = Data.Name;
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
            Inventory inv = FindObjectOfType<Inventory>();

            if (IsActivated)
            {
                IsActivated = false;
                ResetProperty();
                inv.AddFragments((int)Data.Category);

                inv.ActiveItems.Remove(gameObject);
                inv.UnactiveItems.Add(gameObject);
                inv.RelocateItems();

                _img.sprite = _buttonSprites[0];
            }
            else if (inv.GetFragments() >= (int)Data.Category)
            {
                IsActivated = true;
                ApplyProperty();
                inv.AddFragments(-(int)Data.Category);

                inv.UnactiveItems.Remove(gameObject);
                inv.ActiveItems.Add(gameObject);
                inv.RelocateItems();

                _img.sprite = _buttonSprites[1];
            } 
            else
            {
                Debug.Log("[INFO] NO SE HA PODIDO ACTIVAR EL ITEM");
                //Annadir efecto visual que indique que no se ha podido activar el item porque no se tiene el numero de fragmentos necesarios
            }
            

        }

        public bool IsActive() => IsActivated;
    }

    public enum EItemCategory
    {
        Common = 4,
        Rare = 6,
        Epic = 8,
        Legendary = 10
    }

}
