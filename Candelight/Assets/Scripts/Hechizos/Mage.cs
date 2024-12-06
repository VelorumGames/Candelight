using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hechizos.Elementales;
using Hechizos.DeForma;
using System.Data;
using Player;
using UI;
using SpellInteractuable;
using Cameras;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

namespace Hechizos
{
    public class Mage : MonoBehaviour
    {
        public static Mage Instance;

        [SerializeField] int _maxElements;
        static List<AElementalRune> _activeElements = new List<AElementalRune>(); // Propiedad que mantiene el elemento activo (o plural) del mago

        PlayerController _cont;
        CameraManager _cam;
        BuffFeedback _buff;

        public GameObject[] Projectiles;
        GameObject _lastProjectile;
        [SerializeField] float _projectileSpeed;
        [SerializeField] float _projectileSpeedFactor = 1f;
        public GameObject Explosion;
        public GameObject Melee;

        float _oProjVolume;
        int _numProjectiles = 1;
        bool _extraProjs;

        List<EElements> _trailElements = new List<EElements>();

        public event System.Action<ARune> OnNewRuneActivation;

        [Space(10)]
        public GameObject ElectricArea;

        private void OnEnable()
        {
            _cont = FindObjectOfType<PlayerController>();
            _buff = _cont.GetComponent<BuffFeedback>();

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            ARune.RegisterMage(this);

            DontDestroyOnLoad(gameObject);

            _oProjVolume = Projectiles[0].GetComponent<AudioSource>().volume;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _cam = FindObjectOfType<CameraManager>();
        }

        void OnSceneUnloaded(Scene scene)
        {

        }

        #region Active Elements

        // Método para cambiar el elemento activo cuando se usa un glifo elemental
        public void SetActiveElements(AElementalRune[] runes)
        {
            if (runes.Length > 0)
            {
                //Debug.Log($"Se registran {runes.Length} nuevos elementos");
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
            //Debug.Log("Se instancia proyectil");

            _lastProjectile = GetAvailableProjectile();

            if (GameSettings.RemainingMagicTutorial)
            {
                FindObjectOfType<UIManager>().ShowTutorial("Algunos hechizos dejarán un resquicio de magia que puedes aprovechar.\n Haz CLICK para invocarlo de nuevo rápidamente.");
                GameSettings.RemainingMagicTutorial = false;
            }

            StartCoroutine(DelayedProjectile(numProj * 0.5f)); //NumProj es el numero del proyectil

            return _lastProjectile;
        }

        IEnumerator DelayedProjectile(float time)
        {
            yield return new WaitForSeconds(time);
            
            if (_extraProjs) //Si se lanzan tres
            {
                GameObject[] projs = new GameObject[3];
                projs[0] = _lastProjectile;
                projs[1] = GetAvailableProjectile();
                projs[2] = GetAvailableProjectile();

                int count = 0;
                foreach(var proj in projs)
                {
                    proj.SetActive(true);

                    foreach (var trailEl in _trailElements)
                    {
                        if (IsElementActive(trailEl)) proj.GetComponent<Projectile>().ShowTrail(trailEl);
                    }

                    proj.transform.position = _cont.transform.position;
                    Vector3 orientation = _cont.GetOrientation();
                    if (count == 0) orientation = Quaternion.AngleAxis(30f, Vector3.up) * orientation;
                    else if (count == 1) orientation = Quaternion.AngleAxis(-30f, Vector3.up) * orientation;

                    proj.GetComponent<Projectile>().Push(orientation, 8f);
                    if (IsActiveElement("Electric")) proj.GetComponent<Projectile>().OnImpact += SpawnElectricArea;
                    //proj.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                    //proj.GetComponent<Rigidbody>().AddForce(_projectileSpeed * _projectileSpeedFactor * orientation.normalized, ForceMode.Impulse);

                    count++;
                    yield return null;
                }

                _cam.Shake(10f, 0.2f, 0.5f);
                yield return null;
            }
            else //Si solo se lanza un proyectil
            {
                _lastProjectile.SetActive(true);

                foreach (var trailEl in _trailElements)
                {
                    if (IsElementActive(trailEl)) _lastProjectile.GetComponent<Projectile>().ShowTrail(trailEl);
                }

                _lastProjectile.transform.position = _cont.transform.position;
                _lastProjectile.GetComponent<Projectile>().Push(_cont.GetOrientation(), 8f);
                if (IsActiveElement("Electric")) _lastProjectile.GetComponent<Projectile>().OnImpact += SpawnElectricArea;



                _cam.Shake(6f, 0.1f, 0.4f);

                yield return null;
            }
        }

        void SpawnElectricArea(Transform obj)
        {
            if (GameSettings.ElectricFingers)
            {
                Instantiate(ElectricArea, obj.transform.position, Quaternion.identity);
            }
        }

        GameObject GetAvailableProjectile()
        {
            GameObject proj = null;
            proj = Projectiles[Random.Range(0, Projectiles.Length)];
            //proj.GetComponent<Projectile>().RegisterTypes(_activeElements.ToArray());
            while (proj.activeInHierarchy)
            {
                proj = Projectiles[Random.Range(0, Projectiles.Length)];
            }
            return proj;
        }

        public GameObject SpawnProjectileWithRandomDirection()
        {
            _lastProjectile = Projectiles[Random.Range(0, Projectiles.Length)];
            //_lastProjectile.GetComponent<Projectile>().RegisterTypes(_activeElements.ToArray());
            while (_lastProjectile.activeInHierarchy)
            {
                _lastProjectile = Projectiles[Random.Range(0, Projectiles.Length)];
            }

            _lastProjectile.GetComponent<AudioSource>().volume = 0.05f;
            _lastProjectile.GetComponent<Projectile>().OnEnd += ResetProjVolume;

            _lastProjectile.SetActive(true);
            _lastProjectile.transform.position = _cont.transform.position;
            _lastProjectile.GetComponent<Rigidbody>().AddForce(_projectileSpeed * new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)), ForceMode.Impulse);

