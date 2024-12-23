using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine;

namespace ARPG.Animation
{
    public class AnimHelper
    {
        #region Playables相关

        public static void SetOutput(PlayableGraph graph, Animator animator, AnimBehaviour behaviour)
        {
            var root = new Root(graph);
            root.AddInput(behaviour);
            AnimationPlayableOutput.Create(graph, "Anim", animator).SetSourcePlayable(root.GetAdapterPlayable());
        }

        public static void Start(PlayableGraph graph)
        {
            GetAdapter(graph.GetOutputByType<AnimationPlayableOutput>(0).GetSourcePlayable()).Enable();
            graph.Play();
        }

        public static void Start(PlayableGraph graph, AnimBehaviour root)
        {
            root.Enable();
            graph.Play();
        }

        public static void Enable(Playable playable)
        {
            AnimAdapter adapter = GetAdapter(playable);
            adapter?.Enable();
        }

        public static void Enable(AnimationMixerPlayable mixer, int index)
        {
            Enable(mixer.GetInput(index));
        }

        public static void Disable(Playable playable)
        {
            AnimAdapter adapter = GetAdapter(playable);
            adapter?.Disable();
        }

        public static void Disable(AnimationMixerPlayable mixer, int index)
        {
            Disable(mixer.GetInput(index));
        }

        public static AnimAdapter GetAdapter(Playable playable)
        {
            if (typeof(AnimAdapter).IsAssignableFrom(playable.GetPlayableType()))
            {
                return ((ScriptPlayable<AnimAdapter>)playable).GetBehaviour();
            }
            return null;
        }

        #endregion Playables相关

        public static ComputeShader GetComputer(string name)
        {
            return Object.Instantiate(Resources.Load<ComputeShader>("Compute/" + name));
        }
    }
}