using ARPG.Animation;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace ARPG.BB
{
    public class BlackBoard : MonoBehaviour
    {
        [Serializable]
        private class BlackBoardTimer
        {
            public float Timer;
            public string Key;
            public mData Value;

            public BlackBoardTimer(float timer, string key, mData value)
            {
                Timer = timer;
                Key = key;
                Value = value;
            }
        }

        [Serializable]
        private class mData
        {
            public enum Type
            { Int, Float, Bool, String, Vector3, Transform };

            public string name;
            public Type type;
            public int intValue;
            public float floatValue;
            public bool boolValue;
            public string stringValue;
            public Vector3 vector3Value;
            public Transform transformValue;

            public mData(string key, int value)
            {
                name = key;
                type = Type.Int;
                intValue = value;
            }

            public mData(string key, float value)
            {
                name = key;
                type = Type.Float;
                floatValue = value;
            }

            public mData(string key, bool value)
            {
                name = key;
                type = Type.Bool;
                boolValue = value;
            }

            public mData(string key, string value)
            {
                name = key;
                type = Type.String;
                stringValue = value;
            }

            public mData(string key, Vector3 value)
            {
                name = key;
                type = Type.Vector3;
                vector3Value = value;
            }

            public mData(string key, Transform value)
            {
                name = key;
                type = Type.Vector3;
                transformValue = value;
            }
        }

        [SerializeField] private SerializedDictionary<string, mData> mDatas = new SerializedDictionary<string, mData>();
        [SerializeField] private List<BlackBoardTimer> mTimers = new List<BlackBoardTimer>();

        // 设置数据
        public void SetValue(string key, bool value)
        {
            mDatas[key] = new mData(key, value);
        }

        public void SetValue(string key, bool value, float expiredTime, bool expiredValue)
        {
            foreach (var timer in mTimers)
            {
                if (timer.Key == key) return;
            }

            SetValue(key, value);
            mTimers.Add(new BlackBoardTimer(expiredTime, key, new mData(key, expiredValue)));
        }

        public void SetValue(string key, int value)
        {
            mDatas[key] = new mData(key, value);
        }

        public void SetValue(string key, int value, float expiredTime, int expiredValue)
        {
            foreach (var timer in mTimers)
            {
                if (timer.Key == key) return;
            }

            SetValue(key, value);
            mTimers.Add(new BlackBoardTimer(expiredTime, key, new mData(key, expiredValue)));
        }

        public void SetValue(string key, float value)
        {
            mDatas[key] = new mData(key, value);
        }

        public void SetValue(string key, float value, float expiredTime, float expiredValue)
        {
            foreach (var timer in mTimers)
            {
                if (timer.Key == key) return;
            }

            SetValue(key, value);
            mTimers.Add(new BlackBoardTimer(expiredTime, key, new mData(key, expiredValue)));
        }

        public void SetValue(string key, string value)
        {
            mDatas[key] = new mData(key, value);
        }

        public void SetValue(string key, Vector3 value)
        {
            mDatas[key] = new mData(key, value);
        }

        public void SetValue(string key, Vector3 value, float expiredTime, Vector3 expiredValue)
        {
            foreach (var timer in mTimers)
            {
                if (timer.Key == key) return;
            }

            mDatas[key] = new mData(key, value);
            mTimers.Add(new BlackBoardTimer(expiredTime, key, new mData(key, expiredValue)));
        }

        public void SetValue(string key, Transform value)
        {
            mDatas[key] = new mData(key, value);
        }

        public void SetValue(string key, Transform value, float expiredTime, Transform expiredValue)
        {
            foreach (var timer in mTimers)
            {
                if (timer.Key == key) return;
            }

            mDatas[key] = new mData(key, value);
            mTimers.Add(new BlackBoardTimer(expiredTime, key, new mData(key, expiredValue)));
        }

        // 访问数据
        public int GetInt(string key)
        {
            if (mDatas.TryGetValue(key, out var value) && value.type == mData.Type.Int)
            {
                return value.intValue;
            }
            throw new KeyNotFoundException($"Key '{key}' not found or not an int.");
        }

        public float GetFloat(string key)
        {
            if (mDatas.TryGetValue(key, out var value) && value.type == mData.Type.Float)
            {
                return value.floatValue;
            }
            throw new KeyNotFoundException($"Key '{key}' not found or not a float.");
        }

        public bool GetBool(string key)
        {
            if (mDatas.TryGetValue(key, out var value) && value.type == mData.Type.Bool)
            {
                return value.boolValue;
            }
            throw new KeyNotFoundException($"Key '{key}' not found or not a bool.");
        }

        public string GetString(string key)
        {
            if (mDatas.TryGetValue(key, out var value) && value.type == mData.Type.String)
            {
                return value.stringValue;
            }
            throw new KeyNotFoundException($"Key '{key}' not found or not a string.");
        }

        public Vector3 GetVector3(string key)
        {
            if (mDatas.TryGetValue(key, out var value) && value.type == mData.Type.Vector3)
            {
                return value.vector3Value;
            }
            throw new KeyNotFoundException($"Key '{key}' not found or not a string.");
        }

        public Transform GetTransform(string key)
        {
            if (mDatas.TryGetValue(key, out var value) && value.type == mData.Type.Transform)
            {
                return value.transformValue;
            }
            throw new KeyNotFoundException($"Key '{key}' not found or not a string.");
        }

        private void Update()
        {
            for (int i = mTimers.Count - 1; i >= 0; i--)
            {
                var timer = mTimers[i];
                timer.Timer -= Time.deltaTime / Time.timeScale;
                if (timer.Timer <= 0.0f)
                {
                    mDatas[timer.Key] = timer.Value;
                    mTimers.RemoveAt(i);
                }
            }
        }
    }
}