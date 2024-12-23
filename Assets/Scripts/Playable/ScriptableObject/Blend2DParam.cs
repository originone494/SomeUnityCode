using System;
using UnityEngine;

namespace ARPG.Animation
{
    [Serializable, CreateAssetMenu(fileName = "New Animation Setting", menuName = "New Animation Setting/Blend2D")]
    public class Blend2DParam : AnimParam
    {
        public BlendClip2D[] blend2DClips;

        private void OnEnable()
        {
            type = Type.Blend2DClip;
            animInfo.blend2DClips = blend2DClips;
            animName = name;
        }
    }
}