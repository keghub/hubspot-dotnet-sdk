using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

namespace Tests
{
    public static class NUnitHelpers
    {
        public static Constraint Combine<T>(IEnumerable<T> items, Func<T, Constraint> constraintPerItem, CombineOperator op)
        {
            Constraint result = null;

            foreach (var item in items)
            {
                if (result == null)
                {
                    result = constraintPerItem(item);
                }
                else
                {
                    if (op is CombineOperator.And)
                    {
                        result &= constraintPerItem(item);
                    }
                    else
                    {
                        result |= constraintPerItem(item);
                    }
                }
            }

            return result;
        }

        public static Constraint All<T>(IEnumerable<T> items, Func<T, Constraint> constraintPerItem)
            => Combine<T>(items, constraintPerItem, CombineOperator.And);

        public static Constraint Any<T>(IEnumerable<T> items, Func<T, Constraint> constraintPerItem)
            => Combine<T>(items, constraintPerItem, CombineOperator.Or);
    }

    public enum CombineOperator
    {
        And, Or
    }
}