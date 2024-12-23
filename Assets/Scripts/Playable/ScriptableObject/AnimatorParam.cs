using System;
using UnityEngine;

namespace ARPG.Animation
{
    [Serializable, CreateAssetMenu(fileName = "New Animation Setting", menuName = "New Animation Setting/MixAnimator")]
    public class AnimatorParam : AnimParam
    {
        public Animator animator;

        private void OnEnable()
        {
            type = Type.Animator;
            animInfo.animator = animator;
            animName = name;
        }
    }
}