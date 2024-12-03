using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactuables
{
    public class OpenFlowerInter : AInteractuables
    {
        Animator _anim;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            OnTrigger += Open;
        }

        void Open()
        {

        }

        public override void Interaction()
        {
            
        }

        private void OnDisable()
        {
            OnTrigger -= Open;
        }
    }
}