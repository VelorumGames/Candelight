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
using Player;
using Cameras;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEditor.Experimental.GraphView;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [Header("GENERAL")]
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
        public GameObject RemoveInventoryNotif;
        public GameObject InventoryUI;
        public GameObject FragmentHalo;
        public GameObject SpellHalo;
        public Image FadeImage;
        [SerializeField] MinimapManager _minimap;
        [SerializeField] UIGameState _state;

        [Space(10)]
        [Header("FEEDBACK")]
        public Image RedFilter;
        public Image WhiteFilter;
        Volume _vol;
        float _spellTimeScale = 0.5f;
        float _prevTimeScale = 1f;

        ShowInstructions _showInstr;
        CameraManager _camMan;
        PlayerController _player;
        InputManager _input;
        Inventory _inv;

        Coroutine _timeFreeze;

        Stack<GameObject> _windows = new Stack<GameObject>();

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            else Instance = this;

            _vol = FindObjectOfType<Volume>();

            _showInstr = FindObjectOfType<ShowInstructions>();
            _camMan = FindObjectOfType<CameraManager>();
            _player = FindObjectOfType<PlayerController>();
            _input = FindObjectOfType<InputManager>();
            _inv = FindObjectOfType<Inventory>();
        }

        private void OnEnable()
        {
            if (_player != null) _player.OnDamage += PlayerDamageFeedback;
            if (_input != null)
            {
                _input.OnStartElementMode += EnterSpellModeFeedback;
                _input.OnExitElementMode += ExitSpellModeFeedback;
                _input.OnStartShapeMode += EnterSpellModeFeedback;
                _input.OnExitShapeMode += ExitSpellModeFeedback;
            }
            if (_inv)
            {
                _inv.OnFragmentsChange += ShowFragmentHalo;
            }
        }

        private void Start()
        {
            SpellHalo.transform.parent = Camera.main.transform;
            SpellHalo.SetActive(false);

            FragmentHalo.transform.parent = Camera.main.transform;
            FragmentHalo.SetActive(false);
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 40, 200, 70), $"FPS: {CalculateFPS(1.0f / Time.deltaTime)/*}\nCandle (Nodes left): {_candle}\nCurrent Node: {ActualNodeName}\nNext Node: {NextNodeName*/}");
            if (SceneManager.GetActiveScene().name == "LevelScene" || SceneManager.GetActiveScene().name == "CalmScene")
            {
                if (GUI.Button(new Rect(200, 40, 150, 20), "FINISH LEVEL")) FindObjectOfType<MapManager>().EndLevel();
            }
            else if(SceneManager.GetActiveScene().name == "ChallengeScene")
            {
                if (GUI.Button(new Rect(200, 40, 150, 20), "FINISH LEVEL")) FindObjectOfType<SimpleRoomManager>().EndLevel();
            }
            if (SceneManager.GetActiveScene().name == "MenuScene")
            {
                if (GUI.Button(new Rect(350, 40, 150, 20), "REMOVE DATA")) SaveSystem.RemovePreviousGameData();
            }
            //if (GUI.Button(new Rect(500, 40, 150, 20), "CALM SCENE")) SceneManager.LoadScene("CalmScene");
            if (GUI.Button(new Rect(10, 100, 200, 20), "ADD ITEM")) FindObjectOfType<Inventory>().AddItem(FindObjectOfType<Inventory>().GetRandomItem(EItemCategory.Rare));
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
            if (ARune.MageManager != null)
            {
                _chains = "";
                foreach (var rune in ARune.Spells.Keys)
                {
                    if (ARune.FindSpell(rune, out var spell)) _chains += $"{spell.Name}: {ARune.InstructionsToString(rune)}\n";
                }

                _elements = "";
                foreach (var el in ARune.MageManager.GetActiveElements())
                {
                    _elements += $"{el.Name} ";
                }
            }
        }

        public void ShowItemNotification(AItem item)
        {
            InventoryNotif.SetActive(true);
            InventoryNotif.GetComponent<ItemNotification>().LoadItemInfo(item.Data);
        }

        public void ShowRemoveItemNotification(AItem item)
        {
            RemoveInventoryNotif.SetActive(true);
            RemoveInventoryNotif.GetComponent<ItemNotification>().LoadItemInfo(item.Data);
        }

        public void RegisterCandle(float candle)
        {
            _candle = candle;
        }

        #region Spell UI

        public void ShowNewInstruction(ESpellInstruction instr)
        {
            _showInstr.ShowInstruction(instr);
        }

        public void ShowValidSpell(AShapeRune spell)
        {
            //Debug.Log("Se mostrara: " + spell);
            if (spell != null)
            {
                StartCoroutine(_showInstr.ShowValidInstructions());
                _showInstr.ShowShapeResult(spell);
            }
            else _showInstr.ResetSprites();
        }
        public void ShowValidElements(AElementalRune[] elements)
        {
            //Debug.Log("Se mostrara: " + elements);
            if (elements != null)
            {
                StartCoroutine(_showInstr.ShowValidInstructions());
                _showInstr.ShowElementsResult(elements);
            }
            else _showInstr.ResetSprites();
        }

        public void ShowElements()
        {
            if (_showInstr != null) _showInstr.ShowElements();
        }

        #endregion

        #region Fades

        public void FadeToBlack(float duration, Action onEnd)
        {
            if (FadeImage != null) FadeImage.DOColor(Color.black, duration).Play().OnComplete(() => onEnd()).SetUpdate(true);
        }

        public void FadeFromBlack(float duration) //Esto puede lanzar excepcion si el jugador cambia de escena demasiado rapido. Por ahora el safe mode lo mantiene a raya, pero hay que solucionarlo
        {
            if (FadeImage != null)
            {
                FadeImage.color = new Color(0f, 0f, 0f, 1f);
                FadeImage.DOColor(new Color(0f, 0f, 0f, 0f), duration).Play().SetUpdate(true);
            }
        }

        public void FadeFromBlack(float timeOffset, float duration)
        {
            StartCoroutine(ManageTimeOffsetBlack(timeOffset, duration));
        }

        IEnumerator ManageTimeOffsetBlack(float offset, float duration)
        {
            FadeImage.color = new Color(0f, 0f, 0f, 1f);
            yield return new WaitForSeconds(offset);

            if (FadeImage != null)
            {
                FadeImage.DOColor(new Color(0f, 0f, 0f, 0f), duration).Play().SetUpdate(true);

                yield return null;
            }
        }

        public void FadeToWhite(float duration, Ease ease, Action onEnd)
        {
            FadeImage.color = new Color(1f, 1f, 1f, 0f);
            if (FadeImage != null) FadeImage.DOColor(Color.white, duration).Play().OnComplete(() => onEnd()).SetEase(ease).SetUpdate(true);
        }

        public void FadeToWhite(float duration, Action onEnd)
        {
            FadeImage.color = new Color(1f, 1f, 1f, 0f);
            if (FadeImage != null) FadeImage.DOColor(Color.white, duration).Play().OnComplete(() => onEnd()).SetUpdate(true);
        }

        public void FadeFromWhite(float duration)
        {
            if (FadeImage != null)
            {
                FadeImage.color = new Color(1f, 1f, 1f, 1f);
                FadeImage.DOColor(new Color(1f, 1f, 1f, 0f), duration).Play().SetUpdate(true);
            }
        }

        #endregion

        #region Minimap

        public void RegisterMinimapRoom(int id, Vector2 offset, ERoomType type) => _minimap.RegisterMinimapRoom(id, offset, type);
        public void UpdateMinimapRoom(int id, ERoomType newType) => _minimap.UpdateRoom(id, newType);
        public void ShowMinimapRoom(int id) => _minimap.ShowPlayerInRoom(id);

        #endregion

        #region UI Menus

        public void OnUIBack(InputAction.CallbackContext ctx)
        {
            //Debug.Log("Se intenta hacia atras: " + _windows.Count);
            if (_windows.TryPop(out var window))
            {
                //Debug.Log("Hacia atras: " + _windows.Count);
                if (window != null)
                {
                    window.SetActive(false);

                    if (_windows.Count == 0) FindObjectOfType<InputManager>().LoadPreviousControls();
                }
            }
        }

        public void LoadUIWindow(GameObject window)
        {
            window.SetActive(true);
            _windows.Push(window);

            if (_windows.Count == 1) FindObjectOfType<InputManager>()?.LoadControls(EControlMap.UI);
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

        public void ShowWarning(Action action, string desc)
        {
            LoadUIWindow(Warning);
            Warning.GetComponent<WarningWindow>().Show(action, desc);
        }

        public void ShowWarning(Action action, string desc, string yes, string no)
        {
            LoadUIWindow(Warning);
            Warning.GetComponent<WarningWindow>().Show(action, desc, yes, no);
        }

        #endregion

        #region Feedback

        public void BookInstructionFeedback(float duration)
        {
            if (_vol.sharedProfile.TryGet(out Vignette vig))
            {
                float oIntensity = vig.intensity.GetValue<float>();

                DOTween.To(vig.intensity.Override, oIntensity, 0.5f, duration * 0.2f).Play().OnComplete(() => DOTween.To(vig.intensity.Override, 0.5f, oIntensity, duration * 0.8f).Play());
            }
        }

        public void PlayerDamageFeedback(float damage, float remHealth)
        {
            RedFilter.DOFade((1f - remHealth) * 0.5f, 0.2f).Play().OnComplete(() => RedFilter.DOFade(0, 0.2f).Play()).SetUpdate(true);
            _camMan.Shake(5f, 20f, 0.4f);
        }

        public void EnemyDamageFeedback(float damage, float remHealth)
        {
            WhiteFilter.DOFade(0.05f, 0.1f).Play().OnComplete(() => WhiteFilter.DOFade(0, 0.2f).Play()).SetUpdate(true);

            //if (_timeFreeze != null) StopCoroutine(_timeFreeze);
            _timeFreeze = StartCoroutine(FreezeGame(0.15f, remHealth));
        }

        public void WinCombat()
        {
            WhiteFilter.DOFade(0.3f, 0.3f).Play().OnComplete(() => WhiteFilter.DOFade(0, 0.3f).Play()).SetUpdate(true);
        }

        public IEnumerator FreezeGame(float duration, float freezeScale)
        {
            //float oScale = Time.timeScale;
            Time.timeScale = 0.1f;

            yield return new WaitForSecondsRealtime(duration);

            Time.timeScale = 1;

            //Por si acaso
            yield return new WaitForSecondsRealtime(1f);

            if (Time.timeScale != 1) Time.timeScale = 1;
        }

        void EnterSpellModeFeedback()
        {
            _prevTimeScale = Time.timeScale;
            Time.timeScale = _spellTimeScale;

            SpellHalo.SetActive(true);
            SpellHalo.transform.localPosition = new Vector3(0f, GetScreenSizeOffset(), 0.5f);

            GetScreenSizeOffset();
        }

        void ExitSpellModeFeedback()
        {
            Time.timeScale = _prevTimeScale;

            SpellHalo.SetActive(false);
        }

        void ShowFragmentHalo(int prev, int num)
        {
            FragmentHalo.transform.localPosition = new Vector3(0.5f, GetScreenSizeOffset(), 0.5f);
            FragmentHalo.SetActive(true);
            Invoke("ResetHalo", 3f);
        }

        float GetScreenSizeOffset()
        {
            if (Screen.height < 627) return -0.5f;
            else if (Screen.height < 716) return -0.57f;
            else return -0.66f;
            // 627 -> -0.5f
            // 716 -> -0.57f
            // 902 -> -0.66f
            //Debug.Log("Screen: " + Screen.height);
        }

        public void ResetHalo() => FragmentHalo.SetActive(false);

        public void ShowState(EGameState state) => _state.Show(state);

        public void HideState() => _state.Hide();

        #endregion

        private void OnDisable()
        {
            if (_player != null) _player.OnDamage -= PlayerDamageFeedback;
            if (_input != null)
            {
                _input.OnStartElementMode -= EnterSpellModeFeedback;
                _input.OnExitElementMode -= ExitSpellModeFeedback;
                _input.OnStartShapeMode -= EnterSpellModeFeedback;
                _input.OnExitShapeMode -= ExitSpellModeFeedback;
            }
            if (_inv)
            {
                _inv.OnFragmentsChange -= ShowFragmentHalo;
            }
        }
    }
}