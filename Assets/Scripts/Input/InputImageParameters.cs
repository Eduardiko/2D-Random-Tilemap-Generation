using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse
{
    public class InputImageParameters
    {
        //? means that it can be null
        Vector2Int? bottomLeftTileCoords = null;
        Vector2Int? topRightTileCoords = null;
        BoundsInt inputTileMapBounds;
        private int width = 0, height = 0;

        TileBase[] inputTilemapTilesArray;

        Queue<TileContainer> queueOfTiles = new Queue<TileContainer>();
        private Tilemap inputTilemap;


        public Queue<TileContainer> QueueOfTiles { get => queueOfTiles; set => queueOfTiles = value; }
        public int Width { get => width; }
        public int Height { get => height; }

        public InputImageParameters(Tilemap inputTilemap)
        {
            //Assign Tilemap values before cleanup
            this.inputTilemap = inputTilemap;
            this.inputTileMapBounds = this.inputTilemap.cellBounds;

            //Get 1D array of all tiles -> [0] is at bottom left ([0][0])_3
            this.inputTilemapTilesArray = this.inputTilemap.GetTilesBlock(this.inputTileMapBounds);

            //Clean Tilemap
            ExtractNonEmptyTiles();
            VerifyInputTiles();
        }

        private void ExtractNonEmptyTiles()
        {
            for (int row = 0; row < inputTileMapBounds.size.y; row++)
            {
                for (int col = 0; col < inputTileMapBounds.size.x; col++)
                {
                    //2D to 1D conversion
                    int index = col + (row * inputTileMapBounds.size.x);

                    TileBase tile = inputTilemapTilesArray[index];

                    //First non-empty tile will be bottom left
                    if(bottomLeftTileCoords == null && tile != null)
                    {
                        //Columns = x and Rows = y but remember that jagged arrays use [rows][columns]
                        bottomLeftTileCoords = new Vector2Int(col, row);
                    }

                    //Last non-empty tile will be top right
                    if (tile != null)
                    {
                        queueOfTiles.Enqueue(new TileContainer(tile, col, row));
                        topRightTileCoords = new Vector2Int(col, row);
                    }
                }
            }

        }
        private void VerifyInputTiles()
        {
            //Security Check
            if(bottomLeftTileCoords == null || topRightTileCoords == null)
            {
                throw new System.Exception("WFC: Input tilemap is empty");
            }

            int minX = bottomLeftTileCoords.Value.x;
            int maxX = topRightTileCoords.Value.x;

            int minY = bottomLeftTileCoords.Value.y;
            int maxY = topRightTileCoords.Value.y;

            //+1 because indexes start from 0 but width/height start from 1
                //If there is 1 tile it will be index 0 but have a w/h of 1,1
            height = Math.Abs(maxX - minX) + 1;
            width = Math.Abs(maxY - minY) + 1;

            //Security Check 2
            int tileCount = width * height;
            if(queueOfTiles.Count != tileCount)
            {
                throw new System.Exception("WFC: Tilemap has empty fields");
            }

            //Security Check 3
            if (queueOfTiles.Any(tile => tile.X > maxX || tile.X < minX || tile.Y > maxY || tile.Y < minY))
            {
                throw new System.Exception("WFC: Tilemap has overflowing fields");
            }
        }

    }
}
