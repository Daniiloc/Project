using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Point
    {
        protected readonly float x, y;
        protected readonly int id, relations, floor;
        protected readonly bool stair;
        protected readonly Point[] relPoints;
        private static int idCount;
        public Point() { 
            x = y = 0; id = idCount; 
            idCount++; 
            stair = false;
        }
        public Point(float x1, float y1)
        {
            x = x1;
            y = y1;
            stair = false;
        }
        public Point(float x1, float y1, int rel, int fl)
        {
            x = x1; y = y1; 
            relations = rel; relPoints = new Point[rel];
            floor = fl;
            id = idCount + floor * 1000;
            idCount++;
            stair = false;
        }
        public Point(float x1, float y1, int rel, int fl, bool st)
        {
            x = x1; y = y1;
            relations = rel; relPoints = new Point[rel];
            floor = fl;
            id = idCount + floor * 1000;
            idCount++;
            stair = st;
        }
        public Point(float x1, float y1, int rel, ref Point p, int fl) { 
            x = x1; y = y1;
            floor = fl;
            id = idCount + floor * 1000; 
            idCount++;
            relations = rel; relPoints = new Point[rel];
            relPoints[0] = p;
            stair = false;
        }
        public Point(float x1, float y1, int rel, ref Point p, int fl, bool st)
        {
            x = x1; y = y1;
            floor = fl;
            id = idCount + floor * 1000;
            idCount++;
            relations = rel; relPoints = new Point[rel];
            relPoints[0] = p;
            stair = st;
        }
        public float GetX() { return x; }
        public float GetY() { return y; }
        public int GetID() { return id; }
        public int GetFloor() { return floor; }
        public bool IsStair() { return stair; }
        public Point[] GetRelPoints() { return relPoints; }
        public int GetRelPointInd()
        {
            int t = -1;
            for (int i = 0; i < relPoints.Length; i++)
            {
                if (relPoints[i] is null) { t = i; break; }
            }
            return t; 
        }
        public int GetNumOfRel()
        {
            return relPoints.Length;
        }
        public Point GetRelPoint(int ind)
        {
            return relPoints[ind];
        }
        public void SetRelPoint(int ind, ref Point p) { 
            relPoints[ind] = p;
        }
        public static double DistBetween(Point a, Point b) { return Math.Sqrt(Math.Pow(b.x - a.x, 2) + Math.Pow(b.y - a.y, 2)); }
        public Point Copy()
        {
            var a = idCount; 
            idCount = id / 1000 - 1;
            Point b = new Point(x, y, relations, id / 1000, stair); 
            idCount = a;
            for(int i = 0; i < relPoints.Length; i++)
            {
                b.SetRelPoint(i, ref relPoints[i]);
            }
            return b;
        }
        public override bool Equals(object obj)
        {
            if (obj is Point p) return x == p.GetX() && y == p.GetY() && floor == p.GetFloor();
            return false;
        }
        public override int GetHashCode()
        {
            return Convert.ToInt32(Math.Truncate(19 * (x * 0.6180339887 - Math.Truncate(x * 0.6180339887)))) + 
                Convert.ToInt32(Math.Truncate(19 * (y * 0.6180339887 - Math.Truncate(y * 0.6180339887)))); // h(k) = Ц.з(M * Д.ч(k * a))
        }
        public override string ToString()
        {
            string temp = "Relate to: ";
            if (!this.stair)
            {
                foreach (var a in relPoints)
                {
                    temp += a.GetID() + " ";
                }
            }
            else
            {
                temp += relPoints[0].GetID() + " ";
            }
            return String.Format("ID: {0}  X: {1}  Y: {2}  Stair: {3, -5}  Rel: {4}  {5}", id, x, y, stair, relations, temp);
        }
    }
}