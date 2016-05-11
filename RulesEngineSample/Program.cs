using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RulesEngineSample
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Rule> rules = new List<Rule>
            {
                 // Create some rules using LINQ.ExpressionTypes for the comparison operators
                 new Rule ( "MaxSumAssured", ExpressionType.Equal, "250000"),
                 new Rule ( "ProductCode", ExpressionType.Equal, "2001"),
                 new Rule ( "MinEntryAge", ExpressionType.Equal, "59" )

            };

            var compiledMakeModelYearRules = PrecompiledRules.CompileRule(new List<ProductMetaData>(), rules);

            var productMetaData = new List<ProductMetaData>() {
                new ProductMetaData() { MaxSumAssured = 250000, ProductCode=2001, ProductVersion=1, MinSumAssured=1500, MinEntryAge=18, MinPremium=5 },
                new ProductMetaData() { MaxSumAssured = 150000, ProductCode=2001, ProductVersion=2, MinSumAssured=1500, MinEntryAge=18, MinPremium=5 },
                new ProductMetaData() { MaxSumAssured = 50000, ProductCode=2001, ProductVersion=3, MinSumAssured=1500, MinEntryAge=18, MinPremium=5 },
                new ProductMetaData() { MaxSumAssured = 500000, ProductCode=2001, ProductVersion=4, MinSumAssured=1500, MinEntryAge=18, MinPremium=5 },
                new ProductMetaData() { MaxSumAssured = 450000, ProductCode=2001, ProductVersion=5, MinSumAssured=1500, MinEntryAge=18, MinPremium=5 }
            };

            productMetaData.ForEach(pm => {
                if (compiledMakeModelYearRules.TakeWhile(i => i(pm)).Count() > 0){
                    Console.WriteLine("PM model: {0} {1} Passed the compiled rules engine check!", pm.ProductCode, pm.ProductVersion);
                }
                else
                {
                    Console.WriteLine("PM model: {0} {1} Failed!", pm.ProductCode, pm.ProductVersion);
                }
            });

        }
    }
}
