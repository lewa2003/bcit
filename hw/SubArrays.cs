using System;
using System.Collections.Generic;
using System.Text;

namespace hw
{
    public struct StartEnd
    {
        public int Start;
        public int End;

        public StartEnd(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
    public static class SubArrays
    {
        public static List<StartEnd> DivideSubArrays(int begin, int end, int subArraysCount)
        {
            List<StartEnd> result = new List<StartEnd>();

            if ((end - begin) <= subArraysCount)
            {
                result.Add(new StartEnd(0, (end - begin)));
            }
            else
            {
                int delta = (end - begin) / subArraysCount;
                int currentBegin = begin;

                while ((end - currentBegin) >= 2 * delta)
                {
                    result.Add(new StartEnd(currentBegin, currentBegin + delta));
                    currentBegin += delta;
                }
                result.Add(new StartEnd(currentBegin, end));
            }
            return result;
        }
    }
}
