using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ParallelTask
{
    public class TestExpression
    {
        public static void StartsWith()
        {
            Expression<Func<string, string, bool>> expression =
                (x, y) => x.StartsWith(y);
            var compiled = expression.Compile();
            Console.WriteLine(compiled("First", "Second"));
            Console.WriteLine(compiled("First", "Fir"));
        }

        public static void Range()
        {
            ParallelEnumerable.Range(0, 1000)
                              .Where(x => x % 2 != 0)
                              .Reverse()
                              .ForAll(item => Console.WriteLine("ParallelEnumerable: " + item));
        }
    }
}
