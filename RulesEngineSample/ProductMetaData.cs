using System.Collections.Generic;
using System.Linq;
using System;
using System.Linq.Expressions;

namespace RulesEngineSample
{

    public class Rule
    {
        ///
        /// Denotes the rules predictate (e.g. Name); comparison operator(e.g. ExpressionType.GreaterThan); value (e.g. "Cole")
        /// 
        public string ComparisonPredicate { get; set; }
        public ExpressionType ComparisonOperator { get; set; }
        public string ComparisonValue { get; set; }

        /// 
        /// The rule method that 
        /// 
        public Rule(string comparisonPredicate, ExpressionType comparisonOperator, string comparisonValue)
        {
            ComparisonPredicate = comparisonPredicate;
            ComparisonOperator = comparisonOperator;
            ComparisonValue = comparisonValue;
        }
    }

    public class ProductMetaData
    {
        public decimal MaxSumAssured { get; set; }
        public decimal MinSumAssured { get; set; }
        public int ProductCode { get; set; }

        public int ProductVersion { get; set; }

        public int MinPremium { get; set; }

        public int MinEntryAge { get; set; }

    }

    public class PrecompiledRules
    {
        ///
        /// A method used to precompile rules for a provided type
        /// 
        public static List<Func<T, bool>> CompileRule<T>(List<T> targetEntity, List<Rule> rules)
        {
            var compiledRules = new List<Func<T, bool>>();

            // Loop through the rules and compile them against the properties of the supplied shallow object 
            rules.ForEach(rule =>
            {
                var genericType = Expression.Parameter(typeof(T));
                var key = MemberExpression.Property(genericType, rule.ComparisonPredicate);
                var propertyType = typeof(T).GetProperty(rule.ComparisonPredicate).PropertyType;
                var value = Expression.Constant(Convert.ChangeType(rule.ComparisonValue, propertyType));
                var binaryExpression = Expression.MakeBinary(rule.ComparisonOperator, key, value);

                compiledRules.Add(Expression.Lambda<Func<T,bool>>(binaryExpression, genericType).Compile());
            });

            // Return the compiled rules to the caller
            return compiledRules;
        }
    }
}
