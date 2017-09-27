using System.Collections.Generic;

namespace Gma.DataStructures.StringSearch
{
    internal class ResultInfo
    {

        public int TotalResults { get; set; }
        public IEnumerable<int> Results { get; set; }

        public ResultInfo(IEnumerable<int> results, int totalResults)
        {
            this.TotalResults = totalResults;
            this.Results = results;
        }
    }
}