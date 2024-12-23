using System;
using UnityEngine;

namespace ARPG.Animation
{
    [Serializable, CreateAssetMenu(fileName = "New Animation Setting", menuName = "New Animation Setting/Blend1D")]
    public class Blend1DParam : AnimParam
    {
        public BlendClip1D[] blend1DClips;

        private void OnEnable()
        {
            type = Type.Blend1DClip;
            animInfo.blend1DClips = blend1DClips;
            animName = name;
        }
    }
}