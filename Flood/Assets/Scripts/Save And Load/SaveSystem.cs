using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {

    public static void SaveWorld (WorldManager world) {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/world.wld";
        FileStream stream = new FileStream(path, FileMode.Create);

        WorldData data = new WorldData(world);
        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("World Saved At: " + path);


    }

    public static WorldData LoadWorld() {

        string path = Application.persistentDataPath + "/world.wld";
        if (File.Exists(path)) {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            WorldData world = formatter.Deserialize(stream) as WorldData;
            stream.Close();

            return world;

        } else {
            Debug.LogError(path + " has no save!");
            return null;
        }
    }




}
