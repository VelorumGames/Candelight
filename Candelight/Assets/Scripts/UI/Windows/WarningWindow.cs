using Items;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Window
{
    public class WarningWindow : AUIWindow
    {
        System.Action _action;

        [SerializeField] TextMeshProUGUI _description;
        [SerializeField] TextMeshProUGUI _yes;
        [SerializeField] TextMeshProUGUI _no;

        protected override void OnStart()
        {
            
        }

        protected override void OnClose()
        {
            _action = null;
        }

        public void Show(System.Action act, string descr, string yesText, string noText)
        {
            gameObject.SetActive(true);

            _action = act;
            _description.text = descr;
            _yes.text = yesText;
            _no.text = noText;
        }

        public void Show(System.Action act, string descr)
        {
            gameObject.SetActive(true);

            _action = act;
            _description.text = descr;
            _yes.text = "SÍ";
            _no.text = "NO";
        }

        public void Hide()
        {
            _action = null;
            gameObject.SetActive(false);
        }

        public void Execute()
        {
            if (_action != null) _action();
        }
    }
}