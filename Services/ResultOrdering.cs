using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodMatchTester.Services
{
    public class ResultOrdering
    {
        public static List<CsvResult> OrderMatchResults(List<CsvResult> csvResults)
        {
            var result = csvResults.First().ResultPercentage;

            if (!csvResults.All(x => x.ResultPercentage == result))
            {
                return csvResults.OrderByDescending(x => x.ResultPercentage).ToList();
            }
            return csvResults.OrderBy(x => x.CsvStringResult).ToList();
        }
    }
}
