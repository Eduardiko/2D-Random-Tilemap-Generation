using System.Collections;
using System.Collections.Generic;
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
        //for (int row = 0; row < grid.Length; row++)
        //{
        //    for (int col = 0; col < grid[0].Length; col++)
        //    {
        //        Debug.Log("Row: " + row + "Column: " + col + "Tilename: " + grid[row][col].Value.name); 
        //    }
        //}

        ValuesManager<TileBase> valuesManager = new ValuesManager<TileBase>(grid);
        StringBuilder builder = null;
        List<string> list = new List<string>();

        for (int row = 0; row < grid.Length; row++)
        {
            builder = new StringBuilder();
            for (int col = -3; col < grid[0].Length; col++)
            {
                builder.Append(valuesManager.GetGridValueIncludingOffset(col, row) + " ");
            }

            list.Add(builder.ToString());
        }

        list.Reverse();
        foreach(var item in list)
        {
            Debug.Log(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
