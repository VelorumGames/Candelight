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
        [SerializeField] TextMeshProUGUI _ok;

        GameObject _yesGO;
        GameObject _noGO;
        GameObject _okGO;

        private void Awake()
        {
            _yesGO = _yes.GetComponent<RectTransform>().parent.gameObject;
            _noGO = _no.GetComponent<RectTransform>().parent.gameObject;
            _okGO = _ok.GetComponent<RectTransform>().parent.gameObject;
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnClose()
        {
            _action = null;
        }

        public void Show(System.Action act, string descr, string yesText, string noText)
        {
            _yesGO.SetActive(true);
            _noGO.SetActive(true);
            _okGO.SetActive(false);

            _action = act;
            _description.text = descr;
            _yes.text = yesText;
            _no.text = noText;
        }

        public void Show(System.Action act, string descr)
        {
            _yesGO.SetActive(true);
            _noGO.SetActive(true);
            _okGO.SetActive(false);

            _action = act;
            _description.text = descr;
            _yes.text = "SÍ";
            _no.text = "NO";
        }

        public void Show(System.Action act, string descr, string oneOption)
        {
            _yesGO.SetActive(false);
            _noGO.SetActive(false);
            _okGO.SetActive(true);

            _action = act;
            _description.text = descr;
            _ok.text = oneOption;
        }

        public void Hide()
        {
            _action = null;

            FindObjectOfType<UIManager>().Back();
        }

        public void Execute()
        {
            if (_action != null) _action();
        }
    }
}