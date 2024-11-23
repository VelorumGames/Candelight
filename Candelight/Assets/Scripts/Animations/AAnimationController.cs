using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Animations
{
    public class AAnimationController : MonoBehaviour
    {
        SpriteRenderer _rend;
        protected Animator Anim;

        protected ESpriteOrientation Orientation;

        protected void Awake()
        {
            _rend = GetComponent<SpriteRenderer>();
            Anim = GetComponent<Animator>();
        }

        public void FlipX()
        {
            _rend.flipX = !_rend.flipX;
        }
    }

    public enum ESpriteOrientation
    {
        Down = -1,
        Up = 1,
        Left = -2,
        Right = 2
    }
}