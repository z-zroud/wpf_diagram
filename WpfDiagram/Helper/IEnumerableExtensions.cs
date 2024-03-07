using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDiagram.Helper
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum Double value
        /// if the sequence is not empty; otherwise returns the specified default value.
        /// </summary>
        /// <typeparam name="TSource"> The type of the elements of source. </typeparam>
        /// <param name="source"> A sequence of values to determine the minimum value of. </param>
        /// <param name="selector"> A transform function to apply to each element. </param>
        /// <param name="defaultValue"> The default value. </param>
        /// <returns> The minimum value in the sequence or default value if sequence is empty. </returns>
        public static double MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double defaultValue=default(double))
        {
            if (source.Any<TSource>())
                return source.Min<TSource>(selector);

            return defaultValue;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum Double value
        /// if the sequence is not empty; otherwise returns the specified default value.
        /// </summary>
        /// <typeparam name="TSource"> The type of the elements of source. </typeparam>
        /// <param name="source"> A sequence of values to determine the maximum value of. </param>
        /// <param name="selector"> A transform function to apply to each element. </param>
        /// <param name="defaultValue"> The default value. </param>
        /// <returns> The maximum value in the sequence or default value if sequence is empty. </returns>
        public static double MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double defaultValue=default(double))
        {
            if (source.Any<TSource>())
                return source.Max<TSource>(selector);

            return defaultValue;
        }


        public static TResult MinOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult defaultValue)
        {
            if (source.Any())
            {
                return source.Min(selector);
            }
            return defaultValue;

        }


        public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult defaultValue)
        {
            if (source.Any())
            {
                return source.Max(selector);
            }
            return defaultValue;
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum Double value
        /// if the sequence is not empty; otherwise returns the specified default value.
        /// </summary>
        /// <typeparam name="TSource"> The type of the elements of source. </typeparam>
        /// <param name="source"> A sequence of values to determine the minimum value of. </param>
        /// <param name="selector"> A transform function to apply to each element. </param>
        /// <param name="defaultValue"> The default value. </param>
        /// <returns> The minimum value in the sequence or default value if sequence is empty. </returns>
        public static double SumOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double defaultValue=default(double))
        {
            if (source.Any<TSource>())
                return source.Sum<TSource>(selector);

            return defaultValue;
        }

   
    }
}
