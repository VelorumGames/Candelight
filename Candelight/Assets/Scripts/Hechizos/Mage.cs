using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hechizos.Elementales;
using Hechizos.DeForma;
using System.Data;
using Player;
using UI;

namespace Hechizos
{
    public class Mage : MonoBehaviour
    {
        public static Mage Instance;

        [SerializeField] int _maxElements;
        static List<AElementalRune> _activeElements = new List<AElementalRune>(); // Propiedad que mantiene el elemento activo (o plural) del mago

        PlayerController _cont;

        public GameObject Projectile;
        [SerializeField] float _projectileSpeed;
        public GameObject Explosion;
        public GameObject Melee;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            ARune.RegisterMage(this);
        }

        private void OnEnable()
        {
            _cont = FindObjectOfType<PlayerController>();
        }

        #region Active Elements

        // Método para cambiar el elemento activo cuando se usa un glifo elemental
        public void SetActiveElements(AElementalRune[] runes)
        {
            Debug.Log("Se registran nuevos elementos");
            ResetActiveElements();
            _activeElements.Clear();

            foreach (var r in runes)
            {
                _activeElements.Add(r);
            }

            foreach (var rune in ARune.Spells.Values)
            {
                if (rune.GetType().IsSubclassOf(typeof(AShapeRune)))
                {
                    AShapeRune shapeRune = (AShapeRune)rune;
                    Debug.Log("Se encuentra una runa de forma a la que aplicar: " + shapeRune.Name);
                    foreach (var element in runes)
                    {
                        Debug.Log("Se aplica elemento: " + element.Name);
                        shapeRune.LoadElements(element.GetActions());
                    }
                }
            }
        }

        public void SetInitialElement(AElementalRune element)
        {
            AElementalRune[] initial = new AElementalRune[1];
            initial[0] = element;
            SetActiveElements(initial);
        }

        public void AddActiveElement(AElementalRune rune)
        {
            if (_activeElements.Count < _maxElements)
            {
                _activeElements.Add(rune);
            }
            else Debug.Log("ERROR: No se pueden almacenar mas elementos");
        }

        public void ResetActiveElements()
        {
            foreach (var rune in ARune.Spells.Values)
            {
                if (rune.GetType().IsSubclassOf(typeof(AShapeRune)))
                {
                    AShapeRune shapeRune = (AShapeRune)rune;
                    Debug.Log("Se encuentra una runa de forma a la que desaplicar: " + shapeRune.Name);
                    shapeRune.ResetElements();
                }
            }

            _activeElements.Clear();
        }

        // Método para obtener el elemento activo actual
        public List<AElementalRune> GetActiveElements()
        {
            return _activeElements;
        }

        public int GetMaxElements() => _maxElements;

        #endregion

        #region Spell Functions
        public GameObject SpawnProjectile()
        {
            GameObject proj = Instantiate(Projectile);
            proj.transform.position = _cont.transform.position;
            proj.GetComponent<Rigidbody>().AddForce(_projectileSpeed * _cont.Orientation, ForceMode.Impulse);
            return proj;
        }

        public GameObject SpawnExplosion()
        {
            GameObject proj = Instantiate(Explosion);
            return proj;
        }

        public GameObject SpawnMelee()
        {
            GameObject proj = Instantiate(Melee);
            return proj;
        }

        public GameObject SpawnBuff()
        {
            return null;
        }
        #endregion
    }
}
