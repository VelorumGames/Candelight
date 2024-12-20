using BehaviourAPI.Core.Actions;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UI;
using UI.Window;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using World;

namespace Items
{
    public class Inventory : MonoBehaviour
    {
        //public static Inventory Instance;
        UIManager _uiMan;

        public WorldInfo World;

        [Header("===POOLS===")]
        [Space(10)]
        public GameObject[] CommonItemPool;
        public GameObject[] RareItemPool;
        public GameObject[] EpicItemPool;
        public GameObject[] LegendaryItemPool;
        public GameObject Fragment;

        int m_frags;
        [SerializeField] int _totalNumFragments
        {
            get => m_frags;
            set
            {
                if (value != m_frags)
                {
                    if (OnFragmentsChange != null) OnFragmentsChange(m_frags, value);
                    m_frags = value;
                    
                }
            }
        }
        public event System.Action<int, int> OnFragmentsChange;

        [Space(20)]
        [Header("===PLAYER INVENTORY===")]
        [Space(10)]
        //public List<AItem> ItemsList = new List<AItem>();
        //public List<GameObject> ItemsList = new List<GameObject>();
        //public List<AItem> ConstantItemsList = new List<AItem>();
        public List<GameObject> UnactiveItems = new List<GameObject>();
        public List<GameObject> ActiveItems = new List<GameObject>();
        public List<GameObject> MarkItems = new List<GameObject>();

        [Space(10)]
        InventoryWindow _window;

        [Space(10)]
        public Vector3 Position; //(-210f, 100f, 0f);
        public Vector3 Offset;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            //Debug. Deberia estar desactivado
            //AddFragments(100);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            _uiMan = FindObjectOfType<UIManager>();
            if (_uiMan != null)
            {
                _window = _uiMan.InventoryUI.GetComponent<InventoryWindow>();
            }

            AItem[] allItems = FindObjectsOfType<AItem>();

            if (UnactiveItems.Count == 0 && ActiveItems.Count == 0)
            {
                Debug.Log($"Se han encontrado {allItems.Length} items");
                
                foreach (var item in allItems)
                {
                    if (item.IsActive()) ActiveItems.Add(item.gameObject);
                    else if (!MarkItems.Contains(item.gameObject)) UnactiveItems.Add(item.gameObject);
                    else
                    {
                        //Buscamos un frame libre para colocar el item
                        foreach (var frame in FindObjectsOfType<EternalFrame>(true))
                        {
                            if (frame.IsUnlocked())
                            {
                                MarkItems.Remove(item.gameObject);
                                MarkItem(item.gameObject, frame.Id);
                                break;
                            }
                        }
                    }
                }
            }

            if (MarkItems.Count > 0)
            {
                foreach (var item in allItems)
                {
                    //Buscamos un frame libre para colocar el item
                    if (MarkItems.Contains(item.gameObject))
                    {
                        foreach (var frame in FindObjectsOfType<EternalFrame>(true))
                        {
                            if (frame.IsUnlocked())
                            {
                                MarkItems.Remove(item.gameObject);
                                MarkItem(item.gameObject, frame.Id);
                                break;
                            }
                        }
                    }
                }
            }

            if (scene.name == "WorldScene")
            {
                SecureItems();
            }
        }

        void OnSceneUnloaded(Scene scene)
        {
            ActiveItems.Clear();
            UnactiveItems.Clear();
            //MarkItems.Clear();
        }

        public void AddFragments(int numFragments)
        {
            _totalNumFragments += numFragments;
        }

        public int GetFragments() => _totalNumFragments;
        
        public void ApplyAllItems()
        {
            foreach (var item in ActiveItems) item.GetComponent<AItem>().ApplyItem();
        }

        public void SpawnFragments(int num, float probability, Transform location)
        {
            //Debug.Log($"{location.gameObject.name} suelta {num} fragmentos");
            for (int i = 0; i < num; i++)
            {
                if (Random.value <= probability)
                {
                    GameObject fragment = Instantiate(Fragment);
                    fragment.transform.position = location.position + new Vector3(Random.Range(-0.5f, 5f), 0f, Random.Range(-0.5f, 0.5f));
                }
            }
        }

        public void SpawnFragments(int num, float probability, Vector3 location)
        {
            //Debug.Log($"{location.gameObject.name} suelta {num} fragmentos");
            for (int i = 0; i < num; i++)
            {
                if (Random.value <= probability)
                {
                    GameObject fragment = Instantiate(Fragment);
                    fragment.transform.position = location + new Vector3(Random.Range(-0.5f, 5f), 0f, Random.Range(-0.5f, 0.5f));
                }
            }
        }

        public bool FindItem(string name, out int count)
        {
            count = 0;
            foreach (var item in ActiveItems)
            {
                if (item.GetComponent<AItem>().Data.Name == name) count++;
            }
            foreach (var item in UnactiveItems)
            {
                if (item.GetComponent<AItem>().Data.Name == name) count++;
            }

            return count > 0;
        }

