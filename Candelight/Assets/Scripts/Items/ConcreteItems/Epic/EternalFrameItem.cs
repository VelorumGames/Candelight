using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Items
{
    public class EternalFrameItem : AItem
    {
        EternalFrame _frame;

        void OnSceneChanged(Scene scene, LoadSceneMode mode)
        {
            EternalFrame[] frames = FindObjectsOfType<EternalFrame>();

            foreach(var f in frames)
            {
                if (!f.IsUnlocked())
                {
                    _frame = f;
                    _frame.UnlockFrame();
                }
            }
        }

        protected override void ApplyProperty()
        {
            EternalFrame[] frames = FindObjectsOfType<EternalFrame>();

            foreach (var f in frames)
            {
                if (!f.IsUnlocked())
                {
                    _frame = f;
                    _frame.UnlockFrame();
                }
            }

            SceneManager.sceneLoaded += OnSceneChanged;
        }

        protected override void ResetProperty()
        {
            if (_frame) _frame.LockFrame();

            SceneManager.sceneLoaded -= OnSceneChanged;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneChanged;
        }
    }
}