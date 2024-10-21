using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{



    public class Inventory : MonoBehaviour
    {
        public int TotalNumFragments;

        public List<AItem> ItemsList = new List<AItem>();

        public void AddFragments(int numFragments)
        {
            TotalNumFragments += numFragments;
        }
        
        public void ApplyAllItems()
        {
            foreach (AItem item in ItemsList)
            {
                item.Activation();
            }
        }

        public void AddItem(GameObject item)
        {
            ItemsList.Add(item.GetComponent<AItem>());
        }

        
    }

}