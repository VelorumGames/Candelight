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
using System.Collections;
using System;
using UI.Window;

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
        float[] _fps = new float[10];
        int _fpsCount;
        float _fpsSum;

        public GameObject PauseMenu;
        public GameObject Warning;
        public GameObject Options;
        public GameObject InventoryNotif;
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
            GUI.Label(new Rect(10, 40, 200, 70), $"FPS: {CalculateFPS(1.0f / Time.deltaTime)}\nCandle (Nodes left): {_candle}\nCurrent Node: {ActualNodeName}\nNext Node: {NextNodeName}");
            if (SceneManager.GetActiveScene().name == "LevelScene" || SceneManager.GetActiveScene().name == "CalmScene")
            {
                if (GUI.Button(new Rect(200, 40, 150, 20), "FINISH LEVEL")) FindObjectOfType<MapManager>().EndLevel();
            }
            else if(SceneManager.GetActiveScene().name == "ChallengeScene")
            {
                if (GUI.Button(new Rect(200, 40, 150, 20), "FINISH LEVEL")) FindObjectOfType<SimpleRoomManager>().EndLevel();
            }
            //if (GUI.Button(new Rect(350, 40, 150, 20), "LEVEL SCENE")) SceneManager.LoadScene("LevelScene");
            //if (GUI.Button(new Rect(500, 40, 150, 20), "CALM SCENE")) SceneManager.LoadScene("CalmScene");
            if (GUI.Button(new Rect(10, 100, 200, 20), "ADD ITEM")) Inventory.Instance.AddItem(Inventory.Instance.GetRandomItem(EItemCategory.Rare));
            if (GUI.Button(new Rect(10, 120, 200, 20), "CREATE RUNES")) ARune.CreateAllRunes(FindObjectOfType<Mage>());
            GUI.Label(new Rect(10, 140, 200, 500), $"Current elements: {_elements}\nActive runes:\n{_chains}");
        }

        int CalculateFPS(float frameSpeed)
        {
            _fps[_fpsCount] = frameSpeed;
            _fpsCount++;

            if (_fpsCount == _fps.Length)
            {
                _fpsSum = 0;
                _fpsCount = 0;
                foreach (var fps in _fps) _fpsSum += fps;
            }

            return (int) _fpsSum / _fps.Length;
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

        public void FadeToBlack(float duration, System.Action onEnd)
        {
            if (FadeImage != null) FadeImage.DOColor(Color.black, duration).OnComplete(() => onEnd());
        }

        public void FadeFromBlack(float duration) //Esto puede lanzar excepcion si el jugador cambia de escena demasiado rapido. Por ahora el safe mode lo mantiene a raya, pero hay que solucionarlo
        {
            if (FadeImage != null)
            {
                Debug.Log("Fade image: " + FadeImage);
                FadeImage.color = new Color(0f, 0f, 0f, 1f);
                FadeImage.DOColor(new Color(0f, 0f, 0f, 0f), duration).Play();
            }
        }

        public void FadeFromBlack(float timeOffset, float duration) //Esto puede lanzar excepcion si el jugador cambia de escena demasiado rapido. Por ahora el safe mode lo mantiene a raya, pero hay que solucionarlo
        {
            StartCoroutine(ManageTimeOffsetBlack(timeOffset, duration));
        }

        IEnumerator ManageTimeOffsetBlack(float offset, float duration)
        {
            FadeImage.color = new Color(0f, 0f, 0f, 1f);
            yield return new WaitForSeconds(offset);

            if (FadeImage != null)
            {
                FadeImage.DOColor(new Color(0f, 0f, 0f, 0f), duration).Play();

                yield return null;
            }
        }

        public void FadeToWhite(float duration, System.Action onEnd)
        {
            if (FadeImage != null) FadeImage.DOColor(Color.white, duration).OnComplete(() => onEnd());
        }

        public void FadeFromWhite(float duration)
        {
            if (FadeImage != null)
            {
                FadeImage.color = new Color(1f, 1f, 1f, 1f);
                FadeImage.DOColor(new Color(1f, 1f, 1f, 0f), duration);
            }
        }

        public void RegisterMinimapRoom(int id, Vector2 offset, ERoomType type) => _minimap.RegisterMinimapRoom(id, offset, type);
        public void UpdateMinimapRoom(int id, ERoomType newType) => _minimap.UpdateRoom(id, newType);
        public void ShowMinimapRoom(int id) => _minimap.ShowPlayerInRoom(id);

        public void ShowItemNotification(AItem item)
        {
            InventoryNotif.SetActive(true);
            InventoryNotif.GetComponent<ItemNotification>().LoadItemInfo(item.Data);
        }

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

        public void ShowWarning(Action action, string desc) => Warning.GetComponent<WarningWindow>().Show(action, desc);

        public void ShowWarning(Action action, string desc, string yes, string no) => Warning.GetComponent<WarningWindow>().Show(action, desc, yes, no);

        #endregion
    }
}