        public bool FindItem(string name, out AItem item)
        {
            item = null;
            foreach (var it in ActiveItems)
            {
                if (it.GetComponent<AItem>().Data.Name == name)
                {
                    item = it.GetComponent<AItem>();
                    return true;
                }
            }
            foreach (var it in UnactiveItems)
            {
                if (it.GetComponent<AItem>().Data.Name == name)
                {
                    item = it.GetComponent<AItem>();
                    return true;
                }
            }

            return false;
        }

        public void MarkItem(GameObject itemButton)
        {
            UnactiveItems.Remove(itemButton);
            if (ActiveItems.Remove(itemButton))
            {
                itemButton.GetComponent<AItem>().SetActivation();
            }

            MarkItems.Add(itemButton);
            _window.ShowItemInFrame(itemButton.GetComponent<AItem>());

            itemButton.GetComponent<RectTransform>().localPosition = new Vector3(9999f, 9999f, 9999f); //Lo sacamos fuera de la vista

            if (_window.gameObject.activeInHierarchy) RelocateItems();
        }

        public void MarkItem(GameObject itemButton, int frameId)
        {
            UnactiveItems.Remove(itemButton);
            if (ActiveItems.Remove(itemButton))
            {
                itemButton.GetComponent<AItem>().SetActivation();
            }

            MarkItems.Add(itemButton);
            _window.ShowItemInFrame(itemButton.GetComponent<AItem>(), frameId);

            itemButton.GetComponent<RectTransform>().localPosition = new Vector3(9999f, 9999f, 9999f); //Lo sacamos fuera de la vista

            if (_window.gameObject.activeInHierarchy) RelocateItems();
        }

        public void ResetMarkItem(GameObject itemButton)
        {
            Debug.Log("INTENTO RESETEO MARCO");
            if (MarkItems.Remove(itemButton))
            {
                Debug.Log("RESETEO MARCO");
                UnactiveItems.Add(itemButton);

                if (_window.gameObject.activeInHierarchy) RelocateItems();
            }
        }

        public void ActivateItem(AItem item)
        {
            AddFragments(-(int)item.Data.Category);

            UnactiveItems.Remove(item.gameObject);
            ActiveItems.Add(item.gameObject);
            if (_window.gameObject.activeInHierarchy) RelocateItems();
        }

        public void DeactivateItem(AItem item)
        {
            AddFragments((int)item.Data.Category);

            ActiveItems.Remove(item.gameObject);
            UnactiveItems.Add(item.gameObject);
            if (_window.gameObject.activeInHierarchy) RelocateItems();
        }

        public bool RemoveItem(string name)
        {
            if (FindItem(name, out AItem item))
            {
                _uiMan.ShowRemoveItemNotification(item);

                if (ActiveItems.Remove(item.gameObject))
                {
                    Debug.Log("Desactivo: " + item.gameObject.name);
                    item.SetActivation();
                }
                UnactiveItems.Remove(item.gameObject);

                Destroy(item.gameObject);
                return true;
            }
            return false;
        }

        public void SafeRemoveItem(GameObject item)
        {
            //_uiMan.ShowRemoveItemNotification(item.GetComponent<AItem>());

            if (item.GetComponent<AItem>().IsActive())
            {
                Debug.Log("Desactivo: " + item.name);
                item.GetComponent<AItem>().SetActivation();
            }
            //UnactiveItems.Remove(item);

            Destroy(item);
        }

        public void ResetInventory()
        {
            ActiveItems.Clear();
            UnactiveItems.Clear();

            _totalNumFragments = 0;
        }

        int timeout = 50;
        public string AddItem(GameObject item, EItemCategory catIfUnable)
        {
            while(!MaxCheck(item.GetComponent<AItem>()) && timeout-- > 0)
            {
                item = GetRandomItem(catIfUnable);
            }
            timeout = 50;

            Debug.Log("Nuevo objeto en el inventario: " + item.name);
            _uiMan.ShowItemNotification(item.GetComponent<AItem>());

            GameObject button = Instantiate(item);
            UnactiveItems.Add(button);

            return item.GetComponent<AItem>().Data.Name;
        }

        public void LoadItems() => RelocateItems();

        public void UnloadItems()
        {
            foreach (var item in ActiveItems)
            {
                item.GetComponent<RectTransform>().SetParent(null);
                DontDestroyOnLoad(item.gameObject);
            }
            foreach (var item in UnactiveItems)
            {
                item.GetComponent<RectTransform>().SetParent(null);
                DontDestroyOnLoad(item.gameObject);
            }
            foreach (var item in MarkItems)
            {
                item.GetComponent<RectTransform>().SetParent(null);
                DontDestroyOnLoad(item.gameObject);
            }
        }

