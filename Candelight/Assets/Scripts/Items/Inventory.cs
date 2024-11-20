using BehaviourAPI.Core.Actions;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using World;

namespace Items
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance;
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
        public List<GameObject> ItemsList = new List<GameObject>();
        public List<GameObject> UnactiveItems = new List<GameObject>();
        public List<GameObject> ActiveItems = new List<GameObject>();

        [Space(10)]
        [SerializeField] RectTransform _itemContainer;

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
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            _uiMan = FindObjectOfType<UIManager>();
            if (_uiMan != null) _itemContainer = _uiMan.InventoryUI.GetComponent<RectTransform>();

            if(scene.name == "WorldScene")
            {
                SecureItems();
            }
        }

        void OnSceneUnloaded(Scene scene)
        {
            
        }

        public void AddFragments(int numFragments)
        {
            _totalNumFragments += numFragments;
        }

        public int GetFragments() => _totalNumFragments;
        
        public void ApplyAllItems()
        {
            foreach (var item in ItemsList)
            {
                item.GetComponent<AItem>().ApplyItem();
            }
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

        public void AddItem(GameObject item)
        {
            Debug.Log("Nuevo objeto en el inventario: " + item.name);
            if(MaxCheck(item.GetComponent<AItem>()))
            {
                ItemsList.Add(item);
                _uiMan.ShowItemNotification(item.GetComponent<AItem>());
                //ItemsList.Add(item.GetComponent<AItem>());

                //GameObject itemButton = Instantiate(item, _itemContainer);
                //itemButton.GetComponent<RectTransform>().localPosition = Position + (ItemsList.Count - 1) * Offset; //Los vectores siempre a la derecha de la multiplicacion
            }
        }

        public bool LoadItems()
        {
            //Cada vez que se abra el inventario, se cargaran los datos almacenados en este script (en caso de ser necesarios)
            foreach(var item in ItemsList)
            {
                GameObject itemButton = Instantiate(item, _itemContainer);
                if (itemButton.GetComponent<AItem>().IsActive()) ActiveItems.Add(itemButton);
                else UnactiveItems.Add(itemButton);

                item.GetComponent<AItem>().IsNew = false;
                //itemButton.GetComponent<RectTransform>().localPosition = Position + (ItemsList.Count - 1) * Offset; //Los vectores siempre a la derecha de la multiplicacion
            }

            RelocateItems();

            return true;
        }

        public void LooseItemsOnNodeExit()
        {
            //El jugador perdera el progreso del inventario al salirse del nodo 
            ActiveItems.RemoveAll(item =>
            {
                if (item.GetComponent<AItem>().IsNew) item.GetComponent<AItem>().SetActivation();
                return item.GetComponent<AItem>().IsNew;
            });

            UnactiveItems.RemoveAll(item => item.GetComponent<AItem>().IsNew);
        }

        public void SecureItems()
        {
            foreach (var item in ItemsList) item.GetComponent<AItem>().IsNew = false;
        }

        public void UnloadItems()
        {
            ActiveItems.Clear();
            UnactiveItems.Clear();
        }

        public void RelocateItems()
        {
            int count = 0;
            foreach (var item in UnactiveItems)
            {
                item.GetComponent<RectTransform>().localPosition = Position + count++ * Offset; //Los vectores siempre a la derecha de la multiplicacion
            }
            count = 0;
            foreach (var item in ActiveItems)
            {
                item.GetComponent<RectTransform>().localPosition = Position + count++ * Offset + new Vector3(300f, 0f, 0f); //Los vectores siempre a la derecha de la multiplicacion
            }
        }

        bool MaxCheck(AItem item)
        {
            int num = 0;
            foreach(var i in ItemsList)
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

        public void LoadInventory(int[] activeItems, int[] unactiveItems, int fragments)
        {
            _totalNumFragments = 999999;

            foreach (var id in activeItems)
            {
                GameObject item = SearchForItem(id);
                ActiveItems.Add(item);
                item.GetComponent<AItem>().SetActivation();
            }

            foreach (var id in unactiveItems)
            {
                UnactiveItems.Add(SearchForItem(id));
            }

            _totalNumFragments = fragments;
        }

        GameObject SearchForItem(int id)
        {
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