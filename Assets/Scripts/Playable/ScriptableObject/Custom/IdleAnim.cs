using UnityEngine;
using UnityEngine.Playables;

namespace ARPG.Animation
{
    public class IdleAnim : AnimBehaviour
    {
        private Mixer m_mixer;
        private float m_timeToNext;
        private bool m_isPlayDefault;
        private AnimUnit m_anim;
        private RandomSelector m_random;

        public IdleAnim(PlayableGraph graph, AnimationClip[] clips, float enterTime) : base(graph, enterTime)
        {
            m_mixer = new Mixer(graph);

            m_anim = new AnimUnit(graph, clips[0], 0.5f);
            m_random = new RandomSelector(graph);
            for (int i = 1; i < clips.Length; i++)
            {
                m_random.AddInput(clips[i], 0.5f);
            }

            m_mixer.AddInput(m_anim);
            m_mixer.AddInput(m_random);

            m_adapterPlayable.AddInput(m_mixer.GetAdapterPlayable(), 0, 1f);

            m_timeToNext = 10f;
            m_isPlayDefault = true;
            m_random.Select();
        }

        public override void Enable()
        {
            base.Enable();
            //Debug.Log("enable");
            m_timeToNext = 10f;
            m_isPlayDefault = true;
            m_random.Select();

            m_adapterPlayable.SetTime(0f);
            m_adapterPlayable.SetTime(0f);
            m_adapterPlayable.SetSpeed(1f);
            m_mixer.Enable();
        }

        public override void Disable()
        {
            base.Disable();
            m_adapterPlayable.SetSpeed(0f);
            m_mixer.Disable();
        }

        public override void Execute(Playable playable, FrameData info)
        {
            base.Execute(playable, info);

            if (!enable) return;

            m_timeToNext -= info.deltaTime;
            //Debug.Log(m_timeToNext + " " + m_isPlayDefault + " " + m_random.currentIndex);
            if (m_isPlayDefault)
            {
                if (m_timeToNext <= m_random.GetEnterTime())
                {
                    m_mixer.TransitionTo(1);

                    if (!m_mixer.isTransition)
                    {
                        m_timeToNext = m_random.GetAnimLength();
                        m_isPlayDefault = false;
                    }
                }
            }
            else
            {
                if (m_timeToNext <= m_anim.GetEnterTime())
                {
                    m_mixer.TransitionTo(0);
                    if (!m_mixer.isTransition)
                    {
                        m_timeToNext = Random.Range(8f, 12f);
                        m_isPlayDefault = true;
                        m_random.Select();
                    }
                }
            }
        }
    }
}