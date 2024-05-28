using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataManager
{
    static string persistentDataPath = Application.persistentDataPath;
    public static string PersistentDataPath => persistentDataPath;

    /// <summary>
    /// Create or update file. Create folder if not exist.
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="location">The location of the file.</param>
    /// <param name="data">The data to be saved.</param>
    public static void Save(string fileName, object data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = PersistentDataPath + "/";
        Directory.CreateDirectory(path);
        // clear file content first
        if (File.Exists(path + fileName))
        {
            File.WriteAllText(path + fileName, string.Empty);
        }
        FileStream file = File.Open(path + fileName, FileMode.OpenOrCreate);
        try
        {
            bf.Serialize(file, data);
            file.Position = 0;
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            file.Close();
        }
    }

    /// <summary>
    /// Load data from file.
    /// </summary>
    /// <typeparam name="T">The type of data to load.</typeparam>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="location">The location of the file.</param>
    /// <param name="defaultValue">The default value to return if the file does not exist or an error occurs during deserialization.</param>
    /// <returns>The loaded data of type T.</returns>
    public static T Load<T>(string fileName, T defaultValue = default(T))
    {
        FileStream file = null;
        try
        {
            string path = PersistentDataPath + "/" + fileName;
            file = File.Open(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            file.Position = 0;
            return (T)bf.Deserialize(file);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e);
            return defaultValue;
        }
        finally
        {
            file?.Close();
        }
    }

}
