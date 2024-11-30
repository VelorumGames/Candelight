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
            if (GameSettings.Online) FindObjectOfType<UIManager>().ShowWarning(() => Application.OpenURL(_url), "Se abrir� una nueva pesta�a para que puedas gestionar tu donaci�n. �Est�s seguro?");
            else FindObjectOfType<UIManager>().ShowTutorial("Las donaciones no est�n disponibles en el modo Sin Conexi�n.");
        }
    }
}