using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evo.Core.Debugging
{
    
    public static class TimeManagment
    {
        public enum ShowOptions
        {
            SkipValues,
            DontSkipValues
        }
        public static long GetExecutionTime(Action function)
        {
            var startTime = DateTime.Now.Ticks;

            function();

            return DateTime.Now.Ticks - startTime;
        }

        public static void LogReturnValue<T>(Func<T> function, ShowOptions showOptions = ShowOptions.DontSkipValues, List<T> valuesToSkip = null)
        {
            var result = function();
            if (showOptions == ShowOptions.SkipValues && valuesToSkip.Contains(result))
                return;
            Console.WriteLine(result.ToString());
        }

        public static void LogExecutionTime(Action function)
        {
            TimeManagment.LogReturnValue(
                () =>
                    TimeManagment.GetExecutionTime(function)
                , ShowOptions.SkipValues, new List<long>(new long[] {0}));
        }

        public static void LogExecutionTime<T>(Func<T> function)
        {
            TimeManagment.LogReturnValue(
                () =>
                    TimeManagment.GetExecutionTime(function)
                );
        }

        public static long GetExecutionTime<T>(Func<T> function)
        {
            var startTime = DateTime.Now.Ticks;

            function();

            return DateTime.Now.Ticks - startTime;
        }
    }
}
