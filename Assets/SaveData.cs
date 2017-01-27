using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveData
{

    public static void SaveBotData(List<BotData> list)
    {
        List<string> data = new List<string>();
        foreach (BotData bot in list)
        {
            
            data.Add(bot.BotName);
            data.Add("Deaths: " + bot.Deaths.ToString());
            data.Add("Kills: " + bot.Kills.ToString());
            data.Add("\n");   
        }
        File.WriteAllLines("TestData.txt", data.ToArray());

        /*List<int> data = new List<int>();
        data.Add(GameManager.Instance.powerUps);
        data.Add(GameManager.Instance.HighScore);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedData.stb");
        // Save data here
        bf.Serialize(file, list);
        bf.Serialize()
        file.Close();*/
    }
}
