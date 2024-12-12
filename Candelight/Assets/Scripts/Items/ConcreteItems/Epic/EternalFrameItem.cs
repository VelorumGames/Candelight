using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Items
{
    public class EternalFrameItem : AItem
    {
        public EternalFrame Frame;
        public int Id;
        bool _active;

        void OnSceneChanged(Scene scene, LoadSceneMode mode)
        {
            if (_active)
            {
                EternalFrame[] frames = FindObjectsOfType<EternalFrame>();

                foreach (var f in frames)
                {
                    if (!f.IsUnlocked())
                    {
                        Id = f.Id;
                        Frame = f;
                        f.UnlockFrame();
                        break;
                    }
                }
            }
        }

        protected override void ApplyProperty()
        {
            _active = true;

            EternalFrame[] frames = FindObjectsOfType<EternalFrame>();

            foreach (var f in frames)
            {
                if (!f.IsUnlocked())
                {
                    Frame = f;
                    Frame.UnlockFrame();
                    break;
                }
            }

            SceneManager.sceneLoaded += OnSceneChanged;
        }

        protected override void ResetProperty()
        {
            _active = false;

            Debug.Log("UUUUUUUUUUUUUUU: " + Frame);
            if (Frame) Frame.LockFrame();

            SceneManager.sceneLoaded -= OnSceneChanged;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneChanged;
        }
    }
}