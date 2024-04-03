using System.Collections;
using System;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager saveGlob; // the Dictionary used to save and load data to/from disk
    protected string savePath;
    public int recordFireFlies;
    public int recordFruit;
    public int level;

    public float lifeNow;
    public string enterFrom;

    private void Awake()
    {
        if (saveGlob == null)
        {
            DontDestroyOnLoad(gameObject);
            saveGlob = this;
            savePath = Application.persistentDataPath + "/sv0.dat";
            loadDataFromDisk();
            level = 6; // test
            lifeNow = 55f; // test
            enterFrom = "maze"; // test
        }
        else if (saveGlob != this)
        {
            Destroy(gameObject);
        }
    }

    public void saveDataToDisk()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);

        PlayerData data = new PlayerData();
        data.recordFireFlies = recordFireFlies;
        data.recordFruit = recordFruit;
        data.level = level;

        bf.Serialize(file, data);
        file.Close();
    }

    /**
     * Loads the save data from the disk
     */
    public void loadDataFromDisk()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            recordFireFlies = data.recordFireFlies;
            recordFruit = data.recordFruit;
            level = data.level;
        }
    }

    [Serializable]
    class PlayerData
    {
        public int recordFireFlies;
        public int recordFruit;
        public int level;
    }
}
