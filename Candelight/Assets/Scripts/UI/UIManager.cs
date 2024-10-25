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

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        public string NextNodeName;
        public string ActualNodeName;
        public string Chains;
        float _candle;

        public GameObject PauseMenu;
        public GameObject Options;
        public GameObject InventoryUI;
        public Image FadeImage;

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
            GUI.Label(new Rect(10, 10, 200, 50), $"FPS: {1.0f / Time.deltaTime}\nCandle (Nodes left): {_candle}\nCurrent Node: {ActualNodeName}\nNext Node: {NextNodeName}");
            if (GUI.Button(new Rect(200, 10, 150, 20), "WORLD SCENE")) SceneManager.LoadScene("WorldScene");
            if (GUI.Button(new Rect(350, 10, 150, 20), "LEVEL SCENE")) SceneManager.LoadScene("LevelScene");
            if (GUI.Button(new Rect(500, 10, 150, 20), "CALM SCENE")) SceneManager.LoadScene("CalmScene");
            if (GUI.Button(new Rect(10, 60, 200, 20), "CREATE RUNES")) ARune.CreateAllRunes(FindObjectOfType<Mage>());
            GUI.Label(new Rect(10, 80, 200, 500), Chains);
        }

        private void Update()
        {
            Chains = "";
            foreach(var rune in ARune.Spells.Keys)
            {
                Chains += $"{ARune.Spells[rune].Name}: {ARune.InstructionsToString(rune)}\n";
            }
        }

        public void ShowNewInstruction(ESpellInstruction instr)
        {
            _showInstr.ShowInstruction(instr);
        }

        public void ShowValidSpell(AShapeRune spell)
        {
            Debug.Log("Se mostrara: " + spell);
            if (spell != null) StartCoroutine(_showInstr.ShowValidInstructions());
            else _showInstr.ResetSprites();
        }
        public void ShowValidElements(AElementalRune[] elements)
        {
            Debug.Log("Se mostrara: " + elements);
            if (elements != null) StartCoroutine(_showInstr.ShowValidInstructions());
            else _showInstr.ResetSprites();
        }

        public void ShowElements()
        {
            if (_showInstr != null) _showInstr.ShowElements();
        }

        public void RegisterCandle(float candle)
        {
            _candle = candle;
        }

        public void FadeToBlack(float duration)
        {
            FadeImage.DOColor(Color.black, duration);
        }

        public void FadeToWhite(float duration)
        {
            FadeImage.DOColor(Color.white, duration);
        }

        #region UI Menus

        public void OnUIBack(InputAction.CallbackContext ctx)
        {
            if (_windows.TryPop(out var window))
            {
                window.SetActive(false);

                if (_windows.Count == 0) InputManager.Instance.LoadPreviousControls();
            }
        }

        public void LoadUIWindow(GameObject window)
        {
            window.SetActive(true);
            _windows.Push(window);

            if (_windows.Count == 1) InputManager.Instance.LoadControls(EControlMap.UI);
        }

        public void Back() => OnUIBack(new InputAction.CallbackContext());

        public void OnOpenOptions()
        {
            LoadUIWindow(Options);
        }

        #endregion
    }
}