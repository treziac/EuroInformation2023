using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

/*******
* Read input from Console
* Use: Console.WriteLine to output your result to STDOUT.
* Use: Console.Error.WriteLine to output debugging information to STDERR;
*       
* ***/

namespace EuroInformation2023
{
    static class Program
    {
        static void Exo1(string[] args)
        {
            var aS = Console.ReadLine();
            var bS = Console.ReadLine();
            var a = aS.Split(' ').Skip(1).Select(int.Parse).ToArray();
            var b = bS.Split(' ').Skip(1).Select(int.Parse).ToArray();

            int a1 = a[0] * (a[1] + a[2]);
            int b1 = b[0] * (b[1] + b[2]);
            Console.WriteLine(((a1 < b1) ? aS : bS).Split(' ').First());
        }


        static void Exo2(string[] args)
        {
            var x = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            Dictionary<string, List<string>> dico = new Dictionary<string, List<string>>();
            int M = x[0];
            int C = x[1];

            for (int i = 0; i < C; i++)
            {
                var m = Console.ReadLine().Split(' ').ToArray();
                if (!dico.TryGetValue(m[0], out var list))
                {
                    dico[m[0]] = list = new List<string>();
                }
                list.Add(m[1]);
            }

            var a = dico.Single(y => y.Value.Count == M - 2).Key;
            Console.WriteLine(dico.Single(y => y.Value.Count == 1 && y.Value.SingleOrDefault() == a).Key);

        }


        static int lcm(params int[] numbers)
        {
            int g = numbers[0];
            foreach (var number in numbers)
                g = lcm(g, number);
            return g;
        }

