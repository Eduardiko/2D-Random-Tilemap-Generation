using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

namespace WaveFunctionCollapse
{
    public static class PatternFinder
    {
        internal static PatternDataResults GetPatternDataFromGrid<T>(ValuesManager<T> valueManager, int patternSize, bool equalWeights)
        {
            Dictionary<string, PatternData> patternHashCodeDictionary = new Dictionary<string, PatternData>();
            Dictionary<int, PatternData> patternIndexDictionary = new Dictionary<int, PatternData>();

            //PatterGrid can have different size from ValueGrid depending on PatternSize
            Vector2 sizeOfGrid = valueManager.GetGridSize();
            int patternGridSizeX = 0;
            int patternGridSizeY = 0;

            int rowMin = -1, rowMax = -1;
            int colMin = -1, colMax = -1;

            //For PatternSize 1 and 2 there is special behaviour
            if(patternSize < 3)
            {
                patternGridSizeX = (int)sizeOfGrid.x + 3 - patternSize;
                patternGridSizeX = (int)sizeOfGrid.y + 3 - patternSize;
                
                rowMax = patternGridSizeY - 1;
                colMax = patternGridSizeX - 1;

            } else
            {
                patternGridSizeX = (int)sizeOfGrid.x + patternSize - 1;
                patternGridSizeY = (int)sizeOfGrid.y + patternSize - 1;
                
                //Setting starting index for pattern
                rowMin = 1 - patternSize;
                colMin = 1 - patternSize;

                rowMax = (int)sizeOfGrid.y;
                colMax = (int)sizeOfGrid.x;
            }

            //Create PatternIndicesGrid from extracting patterns
            int[][] patternIndicesGrid = MyCollectionExtension.CreateJaggedArray<int[][]>(patternGridSizeY, patternGridSizeX);
            int totalFrequency = 0;
            int patternIndex = 0;

            for (int row = rowMin; row < rowMax; row++)
            {
                for (int col = rowMin; col < colMax; col++)
                {
                    int[][] gridValues = valueManager.GetPatternValuesFromGridAt(col, row, patternSize);
                    string hashValue = HashCodeCalculator.CalculateHashCode(gridValues);

                    if(patternHashCodeDictionary.ContainsKey(hashValue) == false)
                    {
                        Pattern pattern = new Pattern(gridValues, hashValue, patternIndex);
                        patternIndex++;
                        AddNewPattern(patternHashCodeDictionary, patternIndexDictionary, hashValue, pattern);
                    } else
                    {
                        if(equalWeights == false)
                        {
                            patternIndexDictionary[patternHashCodeDictionary[hashValue].Pattern.Index].AddToFrequency();
                        }
                    }

                    totalFrequency++;


                    if(patternSize < 3)
                    {
                        patternIndicesGrid[row + 1][col + 1]  = patternHashCodeDictionary[hashValue].Pattern.Index;
                    } else
                    {
                        patternIndicesGrid[row + patternSize - 1][col + patternSize - 1] = patternHashCodeDictionary[hashValue].Pattern.Index;
                    }
                }
            }

            //After calculating all patterns, calculate relative frequency for all of them
            CalculateRelativeFrequency(patternIndexDictionary, totalFrequency);
            return new PatternDataResults(patternIndicesGrid, patternIndexDictionary);
        }

        private static void CalculateRelativeFrequency(Dictionary<int, PatternData> patternIndexDictionary, int totalFrequency)
        {
            foreach (var item in patternIndexDictionary.Values)
            {
                item.CalculateRelativeFrequency(totalFrequency);
            }
        }

        private static void AddNewPattern(Dictionary<string, PatternData> patternHashCodeDictionary, Dictionary<int, PatternData> patternIndexDictionary, string hashValue, Pattern pattern)
        {
            PatternData data = new PatternData(pattern);
            patternHashCodeDictionary.Add(hashValue, data);
            patternIndexDictionary.Add(pattern.Index, data);
        }

        internal static Dictionary<int, PatternNeighbours> FindPossibleNeighbours(IFindNeighbourStrategy strategy, PatternDataResults patternFinderResult)
        {
            return strategy.FindNeighbours(patternFinderResult);
        }

        public static PatternNeighbours CheckNeighboursInEachDirection(int x, int y, PatternDataResults patternDataResults)
        {
            PatternNeighbours patternNeighbours = new PatternNeighbours();
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                int possiblePatternIndex = patternDataResults.GetNeighbourInDirection(x, y, dir);
                if(possiblePatternIndex >= 0)
                {
                    patternNeighbours.AddPatternToDictionary(dir, possiblePatternIndex);
                }
            }
            return patternNeighbours;
        }

        public static void AddNeighboursToDictionary(Dictionary<int, PatternNeighbours> dictionary, int patternIndex, PatternNeighbours neighbours)
        {
            if(dictionary.ContainsKey(patternIndex) == false)
            {
                dictionary.Add(patternIndex, neighbours);
            }

            dictionary[patternIndex].AddNeighbour(neighbours);
        }

    }
}
