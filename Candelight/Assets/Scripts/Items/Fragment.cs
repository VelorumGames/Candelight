using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Fragment : MonoBehaviour
    {
        public AudioClip[] _sounds;
        AudioSource _audio;

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
        }

        private void Start()
        {
            StartCoroutine(AutoAdd());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _audio.PlayOneShot(_sounds[Random.Range(0, _sounds.Length)]);

                FindObjectOfType<Inventory>().AddFragments(1);
                transform.localScale = new Vector3();
                GetComponent<Collider>().enabled = false;
                Invoke("DelayedDeath", 2f);
            }
        }

        IEnumerator AutoAdd()
        {
            yield return new WaitForSeconds(20f);

            FindObjectOfType<Inventory>().AddFragments(1);
            Destroy(gameObject);
        }

        public void DelayedDeath() => Destroy(gameObject);
    }
}