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

        public MageInfo Info;

        [SerializeField] int _maxElements;
        public List<AElementalRune> ActiveElements; // Propiedad que mantiene el elemento activo (o plural) del mago

        PlayerController _cont;

        public GameObject Projectile;
        [SerializeField] float _projectileSpeed;
        public GameObject Explosion;
        public GameObject Melee;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            ActiveElements = new List<AElementalRune>();
            ARune.RegisterMage(this);
        }

        private void OnEnable()
        {
            _cont = FindObjectOfType<PlayerController>();
        }

        #region Active Elements

        // M�todo para cambiar el elemento activo cuando se usa un glifo elemental
        public void SetActiveElements(AElementalRune[] runes)
        {
            ResetActiveElements();
            ActiveElements.Clear();

            foreach(var r in runes)
            {
                ActiveElements.Add(r);
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

        public void AddActiveElement(AElementalRune rune)
        {
            if (ActiveElements.Count < _maxElements)
            {
                ActiveElements.Add(rune);
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

            ActiveElements.Clear();
        }

        // M�todo para obtener el elemento activo actual
        public List<AElementalRune> GetActiveElements()
        {
            return ActiveElements;
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
