using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfDiagram.Helper
{
    public static class DrawingHelper
    {
        public static List<Point> GetPoints(Point startPoint, double angle, List<Tuple<double, double>> angleAndWidth)
        {
            List<Point> points = new List<Point>();
            points.Add(startPoint);

            Point thisPoint = startPoint;
            double thisAngle = angle;

            Point nextPoint;
            foreach (var item in angleAndWidth)
            {
                nextPoint = GetEndPointByTrigonometric(thisPoint, thisAngle, item.Item1);
                //if (thisPoint != nextPoint)
                {
                    points.Add(nextPoint);
                }

                thisPoint = nextPoint;
                thisAngle = thisAngle + item.Item2;
            }


            return points;
        }


        /// <summary>
        /// 通过三角函数求终点坐标
        /// </summary>
        /// <param name="angle">角度</param>
        /// <param name="startPoint">起点</param>
        /// <param name="distance">距离</param>
        /// <returns>终点坐标</returns>
        public static Point GetEndPointByTrigonometric(Point startPoint, double angle, double distance)
        {
            //角度转弧度
            var radian = (angle * Math.PI) / 180;

            //计算新坐标 r 就是两者的距离
            Point endPoint = new Point(startPoint.X + distance * Math.Cos(radian), startPoint.Y + distance * Math.Sin(radian));

            return endPoint;
        }

        /// <summary>
        /// 通过三角函数求终点坐标
        /// </summary>
        /// <param name="angle">角度</param>
        /// <param name="startPoint">起点</param>
        /// <param name="distance">距离</param>
        /// <returns>终点坐标</returns>
        public static Point GetEndPointByDirection(Point startPoint, double direction, double distance)
        {
            return GetEndPointByTrigonometric(startPoint, 90 - direction, distance);
        }

        public static double GetAngle(Point startPoint, Point endPoint)
        {
            var radian = (endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);
            var angle = radian * 180 / Math.PI;
            return angle;
        }

        public static double GetDirection(Point startPoint, Point endPoint)
        {

            var radian = Math.Atan2(endPoint.Y - startPoint.Y, endPoint.X - startPoint.X);
            var angle = radian * 180 / Math.PI;


            return ConvertToPositive360(90 - angle);

        }

        public static double ConvertToPositive360(double angle)
        {
            double positiveAngle = angle % 360;
            if (positiveAngle < 0)
            {
                positiveAngle += 360;
            }
            return positiveAngle;
        }

        public static Point RotatePoint(Point point, Point center, double angle)
        {
            var x = (point.X - center.X) * Math.Cos(Math.PI / 180.0 * angle) - (point.Y - center.Y) * Math.Sin(Math.PI / 180.0 * angle) + center.X;
            var y = (point.X - center.X) * Math.Sin(Math.PI / 180.0 * angle) + (point.Y - center.Y) * Math.Cos(Math.PI / 180.0 * angle) + center.Y;
            return new Point(x, y);
        }
    }
}
