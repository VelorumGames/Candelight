using System.Collections;
using System.Collections.Generic;
using System.IO;
using UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Items
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance;

        [Header("===POOLS===")]
        [Space(10)]
        public GameObject[] CommonItemPool;
        public GameObject[] RareItemPool;
        public GameObject[] EpicItemPool;
        public GameObject[] LegendaryItemPool;
        public GameObject Fragment;

        [SerializeField] int _totalNumFragments;

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

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            //DEBUG
            //AddItem(GetRandomItem(EItemCategory.Epic));
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            _itemContainer = FindObjectOfType<UIManager>().InventoryUI.GetComponent<RectTransform>();
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
            Debug.Log($"{location.gameObject.name} suelta {num} fragmentos");
            for (int i = 0; i < num; i++)
            {
                if (Random.value <= probability)
                {
                    GameObject fragment = Instantiate(Fragment);
                    fragment.transform.position = location.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
                }
            }
        }

        public void AddItem(GameObject item)
        {
            Debug.Log("Nuevo objeto en el inventario: " + item.name);
            if(MaxCheck(item.GetComponent<AItem>()))
            {
                ItemsList.Add(item);
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
                //itemButton.GetComponent<RectTransform>().localPosition = Position + (ItemsList.Count - 1) * Offset; //Los vectores siempre a la derecha de la multiplicacion
            }

            RelocateItems();

            return true;
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

        private void OnDisable()
        {
            
        }
    }

}