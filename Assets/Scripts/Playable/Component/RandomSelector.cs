﻿using UnityEngine;
using UnityEngine.Playables;

namespace ARPG.Animation
{
    public class RandomSelector : AnimSelector
    {
        public RandomSelector(PlayableGraph graph) : base(graph)
        {
        }

        public override int Select()
        {
            currentIndex = Random.Range(0, clipCount);
            return currentIndex;
        }
    }
}