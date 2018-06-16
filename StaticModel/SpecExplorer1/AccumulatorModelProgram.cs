using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Microsoft.Modeling;

namespace SpecExplorer1
{
    /// <summary>
    /// An example model program.
    /// </summary>
    static class AccumulatorModelProgram
    {
        static int accumulator;

        /// <summary>
        /// A rule that models the action of incrementing
        /// the accumulator by a number.
        /// </summary>
        /// <param name="x">The increment to be added to the accumulator.</param>
        [Rule(Action = "Add(x)")]
        static void AddRule(int x)
        {
            Condition.IsTrue(x > 0);
            accumulator += x;
        }

        /// <summary>
        /// A rule that models the action of reading the 
        /// current value of the accumulator and then setting
        /// it back to zero.
        /// </summary>
        /// <returns>The value of the accumulator before being reset.</returns>
        [Rule(Action = "ReadAndReset()/result")]
        static int ReadAndResetRule()
        {
            Condition.IsTrue(accumulator > 0);
            int oldValue = accumulator;
            accumulator = 0;
            return oldValue;
        }

    }
}
