using System;
using System.Collections.Generic;
using System.Linq;
using SFML.System;

namespace AStarSimulation
{
    internal static class Utils
    {
        private static readonly Random Random = new Random();

        public static double Clamp(double value, double limit)
        {
            if (value > limit)
            {
                value = limit;
            }
            else if (value < -limit)
            {
                value = -limit;
            }

            return value;
        }

        /// <summary>
        /// Return whether or not the two position vectors are within a specified range of eachother
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static bool WithinRange(Vector2f u, Vector2f v, double range)
        {
            if (Math.Abs(u.X - v.X) > range)
                return false;
            if (Math.Abs(u.Y - v.Y) > range)
                return false;
            return Length(u - v) < range;
        }

        /// <summary>
        /// Return whether or not the two position vectors are within a specified range of eachother.
        /// Distance receives the range between them. Distance will be double.PositiveInfiniy if vectors
        /// are not within range of eachother.
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="range"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static bool WithinRange(Vector2f u, Vector2f v, double range, out double distance)
        {
            if (Math.Abs(u.X - v.X) > range)
            {
                distance = double.PositiveInfinity;
                return false;
            }
            if (Math.Abs(u.Y - v.Y) > range)
            {
                distance = double.PositiveInfinity;
                return false;
            }

            var len = Length(u - v);

            if (len <= range)
            {
                distance = len;
                return true;
            }
            distance = double.PositiveInfinity;
            return false;
        }

        public static Vector2f Normalize(Vector2f v)
        {
            var length = (float) Length(v);

            return Math.Abs(length) > float.Epsilon ? new Vector2f(v.X / length, v.Y / length) : new Vector2f(0, 0);
        }

        public static Vector2f Scale(Vector2f v, double length)
        {
            var scaleFactor = (float) (length / Length(v));
            return new Vector2f(v.X * scaleFactor, v.Y * scaleFactor);
        }

        public static double SquaredDistance(Vector2f v, Vector2f u)
        {
            return SquaredLength(v - u);
        }

        public static double Distance(Vector2f v, Vector2f u)
        {
            return Length(v - u);
        }

        public static double SquaredLength(Vector2f v)
        {
            return (v.X * v.X + v.Y * v.Y);
        }

        public static double Length(Vector2f v)
        {
            return Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }

        public static Vector2f RandomVector(double scaleFactor)
        {
            double x, y;

            lock (Random)
            {
                x = RandomSignedDouble();
                y = RandomSignedDouble();
            }


            x *= scaleFactor;
            y *= scaleFactor;

            return new Vector2f((float) x, (float) y);
        }

        public static double RandomSignedDouble()
        {
            var sign = RandomBool();
            double retValue;

            lock (Random)
            {
                retValue = Random.NextDouble();
            }

            if (sign)
                return retValue;

            return -retValue;
        }

        public static bool RandomBool()
        {
            int rand;

            lock (Random)
            {
                rand = Random.Next(2);
            }

            return rand != 0;
        }

        public static Vector2f AverageVector(List<Vector2f> vectors)
        {
            var result = new Vector2f(0, 0);

            result = vectors.Aggregate(result, (current, i) => current + i);

            result /= vectors.Count;

            return result;
        }

        public static Vector2f AverageVector(Vector2f v, Vector2f u)
        {
            return (v + u) / 2;
        }
    }
}