            _cam.Shake(5f, 0.4f, 0.5f);

            return _lastProjectile;
        }

        void ResetProjVolume(Transform _)
        {
            foreach (var proj in Projectiles)
            {
                proj.GetComponent<AudioSource>().volume = _oProjVolume;
                proj.GetComponent<Projectile>().OnEnd -= ResetProjVolume;
            }
        }

        public GameObject SpawnExplosion()
        {
            GameObject expl = Instantiate(Explosion);
            //expl.GetComponent<Explosion>().RegisterTypes(_activeElements.ToArray());
            expl.transform.position = _cont.transform.position;
            if (IsActiveElement("Electric")) expl.GetComponent<Explosion>().OnImpact += SpawnElectricArea;

            _cam.Shake(20f, 0.2f, 1f);

            return expl;
        }

        public void SpawnCustomExplosion(Transform location, AElementalRune element, float range)
        {
            GameObject expl = Instantiate(Explosion);
            expl.transform.position = location.position;
            expl.transform.localScale *= range;
            AElementalRune[] runes = new AElementalRune[1];
            runes[0] = element;
            //expl.GetComponent<Explosion>().RegisterTypes(runes);
            _cam.Shake(10f, 0.2f, 1f);
        }

        public GameObject SpawnMelee()
        {
            GameObject mel = Instantiate(Melee);
            //mel.GetComponent<Melee>().RegisterTypes(_activeElements.ToArray());
            mel.transform.position = _cont.transform.position;

            mel.GetComponent<Rigidbody>().AddForce(_projectileSpeed * _projectileSpeedFactor * 0.15f * _cont.GetOrientation(), ForceMode.Impulse);
            if (IsActiveElement("Electric")) mel.GetComponent<Melee>().OnImpact += SpawnElectricArea;

            _cam.Shake(2f, 0.1f, 0.3f);

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

        public bool IsActiveElement(string name)
        {
            foreach (var rune in GetActiveElements())
            {
                if (rune.Name == name) return true;
            }
            return false;
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

        public void SetExtraProjectiles(bool b) => _extraProjs = b;

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

        public void RuneActivation(ARune rune)
        {
            if (OnNewRuneActivation != null) OnNewRuneActivation(rune);

            //TODO. Esto que sirve de placeholder por el momento y ya encontraremos una manera mas diegetica de presentarlo
            if (rune is ElectricRune && SceneManager.GetActiveScene().name != "MenuScene") ElementsMixTutorial();
        }

        public void SetExtraProjSpeed(float speed) => _projectileSpeedFactor = speed;

        public void ManageBuff(bool canReset, IEnumerator func)
        {
            if (canReset) StartCoroutine(func);
            _buff.LoadBuff();
        }

        public void ManageResetBuff()
        {
            _buff.ResetBuff();
        }

        void ElementsMixTutorial()
        {
            FindObjectOfType<UIManager>().ShowTutorial("Activa varios elementos a la vez combinando sus instrucciones");
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;

            ResetProjVolume(null);
        }
    }
}
