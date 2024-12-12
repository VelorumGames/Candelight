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
            if (GameSettings.Online) _ui.ShowWarning(Manage, "Se abrir� una nueva pesta�a. �Est�s seguro?");
            else FindObjectOfType<UIManager>().ShowTutorial("Esta funci�n no est� disponible en el modo Sin Conexi�n.");
        }

        void Manage()
        {
            Application.OpenURL(_url);
            _ui.Back();
        }
    }
}