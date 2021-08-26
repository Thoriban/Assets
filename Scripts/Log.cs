using System;
using System.IO;
using UnityEngine;

public static class Log
{
    static string filePath = Application.persistentDataPath + "/Logs/appLog.txt";

    public static void StartUpEntry()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Logs/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Logs/");   
        }

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        File.Create(filePath).Close();
    }


    public static void Write(string message)
    {
        string hour0 = "";
        string min0 = "";
        string sec0 = "";

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }

        DateTime now = DateTime.Now;

        using (StreamWriter sw = new StreamWriter(filePath, true))
        {
            if (now.Hour < 10)
            {
                hour0 = "0";
            }
            if (now.Minute < 10)
            {
                min0 = "0";
            }
            if (now.Second < 10)
            {
                sec0 = "0";
            }

            sw.WriteLine("[" + now.Day + "/" + now.Month + "/" + now.Year + " " + hour0 + now.Hour + ":" + min0 + now.Minute + ":" + sec0 + now.Second + "] " + message);

            sw.Dispose();
        }
    }
}
