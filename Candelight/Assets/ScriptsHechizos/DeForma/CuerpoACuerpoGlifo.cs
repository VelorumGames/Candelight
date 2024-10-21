using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuerpoACuerpoGlifo : GlifoDeForma
{
    public CuerpoACuerpoGlifo(string elementoActivo) : base(elementoActivo)
    {
        Nombre = "Cuerpo a Cuerpo";
    }

    public override void AplicarEfecto()
    {
        base.AplicarEfecto();

        // Aquí defines cómo se comporta el proyectil basado en el elemento activo.
        switch (elementoActivo)
        {
            case "Fuego":
                Debug.Log("Cuerpo a Cuerpo de Fuego");
                // Aquí implementas la lógica del proyectil de fuego
                break;
            case "Electricidad":
                Debug.Log("Cuerpo a Cuerpo Eléctrico");
                // Aquí implementas la lógica del proyectil eléctrico
                break;
            case "Cósmico":
                Debug.Log("Cuerpo a Cuerpo Cósmico");
                // Aquí implementas la lógica del proyectil cósmico
                break;
            case "Fantasmal":
                Debug.Log("Cuerpo a Cuerpo Fantasmal");
                // Aquí implementas la lógica del proyectil cósmico
                break;
            default:
                Debug.Log("Elemento activo no reconocido.");
                break;
        }
    }
}
