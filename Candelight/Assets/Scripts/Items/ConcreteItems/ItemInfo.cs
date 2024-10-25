using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ConcreteItems
{
    [CreateAssetMenu(menuName = "Item Info")]
    public class ItemInfo : ScriptableObject
    {
        public string Name;
        public string Description;
        public int Max;
        public EItemCategory Category;
    }
}
