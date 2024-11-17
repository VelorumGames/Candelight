using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SombraComportamiento : MonoBehaviour
{
    
    public bool EquipadoFuego() //Devuelve true si el jugador tiene equipado el elemento de fuego entre sus elementos equipados
    {
        return true;
    }
    

    public void Orbitar()
    {
        //Rotate Around del personaje
        //Si hacen contacto con colisiones de la sala, las atraviesan pero su sprite se vuelve mas transparente o de otro color (o una animacion en el que se vuelven negras)
        //El circulo o circulos de Sombras se va a ir estrechando
        //Si un circulo hace contacto con el jugador, se llama a la función ataqueCuerpoACuerpo

        //Si pasa un tiempo sin que ataquen a los children del circulo, se llama a la funcion multiplicar()

    }

    public void Multiplicar()
    {
        //Generar un child en un anillo exterior. El circulo exterior tendra una velocidad menor o mayor de rotacion
        //Cada circulo tendra un numero maximo de sombras. Si se pasa de ese numero, la sombra nueva se genera en otro anillo exterior.
        //El numero maximo de anillos adicionales que se pueden generar es tres
    }

    public void Alejarse()
    {
        //Animacion de enfado
        //El circulo o circulos de sombras se hacen mas grandes

    }

    public void Disparar()
    {
        //Un disparo de fuego pero de color morado/negro sale de cada child hacia el jugador
    }

    public void AtaqueCuerpoACuerpo() //Funciona por contacto 
    {
        //El jugador recibe danno multiplicado por el numero de sombras del circulo

        //Y las Sombras de ese circulo desaparecen
    }


}
