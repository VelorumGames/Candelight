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
using Menu;
using World;
using TMPro;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [Header("GENERAL")]
        public WorldInfo World;
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
        public DeathWindow _deathWindow;
        public Image FadeImage;
        [SerializeField] MinimapManager _minimap;
        [SerializeField] UIGameState _state;

        [Space(10)]
        public GameObject TutorialNotif;

        [Space(10)]
        [Header("FEEDBACK")]
        public Image RedFilter;
        public Image WhiteFilter;
        Volume _vol;
        float _spellTimeScale = 0.4f;
        float _prevTimeScale = 1f;
        //bool _isInSpellMode;

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

            Time.timeScale = 1f;
        }

        private void OnEnable()
        {
            if (_player != null)
            {
                _player.OnDamage += PlayerDamageFeedback;
                _player.OnNewInstruction += ShowNewInstruction;
                _player.OnSpell += ShowValidSpell;
                _player.OnElements += ShowValidElements;
                World.OnCandleChanged += RegisterCandle;
            }
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

            World.OnPlayerDeath += Death;
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

        void Death()
        {
            Invoke("DelayedDeath", 2f);
        }

        public void DelayedDeath() => _deathWindow.gameObject.SetActive(true);

        #region Spell UI

        public void ShowNewInstruction(ESpellInstruction instr)
        {
            if (_showInstr == null) _showInstr = FindObjectOfType<ShowInstructions>();
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

        public void ManageAuxiliarRuneReset() => Invoke("AuxiliarResetRuneSprites", 0.5f);

        public void AuxiliarResetRuneSprites()
        {
            if (_showInstr != null && !_input.IsInSpellMode()) _showInstr.ResetSprites();
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

            //_input.LoadControls(EControlMap.None);
            yield return new WaitForSeconds(offset);
            //_input.LoadPreviousControls();

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
                    UIMoveOnDisable[] endMoves = window.GetComponentsInChildren<UIMoveOnDisable>();
                    foreach (var m in endMoves) m.DisableElement();

                    UIFadeOnDisable[] endFades = window.GetComponentsInChildren<UIFadeOnDisable>();
                    foreach (var f in endFades) f.FadeElement();

                    if (endMoves.Length == 0 && endFades.Length == 0) window.SetActive(false);
                }

                if (_windows.Count == 0) _input.LoadPreviousControls();
            }
        }

        public void LoadUIWindow(GameObject window)
        {
            if (window != null && !_input.IsInSpellMode())
            {
                window.SetActive(true);
                _windows.Push(window);

                if (_windows.Count == 1) _input?.LoadControls(EControlMap.UI);
            }
        }

        public void LoadUIWindow(GameObject window, string key)
        {
            if (window != null && !_input.IsInSpellMode())
            {
                window.SetActive(true);
                _windows.Push(window);

                if (_windows.Count == 1) _input.LoadControls(EControlMap.UI);

                InputAction back = _input.Input.FindActionMap("UI").FindAction("Back");

                back.AddBinding($"<Keyboard>/{key}");
                back.performed += OnResetBack;
            }
        }

        void OnResetBack(InputAction.CallbackContext _)
        {
            int numBinds = _input.Input.FindActionMap("UI").FindAction("Back").bindings.Count;

            InputAction back = _input.Input.FindActionMap("UI").FindAction("Back");

            back.ChangeBinding(numBinds - 1).Erase();
            back.performed -= OnResetBack;
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
            yield return new WaitForSecondsRealtime(0.5f);

            AuxiliarTimeReset();
        }

        void EnterSpellModeFeedback()
        {
            _prevTimeScale = Time.timeScale;
            Time.timeScale = _spellTimeScale;

            SpellHalo.SetActive(true);
            float offset = GetScreenSizeOffset();
            SpellHalo.transform.localPosition = new Vector3(0f, -offset, offset);

            GetScreenSizeOffset();
        }

        void ExitSpellModeFeedback()
        {
            Time.timeScale = _prevTimeScale;

            SpellHalo.SetActive(false);

            Invoke("AuxiliarTimeReset", 0.5f);
        }

        public void AuxiliarTimeReset()
        {
            if (!_input.IsInSpellMode()) Time.timeScale = 1;
        }

        void ShowFragmentHalo(int prev, int num)
        {
            float offset = GetScreenSizeOffset();
            FragmentHalo.transform.localPosition = new Vector3(0.5f, -offset, offset);
            FragmentHalo.SetActive(true);
            Invoke("ResetHalo", 3f);
        }

        float GetScreenSizeOffset()
        {
            return (0.38f * Screen.height + 35.66f) / 484f;
        }

        public void ResetHalo() => FragmentHalo.SetActive(false);

        public void ShowState(EGameState state) => _state.Show(state);

        public void HideState() => _state.Hide();

        #endregion

        public void ShowTutorial(string s)
        {
            TutorialNotif.SetActive(true);
            TutorialNotif.GetComponentInChildren<TextMeshProUGUI>().text = s;
        }

        public void ShowTutorial(string s, float duration)
        {
            TutorialNotif.GetComponent<ItemNotification>().SetDuration(duration);
            TutorialNotif.SetActive(true);
            TutorialNotif.GetComponentInChildren<TextMeshProUGUI>().text = s;
        }

        private void OnDisable()
        {
            if (_player != null)
            {
                _player.OnDamage -= PlayerDamageFeedback;
                _player.OnNewInstruction -= ShowNewInstruction;
                _player.OnSpell -= ShowValidSpell;
                _player.OnElements -= ShowValidElements;
                World.OnCandleChanged -= RegisterCandle;
            }
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

            World.OnPlayerDeath += Death;
        }
    }
}