        public void LoseItemsOnNodeExit()
        {
            //El jugador perdera el progreso del inventario al salirse del nodo 
            for (int i = 0; i < ActiveItems.Count; i++)
            {
                if (ActiveItems[i].GetComponent<AItem>().IsNew)
                {
                    SafeRemoveItem(ActiveItems[i]);
                    //ActiveItems[i] = null;
                }
            }
            for (int i = 0; i < UnactiveItems.Count; i++)
            {
                if (UnactiveItems[i].GetComponent<AItem>().IsNew)
                {
                    SafeRemoveItem(UnactiveItems[i]);
                    //UnactiveItems[i] = null;
                }
            }
            //foreach (var item in MarkItems)
            //{
            //    if (item.GetComponent<AItem>().IsNew)
            //    {
            //        Destroy(item.gameObject);
            //    }
            //}

            ActiveItems.Clear();
            UnactiveItems.Clear();
        }

        public void SecureItems()
        {
            foreach (var item in ActiveItems)
            {
                if (item != null)
                {
                    item.GetComponent<AItem>().IsNew = false;
                }
            }
            foreach (var item in UnactiveItems)
            {
                if (item != null)
                {
                    item.GetComponent<AItem>().IsNew = false;
                }
            }
        }

        public void RelocateItems()
        {
            int count = 0;
            foreach (var item in UnactiveItems)
            {
                //item.GetComponent<RectTransform>().localPosition = Position + count++ * Offset; //Los vectores siempre a la derecha de la multiplicacion
                _window.ManageItemPosition(item, Position + count++ * Offset, true);
                item.GetComponent<RectTransform>().localScale = 0.525f * Vector3.one;
            }
            count = 0;
            foreach (var item in ActiveItems)
            {
                //item.GetComponent<RectTransform>().localPosition = Position + count++ * Offset + new Vector3(300f, 0f, 0f); //Los vectores siempre a la derecha de la multiplicacion
                _window.ManageItemPosition(item, Position + count++ * Offset, false);
                item.GetComponent<RectTransform>().localScale = 0.525f * Vector3.one;
            }
        }

        bool MaxCheck(AItem item)
        {
            int num = 0;
            foreach(var i in ActiveItems)
            {
                if (i.GetComponent<AItem>().Data.Name == item.Data.Name) num++;
            }
            foreach (var i in UnactiveItems)
            {
                if (i.GetComponent<AItem>().Data.Name == item.Data.Name) num++;
            }
            return num < item.Data.Max;
        }

        public GameObject GetRandomItem(EItemCategory cat)
        {
            GameObject item = null;
            switch(cat)
            {
                case EItemCategory.Common:
                    item = CommonItemPool[Random.Range(0, CommonItemPool.Length)];
                    break;
                case EItemCategory.Rare:
                    item = RareItemPool[Random.Range(0, RareItemPool.Length)];
                    break;
                case EItemCategory.Epic:
                    item = EpicItemPool[Random.Range(0, EpicItemPool.Length)];
                    break;
                case EItemCategory.Legendary:
                    item = LegendaryItemPool[Random.Range(0, LegendaryItemPool.Length)];
                    break;
            }
            return item;
        }

        public GameObject GetRandomItem()
        {
            GameObject item = null;
            switch (Random.Range(0, 4))
            {
                case 0:
                    item = CommonItemPool[Random.Range(0, CommonItemPool.Length)];
                    break;
                case 1:
                    item = RareItemPool[Random.Range(0, RareItemPool.Length)];
                    break;
                case 2:
                    item = EpicItemPool[Random.Range(0, EpicItemPool.Length)];
                    break;
                case 3:
                    item = LegendaryItemPool[Random.Range(0, LegendaryItemPool.Length)];
                    break;
            }
            return item;
        }

        public void LoadInventory(int[] activeItems, int[] unactiveItems, int[] markItems, int fragments)
        {
            _totalNumFragments = 999999;

            if (activeItems != null)
            {
                foreach (var id in activeItems)
                {
                    GameObject item = Instantiate(SearchForItem(id));
                    ActiveItems.Add(item);
                    item.GetComponent<AItem>().SetActivation();

                    
                }
            }

            if (unactiveItems != null)
            {
                foreach (var id in unactiveItems)
                {
                    GameObject item = Instantiate(SearchForItem(id));
                    UnactiveItems.Add(item);
                }
            }

            if (markItems != null)
            {
                int frameId = 0;
                foreach (var id in markItems)
                {
                    GameObject item = Instantiate(SearchForItem(id));
                    MarkItem(item, frameId++);
                }
            }

            _totalNumFragments = fragments;

            UnloadItems();
        }

        GameObject SearchForItem(int id)
        {
            Debug.Log("Busco item con id: " + id);
            foreach (var item in CommonItemPool)
            {
                if (item.GetComponent<AItem>().Data.Id == id) return item;
            }

            foreach (var item in RareItemPool)
            {
                if (item.GetComponent<AItem>().Data.Id == id) return item;
            }

            foreach (var item in EpicItemPool)
            {
                if (item.GetComponent<AItem>().Data.Id == id) return item;
            }

            foreach (var item in LegendaryItemPool)
            {
                if (item.GetComponent<AItem>().Data.Id == id) return item;
            }

            return null;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}