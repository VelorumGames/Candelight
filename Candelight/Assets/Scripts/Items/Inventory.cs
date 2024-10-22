using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Items
{



    public class Inventory : MonoBehaviour
    {
        public int TotalNumFragments;

        public List<AItem> ItemsList = new List<AItem>();

        public GameObject TestItem;

        public Vector3 Position; //(-210f, 100f, 0f);

        public Vector3 Offset; 


        private void Start()
        {
            for(int i = 0; i < 3; i++)
            {
                AddItem(TestItem);

            }
        }
        public void AddFragments(int numFragments)
        {
            TotalNumFragments += numFragments;
        }
        
        public void ApplyAllItems()
        {
            foreach (AItem item in ItemsList)
            {
                item.ApplyItem();
            }
        }

        public void AddItem(GameObject item)
        {
            ItemsList.Add(item.GetComponent<AItem>());

           GameObject itemButton = Instantiate(item, transform);

           itemButton.GetComponent<RectTransform>().localPosition = Position + (ItemsList.Count-1) * Offset; //Los vectores siempre a la derecha de la multiplicacion
        }

        
    }

}