using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{
    public Camera minimapCam;
    public RectTransform boarder;
    void Start()
    {
        float mapW = BKBorder.w;
        float mapH = BKBorder.h;
        //高度固定，宽度适应
        minimapCam.orthographicSize = mapH/2;
        minimapCam.rect = new Rect(0,0,1,mapH/mapW);
        GetComponent<RectTransform>().sizeDelta = new Vector2(400 * mapW / mapH, 400);
        GetComponent<RawImage>().uvRect = new Rect(0, 0, 1, mapH / mapW);
        boarder.sizeDelta= new Vector2(400 * mapW / mapH, 400);
    }


    void Update()
    {
        
    }
}
