using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ARPG.Animation
{
    public class AnimMixAnimator : AnimBehaviour
    {
        private AnimatorControllerPlayable m_animatorControllerPlayable;
        private Animator m_animator;
        private float time;

        public AnimMixAnimator(PlayableGraph graph, Animator animator, float entetTime = 0f) : base(graph, entetTime)
        {
            m_animator = animator;
            m_animatorControllerPlayable = AnimatorControllerPlayable.Create(graph, animator.runtimeAnimatorController);
            m_adapterPlayable.AddInput(m_animatorControllerPlayable, 0, 1f);

            var controller = m_animator.runtimeAnimatorController as RuntimeAnimatorController;

            List<float> speeds = new List<float>();
            foreach (var anim in controller.animationClips)
            {
                speeds.Add(anim.length);
            }

            m_animLength = 0f;
            var clips = m_animator.runtimeAnimatorController.animationClips;
            foreach (var anim in clips)
            {
                m_animLength += anim.length / speeds[0];
                speeds.RemoveAt(0);
            }

            //Debug.Log(m_animLength);

            Disable();
        }

        public override void Enable()
        {
            base.Enable();

            m_adapterPlayable.SetTime(0);
            m_adapterPlayable.SetTime(0);
            m_adapterPlayable.SetSpeed(1f);
            m_animator.Play("Start");
            m_animator.Update(0);

            time = m_animLength;
        }

        public override void Execute(Playable playable, FrameData info)
        {
            base.Execute(playable, info);

            if (!enable) return;
            time -= info.deltaTime;
        }

        public override void Disable()
        {
            base.Disable();
            m_animator.Play("Start");
            m_animator.Update(0);
            m_adapterPlayable.SetSpeed(0f);
        }

        public void CrossFade(string stateName, float transitionDuration)
        {
            m_animator.CrossFade(stateName, transitionDuration);
        }

        public float IsAnimationComplete()
        {
            if (time <= m_enterTime)
            {
                return 1.1f;
            }
            else
            {
                //Debug.Log(time);
                //Debug.Log(100 * (m_animLength - time) / m_animLength);
                return (m_animLength - time) / m_animLength;
            }
        }

        public void SetFloat(string name, float value)
        { m_animator.SetFloat(name, value); }

        public void SetBool(string name, bool value)
        { m_animator.SetBool(name, value); }

        public void SetTrigger(string name)
        { m_animator.SetTrigger(name); }
    }
}