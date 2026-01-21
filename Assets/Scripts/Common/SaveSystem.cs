using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void Save<T>(string key, T data)
    {
        if (string.IsNullOrEmpty(key))
        {
            return;
        }
        var json = JsonUtility.ToJson(data);
        File.WriteAllText(GetPath(key), json);
    }

    public static bool TryLoad<T>(string key, out T data)
    {
        data = default;
        if (string.IsNullOrEmpty(key))
        {
            return false;
        }
        var path = GetPath(key);
        if (!File.Exists(path))
        {
            return false;
        }
        var json = File.ReadAllText(path);
        data = JsonUtility.FromJson<T>(json);
        return true;
    }

    public static void Delete(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return;
        }
        var path = GetPath(key);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private static string GetPath(string key)
    {
        return Path.Combine(Application.persistentDataPath, key + ".json");
    }
}
