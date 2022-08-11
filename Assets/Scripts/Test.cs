using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaveFunctionCollapse;

public class Test : MonoBehaviour
{
    public Tilemap testMap;

    // Start is called before the first frame update
    void Start()
    {
        InputReader reader = new InputReader(testMap);
        var grid = reader.ReadInputToGrid();
        ValuesManager<TileBase> valueManager = new ValuesManager<TileBase>(grid);


        //--------------------- TEST 1: Print Input TileMap Info ---------------------
        //for (int row = 0; row < grid.Length; row++)
        //{
        //    for (int col = 0; col < grid[0].Length; col++)
        //    {
        //        Debug.Log("Row: " + row + "Column: " + col + "Tilename: " + grid[row][col].Value.name); 
        //    }
        //}


        //--------------------- TEST 2: Print ValueGrid ---------------------
        //StringBuilder builder = null;
        //List<string> list = new List<string>();

        //for (int row = 0; row < grid.Length; row++)
        //{
        //    builder = new StringBuilder();
        //    for (int col = 0; col < grid[0].Length; col++)
        //    {
        //        builder.Append(valueManager.GetGridValueIncludingOffset(col, row) + " ");
        //    }

        //    list.Add(builder.ToString());
        //}

        //list.Reverse();
        //foreach(var item in list)
        //{
        //    Debug.Log(item);
        //}


        //--------------------- TEST 3: Print PatternGrid w/Offset ---------------------
        // 3.1 PatternSize 1
        //PatternManager manager = new PatternManager(1);
        //manager.ProcessGrid(valueManager, false);

        // 3.2 PatternSize 2 or >
        PatternManager manager = new PatternManager(2);
        manager.ProcessGrid(valueManager, false);
        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            Debug.Log(dir.ToString() + " " + string.Join(" ", manager.GetPossibleNeighboursForPatternInDirection(0, dir).ToArray()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
