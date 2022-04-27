using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public static class SaveSystem {

    public static void SaveWorld (WorldManager world) {

        BinaryFormatter formatter = new BinaryFormatter();
        //string path = Application.persistentDataPath + "/world.wld";
        string path = Application.dataPath + "/Levels/new_world.wld";
        FileStream stream = new FileStream(path, FileMode.Create);

        WorldData data = new WorldData(world);
        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("World Saved At: " + path);


    }

    public static WorldData LoadWorld(int level) {


        //string path = Application.persistentDataPath + "/world.wld";

        string path = Application.dataPath;
        if (path.Contains("Assets")) {
            path = path + "/Levels/world" + level.ToString() + ".wld";
        } else {
            path = Application.dataPath + "/world" + level.ToString() + ".wld";
            path = path.Replace("/Flood_Data", "/Levels");
        }
        
        


        if (File.Exists(path)) {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            WorldData world = formatter.Deserialize(stream) as WorldData;
            stream.Close();

            return world;

        } else {
            Debug.LogError(path + " has no save!");
            SceneManager.LoadScene("Main Menu");

            return null;
        }
    }




}
