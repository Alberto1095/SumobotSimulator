using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ConfigurationManager : MonoBehaviour
{
    private static string FOLDER_PATH = "/Resources/SavedData/";
    public static ConfigurationManager Instance = null;
    private List<ConfigTxtFile> list;

    // Initialize the singleton instance.
    private void Awake()
    {

        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            ReadSavedFiles();
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Dont destroy on load
        DontDestroyOnLoad(gameObject);
    }

    public void SaveConfig(string name,SumobotIAConfiguration config)
    {
        ConfigTxtFile txt = new ConfigTxtFile();
        string path = Application.dataPath + FOLDER_PATH + name+".txt";
        txt.SaveToFile(path, config);
        list.Add(txt);       
    }


    private void ReadSavedFiles()
    {
        list = new List<ConfigTxtFile>();

        string path = Application.dataPath+FOLDER_PATH;
        DirectoryInfo folder = new DirectoryInfo(path);
        FileInfo[] files = folder.GetFiles("*.txt");

        foreach(FileInfo f in files)
        {

            ConfigTxtFile txt = new ConfigTxtFile();
            txt.ReadFromFile(f.FullName, f.Name);
            list.Add(txt);
        }     

    }

    public SumobotIAConfiguration GetConfigByName(string name)
    {
        SumobotIAConfiguration c = null;
        foreach(ConfigTxtFile file in list)
        {
            if (file.fileName.Equals(name))
            {
                c = file.iaConfig;
                break;
            }
        }

        return c;
    }

    public List<string> GetFileNameList()
    {
        List<string> l = new List<string>();
        foreach (ConfigTxtFile file in list)
        {
            l.Add(file.fileName);
        }

        return l;
    }

}
