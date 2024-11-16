using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Window
{
    public class InventoryWindow : AUIWindow
    {
        //bool _loadedItems;
        protected override void OnStart()
        {
            Inventory.Instance.LoadItems();
        }

        protected override void OnClose()
        {
            Inventory.Instance.UnloadItems();
        }
    }
}
