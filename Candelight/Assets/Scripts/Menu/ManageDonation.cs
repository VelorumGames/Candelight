using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Menu
{
    public class ManageDonation : MonoBehaviour
    {
        string _url;

        public void GoToDonation()
        {
            if (GameSettings.Online) FindObjectOfType<UIManager>().ShowWarning(() => Application.OpenURL(_url), "Se abrirá una nueva pestaña para que puedas gestionar tu donación. ¿Estás seguro?");
            else FindObjectOfType<UIManager>().ShowTutorial("Las donaciones no están disponibles en el modo Sin Conexión.");
        }
    }
}