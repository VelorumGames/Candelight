using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Window
{
    public class ShopWindow : AUIWindow
    {
        protected override void OnClose()
        {
            
        }

        protected override void OnStart()
        {
            if (!GameSettings.Online)
            {
                gameObject.SetActive(false);
                FindObjectOfType<UIManager>().Back();
                FindObjectOfType<UIManager>().ShowTutorial("La tienda de mejoras no está disponible en el Modo Sin Conexión");
            }
        }
    }
}