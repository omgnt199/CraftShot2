using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public static class Commons
    {

        public static Coroutine SetTimeout(this MonoBehaviour monoBehaviour, Action callback, float delay)
        {
            return monoBehaviour.StartCoroutine(TimeoutScaledTime(delay, callback));
        }

        public static Coroutine SetTimeoutScaledTime(this MonoBehaviour monoBehaviour, Action callback, float delay)
        {
            return monoBehaviour.StartCoroutine(TimeoutScaledTime(delay, callback));
        }

        public static Coroutine SetTimeoutUnscaledTime(this MonoBehaviour monoBehaviour, Action callback, float delay)
        {
            return monoBehaviour.StartCoroutine(TimeoutUnscaledTime(delay, callback));
        }

        public static Coroutine SetTimeout(this MonoBehaviour monoBehaviour, float delay, Action callback)
        {
            return monoBehaviour.StartCoroutine(Timeout(delay, callback));
        }

        public static Coroutine WaitNextFrame(this MonoBehaviour monoBehaviour, Action callback)
        {
            return monoBehaviour.StartCoroutine(WaitNextFrame(callback));
        }

        public static Coroutine WaitUntil(this MonoBehaviour monoBehaviour, Func<bool> predicate, Action callback)
        {
            return monoBehaviour.StartCoroutine(WaitUntil(predicate, callback));
        }


        private static IEnumerator WaitNextFrame(Action callback)
        {
            yield return new WaitForEndOfFrame();
            callback();
        }

        private static IEnumerator WaitUntil(Func<bool> predicate, Action callback)
        {
            yield return new WaitUntil(predicate);
            callback();
        }

        private static IEnumerator Timeout(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);
            callback();
        }

        private static IEnumerator TimeoutScaledTime(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);
            callback();
        }

        private static IEnumerator TimeoutUnscaledTime(float delay, Action callback)
        {
            yield return new WaitForSecondsRealtime(delay);
            callback();
        }



        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

    }

    [Serializable]
    public class ValueKeyPair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public ValueKeyPair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
    [Serializable]
    public class ValueKeyPair<TKey, T1, T2>
    {
        public TKey Key;
        public T1 V1;
        public T2 V2;

        public ValueKeyPair(TKey key, T1 v1, T2 v2)
        {
            Key = key;
            V1 = v1;
            V2 = v2;
        }
    }
}