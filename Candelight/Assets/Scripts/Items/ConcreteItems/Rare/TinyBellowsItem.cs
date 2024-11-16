using Hechizos;
using Hechizos.DeForma;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ConcreteItems
{
    public class TinyBellowsItem : AItem
    {
        BuffRune _buff;
        PlayerController _cont;

        protected override void ApplyProperty()
        {
            if (_cont == null) _cont = FindObjectOfType<PlayerController>();

            //Los potenciadores aumentan un 10% mas el daño de todos los hechizos, pero la vela se agota un 10% mas rapido.
            if (ARune.FindSpell("Buff", out var rune))
            {
                _buff = rune as BuffRune;
                if (_buff != null)
                {
                    _buff.AddNewFactor(_buff.GetNewFactor() * 1.1f);
                    _cont.AddCandleFactor(_cont.GetCandleFactor() * 1.1f);
                }
                else Debug.LogWarning("ERROR: No se ha podido aplicar TinyBellows porque el casteo a BuffRune no ha sido exitoso o no se ha encontrado en el diccionario de runas");
            }
        }

        protected override void ResetProperty()
        {
            if (_buff != null)
            {
                _buff.RemoveNewFactor(_buff.GetNewFactor() * 1.1f);
                _cont.RemoveCandleFactor(_cont.GetCandleFactor() * 1.1f);
            }
            else Debug.LogWarning("ERROR: No se ha encontrado BuffRune");
        }
    }
}
