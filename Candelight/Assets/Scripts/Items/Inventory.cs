using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Items
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance;

        [SerializeField] int _totalNumFragments;

        public List<AItem> ItemsList = new List<AItem>();

        [SerializeField] RectTransform _itemContainer;

        public Vector3 Position; //(-210f, 100f, 0f);
        public Vector3 Offset;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;
        }

        public void AddFragments(int numFragments)
        {
            _totalNumFragments += numFragments;
        }

        public int GetFragments() => _totalNumFragments;
        
        public void ApplyAllItems()
        {
            foreach (AItem item in ItemsList)
            {
                item.ApplyItem();
            }
        }

        public void AddItem(GameObject item)
        {
            if(MaxCheck(item.GetComponent<AItem>()))
            {
                ItemsList.Add(item.GetComponent<AItem>());

                GameObject itemButton = Instantiate(item, _itemContainer);
                itemButton.GetComponent<RectTransform>().localPosition = Position + (ItemsList.Count - 1) * Offset; //Los vectores siempre a la derecha de la multiplicacion
            }
        }

        bool MaxCheck(AItem item)
        {
            int num = 0;
            foreach(var i in ItemsList)
            {
                if (i.Data.Name == item.Data.Name) num++;
            }
            return num < item.Data.Max;
        }
    }

}