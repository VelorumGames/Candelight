using Items;
using Player;
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

        [SerializeField] Color _unused;
        [SerializeField] Color _used;
        [SerializeField] MeshRenderer _heart;

        private void Awake()
        {
            _inv = FindObjectOfType<Inventory>();
            _ui = FindObjectOfType<UIManager>();

            _heart.sharedMaterial.SetColor("_SecondColor", _unused);
        }

        public override void Interaction()
        {
            if (_world.Candle >= _world.MAX_CANDLE) _ui.ShowWarning(NullAction, "Tu vela no ha perdido cera todavía. No puedes activar el altar.", "Ok");
            else if (_inv.GetFragments() < _minFrags) _ui.ShowWarning(NullAction, "Todavía no posees fragmentos suficientes.", "Ok");
            else  _ui.ShowWarning(ManageFragments, "Recuperarás un cuarto de tu salud total a cambio de la mitad de tus fragmentos. ¿Estás seguro?");
        }

        void ManageFragments()
        {
            _heart.sharedMaterial.SetColor("_SecondColor", _used);

            _world.Candle += _world.MAX_CANDLE * 0.25f;
            _inv.AddFragments(-_inv.GetFragments() / 2);
            FindObjectOfType<UIManager>().Back();
            FindObjectOfType<PlayerController>().UnloadInteraction();

            GetComponent<Collider>().enabled = false;
        }

        void NullAction()
        {
            FindObjectOfType<UIManager>().Back();
        }
    }
}