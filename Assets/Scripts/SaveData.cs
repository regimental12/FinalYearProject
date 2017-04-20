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
        File.WriteAllLines("TestData" + Time.time + ".txt", data.ToArray());
    }
}
