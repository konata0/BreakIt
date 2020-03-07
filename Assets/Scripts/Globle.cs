using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Globle : MonoBehaviour{
    public static JsonData levelData;
    public static int row = 6;
    public static int col = 10;
    public static int levelNumber = 1;
    public static int levelIndex = 0;

    public static float playerLength;

    // 从JSON文件读取关卡数据
    public static void readLevelData(){
        StreamReader streamReader = new StreamReader(Application.dataPath + @"/GameData/LevelData.json");
        string str = streamReader.ReadToEnd();
        levelData = JsonMapper.ToObject(str);
    }

    // 获取关卡数据
    public static List<List<int>> getLevelData(){
        List<List<int>> re = new List<List<int>>();
        for(int i = 0; i <= row - 1; i++){
            re.Add(new List<int>());
            for(int j = 0; j <= col - 1; j++){
                re[i].Add((int)(levelData["level" + levelIndex.ToString()]["data"][i * col + j]));
            }
        }   
        return re;
    }
    // 获取关卡背景设置
    public static int getLevelBackground(){  
        return (int)(levelData["level" + levelIndex.ToString()]["bg"]);
    }

}
