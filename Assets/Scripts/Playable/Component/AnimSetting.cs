using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace ARPG.Animation
{
    [Serializable]
    public class AnimSetting
    {
        public AnimParam param;
    }

    [Serializable]
    public class AnimParam : ScriptableObject
    {
        public struct AnimInfo
        {
            public AnimationClip clip;
            public BlendClip1D[] blend1DClips;
            public BlendClip2D[] blend2DClips;
            public AnimBehaviour animBehaviour;
            public Animator animator;
        }

        public enum Type
        {
            Single,
            Blend1DClip,
            Blend2DClip,
            Animator,
            Custom
        }

        public string animName = "Anim";
        public Type type = Type.Single;
        public AnimInfo animInfo;
        public float enterTime;

        public virtual void Init(PlayableGraph graph)
        {
        }
    }
}