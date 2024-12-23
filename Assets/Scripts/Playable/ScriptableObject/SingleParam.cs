using System;
using UnityEngine;

namespace ARPG.Animation
{
    [Serializable, CreateAssetMenu(fileName = "New Animation Setting", menuName = "New Animation Setting/SingleAnimation")]
    public class SingleParam : AnimParam
    {
        public AnimationClip clip;

        private void OnEnable()
        {
            type = Type.Single;
            animInfo.clip = clip;
            animName = name;
        }
    }
}