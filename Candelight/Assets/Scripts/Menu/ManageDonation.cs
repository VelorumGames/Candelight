using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using static System.Net.WebRequestMethods;

namespace Menu
{
    public class ManageDonation : MonoBehaviour
    {
        string _url = "https://velorumgames.github.io/";

        UIManager _ui;

        private void Awake()
        {
            _ui = FindObjectOfType<UIManager>();
        }

        public void GoToDonation()
        {
            if (GameSettings.Online) _ui.ShowWarning(Manage, "Se abrirá una nueva pestaña. ¿Estás seguro?");
            else FindObjectOfType<UIManager>().ShowTutorial("Esta función no está disponible en el modo Sin Conexión.");
        }

        void Manage()
        {
            Application.OpenURL(_url);
            _ui.Back();
        }
    }
}