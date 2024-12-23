using System;
using UnityEngine;
using UnityEngine.Playables;

namespace ARPG.Animation
{
    public class CustomParam : AnimParam
    {
        public override void Init(PlayableGraph graph)
        {
            base.Init(graph);
        }

        protected virtual void OnEnable()
        {
            type = Type.Custom;
            animName = name;
        }
    }
}