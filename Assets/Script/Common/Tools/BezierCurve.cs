using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public static Vector3 GetValue(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4, float time)
    {
        return point1 * Mathf.Pow((1 - time), 3) +
            3 * point2 * time * Mathf.Pow((1 - time), 2) +
            3 * point3 * Mathf.Pow(time, 2) * (1 - time) +
            point4 * Mathf.Pow(time, 3);
    }
    public static float GetValue(float point1, float point2, float point3, float point4, float time)
    {
        return point1 * Mathf.Pow((1 - time), 3) +
            3 * point2 * time * Mathf.Pow((1 - time), 2) +
            3 * point3 * Mathf.Pow(time, 2) * (1 - time) +
            point4 * Mathf.Pow(time, 3);
    }

    public static T Ease<T>(float time)
    {
        if (typeof(T) == typeof(Vector3))
        {
            return (T)Convert.ChangeType(GetValue(new Vector3(0.25f, 0.25f, 0.25f), new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.25f, 0.25f, 0.25f), Vector3.one, time), typeof(T));
        }
        else if (typeof(T) == typeof(float))
        {
            return (T)Convert.ChangeType(GetValue(0.25f, 0.1f, 0.25f, 1, time), typeof(T));
        }
        return default(T);
    }

    public static T Linear<T>(float time)
    {
        if (typeof(T) == typeof(Vector3))
        {
            return (T)Convert.ChangeType(GetValue(Vector3.zero, Vector3.zero, Vector3.one, Vector3.one, time), typeof(T));
        }
        else if (typeof(T) == typeof(float))
        {
            return (T)Convert.ChangeType(GetValue(0, 0, 1, 1, time), typeof(T));
        }
        return default(T);
    }

    public static T EaseIn<T>(float time)
    {
        if (typeof(T) == typeof(Vector3))
        {
            return (T)Convert.ChangeType(GetValue(new Vector3(0.42f, 0.42f, 0.42f), Vector3.zero, Vector3.one, Vector3.one, time), typeof(T));
        }
        else if (typeof(T) == typeof(float))
        {
            return (T)Convert.ChangeType(GetValue(0.42f, 0, 1, 1, time), typeof(T));
        }
        return default(T);
    }

    public static T EaseOut<T>(float time)
    {
        if (typeof(T) == typeof(Vector3))
        {
            return (T)Convert.ChangeType(GetValue(Vector3.zero, Vector3.zero, new Vector3(0.58f, 0.58f, 0.58f), Vector3.one, time), typeof(T));
        }
        else if (typeof(T) == typeof(float))
        {
            return (T)Convert.ChangeType(GetValue(0, 0, 0.58f, 1, time), typeof(T));
        }
        return default(T);
    }

    public static T EaseInOut<T>(float time)
    {
        if (typeof(T) == typeof(Vector3))
        {
            return (T)Convert.ChangeType(GetValue(new Vector3(0.42f, 0.42f, 0.42f), Vector3.zero, new Vector3(0.58f, 0.58f, 0.58f), Vector3.one, time), typeof(T));
        }
        else if (typeof(T) == typeof(float))
        {
            return (T)Convert.ChangeType(GetValue(0.42f, 0, 0.58f, 1, time), typeof(T));
        }
        return default(T);
    }

    public static T Bouncing<T>(float time)
    {
        if (typeof(T) == typeof(Vector3))
        {
            return (T)Convert.ChangeType(GetValue(Vector3.zero, Vector3.zero, Vector3.one * 1.5f, Vector3.one, time), typeof(T));
        }
        else if (typeof(T) == typeof(float))
        {
            return (T)Convert.ChangeType(GetValue(0, 0, 1.5f, 1f, time), typeof(T));
        }
        return default(T);
    }

    public static T BouncingR<T>(float time)
    {
        if (typeof(T) == typeof(Vector3))
        {
            return (T)Convert.ChangeType(GetValue(Vector3.zero, Vector3.one * -0.2f, Vector3.one * -0.2f, Vector3.one, time), typeof(T));
        }
        else if (typeof(T) == typeof(float))
        {
            return (T)Convert.ChangeType(GetValue(0, -0.2f, -0.2f, 1f, time), typeof(T));
        }
        return default(T);
    }

}
