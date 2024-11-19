using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using DG.Tweening;

namespace UI
{
    public class MinimapManager : MonoBehaviour
    {
        public GameObject[] MinimapRoomPrefabs;
        [SerializeField] GameObject _minimapPlayer;
        [SerializeField] Transform _roomContainer;
        [SerializeField] Transform _globalContainer;
        Dictionary<int, GameObject> _minimapRooms = new Dictionary<int, GameObject>();

        float _movement = 15f;
        bool _lerping;

        /// <summary>
        /// Al crearse una habitacion, se crea su correspondiente indicacion en el minimapa
        /// </summary>
        public void RegisterMinimapRoom(int id, Vector2 offset, ERoomType type)
        {
            //Debug.Log($"ID: {id} para offset {offset}");

            GameObject newRoom = Instantiate(MinimapRoomPrefabs[(int)type], _roomContainer);
            newRoom.GetComponent<RectTransform>().localPosition = offset;

            if (type != ERoomType.Start) newRoom.SetActive(false); //Desactivamos las salas para ir mostrandolas a medida que avanza el jugador
            else
            {
                _roomContainer.GetComponent<RectTransform>().localPosition = -offset + new Vector2(0f, 30f);
                _minimapPlayer.GetComponent<RectTransform>().position = newRoom.GetComponent<RectTransform>().position;
            }
            
            _minimapRooms.Add(id, newRoom);
        }

        public void UpdateRoom(int id, ERoomType newType)
        {
            Vector2 oldOffset = _minimapRooms[id].GetComponent<RectTransform>().localPosition;
            Destroy(_minimapRooms[id]);
            _minimapRooms.Remove(id);

            RegisterMinimapRoom(id, oldOffset, newType);
        }

        public void ShowPlayerInRoom(int id)
        {
            if (!_lerping)
            {
                if (!_minimapRooms[id].activeInHierarchy) _minimapRooms[id].SetActive(true);

                //Pillamos la posicion previa del player
                Vector3 prevPos = _minimapPlayer.GetComponent<RectTransform>().position;

                //Pibe teletransportado
                _minimapPlayer.GetComponent<RectTransform>().position = _minimapRooms[id].GetComponent<RectTransform>().position;

                //Pillamos la posicion del mapa actual
                Vector3 target = _globalContainer.GetComponent<RectTransform>().position;
                Vector3 offset = _minimapPlayer.GetComponent<RectTransform>().position - prevPos;

                //Debug.Log($"X: {offset.x}; Y: {offset.y}");

                if (offset.x == 0 && offset.y == 0) return;

                _lerping = true;
                target += offset.x == 0 ? new Vector3(0f, offset.y > 0 ? -_movement : _movement, 0f) : new Vector3(offset.x > 0 ? -_movement : _movement, 0f, 0f);
                _globalContainer.GetComponent<RectTransform>().DOMove(target, 1f).Play().OnComplete(() => _lerping = false);
            }
        }
    }
}