        static int gcf(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static int lcm(int a, int b)
        {
            return (a / gcf(a, b)) * b;
        }

        static void Exo3(string[] args)
        {
            int N = int.Parse(Console.ReadLine());
            var n = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();

            int index = -1;
            int max = lcm(n);

            for (int i = 0; i < N; i++)
            {
                if (i != 0)
                    n[i - 1]--;
                n[i]++;
                int nmax = lcm(n);
                if (nmax > max)
                {
                    max = nmax;
                    index = i;
                }
            }
            Console.WriteLine(index);
        }

        public class Segment
        {
            public Segment(int id, long start, long end)
            {
                Start = start; End = end; Id = id;
            }
            public long Start { get; set; }
            public long End { get; set; }
            public int Id { get; set; }

        }

        static void Exo5(string[] args)
        {

            var par = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            int P = par[0];
            long pow = (long)Math.Pow(10, P);
            int S = par[1];
            int R = par[2];

            LinkedList<Segment> segments = new LinkedList<Segment>();
            segments.AddFirst(new Segment(-1, 0, pow));
            for (int i = 0; i < R; i++)
            {
                var req = Console.ReadLine().Split(' ').Select(x => (double.Parse(x, CultureInfo.InvariantCulture))).ToArray();
                int id = (int)req[0];
                long start = (long)(req[1]*pow);
                long end = (long)(req[2]*pow);
                long type = (int)req[3];
                //alloc
                var segment = Nodes(segments).First(x => x.Value.Start <= start && x.Value.End >= start);
                {
                    do
                    {
                        if (type == 1 && segment.Value.Id == -1)
                        {
                            var prevEnd = segment.Value.End;
                            //may be a null segment, don't care
                            segment.Value.End = start;

                            segments.AddAfter(segment, new Segment(id, start, Math.Min(end, prevEnd)));

                            segment = segment.Next;

                            if (segment.Previous.Value.Start == segment.Previous.Value.End)
                            {
                                segments.Remove(segment.Previous);
                            }
                            if (end < prevEnd)
                            {
                                segments.AddAfter(segment, new Segment(-1, end, prevEnd));
                                segment = segment.Next;
                            }
                            start = prevEnd;
                        }

                        else if (type == 0 && segment.Value.Id == id)
                        {
                            var prevEnd = segment.Value.End;
                            //may be a null segment, don't care
                            segment.Value.End = start;

                            segments.AddAfter(segment, new Segment(-1, start, Math.Min(end, prevEnd)));

                            segment = segment.Next;
                            if (segment.Previous.Value.Start == segment.Previous.Value.End)
                            {
                                segments.Remove(segment.Previous);
                            }

                            if (end < prevEnd)
                            {
                                segments.AddAfter(segment, new Segment(id, end, prevEnd));
                                segment = segment.Next;
                            }
                            start = prevEnd;
                        }
                        segment = segment.Next;
                        if(segment != null)
                            start = segment.Value.Start;
                    } while (segment != null && segment.Value.Start < end);
                }
            }

            Dictionary<int, long> dico= new Dictionary<int, long>();
            for(int i=0; i<S; i++)
            {
                dico[i] = 0;
            }
            foreach(var segment in segments)
            {
                if (segment.Id != -1)
                    dico[segment.Id] += segment.End - segment.Start;
            }

            var max = dico.Max(x => x.Value);
            Console.WriteLine(dico.Single(x => x.Value == max).Key);
        }

        public static IEnumerable<LinkedListNode<T>> Nodes<T>(this LinkedList<T> list)
        {
            for (var node = list.First; node != null; node = node.Next)
            {
                yield return node;
            }
        }

        //
        //static void Exo4(string[] args)
        //{
        //    // taken from https://www.nayuki.io/page/smallest-enclosing-circle
        //
        //    int N = int.Parse(Console.ReadLine());
        //    List<Point> points = new List<Point>();
        //    for (int i = 0; i < N; i++)
        //    {
        //        var t = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
        //        points.Add(new Point(t[0], t[1]));

        //    }
        //    Console.WriteLine((Math.PI * Math.Pow(SmallestEnclosingCircle.MakeCircle(points).r, 2)).ToString("0.00000"));

        //}
    }
}
    

//    public sealed class SmallestEnclosingCircle
//    {

//        /* 
//         * Returns the smallest circle that encloses all the given points. Runs in expected O(n) time, randomized.
//         * Note: If 0 points are given, a circle of radius -1 is returned. If 1 point is given, a circle of radius 0 is returned.
//         */
//        // Initially: No boundary points known
//        public static Circle MakeCircle(IList<Point> points)
//        {
//            // Clone list to preserve the caller's data, do Durstenfeld shuffle
//            List<Point> shuffled = new List<Point>(points);
//            Random rand = new Random();
//            for (int i = shuffled.Count - 1; i > 0; i--)
//            {
//                int j = rand.Next(i + 1);
//                Point temp = shuffled[i];
//                shuffled[i] = shuffled[j];
//                shuffled[j] = temp;
//            }

//            // Progressively add points to circle or recompute circle
//            Circle c = Circle.INVALID;
//            for (int i = 0; i < shuffled.Count; i++)
//            {
//                Point p = shuffled[i];
//                if (c.r < 0 || !c.Contains(p))
//                    c = MakeCircleOnePoint(shuffled.GetRange(0, i + 1), p);
//            }
//            return c;
//        }


//        // One boundary point known
//        private static Circle MakeCircleOnePoint(List<Point> points, Point p)
//        {
//            Circle c = new Circle(p, 0);
//            for (int i = 0; i < points.Count; i++)
//            {
//                Point q = points[i];
//                if (!c.Contains(q))
//                {
//                    if (c.r == 0)
//                        c = MakeDiameter(p, q);
//                    else
//                        c = MakeCircleTwoPoints(points.GetRange(0, i + 1), p, q);
//                }
//            }
//            return c;
//        }


//        // Two boundary points known
//        private static Circle MakeCircleTwoPoints(List<Point> points, Point p, Point q)
//        {
//            Circle circ = MakeDiameter(p, q);
//            Circle left = Circle.INVALID;
//            Circle right = Circle.INVALID;

//            // For each point not in the two-point circle
//            Point pq = q.Subtract(p);
//            foreach (Point r in points)
//            {
//                if (circ.Contains(r))
//                    continue;

//                // Form a circumcircle and classify it on left or right side
//                double cross = pq.Cross(r.Subtract(p));
//                Circle c = MakeCircumcircle(p, q, r);
//                if (c.r < 0)
//                    continue;
//                else if (cross > 0 && (left.r < 0 || pq.Cross(c.c.Subtract(p)) > pq.Cross(left.c.Subtract(p))))
//                    left = c;
//                else if (cross < 0 && (right.r < 0 || pq.Cross(c.c.Subtract(p)) < pq.Cross(right.c.Subtract(p))))
//                    right = c;
//            }

//            // Select which circle to return
//            if (left.r < 0 && right.r < 0)
//                return circ;
//            else if (left.r < 0)
//                return right;
//            else if (right.r < 0)
//                return left;
//            else
//                return left.r <= right.r ? left : right;
//        }


//        public static Circle MakeDiameter(Point a, Point b)
//        {
//            Point c = new Point((a.x + b.x) / 2, (a.y + b.y) / 2);
//            return new Circle(c, Math.Max(c.Distance(a), c.Distance(b)));
//        }


//        public static Circle MakeCircumcircle(Point a, Point b, Point c)
//        {
//            // Mathematical algorithm from Wikipedia: Circumscribed circle
//            double ox = (Math.Min(Math.Min(a.x, b.x), c.x) + Math.Max(Math.Max(a.x, b.x), c.x)) / 2;
//            double oy = (Math.Min(Math.Min(a.y, b.y), c.y) + Math.Max(Math.Max(a.y, b.y), c.y)) / 2;
//            double ax = a.x - ox, ay = a.y - oy;
//            double bx = b.x - ox, by = b.y - oy;
//            double cx = c.x - ox, cy = c.y - oy;
//            double d = (ax * (by - cy) + bx * (cy - ay) + cx * (ay - by)) * 2;
//            if (d == 0)
//                return Circle.INVALID;
//            double x = ((ax * ax + ay * ay) * (by - cy) + (bx * bx + by * by) * (cy - ay) + (cx * cx + cy * cy) * (ay - by)) / d;
//            double y = ((ax * ax + ay * ay) * (cx - bx) + (bx * bx + by * by) * (ax - cx) + (cx * cx + cy * cy) * (bx - ax)) / d;
//            Point p = new Point(ox + x, oy + y);
//            double r = Math.Max(Math.Max(p.Distance(a), p.Distance(b)), p.Distance(c));
//            return new Circle(p, r);
//        }

//    }



//    public struct Circle
//    {

//        public static readonly Circle INVALID = new Circle(new Point(0, 0), -1);

//        private const double MULTIPLICATIVE_EPSILON = 1 + 1e-14;


//        public Point c;   // Center
//        public double r;  // Radius


//        public Circle(Point c, double r)
//        {
//            this.c = c;
//            this.r = r;
//        }


//        public bool Contains(Point p)
//        {
//            return c.Distance(p) <= r * MULTIPLICATIVE_EPSILON;
//        }


//        public bool Contains(ICollection<Point> ps)
//        {
//            foreach (Point p in ps)
//            {
//                if (!Contains(p))
//                    return false;
//            }
//            return true;
//        }

//    }



//    public struct Point
//    {

//        public double x;
//        public double y;


//        public Point(double x, double y)
//        {
//            this.x = x;
//            this.y = y;
//        }


//        public Point Subtract(Point p)
//        {
//            return new Point(x - p.x, y - p.y);
//        }


//        public double Distance(Point p)
//        {
//            double dx = x - p.x;
//            double dy = y - p.y;
//            return Math.Sqrt(dx * dx + dy * dy);
//        }


//        // Signed area / determinant thing
//        public double Cross(Point p)
//        {
//            return x * p.y - y * p.x;
//        }

//    }
//}