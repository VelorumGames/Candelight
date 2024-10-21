using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionGlifo : GlifoDeForma
{
    public ExplosionGlifo(string elementoActivo) : base(elementoActivo)
    {
        Nombre = "Explosion";
    }

    public override void AplicarEfecto()
    {
        base.AplicarEfecto();

        // Aquí defines cómo se comporta el proyectil basado en el elemento activo.
        switch (elementoActivo)
        {
            case "Fuego":
                Debug.Log("Explosion de Fuego");
                // Aquí implementas la lógica del proyectil de fuego
                break;
            case "Electricidad":
                Debug.Log("Explosion Eléctrico");
                // Aquí implementas la lógica del proyectil eléctrico
                break;
            case "Cósmico":
                Debug.Log("Explosion Cósmico");
                // Aquí implementas la lógica del proyectil cósmico
                break;
            case "Fantasmal":
                Debug.Log("Explosion Fantasmal");
                // Aquí implementas la lógica del proyectil cósmico
                break;
            default:
                Debug.Log("Elemento activo no reconocido.");
                break;
        }
    }
}