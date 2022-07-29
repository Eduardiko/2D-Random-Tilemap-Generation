using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

namespace WaveFunctionCollapse
{
    public class Pattern
    {
        private int _index;
        private int[][] _grid;

        public string hashIndex { get; set; }
        public int Index { get => _index; }

        public Pattern(int[][] grid, string hashCode, int index)
        {
            _grid = grid;
            hashIndex = hashCode;
            _index = index;
        }

        public void SetGridValue(int x, int y, int value)
        {
            _grid[y][x] = value;
        }

        public int GetGridValue(int x, int y)
        {
            return _grid[y][x];
        }

        public bool CheckValueAtPosition(int x, int y, int value)
        {
            return value.Equals(GetGridValue(x, y));
        }

        internal bool ComparePatternToPattern(Direction dir, Pattern pattern)
        {
            int[][] myGrid = GetGridValuesInDirection(dir);
            int[][] otherGrid = pattern.GetGridValuesInDirection(dir.GetOppositeDirectionTo());

            for (int row = 0; row < myGrid.Length; row++)
            {
                for (int col = 0; col < myGrid.Length; col++)
                {
                    if (myGrid[row][col] != otherGrid[row][col])
                        return false;
                }
            }

            return true;
        }

        //This function is a little bit bibu -> needs restudy
        private int[][] GetGridValuesInDirection(Direction dir)
        {
            int[][] gridPartToCompare;

            //Only using 1 .Length because patterns are square-shaped
            //If wanting rectangles it would need modifications

            switch (dir)
            {
                case Direction.UP:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(_grid.Length - 1, _grid.Length);
                    CreatePartOfGrid(0, _grid.Length, 1, _grid.Length, gridPartToCompare);
                    break;
                case Direction.DOWN:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(_grid.Length - 1, _grid.Length);
                    CreatePartOfGrid(0, _grid.Length, 0, _grid.Length - 1, gridPartToCompare);
                    break;
                case Direction.LEFT:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(_grid.Length, _grid.Length - 1);
                    CreatePartOfGrid(0, _grid.Length - 1, 0, _grid.Length, gridPartToCompare);
                    break;
                case Direction.RIGHT:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(_grid.Length, _grid.Length - 1);
                    CreatePartOfGrid(1, _grid.Length, 0, _grid.Length, gridPartToCompare);
                    break;
                default:
                    return _grid;
            }

            return gridPartToCompare;
        }

        //This is Jovanni
        private void CreatePartOfGrid(int minX, int maxX, int minY, int maxY, int[][] gridPartToCompare)
        {
            List<int> tempList = new List<int>();

            for (int row = minY; row < maxY; row++)
            {
                for (int col = minX; col < maxX; col++)
                {
                    tempList.Add(_grid[row][col]);
                }
            }

            for (int i = 0; i < tempList.Count; i++)
            {
                int x = i % gridPartToCompare.Length;
                int y = i / gridPartToCompare.Length;

                gridPartToCompare[x][y] = tempList[i];
            }
        }
    }

}
