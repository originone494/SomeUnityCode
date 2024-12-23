using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ARPG.Animation
{
    public class AnimUnit : AnimBehaviour
    {
        private AnimationClipPlayable m_animClip;
        private float time;

        public AnimUnit(PlayableGraph graph, AnimationClip clip, float enterTime = 0f) : base(graph, enterTime)
        {
            m_animClip = AnimationClipPlayable.Create(graph, clip);

            m_animLength = clip.length;

            m_adapterPlayable.AddInput(m_animClip, 0, 1f);

            Disable();
        }

        public override void Enable()
        {
            base.Enable();

            m_animClip.SetTime(0f);
            m_animClip.SetTime(0f);
            m_adapterPlayable.SetTime(0f);
            m_adapterPlayable.SetTime(0f);

            //m_animClip.Play();
            //m_adapterPlayable.Play();
            m_animClip.SetSpeed(1f);
            m_adapterPlayable.SetSpeed(1f);

            time = m_animLength;
        }

        public override void Disable()
        {
            base.Disable();

            //m_animClip.Pause();
            //m_adapterPlayable.Pause();
            m_animClip.SetSpeed(0f);
            m_adapterPlayable.SetSpeed(0f);
        }

        public override void Execute(Playable playable, FrameData info)
        {
            base.Execute(playable, info);
            time -= info.deltaTime;
        }

        public float IsAnimationComplete()
        {
            if (time <= m_enterTime)
            {
                return 1.1f;
            }
            else
            {
                return (m_animLength - time) / m_animLength;
            }
        }
    }
}