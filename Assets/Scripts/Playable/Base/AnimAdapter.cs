using UnityEngine.Playables;

namespace ARPG.Animation
{
    public class AnimAdapter : PlayableBehaviour
    {
        private AnimBehaviour m_behaviour;

        public void Init(AnimBehaviour behaviour)
        {
            m_behaviour = behaviour;
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            base.PrepareFrame(playable, info);
            m_behaviour?.Execute(playable, info);
        }

        public void Enable()
        {
            m_behaviour?.Enable();
        }

        public void Disable()
        {
            m_behaviour?.Disable();
        }

        public T GetAnimBehaviour<T>() where T : AnimBehaviour
        {
            return m_behaviour as T;
        }

        public float GetAnimEnterTime()
        {
            return m_behaviour.GetEnterTime();
        }

        public override void OnGraphStop(Playable playable)
        {
            base.OnGraphStop(playable);
            m_behaviour?.Stop();
        }
    }
}