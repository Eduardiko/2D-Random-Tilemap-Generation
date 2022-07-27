using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;
using System.Linq;

namespace WaveFunctionCollapse
{
    //Will convert the tile value-based grid into an (int)index-based one
    public class ValuesManager<T>
    {

        int[][] _grid;
        Dictionary<int, IValue<T>> valueIndexDictionary = new Dictionary<int, IValue<T>>();
        int index = 0; 

        public ValuesManager(IValue<T>[][] gridOfValues)
        {
            CreateGridOfIndices(gridOfValues);
        }

        private void CreateGridOfIndices(IValue<T>[][] gridOfValues)
        {
            _grid = MyCollectionExtension.CreateJaggedArray<int[][]>(gridOfValues.Length, gridOfValues[0].Length);

            for (int row = 0; row < gridOfValues.Length; row++)
            {
                for (int col = 0; col < gridOfValues[0].Length; col++)
                {
                    //Assign new index to tile if not existing in the dictionary
                    SendIndexToGridPosition(gridOfValues, row, col);
                }
            }
        }

        private void SendIndexToGridPosition(IValue<T>[][] gridOfValues, int row, int col)
        {
            if (valueIndexDictionary.ContainsValue(gridOfValues[row][col]))
            {
                //Assign already existing key
                var key = valueIndexDictionary.FirstOrDefault(x => x.Value.Equals(gridOfValues[row][col]));
                _grid[row][col] = key.Key;
            } else
            {
                //Assign new index and add key to dictionary
                _grid[row][col] = index;
                valueIndexDictionary.Add(_grid[row][col], gridOfValues[row][col]);
                index++;
            }
        }

        public int GetGridValue(int x, int y)
        {
            //Security Check
            if (x >= _grid[0].Length || y >= _grid.Length || x < 0 || y < 0)
            {
                throw new System.IndexOutOfRangeException("Trying to access out of range value");
            }

            return _grid[y][x];
        }
        
        public IValue<T> GetValueFromIndex(int index)
        {
            if(valueIndexDictionary.ContainsKey(index))
            {
                return valueIndexDictionary[index];
            }

            //Security Check
            throw new System.Exception("Index not found in dictionary");
        }

        public int GetGridValueIncludingOffset(int x, int y)
        {
            //Give proper index if searching outside bounds
            int maxX = _grid[0].Length;
            int maxY = _grid.Length;

            int finalX = x;
            int finalY = y;

            if (x < 0) finalX = x + maxX;
            if (y < 0) finalY = y + maxY;

            if (x >= maxX) finalX = x - maxX;
            if (y >= maxY) finalY = y - maxY;

            return GetGridValue(finalX, finalY);
        }

        public int[][] GetPatternValuesFromGridAt(int x, int y, int patternSize)
        {
            //At 0,0 x and y represent bottom left corner. Pattern gets taken from down/left to up/right
            int[][] arrayToReturn = MyCollectionExtension.CreateJaggedArray<int[][]>(patternSize, patternSize);
            
            for (int row = 0; row < patternSize; row++)
            {
                for (int col = 0; col < patternSize; col++)
                {
                    arrayToReturn[row][col] = GetGridValueIncludingOffset(x + col,y + row);
                }
            }

            return arrayToReturn;
        }

    }
}
