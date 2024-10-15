using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotenciacionGlifo : GlifoDeForma
{
    public PotenciacionGlifo(string elementoActivo) : base(elementoActivo)
    {
        Nombre = "Potenciación";
    }

    public override void AplicarEfecto()
    {
        base.AplicarEfecto();

        // Aquí defines cómo se comporta el proyectil basado en el elemento activo.
        switch (elementoActivo)
        {
            case "Fuego":
                Debug.Log("Lanzando un Proyectil de Fuego");
                // Aquí implementas la lógica del proyectil de fuego
                break;
            case "Electricidad":
                Debug.Log("Lanzando un Proyectil Eléctrico");
                // Aquí implementas la lógica del proyectil eléctrico
                break;
            case "Cósmico":
                Debug.Log("Lanzando un Proyectil Cósmico");
                // Aquí implementas la lógica del proyectil cósmico
                break;
            case "Fantasmal":
                Debug.Log("Lanzando un Proyectil Fantasmal");
                // Aquí implementas la lógica del proyectil cósmico
                break;
            default:
                Debug.Log("Elemento activo no reconocido.");
                break;
        }
    }
}