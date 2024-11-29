using Items;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using World;

namespace Interactuables
{
    public class HealthAltarInter : AInteractuables
    {
        [SerializeField] WorldInfo _world;

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
            if (_world.Candle >= _world.MAX_CANDLE) _ui.ShowWarning(NullAction, "Tu vela no ha perdido cera todav�a. No puedes activar el altar.", "Ok", "Atr�s");
            else if (_inv.GetFragments() < _minFrags) _ui.ShowWarning(NullAction, "Todav�a no posees fragmentos suficientes.", "Ok", "Atr�s");
            else  _ui.ShowWarning(ManageFragments, "Recuperar�s un cuarto de tu salud total a cambio de la mitad de tus fragmentos. �Est�s seguro?");
        }

        void ManageFragments()
        {
            _world.Candle += _world.MAX_CANDLE * 0.25f;
            _inv.AddFragments(-_inv.GetFragments() / 2);
            FindObjectOfType<UIManager>().Back();

            FindObjectOfType<Collider>().enabled = false;
        }

        void NullAction()
        {
            FindObjectOfType<UIManager>().Back();
        }
    }
}