using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class PatternManager
    {
        Dictionary<int, PatternData> patternDataIndexDictionary;
        Dictionary<int, PatternNeighbours> patternPossibleNeighbourDictionary;

        int _patternSize = -1;
        IFindNeighbourStrategy strategy;

        public PatternManager(int patternSize)
        {
            _patternSize = patternSize;
        }

        public void ProcessGrid<T>(ValuesManager<T> valueManager, bool equalWeights, string strategyName = null)
        {
            NeighbourStrategyFactory strategyFactory = new NeighbourStrategyFactory();
            //Choose pattern strategy
            strategy = strategyFactory.CreateInstance(strategyName == null ? _patternSize + "" : strategyName);
            CreatePatterns(valueManager, strategy, equalWeights);
        }

        private void CreatePatterns<T>(ValuesManager<T> valueManager, IFindNeighbourStrategy strategy, bool equalWeights)
        {
            //Find Neighbours and Patterns
            var patternFinderResult = PatternFinder.GetPatternDataFromGrid(valueManager, _patternSize, equalWeights);
            patternDataIndexDictionary = patternFinderResult.PatternIndexDictionary;

            GetPatternNeighbours(patternFinderResult, strategy);

        }

        private void GetPatternNeighbours(PatternDataResults patternFinderResult, IFindNeighbourStrategy strategy)
        {
            patternPossibleNeighbourDictionary = PatternFinder.FindPossibleNeighbours(strategy, patternFinderResult);
        }

        public PatternData GetPatternDataFromIndex(int index)
        {
            return patternDataIndexDictionary[index];
        }

        public HashSet<int> GetPossibleNeighboursForPatternInDirection(int patternIndex, Direction dir)
        {
            return patternPossibleNeighbourDictionary[patternIndex].GetNeighboursInDirection(dir);
        }

        public float GetPatternFrequency(int index)
        {
            return GetPatternDataFromIndex(index).FrequencyRelative;
        }

        public float GetPatternFrequencyLog2(int index)
        {
            return GetPatternDataFromIndex(index).FrequencyRelativeLog2;
        }

        public int GetNumberOfPatterns()
        {
            return patternDataIndexDictionary.Count;
        }
    }
}
