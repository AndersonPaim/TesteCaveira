using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace _Project.Scripts
{
    public static class SaveSystem
    {
        public static SaveData LocalData { get; private set; }

        public static void Save()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/save.data";
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, LocalData);
            stream.Close();
        }

        public static SaveData Load()
        {
            string path = Application.persistentDataPath + "/save.data";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                FileStream stream = new FileStream(path, FileMode.Open);

                SaveData data = formatter.Deserialize(stream) as SaveData;
                stream.Close();

                LocalData = data;
            }
            else
            {
                LocalData = new SaveData();
            }

            return LocalData;
        }
    }
}