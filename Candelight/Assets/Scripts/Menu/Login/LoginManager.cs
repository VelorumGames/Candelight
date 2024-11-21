using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu.Login
{
    public class LoginManager : MonoBehaviour
    {
        string _key = "hh7s89ahkfsauycwqie2682c";

        [SerializeField] TMP_InputField _name;
        [SerializeField] TMP_InputField _pass;
        [SerializeField] TextMeshProUGUI _info;
        [SerializeField] float _infoTime;

        UIManager _ui;

        string _playerName;
        string _password;

        UserData _data;

        private void Awake()
        {
            _ui = FindObjectOfType<UIManager>();
        }

        bool GetInfo()
        {
            if (_name.text.Any(StringCheck))
            {
                _info.color = Color.red;
                _info.text = "Has introducido car�cteres no permitidos en tu nombre";
                Invoke("ResetText", _infoTime);
                return false;
            }
            if (_pass.text.Any(StringCheck))
            {
                _info.color = Color.red;
                _info.text = "Has introducido car�cteres no permitidos en tu nombre";
                Invoke("ResetText", _infoTime);
                return false;
            }
            _playerName = _name.text;
            _password = Encryption.EncryptString(_key, _pass.text);
            return true;
        }

        bool StringCheck(char l) => (l == '/' || l == '{' || l == '}' || l == '"' || l == '.');

        public void Login()
        {
            if (GetInfo()) StartCoroutine(TryLogin($"PlayerAccounts/{_playerName}"));
        }

        IEnumerator TryLogin(string header)
        {
            _ui.ShowState(EGameState.Database);
            yield return Database.Get<UserData>(header, GetUserData);
            _ui.HideState();

            if (_data != null) //Si ha encontrado los datos con el nombre 
            {
                if (_data.Password == _password) //Si coinciden las contrasenas
                {
                    SaveSystem.PlayerName = _data.Name;

                    _ui.ShowState(EGameState.Loading);
                    SceneManager.LoadScene("MenuScene");
                }
                else
                {
                    _info.color = Color.red;
                    _info.text = "Contrase�a incorrecta";
                    Invoke("ResetText", _infoTime);
                }
            }
            else //Si no los ha encontrado
            {
                _info.color = Color.red;
                _info.text = "No se ha encontrado ning�n usuario con este nombre";
                Invoke("ResetText", _infoTime);
            }
        }

        void GetUserData(UserData data)
        {
            _data = data;
        }

        void CheckForExistingUser(UserData data)
        {
            if (data != null)
            {
                _info.color = Color.red;
                _info.text = "Ya existe un usuario con ese nombre";
                Invoke("ResetText", _infoTime);
                StopAllCoroutines(); //Se detiene la corrutina para que no introduzca los nuevos datos
            }
        }

        public void CreateNewUser()
        {
            if (GetInfo()) StartCoroutine(TryCreateUser());
        }

        IEnumerator TryCreateUser()
        {
            _ui.ShowState(EGameState.Database);

            //Comprobamos si ya existia este usuario
            yield return Database.Get<UserData>($"PlayerAccounts/{_playerName}", CheckForExistingUser);

            _data = new UserData(_playerName, _password);
            Debug.Log($"Se registra usuario con nombre {_playerName} y contrase�a {_password}");
            yield return Database.Send($"PlayerAccounts/{_data.Name}", _data);

            _ui.HideState();

            _info.color = Color.green;
            _info.text = "Usuario creado";

            yield return new WaitForSeconds(1f);

            _ui.ShowState(EGameState.Loading);
            SceneManager.LoadScene("MenuScene");
        }

        public void ResetText() => _info.text = "";
    }
}