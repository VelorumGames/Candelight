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

        private void Awake()
        {
            _inv = FindObjectOfType<Inventory>();
            _ui = FindObjectOfType<UIManager>();
        }

        public override void Interaction()
        {
            //TODO
            //Esto esta de placeholder (te quita la mitad de los fragmentos que tengas), pero puede cambiarse segun se vea testeando

            _ui.ShowWarning(() => _inv.AddFragments(-_inv.GetFragments() / 2), "Recuperarás un cuarto de tu salud total a cambio de la mitad de tus fragmentos. ¿Estás seguro?");
        }
    }
}