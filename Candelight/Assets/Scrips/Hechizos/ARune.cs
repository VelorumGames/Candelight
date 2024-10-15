using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos
{
    public abstract class ARune : MonoBehaviour
    {
        public string Name { get; protected set; } // Nombre del glifo

        // M�todo abstracto para aplicar efectos a los glifos, que ser� implementado en las clases derivadas
        public abstract void ApplyEffect();

    }

}