using Cameras;
using DG.Tweening;
using Enemy;
using Hechizos.Elementales;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hechizos
{
    public class ArtifactExplosion : MonoBehaviour
    {
        public float Damage;
        public float MaxSize;
        [SerializeField] float _lifeSpan;

        private void Start()
        {
            transform.DOScale(MaxSize, _lifeSpan).Play();
            GetComponentInChildren<MeshRenderer>().sharedMaterial.DOVector(new Vector2(0.99f, 1f), Shader.PropertyToID("_Opacity"), _lifeSpan).Play().OnComplete(Death);

            FindObjectOfType<CameraManager>().Shake(5f, 1f, 2f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<AController>(out var cont))
            {
                cont.RecieveDamage(Damage * (Vector3.Distance(other.transform.position, transform.position) / MaxSize));
            }
        }

        public void Death() => Destroy(gameObject);
    }
}
