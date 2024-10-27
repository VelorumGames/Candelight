using Items.ConcreteItems;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

namespace Items 
{

    public abstract class AItem : MonoBehaviour
    {
        public ItemInfo Data;

        protected bool IsActivated = false;
             
        private void Start()
        {
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
            if (IsActivated)
            {
                IsActivated = false;
                ResetProperty();
                Inventory.Instance.AddFragments((int)Data.Category);

                Inventory.Instance.ActiveItems.Remove(gameObject);
                Inventory.Instance.UnactiveItems.Add(gameObject);
                Inventory.Instance.RelocateItems();
            }
            else if (Inventory.Instance.GetFragments() >= (int)Data.Category)
            {
                IsActivated = true;
                ApplyProperty();
                Inventory.Instance.AddFragments(-(int)Data.Category);

                Inventory.Instance.UnactiveItems.Remove(gameObject);
                Inventory.Instance.ActiveItems.Add(gameObject);
                Inventory.Instance.RelocateItems();
            } 
            else
            {               
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
