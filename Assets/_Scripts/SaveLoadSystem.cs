using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadSystem
{
    public static void Save<T>(T Data, string Path, Action OnSuccess, Action<Exception> OnError)
    {
        try
        {
            /*if (Path == "")
            {
                Path = Application.persistentDataPath + "/Data/Data.save";
            }*/
            BinaryFormatter binary = new BinaryFormatter();
            FileStream stream = new FileStream(Path, FileMode.Create);
            binary.Serialize(stream, Data);
            stream.Dispose();
            OnSuccess();
        }
        catch (Exception e)
        {
            OnError(e);
        }
    }

    public static void Load<T>(string Path, Action<T> LoadData, Action OnSuccess, Action onNotFind,
        Action<Exception> OnError)
    {
        try
        {
            /*if (Path == "")
            {
                Path = Application.persistentDataPath + "/Data/Data.save";
            }*/
            if (File.Exists(Path))
            {
                BinaryFormatter binary = new BinaryFormatter();
                FileStream stream = File.OpenRead(Path);
                LoadData((T) binary.Deserialize(stream));
                stream.Dispose();
                OnSuccess();
            }
            else
            {
                onNotFind();
            }
        }
        catch (Exception e)
        {
            OnError(e);
        }
    }

    public void Reset()
    {
    }
}