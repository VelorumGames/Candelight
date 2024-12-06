using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class ElectricArea : MonoBehaviour
    {
        [SerializeField] LineRenderer[] _rays;
        [SerializeField] float _range;

        List<EnemyController> _enemies = new List<EnemyController>();

        private void Awake()
        {
            foreach (var ray in _rays) ray.gameObject.SetActive(false);
        }

        private IEnumerator Start()
        {
            //Encuentro los enemigos en el rango
            Collider[] cols = Physics.OverlapSphere(transform.position, _range);
            foreach (var c in cols)
            {
                if (c.TryGetComponent<EnemyController>(out var enemy))
                {
                    _enemies.Add(enemy);
                }
            }

            int minLength = _rays.Length <= _enemies.Count ? _rays.Length : _enemies.Count;
            Vector3[] positions = new Vector3[2];
            for (int i = 0; i < minLength; i++)
            {
                positions[0] = new Vector3(0f, 0f, 0f);
                positions[1] = transform.InverseTransformPoint(_enemies[i].transform.position);

                _rays[i].transform.parent = null;
                _rays[i].gameObject.SetActive(true);
                _rays[i].SetPositions(positions);

                _enemies[i].RecieveDamage(10f);
            }

            yield return new WaitForSeconds(0.25f);

            foreach (var ray in _rays)
            {
                Destroy(ray.gameObject);
            }
            Destroy(gameObject);

        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _range);
        }
    }
}