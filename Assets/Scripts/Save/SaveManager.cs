using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager {
    public static readonly string _path = Application.persistentDataPath + "/player.gucci";

    public static void SaveData(LevelData[] data) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(_path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static LevelData[] LoadData() {
        if (File.Exists(_path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_path, FileMode.Open);
            LevelData[] data = formatter.Deserialize(stream) as LevelData[];
            stream.Close();
            return data;
        } else {
            // Save file not found
            return null;
        }
    }

    // Return number of main levels completed 
    public static int GetMainLevelsCompleted() {
        LevelData[] _levelDataArray = LoadData();
        int levelsCompleted = 0;
        if (_levelDataArray != null) {
            // check for the most recent completed level
            foreach (LevelData data in _levelDataArray) {
                if (data != null) {
                    levelsCompleted++;
                } else {
                    break;
                }
            }
        }
        return levelsCompleted;
    }
}
