using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hechizos.Elementales;
using Hechizos.DeForma;

namespace Hechizos
{
    public class Mage : MonoBehaviour
    {
        [SerializeField] int _maxElements;
        public List<AElementalRune> ActiveElements; // Propiedad que mantiene el elemento activo del mago

        private void Awake()
        {
            ActiveElements = new List<AElementalRune>();
        }

        // Método para cambiar el elemento activo cuando se usa un glifo elemental
        public void SetActiveElement(AElementalRune rune)
        {
            ActiveElements.Clear();
            ActiveElements.Add(rune);
            Debug.Log("Elemento activo ahora es: " + ActiveElements);
        }

        public void AddActiveElement(AElementalRune rune)
        {
            if (ActiveElements.Count < _maxElements)
            {
                ActiveElements.Add(rune);
            }
            else Debug.Log("ERROR: No se pueden almacenar mas elementos");
        }

        // Método para obtener el elemento activo actual
        public List<AElementalRune> GetActiveElements()
        {
            return ActiveElements;
        }

        // Método para lanzar un hechizo con un glifo de forma
        public void ThrowSorcery(AShapeRune currentShape)
        {
            currentShape.ThrowSorcery(ActiveElements.ToArray()); // Llama al método que aplica el efecto del glifo de forma
        }
    }
}
