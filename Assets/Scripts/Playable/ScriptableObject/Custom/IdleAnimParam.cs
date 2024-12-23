using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

namespace ARPG.Animation
{
    [CreateAssetMenu(fileName = "Idle", menuName = "New Animation Setting/Custom/IdleAnim")]
    public class IdleAnimParam : CustomParam
    {
        public AnimationClip[] idleClips;

        public override void Init(PlayableGraph graph)
        {
            base.Init(graph);
            animInfo.animBehaviour = new IdleAnim(graph, idleClips, enterTime);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }
    }
}