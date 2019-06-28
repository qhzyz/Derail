using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour
{
    public int count;
    public int startCount;
    public string fileName;
    public List<BasicAster> asterList = new List<BasicAster>();
    private List<AsterData> dataList = new List<AsterData>();
    public List<GameObject> positionList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(delegate () {
            asterList = SortAsters(asterList);
            foreach (BasicAster i in asterList)
                dataList.Add(i.GetAsterData());
            SaveToFile(fileName);
        });
    }
    List<BasicAster> SortAsters(List<BasicAster> list) {
        List<BasicAster> res = new List<BasicAster>();
        for (int i = 0; i < list.Count;)
        {
            if (!list[i].asterParent)
            {
                res.Add(list[i]);
                list.RemoveAt(i);
            }
            else
                i++;
        }
        redo:
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < res.Count; j++)
            {
                if (list[i].asterParent.GetComponent<BasicAster>().id == res[j].id)
                {
                    list[i].asterParent.GetComponent<BasicAster>().id = j;
                    res.Add(list[i]);
                    list.RemoveAt(i);
                    goto redo;
                }
            }
        }
        for (int i = 0; i < res.Count; i++)
        {
            res[i].id = i;
        }
        return res;
    }

    public void SaveToFile(string fileName)
    {
        BinaryWriter bw = new BinaryWriter(new FileStream(fileName+".map",FileMode.Create));
        bw.Write(count);
        bw.Write(startCount);
        
        for (int i = 0; i < count; i++)
        {
            bw.Write(dataList[i].parentID);
            bw.Write(dataList[i].ID);
            bw.Write(dataList[i].ownerTeamID);
            bw.Write(dataList[i].prefabName);
            bw.Write((double)dataList[i].myPosition.x);
            bw.Write((double)dataList[i].myPosition.y);
            bw.Write((double)dataList[i].myPosition.z);
            bw.Write(dataList[i].isClockwise);
        }
        for (int i = 0; i < startCount; i++)
        {
            bw.Write((double)positionList[i].transform.position.x);
            bw.Write((double)positionList[i].transform.position.y);
            bw.Write((double)positionList[i].transform.position.z);
        }
        bw.Close();
    }
}
