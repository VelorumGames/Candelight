using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UISoundManager : MonoBehaviour
    {
        public AudioClip[] Inventory;
        public AudioClip[] Item; //0: Activar, 1: Desactivar, 2: Enmarcar, 3: Desenmarcar, 4: Hover, 5: Activate frame

        public AudioClip Button;
        public AudioClip CantButton;

        public AudioClip Move;

        public AudioClip[] Pause;

        public AudioClip[] NodeInfo; //0: Mostrar, 1: Entrar, 2:CompletedNode
        public AudioClip LevelName;

        public AudioClip[] Dialogue; //0: Next, 1: End
        public AudioClip[] Voices;

        [SerializeField] AudioSource _audio;
        [SerializeField] AudioSource _voicesAudio;

        public void PlayButtonSound() => _audio.PlayOneShot(Button);
        public void PlayCantButtonSound() => _audio.PlayOneShot(CantButton);

        public void PlayOpenInventory() => _audio.PlayOneShot(Inventory[0]);
        public void PlayCloseInventory() => _audio.PlayOneShot(Inventory[1]);

        public void PlayActivateItem() => _audio.PlayOneShot(Item[0]);
        public void PlayDeactivateItem() => _audio.PlayOneShot(Item[1]);
        public void PlayMarkItem() => _audio.PlayOneShot(Item[2]);
        public void PlayDemarkItem() => _audio.PlayOneShot(Item[3]);
        public void PlayHoverItem() => _audio.PlayOneShot(Item[4]);
        public void PlayActivateFrame() => _audio.PlayOneShot(Item[5]);

        public void PlayOpenPause() => _audio.PlayOneShot(Pause[0]);
        public void PlayClosePause() => _audio.PlayOneShot(Pause[1]);

        public void PlayMove() => _audio.PlayOneShot(Move);

        public void PlayShowNodeInfo() => _audio.PlayOneShot(NodeInfo[0]);
        public void PlayEnterNode() => _audio.PlayOneShot(NodeInfo[1]);
        public void PlayCompletedNode() => _audio.PlayOneShot(NodeInfo[2]);
        public void PlayLevelName() => _audio.PlayOneShot(LevelName);

        public void PlayDialogueNext() => _audio.PlayOneShot(Dialogue[0]);
        public void PlayDialogueEnd() => _audio.PlayOneShot(Dialogue[1]);
        public void PlayVoice()
        {
            _voicesAudio.pitch = Random.Range(0.5f, 1.5f);
            //_voicesAudio.volume = Random.Range(0.45f, 0.5f);
            _voicesAudio.PlayOneShot(Voices[Random.Range(0, Voices.Length)]);
        }
    }
}