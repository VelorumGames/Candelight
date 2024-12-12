using Hechizos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cameras;
using DG.Tweening;
using UI;

namespace SpellInteractuable
{
    public class SI_Explosion : ASpellInteractuable
    {
        [SerializeField] GameObject _explosion;
        [SerializeField] GameObject[] _energies;

        [SerializeField] Transform _initialExpl;

        CameraManager _cam;

        bool _active = true;

        private void Awake()
        {
            _cam = FindObjectOfType<CameraManager>();
        }

        protected override void ApplyInteraction(ASpell spell)
        {
            if (_active)
            {
                StartCoroutine(ManageExplosion());
                _active = false;
            }
        }

        IEnumerator ManageExplosion()
        {
            FindObjectOfType<UISoundManager>().PlayIdrianExplosion();

            foreach (var e in _energies)
            {
                e.SetActive(true);
                _cam.Shake(Random.Range(1f, 2f), Random.Range(5f, 8f), Random.Range(0.1f, 0.5f));

                yield return new WaitForSeconds(1.2f);
            }

            _initialExpl.gameObject.SetActive(true);
            _initialExpl.DOScale(0.4f, 0.3f);

            yield return new WaitForSeconds(1f);

            GameObject expl = Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
