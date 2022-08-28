using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoad
{
    public static void Save<T>(T objectToSave, string key)
    {
        string path = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(path);
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(path + key + ".bin", FileMode.Create))
        {
            try
            {
                formatter.Serialize(fileStream, objectToSave);
            }
            catch (SerializationException exception)
            {
                Debug.Log("Save failed. Error: " + exception.Message);
            }
        }
    }

    public static T Load<T>(string key)
    {
        string path = Application.persistentDataPath + "/saves/";
        BinaryFormatter formatter = new BinaryFormatter();
        T returnValue = default(T);
        using (FileStream fileStream = new FileStream(path + key + ".bin", FileMode.Open))
        {
            try
            {
                returnValue = (T)formatter.Deserialize(fileStream);
            }
            catch (SerializationException exception)
            {
                Debug.Log("Load failed. Error: " + exception.Message);
            }
        }

        return returnValue;
    }

    public static void Save<T>(T objectToSave, string key, string folder)
    {
        string path = Application.persistentDataPath + "/" + folder + "/";
        Directory.CreateDirectory(path);
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(path + key + ".bin", FileMode.Create))
        {
            try
            {
                formatter.Serialize(fileStream, objectToSave);
            }
            catch (SerializationException exception)
            {
                Debug.Log("Save failed. Error: " + exception.Message);
            }
        }
    }

    public static T Load<T>(string key,string folder)
    {
        string path = Application.persistentDataPath + "/" + folder + "/";
        BinaryFormatter formatter = new BinaryFormatter();
        T returnValue = default(T);
        using (FileStream fileStream = new FileStream(path + key + ".bin", FileMode.Open))
        {
            try
            {
                returnValue = (T)formatter.Deserialize(fileStream);
            }
            catch (SerializationException exception)
            {
                Debug.Log("Load failed. Error: " + exception.Message);
            }
        }

        return returnValue;
    }

    public static bool SaveExits(string key)
    {
        string path = Application.persistentDataPath + "/saves/" + key + ".bin";
        return File.Exists(path);
    }

    public static bool SaveExits(string key, string folder)
    {
        string path = Application.persistentDataPath + "/" + folder + "/" + key + ".bin";
        return File.Exists(path);
    }

    public static void SerioyslyDeleteallSave()
    {
        string path = Application.persistentDataPath + "/saves/";
        DirectoryInfo directory = new DirectoryInfo(path);
        directory.Delete(true);
        Directory.CreateDirectory(path);
    }

    public static void SerioyslyDeleteallSave(string folder)
    {
        string path = Application.persistentDataPath + "/" + folder + "/";
        DirectoryInfo directory = new DirectoryInfo(path);
        directory.Delete(true);
        Directory.CreateDirectory(path);
    }
}
