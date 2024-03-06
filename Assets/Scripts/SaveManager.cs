using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }

    // what we wanna save
    public float position_x;
    public float position_y;
    public string currentLevel;
    public float maxHealth;




    private void Awake() {
        if (instance != this && instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
        
        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void Load() {
        if(File.Exists(Application.persistentDataPath + "/PlayerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);
            PlayerData_Storage data = (PlayerData_Storage)bf.Deserialize(file);


            position_x = data.position_x;
            position_y = data.position_y;
            currentLevel = data.currentLevel;
            maxHealth = data.maxHealth;

            file.Close();
        }
    }

    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/PlayerData.dat");
        PlayerData_Storage data = new PlayerData_Storage();

        data.position_x = position_x;
        data.position_y = position_y;
        data.currentLevel = currentLevel;
        data.maxHealth = maxHealth;

        bf.Serialize(file, data);
        file.Close();
    }

}

[System.Serializable]
public class PlayerData_Storage
{
    public float position_x;
    public float position_y;
    public string currentLevel;
    public float maxHealth;

}