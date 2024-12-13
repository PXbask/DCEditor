using System;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DCEditor.Utility
{
    public class MyUtility
    {
        public static void InstantiateMultiple<T>(T prefab, int count, Action<int, T> callback) where T : Object
        {
            for (int i = 0; i < count; i++)
            {
                T obj = Object.Instantiate(prefab);
                callback.Invoke(i, obj);
            }
        }
    }
}
