using Controls;
using DG.Tweening;
using Hechizos;
using Hechizos.DeForma;
using Hechizos.Elementales;
using Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Map;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        public string NextNodeName;
        public string ActualNodeName;
        string _chains;
        string _elements;
        float _candle;

        public GameObject PauseMenu;
        public GameObject Options;
        public GameObject InventoryUI;
        public Image FadeImage;
        [SerializeField] MinimapManager _minimap;

        ShowInstructions _showInstr;

        Stack<GameObject> _windows = new Stack<GameObject>();

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            _showInstr = FindObjectOfType<ShowInstructions>();
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 40, 200, 70), $"FPS: {1.0f / Time.deltaTime}\nCandle (Nodes left): {_candle}\nCurrent Node: {ActualNodeName}\nNext Node: {NextNodeName}");
            //if (GUI.Button(new Rect(200, 40, 150, 20), "WORLD SCENE")) SceneManager.LoadScene("WorldScene");
            //if (GUI.Button(new Rect(350, 40, 150, 20), "LEVEL SCENE")) SceneManager.LoadScene("LevelScene");
            //if (GUI.Button(new Rect(500, 40, 150, 20), "CALM SCENE")) SceneManager.LoadScene("CalmScene");
            if (GUI.Button(new Rect(10, 100, 200, 20), "ADD ITEM")) Inventory.Instance.AddItem(Inventory.Instance.GetRandomItem(EItemCategory.Rare));
            if (GUI.Button(new Rect(10, 120, 200, 20), "CREATE RUNES")) ARune.CreateAllRunes(FindObjectOfType<Mage>());
            GUI.Label(new Rect(10, 140, 200, 500), $"Current elements: {_elements}\nActive runes:\n{_chains}");
        }

        private void Update()
        {
            _chains = "";
            foreach(var rune in ARune.Spells.Keys)
            {
                if (ARune.FindSpell(rune, out var spell)) _chains += $"{spell.Name}: {ARune.InstructionsToString(rune)}\n";
            }

            _elements = "";
            foreach(var el in ARune.MageManager.GetActiveElements())
            {
                _elements += $"{el.Name} ";
            }
        }

        #region Spell UI

        public void ShowNewInstruction(ESpellInstruction instr)
        {
            _showInstr.ShowInstruction(instr);
        }

        public void ShowValidSpell(AShapeRune spell)
        {
            //Debug.Log("Se mostrara: " + spell);
            if (spell != null) StartCoroutine(_showInstr.ShowValidInstructions());
            else _showInstr.ResetSprites();
        }
        public void ShowValidElements(AElementalRune[] elements)
        {
            //Debug.Log("Se mostrara: " + elements);
            if (elements != null) StartCoroutine(_showInstr.ShowValidInstructions());
            else _showInstr.ResetSprites();
        }

        public void ShowElements()
        {
            if (_showInstr != null) _showInstr.ShowElements();
        }

        #endregion

        public void RegisterCandle(float candle)
        {
            _candle = candle;
        }

        public void FadeToBlack(float duration)
        {
            FadeImage.DOColor(Color.black, duration);
        }

        public void FadeFromBlack(float duration)
        {
            FadeImage.color = new Color(0f, 0f, 0f, 1f);
            FadeImage.DOColor(new Color(0f, 0f, 0f, 0f), duration).Play();
        }

        public void FadeToWhite(float duration)
        {
            FadeImage.DOColor(Color.white, duration);
        }

        public void FadeFromWhite(float duration)
        {
            FadeImage.color = new Color(1f, 1f, 1f, 1f);
            FadeImage.DOColor(new Color(1f, 1f, 1f, 0f), duration);
        }

        public void RegisterMinimapRoom(int id, Vector2 offset, ERoomType type) => _minimap.RegisterMinimapRoom(id, offset, type);
        public void UpdateMinimapRoom(int id, ERoomType newType) => _minimap.UpdateRoom(id, newType);
        public void ShowMinimapRoom(int id) => _minimap.ShowPlayerInRoom(id);

        #region UI Menus

        public void OnUIBack(InputAction.CallbackContext ctx)
        {
            Debug.Log("Se intenta hacia atras: " + _windows.Count);
            if (_windows.TryPop(out var window))
            {
                Debug.Log("Hacia atras: " + _windows.Count);
                window.SetActive(false);

                if (_windows.Count == 0) FindObjectOfType<InputManager>().LoadPreviousControls();
            }
        }

        public void LoadUIWindow(GameObject window)
        {
            window.SetActive(true);
            _windows.Push(window);

            if (_windows.Count == 1) FindObjectOfType<InputManager>().LoadControls(EControlMap.UI);
        }

        public void LoadUIWindow(GameObject window, string key)
        {
            window.SetActive(true);
            _windows.Push(window);

            if (_windows.Count == 1) FindObjectOfType<InputManager>().LoadControls(EControlMap.UI);

            FindObjectOfType<InputManager>().Input.FindActionMap("UI").FindAction("Back").AddBinding($"<Keyboard>/{key}");
            FindObjectOfType<InputManager>().Input.FindActionMap("UI").FindAction("Back").performed += OnResetBack;
        }

        void OnResetBack(InputAction.CallbackContext _)
        {
            FindObjectOfType<InputManager>().Input.FindActionMap("UI").FindAction("Back").ChangeBinding(1).Erase();
            FindObjectOfType<InputManager>().Input.FindActionMap("UI").FindAction("Back").performed -= OnResetBack;
        }

        public void Back() => OnUIBack(new InputAction.CallbackContext());

        public void OnOpenOptions()
        {
            LoadUIWindow(Options);
        }

        public void OnReturnToMenu()
        {
            SceneManager.LoadScene("MenuScene");
        }

        #endregion
    }
}