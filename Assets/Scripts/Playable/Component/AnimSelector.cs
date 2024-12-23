using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ARPG.Animation
{
    public class AnimSelector : AnimBehaviour
    {
        public int currentIndex { get; protected set; }
        public int clipCount { get; protected set; }

        private AnimationMixerPlayable m_mixer;
        private List<float> m_clipLength;
        private List<float> m_clipEnterTime;

        public AnimSelector(PlayableGraph graph) :
            base(graph)
        {
            m_mixer = AnimationMixerPlayable.Create(graph);
            m_adapterPlayable.AddInput(m_mixer, 0, 1f);

            currentIndex = -1;
            m_clipLength = new List<float>();
            m_clipEnterTime = new List<float>();
        }

        public override void AddInput(Playable playable)
        {
            base.AddInput(playable);
            m_mixer.SetInputCount(clipCount + 1);
            m_mixer.ConnectInput(clipCount, playable, 0, 0f);
            clipCount++;
        }

        public void AddInput(AnimationClip clip, float enterTime = 0f)
        {
            m_clipLength.Add(clip.length);
            m_clipEnterTime.Add(enterTime);
            AddInput(new AnimUnit(m_adapterPlayable.GetGraph(), clip, enterTime));
        }

        public virtual void Select(int index)
        {
            currentIndex = index;
        }

        public virtual int Select()
        {
            currentIndex = 0;
            return currentIndex;
        }

        public override void Enable()
        {
            base.Enable();

            if (currentIndex < 0) return;
            m_mixer.SetInputWeight(currentIndex, 1f);
            AnimHelper.Enable(m_mixer, currentIndex);

            m_mixer.SetTime(0f);
            m_mixer.SetTime(0f);
            m_mixer.SetSpeed(1f);
            m_adapterPlayable.SetTime(0f);
            m_adapterPlayable.SetTime(0f);
            m_adapterPlayable.SetSpeed(1f);
        }

        public override void Disable()
        {
            base.Disable();
            if (currentIndex < 0 || currentIndex >= clipCount) return;
            for (int i = 0; i < clipCount; i++)
            {
                m_mixer.SetInputWeight(i, 0f);
            }
            AnimHelper.Disable(m_mixer, currentIndex);
            //currentIndex = -1;
            m_mixer.SetSpeed(0f);
            m_adapterPlayable.SetSpeed(0f);
        }

        public override float GetEnterTime()
        {
            if (currentIndex < 0 || currentIndex >= clipCount) return 0f;

            return m_clipEnterTime[currentIndex];
        }

        public override float GetAnimLength()
        {
            if (currentIndex < 0 || currentIndex >= clipCount) return 0f;
            return m_clipLength[currentIndex];
        }
    }
}