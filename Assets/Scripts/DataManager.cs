using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class DataManager {

    public static string path = Application.persistentDataPath + "/blob.bin";
    public static void Save(Blob bob) {
        Debug.Log("The save file location: " + path);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        BlobData data = new BlobData(bob);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    
    public static BlobData Load() {
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            BlobData data = formatter.Deserialize(stream) as BlobData;
            stream.Close();
            return data;
        } else {
            Debug.LogError("Save file not found.");
            return null;
        }
    }

    public static void DeleteSave() {
        File.Delete(path);
    }
}