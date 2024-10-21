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

        // Aqu� defines c�mo se comporta el proyectil basado en el elemento activo.
        switch (elementoActivo)
        {
            case "Fuego":
                Debug.Log("Cuerpo a Cuerpo de Fuego");
                // Aqu� implementas la l�gica del proyectil de fuego
                break;
            case "Electricidad":
                Debug.Log("Cuerpo a Cuerpo El�ctrico");
                // Aqu� implementas la l�gica del proyectil el�ctrico
                break;
            case "C�smico":
                Debug.Log("Cuerpo a Cuerpo C�smico");
                // Aqu� implementas la l�gica del proyectil c�smico
                break;
            case "Fantasmal":
                Debug.Log("Cuerpo a Cuerpo Fantasmal");
                // Aqu� implementas la l�gica del proyectil c�smico
                break;
            default:
                Debug.Log("Elemento activo no reconocido.");
                break;
        }
    }
}
