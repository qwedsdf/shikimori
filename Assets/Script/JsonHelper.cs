using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class JsonHelper 
{
    public static object FromJson<T>(string json)
    {
        if (json.StartsWith("["))
        {
            json = json.Replace("[",string.Empty);
            json = json.Replace("]",string.Empty);
            json = json.Replace("},","}|");
            var data = json.Split('|');
            List<T> list = new List<T>();
            foreach(var d in data) {
                var obj = JsonUtility.FromJson<T>(d);
                list.Add(obj);
            }
            return list;
        }
        else
        {
            T obj = JsonUtility.FromJson<T>(json);
            return obj;
        }
    }

    public static string ToJson<T>(T obj)
    {
        if (obj is IList)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = obj;
            var json = JsonUtility.ToJson(wrapper);
            json = Regex.Replace(json, "^\\{\"Items\":", "");
            json = Regex.Replace(json, "\\}$", "");
            return json;
        }
        else
        {
            var json = JsonUtility.ToJson(obj);
            return json;
        }
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T Items;
    }
}
