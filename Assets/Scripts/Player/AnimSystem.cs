using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using static UnityEngine.Rendering.DebugUI;

namespace ARPG.Animation
{
    public class AnimSystem
    {
        private Animator am;
        private AnimSetting[] animSetting;

        private Dictionary<string, int> animMap;
        private Dictionary<string, AnimUnit> unitMap;
        private Dictionary<string, BlendTree1D> blendTree1DMap;
        private Dictionary<string, BlendTree2D> blendTree2DMap;
        private Dictionary<string, AnimMixAnimator> AnimatorMap;

        private Mixer mixer;
        public PlayableGraph graph;

        public AnimSystem(Animator _am, AnimSetting[] _animSetting)
        {
            animMap = new Dictionary<string, int>();
            unitMap = new Dictionary<string, AnimUnit>();
            blendTree1DMap = new Dictionary<string, BlendTree1D>();
            blendTree2DMap = new Dictionary<string, BlendTree2D>();
            AnimatorMap = new Dictionary<string, AnimMixAnimator>();

            am = _am;
            animSetting = _animSetting;
            graph = PlayableGraph.Create();
            mixer = new Mixer(graph);

            for (int i = 0; i < animSetting?.Length; i++)
            {
                if (animSetting[i].param.type == AnimParam.Type.Single)
                {
                    var anim = new AnimUnit(graph, animSetting[i].param.animInfo.clip, animSetting[i].param.enterTime);
                    unitMap.Add(animSetting[i].param.animName, anim);
                    AddState(animSetting[i].param.animName, anim);
                }
                else if (animSetting[i].param.type == AnimParam.Type.Blend1DClip)
                {
                    var anim = new BlendTree1D(graph, animSetting[i].param.animInfo.blend1DClips, animSetting[i].param.enterTime);
                    blendTree1DMap.Add(animSetting[i].param.animName, anim);
                    AddState(animSetting[i].param.animName, anim);
                }
                else if (animSetting[i].param.type == AnimParam.Type.Blend2DClip)
                {
                    var anim = new BlendTree2D(graph, animSetting[i].param.animInfo.blend2DClips, animSetting[i].param.enterTime);
                    blendTree2DMap.Add(animSetting[i].param.animName, anim);
                    AddState(animSetting[i].param.animName, anim);
                }
                else if (animSetting[i].param.type == AnimParam.Type.Custom)
                {
                    animSetting[i].param.Init(graph);
                    AddState(animSetting[i].param.animName, animSetting[i].param.animInfo.animBehaviour);
                }
                else if (animSetting[i].param.type == AnimParam.Type.Animator)
                {
                    var anim = new AnimMixAnimator(graph, animSetting[i].param.animInfo.animator, animSetting[i].param.enterTime);
                    AnimatorMap.Add(animSetting[i].param.animName, anim);
                    AddState(animSetting[i].param.animName, anim);
                }
            }

            AnimHelper.SetOutput(graph, am, mixer);

            AnimHelper.Start(graph);
        }

        private void AddState(string name, AnimBehaviour anim)
        {
            if (!animMap.ContainsKey(name))
            {
                mixer.AddInput(anim);
                animMap.Add(name, mixer.inputCount - 1);
                //Debug.Log(name + " " + (mixer.inputCount - 1));
            }
        }

        public void TransitionTo(string name, float enterTime = -1f)
        {
            mixer.TransitionTo(animMap[name], enterTime);
        }

        public float IsAnimationComplete(string name)
        {
            if (unitMap.ContainsKey(name))
            {
                return unitMap[name].IsAnimationComplete();
            }
            else
            {
                Debug.Log("判断动画是否结束位置错误，字典超出索引");
                return -1f;
            }
        }

        public void BlendTree1DSetValue(string name, float value)
        {
            if (blendTree1DMap.TryGetValue(name, out BlendTree1D blendTree1D))
            {
                blendTree1D.SetValue(value);
            }
        }

        public void BlendTree1DSetAnimSpeed(string name, float speed)
        {
            if (blendTree1DMap.TryGetValue(name, out BlendTree1D blendTree1D))
            {
                blendTree1D.SetAnimSpeed(speed);
            }
        }

        public float BlendTree1DGetValue(string name)
        {
            if (blendTree1DMap.TryGetValue(name, out BlendTree1D blendTree1D))
            {
                return blendTree1D.CurrentValue;
            }
            return 0f;
        }

        public float BlendTree1DGetMaxWeightAnim(string name)
        {
            if (blendTree1DMap.TryGetValue(name, out BlendTree1D blendTree1D))
            {
                return blendTree1D.GetMaxWeightAnim();
            }
            return -1;
        }

        public void BlendTree2DSetPointer(string name, Vector2 v)
        {
            if (blendTree2DMap.TryGetValue(name, out BlendTree2D blendTree2D))
            {
                blendTree2D.SetPointer(v);
            }
        }

        public void AnimatorCrossFade(string name, string AnimName, float enterTime)
        {
            if (AnimatorMap.TryGetValue(name, out AnimMixAnimator animMixAnimator))
            {
                animMixAnimator.CrossFade(AnimName, enterTime);
            }
        }

        public void AnimatorSetFloat(string name, string ParamName, float value)
        {
            if (AnimatorMap.TryGetValue(name, out AnimMixAnimator animMixAnimator))
            {
                animMixAnimator.SetFloat(ParamName, value);
            }
        }

        public void AnimatorSetBool(string name, string ParamName, bool value)
        {
            if (AnimatorMap.TryGetValue(name, out AnimMixAnimator animMixAnimator))
            {
                animMixAnimator.SetBool(ParamName, value);
            }
        }

        public void AnimatorSetTrigger(string name, string ParamName)
        {
            if (AnimatorMap.TryGetValue(name, out AnimMixAnimator animMixAnimator))
            {
                animMixAnimator.SetTrigger(ParamName);
            }
        }

        public float AnimatorIsAnimationComplete(string name)
        {
            if (AnimatorMap.TryGetValue(name, out AnimMixAnimator animMixAnimator))
            {
                return animMixAnimator.IsAnimationComplete();
            }
            Debug.Log("没在字典中找到动画器");
            return 0f;
        }
    }
}