using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hechizos.Elementales;
using Hechizos.DeForma;
using System.Data;
using Player;
using UI;
using SpellInteractuable;

namespace Hechizos
{
    public class Mage : MonoBehaviour
    {
        public static Mage Instance;

        [SerializeField] int _maxElements;
        static List<AElementalRune> _activeElements = new List<AElementalRune>(); // Propiedad que mantiene el elemento activo (o plural) del mago

        PlayerController _cont;

        public GameObject[] Projectiles;
        GameObject _lastProjectile;
        [SerializeField] float _projectileSpeed;
        public GameObject Explosion;
        public GameObject Melee;

        int _numProjectiles = 1;
        int _extraParallelProjectiles = 1;

        List<EElements> _trailElements = new List<EElements>();

        private void OnEnable()
        {
            _cont = FindObjectOfType<PlayerController>();
        }

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            ARune.RegisterMage(this);

            DontDestroyOnLoad(gameObject);
        }

        #region Active Elements

        // Método para cambiar el elemento activo cuando se usa un glifo elemental
        public void SetActiveElements(AElementalRune[] runes)
        {
            if (runes.Length > 0)
            {
                Debug.Log($"Se registran {runes.Length} nuevos elementos");
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
                        //Debug.Log("Se encuentra una runa de forma a la que aplicar: " + shapeRune.Name);
                        foreach (var element in runes)
                        {
                            //Debug.Log("Se aplica elemento: " + element.Name);
                            shapeRune.LoadElements(element.GetActions());
                        }
                    }
                }

                UIManager.Instance.ShowElements();
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
                    //Debug.Log("Se encuentra una runa de forma a la que desaplicar: " + shapeRune.Name);
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

        public bool IsElementActive(string name)
        {
            foreach(var el in _activeElements)
            {
                if (el.Name == name) return true;
            }
            return false;
        }

        #endregion

        #region Spell Functions
        public GameObject SpawnProjectile(int numProj)
        {
            Debug.Log("Se instancia proyectil");

            _lastProjectile = Projectiles[Random.Range(0, Projectiles.Length)];
            _lastProjectile.GetComponent<Projectile>().RegisterTypes(_activeElements.ToArray());
            while (_lastProjectile.activeInHierarchy)
            {
                _lastProjectile = Projectiles[Random.Range(0, Projectiles.Length)];
            }

            StartCoroutine(DelayedProjectile(numProj * 0.5f)); //NumProj es el numero del proyectil

            return _lastProjectile;
        }

        IEnumerator DelayedProjectile(float time)
        {
            yield return new WaitForSeconds(time);
            _lastProjectile.SetActive(true);
            
            foreach(var trailEl in _trailElements)
            {
                if (IsElementActive(trailEl)) _lastProjectile.GetComponent<Projectile>().ShowTrail(trailEl);
            }

            _lastProjectile.transform.position = _cont.transform.position;
            _lastProjectile.GetComponent<Rigidbody>().AddForce(_projectileSpeed * _cont.GetOrientation(), ForceMode.Impulse);

            yield return null;
        }

        public GameObject SpawnProjectileWithRandomDirection()
        {
            _lastProjectile = Projectiles[Random.Range(0, Projectiles.Length)];
            _lastProjectile.GetComponent<Projectile>().RegisterTypes(_activeElements.ToArray());
            while (_lastProjectile.activeInHierarchy)
            {
                _lastProjectile = Projectiles[Random.Range(0, Projectiles.Length)];
            }

            _lastProjectile.SetActive(true);
            _lastProjectile.transform.position = _cont.transform.position;
            _lastProjectile.GetComponent<Rigidbody>().AddForce(_projectileSpeed * new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)), ForceMode.Impulse);

            return _lastProjectile;
        }

        

        public GameObject SpawnExplosion()
        {
            GameObject expl = Instantiate(Explosion);
            expl.transform.position = _cont.transform.position;
            return expl;
        }

        public GameObject SpawnMelee()
        {
            GameObject mel = Instantiate(Melee);
            mel.transform.position = _cont.transform.position;
            return mel;
        }

        public GameObject SpawnBuff()
        {
            return null;
        }

        public bool TryFindClosestEnemy(out Transform closest)
        {
            bool found = false;

            Collider[] cols = Physics.OverlapSphere(_cont.transform.position, 10f);

            float minDist = 9999;
            closest = null;
            foreach (var c in cols)
            {
                if (c.CompareTag("Enemy"))
                {
                    float dist = Vector3.Distance(c.transform.position, _cont.transform.position);
                    if (dist < minDist)
                    {
                        closest = c.transform;
                        minDist = dist;
                        found = true;
                    }
                }
            }

            return found;
        }

        public void SetProjectileDrag(float d) => _lastProjectile.GetComponent<Rigidbody>().drag = d;
        public void SetProjectileTarget(Transform target, float speed)
        {
            Projectile proj = _lastProjectile.GetComponent<Projectile>();

            proj.Target = target;
            proj.SetFollowSpeed(speed);
            proj.OnUpdate += proj.FollowTarget;
        }

        #endregion

        public Transform GetPlayerTarget() => _cont.transform;

        public void AddExtraSpellThrow(int num) => _numProjectiles += num;
        public void ResetExtraSpellThrows() => _numProjectiles = 1;

        public int GetNumSpells() => _numProjectiles;

        public void AddExtraParallelSpellThrow(int num) => _extraParallelProjectiles += num;
        public void ResetExtraParallelSpellThrows() => _extraParallelProjectiles = 1;

        public void ShowTrail(EElements elem)
        {
            _trailElements.Add(elem);
        }
        public void HideTrail(EElements elem)
        {
            _trailElements.Remove(elem);
        }


        public bool IsElementActive(AElementalRune rune) => _activeElements.Contains(rune);
        public bool IsElementActive(EElements element)
        {
            foreach(var e in _activeElements)
            {
                if (e.Name == element.ToString()) return true;
            }

            return false;
        }
        public bool IsElementActive(EElements element, out AElementalRune rune)
        {
            rune = null;
            foreach (var e in _activeElements)
            {
                if (e.Name == element.ToString())
                {
                    rune = e;
                    return true;
                }
            }

            return false;
        }
    }
}
