using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ARPG.Animation
{
    [System.Serializable]
    public struct BlendClip1D
    {
        public AnimationClip clip;
        public float threshold;
    }

    public class BlendTree1D : AnimBehaviour
    {
        private AnimationMixerPlayable m_mixer;
        private PlayableGraph m_grapha;
        private float[] m_thresholds;
        private int m_clipCount;
        private float m_currentValue;
        private float m_targetValue;
        private float m_elapsedTime;

        public float CurrentValue => m_currentValue;

        public BlendTree1D(PlayableGraph grapha, BlendClip1D[] blendClip1Ds, float enterTime = 0) : base(grapha, enterTime)
        {
            m_grapha = grapha;
            m_mixer = AnimationMixerPlayable.Create(grapha);
            m_adapterPlayable.AddInput(m_mixer, 0, 1f);

            m_thresholds = new float[blendClip1Ds.Length];
            m_clipCount = blendClip1Ds.Length;
            m_currentValue = 0;
            m_targetValue = 0;
            m_elapsedTime = m_enterTime;

            for (int i = 0; i < blendClip1Ds.Length; i++)
            {
                m_mixer.AddInput(AnimationClipPlayable.Create(grapha, blendClip1Ds[i].clip), 0);
                m_thresholds[i] = blendClip1Ds[i].threshold;
            }
        }

        public override void Enable()
        {
            base.Enable();

            m_adapterPlayable.SetTime(0);
            m_mixer.SetTime(0);

            m_adapterPlayable.Play();
            m_mixer.Play();
        }

        public override void Execute(Playable playable, FrameData info)
        {
            base.Execute(playable, info);

            if (Mathf.Abs(m_currentValue - m_targetValue) > 0.1f)
            {
                m_elapsedTime += Time.deltaTime;
                m_currentValue = Mathf.Lerp(m_currentValue, m_targetValue, 0.5f);
            }
            else
            {
                m_currentValue = m_targetValue;
            }

            Tuple<float, int> nl = FindClosestLeft(m_thresholds, m_currentValue);
            float left = 0;
            float right = 0;
            int leftIndex, rightIndex = 0;
            // 到最小的阈值以下
            if (nl.Item1 == float.MinValue)
            {
                right = m_thresholds[0];
                rightIndex = 0;
                for (int i = 1; i < m_clipCount; i++)
                {
                    m_mixer.SetInputWeight(i, 0);
                }
                m_mixer.SetInputWeight(rightIndex, 1);
            }
            //最大的阈值以上
            else if (nl.Item1 >= m_thresholds[m_thresholds.Length - 1])
            {
                left = m_thresholds[nl.Item2];
                leftIndex = nl.Item2;
                for (int i = 0; i < m_clipCount - 1; i++)
                {
                    m_mixer.SetInputWeight(i, 0);
                }
                m_mixer.SetInputWeight(leftIndex, 1);
            }
            //中间
            else
            {
                leftIndex = nl.Item2;
                rightIndex = nl.Item2 + 1;

                left = m_thresholds[leftIndex];
                right = m_thresholds[rightIndex];

                float weight = (m_currentValue - left) / (right - left + 1e-06f);

                for (int i = 0; i < m_clipCount; i++)
                {
                    if (i == leftIndex) continue;
                    if (i == rightIndex) continue;
                    m_mixer.SetInputWeight(i, 0);
                }

                m_mixer.SetInputWeight(rightIndex, weight);
                m_mixer.SetInputWeight(leftIndex, 1 - weight);
            }
        }

        public override void Disable()
        {
            base.Disable();
            m_adapterPlayable.Pause();
            m_mixer.Pause();
        }

        public void AddInput(AnimationClip clip)
        {
            AddInput(AnimationClipPlayable.Create(m_grapha, clip));
        }

        public override void AddInput(Playable playable)
        {
            m_mixer.AddInput(playable, 0, 0);
        }

        public void SetValue(float value)
        {
            if (m_currentValue == value) return;
            m_targetValue = value;
            m_elapsedTime = 0f;
        }

        public void SetAnimSpeed(float speed)
        {
            m_adapterPlayable.SetSpeed(speed);
            m_mixer.SetSpeed(speed);
        }

        public float GetMaxWeightAnim()
        {
            int count = m_mixer.GetInputCount();

            float maxWeight = -1;
            for (int i = 0; i < count; i++)
            {
                float weight = m_mixer.GetInputWeight(i);
                if (maxWeight < weight)
                {
                    maxWeight = weight;
                }
            }
            return maxWeight;
        }

        private static Tuple<float, int> FindClosestLeft(float[] arr, float target)
        {
            int left = 0;
            int right = arr.Length - 1;
            float closestValue = float.MinValue;
            int closestIndex = -1;

            while (left <= right)
            {
                int mid = (left + right) / 2;

                if (arr[mid] < target)
                {
                    // 更新最接近的左边的数
                    if (arr[mid] > closestValue)
                    {
                        closestValue = arr[mid];
                        closestIndex = mid;
                    }

                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            return Tuple.Create(closestValue, closestIndex);
        }
    }
}