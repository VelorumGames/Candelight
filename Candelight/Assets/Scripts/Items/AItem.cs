using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

namespace Items 
{

    public abstract class AItem : MonoBehaviour
    {

        public EItemCategory Category;

        protected bool IsActivated = false;

        public Inventory MyInventory;

        public string Name;

        protected void Awake()
        {
            MyInventory = FindObjectOfType<Inventory>();
        }

        private void Start()
        {
            GetComponentInChildren<TextMeshProUGUI>().text = Name;
        }

        protected abstract void ApplyProperty();

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
                MyInventory.AddFragments((int)Category);
            }
            else if (MyInventory.TotalNumFragments >= (int)Category)
            {
                IsActivated = true;
                ApplyProperty();
                MyInventory.AddFragments(-(int)Category);
            } 
            else
            {               
                //Annadir efecto visual que indique que no se ha podido activar el item porque no se tiene el numero de fragmentos necesarios
            }
            

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
