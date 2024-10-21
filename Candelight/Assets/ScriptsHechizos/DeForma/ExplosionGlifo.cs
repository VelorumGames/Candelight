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

        // Aqu� defines c�mo se comporta el proyectil basado en el elemento activo.
        switch (elementoActivo)
        {
            case "Fuego":
                Debug.Log("Explosion de Fuego");
                // Aqu� implementas la l�gica del proyectil de fuego
                break;
            case "Electricidad":
                Debug.Log("Explosion El�ctrico");
                // Aqu� implementas la l�gica del proyectil el�ctrico
                break;
            case "C�smico":
                Debug.Log("Explosion C�smico");
                // Aqu� implementas la l�gica del proyectil c�smico
                break;
            case "Fantasmal":
                Debug.Log("Explosion Fantasmal");
                // Aqu� implementas la l�gica del proyectil c�smico
                break;
            default:
                Debug.Log("Elemento activo no reconocido.");
                break;
        }
    }
}