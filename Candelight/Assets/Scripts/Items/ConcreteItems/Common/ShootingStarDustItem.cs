using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Items.ConcreteItems
{
    public class ShootingStarDustItem : AItem
    {
        PlayerController _cont;
        [SerializeField] float _speedFactor = 1.05f;

        protected override void ApplyProperty()
        {
            if (_cont == null) _cont = FindObjectOfType<PlayerController>();

            // Aumenta un 5% la velocidad
            if (_cont != null)
            {
                _cont.SetSpeedFactor(_cont.GetSpeedFactor() * _speedFactor);
            }
        }

        protected override void ResetProperty()
        {
            if (_cont != null)
            {
                _cont.SetSpeedFactor(_cont.GetSpeedFactor() / _speedFactor);
            }
        }
    }
}
