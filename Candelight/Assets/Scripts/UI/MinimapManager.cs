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

        /// <summary>
        /// Al crearse una habitacion, se crea su correspondiente indicacion en el minimapa
        /// </summary>
        public void RegisterMinimapRoom(int id, Vector2 offset, ERoomType type)
        {
            //Debug.Log($"ID: {id} para offset {offset}");

            GameObject newRoom = Instantiate(MinimapRoomPrefabs[(int)type], _roomContainer);
            if (type != ERoomType.Start) newRoom.SetActive(false); //Desactivamos las salas para ir mostrandolas a medida que avanza el jugador
            newRoom.GetComponent<RectTransform>().localPosition = offset;
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
            if (!_minimapRooms[id].activeInHierarchy) _minimapRooms[id].SetActive(true);
            _minimapPlayer.GetComponent<RectTransform>().localPosition = _minimapRooms[id].GetComponent<RectTransform>().localPosition;

            //DOTween.To(() => _globalContainer.GetComponent<RectTransform>().localPosition, x => _globalContainer.GetComponent<RectTransform>().localPosition = x, -_minimapPlayer.GetComponent<RectTransform>().localPosition, 1f);
            StopAllCoroutines();
            StartCoroutine(MinimapLerp(_globalContainer.GetComponent<RectTransform>().localPosition, -_minimapPlayer.GetComponent<RectTransform>().localPosition, 1f));
            //_globalContainer.GetComponent<RectTransform>().localPosition = -_minimapPlayer.GetComponent<RectTransform>().localPosition;
        }

        IEnumerator MinimapLerp(Vector3 start, Vector3 end, float time)
        {
            float h = 0;
            while (h < 1)
            {
                _globalContainer.GetComponent<RectTransform>().localPosition = Vector3.Lerp(start, end, h);
                h += Time.deltaTime / time;
                yield return null;
            }
            _globalContainer.GetComponent<RectTransform>().localPosition = end;
        }
    }
}
