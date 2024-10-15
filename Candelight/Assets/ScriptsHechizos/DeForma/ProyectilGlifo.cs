using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilGlifo : GlifoDeForma
{
    public ProyectilGlifo(string elementoActivo) : base(elementoActivo)
    {
        Nombre = "Proyectil";
    }

    public override void AplicarEfecto()
    {
        base.AplicarEfecto();

        // Aqu� defines c�mo se comporta el proyectil basado en el elemento activo.
        switch (elementoActivo)
        {
            case "Fuego":
                Debug.Log("Lanzando un Proyectil de Fuego");
                // Aqu� implementas la l�gica del proyectil de fuego
                break;
            case "Electricidad":
                Debug.Log("Lanzando un Proyectil El�ctrico");
                // Aqu� implementas la l�gica del proyectil el�ctrico
                break;
            case "C�smico":
                Debug.Log("Lanzando un Proyectil C�smico");
                // Aqu� implementas la l�gica del proyectil c�smico
                break;
            case "Fantasmal":
                Debug.Log("Lanzando un Proyectil Fantasmal");
                // Aqu� implementas la l�gica del proyectil c�smico
                break;
            default:
                Debug.Log("Elemento activo no reconocido.");
                break;
        }
    }
}
