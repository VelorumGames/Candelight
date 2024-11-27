using Items;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Interactuables
{
    public class HealthAltarInter : AInteractuables
    {
        Inventory _inv;
        UIManager _ui;

        [SerializeField] int _minFrags;

        private void Awake()
        {
            _inv = FindObjectOfType<Inventory>();
            _ui = FindObjectOfType<UIManager>();
        }

        public override void Interaction()
        {
            if (_inv.GetFragments() >= _minFrags) _ui.ShowWarning(ManageFragments, "Recuperar�s un cuarto de tu salud total a cambio de la mitad de tus fragmentos. �Est�s seguro?");
            else _ui.ShowWarning(NullAction, "Todav�a no posees fragmentos suficientes", "Ok", "Atr�s");
        }

        void ManageFragments()
        {
            _inv.AddFragments(-_inv.GetFragments() / 2);
            FindObjectOfType<UIManager>().Back();
        }

        void NullAction()
        {
            FindObjectOfType<UIManager>().Back();
        }
    }
}