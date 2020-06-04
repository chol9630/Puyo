using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapSection : MonoBehaviour
{
    public static MapSection instance;
    public static int map;


 
    public void oneTileChoice()
    {
        map = 1;
    }

    public void TwoTileChoice()
    {
        map = 2;
    }

}
