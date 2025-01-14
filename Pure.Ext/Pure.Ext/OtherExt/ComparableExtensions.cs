﻿using System;
using System.Collections.Generic;

/// <summary>
/// Comparable Extensions
/// </summary>
public static class ComparableExtensions
{
    /// <summary>
    /// 是否在指定区间中
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "value">The value.</param>
    /// <param name = "minValue">The minimum value.</param>
    /// <param name = "maxValue">The maximum value.</param>
    /// <returns>
    /// 	<c>true</c> if the specified value is between min and max; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsBetween<T>(this T value, T minValue, T maxValue) where T : IComparable<T>
    {
        return IsBetween(value, minValue, maxValue, Comparer<T>.Default);
    }

    /// <summary>
    /// 是否在指定区间中
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "value">The value.</param>
    /// <param name = "minValue">The minimum value.</param>
    /// <param name = "maxValue">The maximum value.</param>
    /// <param name = "comparer">An optional comparer to be used instead of the types default comparer.</param>
    /// <returns>
    /// 	<c>true</c> if the specified value is between min and max; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Note that this does an "inclusive" comparison:  The high & low values themselves are considered "in between".
    /// However, in some context, a exclusive comparion -- only values greater than the low end and lesser than the high end
    /// are "in between" -- is desired; in other contexts, values can be greater or equal to the low end, but only less than the high end.
    /// </remarks>
    public static bool IsBetween<T>(this T value, T minValue, T maxValue, IComparer<T> comparer) where T : IComparable<T>
    {
        if (comparer == null)
            throw new ArgumentNullException("comparer");

        var minMaxCompare = comparer.Compare(minValue, maxValue);
        if (minMaxCompare < 0)
            return ((comparer.Compare(value, minValue) >= 0) && (comparer.Compare(value, maxValue) <= 0));
        //else if (minMaxCompare == 0)				// unnecessary  'else' below handles this case.
        //    return (comparer.Compare(value, minValue) == 0);
        else
            return ((comparer.Compare(value, maxValue) >= 0) && (comparer.Compare(value, minValue) <= 0));
    }

    /// <summary>
    /// 降序排列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DescendingComparer<T> : IComparer<T> where T : IComparable<T>
    {
        public int Compare(T x, T y)
        {
            return y.CompareTo(x);
        }
    }

    /// <summary>
    /// 升序排列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AscendingComparer<T> : IComparer<T> where T : IComparable<T>
    {
        public int Compare(T x, T y)
        {
            return x.CompareTo(y);
        }
    }
}