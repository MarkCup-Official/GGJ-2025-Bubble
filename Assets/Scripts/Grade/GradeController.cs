using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GradeController : MonoBehaviour
{
    public static GradeController _instance;
    public GameObject prefab;
    GameObject list_gameobject;
    StoreManager storeManager;
    public int size_y;
    public int space_y;
    public SortedDictionary<float, string> list = new SortedDictionary<float, string>();

    private void Awake()
    {
        _instance = this;
        list_gameobject = GameObject.Find("List").gameObject;
        storeManager = GameObject.Find("GameController").GetComponent<StoreManager>();
        storeManager.ClickLoadGame();
        // 按照存档添加对应数量的子对象
        int num = list.Count;
        for(int i = 0; i < num; i++){
            GameObject grade = GameObject.Instantiate(prefab) as GameObject;
            grade.transform.parent = list_gameobject.transform;
        }
        // 需要添加新的记录
        if(PlayerPrefs.GetInt("New") == 1){
            PlayerPrefs.SetInt("New", 0);
            string name = PlayerPrefs.GetString("Name");
            float time = PlayerPrefs.GetFloat("Time");
            AddGrade(time, name);
        }
        Repaint();
    }

    void Repaint(){
        int num = list.Count;
        // 改变List高度
        RectTransform transform = list_gameobject.transform.GetComponent<RectTransform>();
        int height = num * size_y + (num - 1) * space_y;
        transform.sizeDelta = new Vector2(0, height); // 宽，高
        // 填充每个子对象的内容
        int index = 0;
        foreach (Transform child in list_gameobject.transform)
        {
            child.gameObject.transform.Find("Name").GetComponent<Text>().text = list.ElementAt(num - 1 - index).Value;
            child.gameObject.transform.Find("Time").GetComponent<Text>().text = list.ElementAt(num - 1 - index).Key.ToString();
            index++;
        }
    }

    public void AddGrade(float time, string name){
        // 原来就有这个玩家
        if(list.ContainsValue(name)){
            float key = list.FirstOrDefault(x=>x.Value==name).Key;
            list.Remove(key);
            list.Add(time, name);
        }
        // 原来没有
        else{
            list.Add(time, name);
            // 添加新的成绩
            GameObject grade = GameObject.Instantiate(prefab) as GameObject;
            grade.transform.parent = list_gameobject.transform;
        }
        Repaint();
        storeManager.ClickSaveGame();
    }
}