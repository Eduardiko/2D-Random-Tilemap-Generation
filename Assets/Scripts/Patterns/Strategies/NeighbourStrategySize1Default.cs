using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse
{
    public class NeighbourStrategySize1Default : IFindNeighbourStrategy
    {
        //Strategy 1 simply checks every cell in PatternGrid (== to ValueGrid due to no multipattern) and adds neighbours to each unique value
        public Dictionary<int, PatternNeighbours> FindNeighbours(PatternDataResults patternFinderResult)
        {
            Dictionary<int, PatternNeighbours> result = new Dictionary<int, PatternNeighbours>();
            FindNeighboursForEachPattern(patternFinderResult, result);

            return result;
        }


        private void FindNeighboursForEachPattern(PatternDataResults patternFinderResult, Dictionary<int, PatternNeighbours> result)
        {
            for (int row = 0; row < patternFinderResult.GetGridLengthY(); row++)
            {
                for (int col = 0; col < patternFinderResult.GetGridLengthX(); col++)
                {
                    PatternNeighbours neighbours = PatternFinder.CheckNeighboursInEachDirection(col, row, patternFinderResult);
                    PatternFinder.AddNeighboursToDictionary(result, patternFinderResult.GetIndexAt(col, row), neighbours);
                }
            }
        }
    }
}

