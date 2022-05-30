using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Project
{
    class Program
    {
        public static void GetAndBuild(out Point[] graphF, out double[][] relMatr, string path)
        {
            string[] text;
            bool nextP = false;
            List<int> crossP = new List<int>();
            List<Point> graph = new List<Point>();

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    text = sr.ReadToEnd().Replace("\r", "").Replace("\n", "").Split(';');
                }
            }
            if (text[0] == "p") graph.Add(new Point(float.Parse(text[1]), float.Parse(text[2]), int.Parse(text[3]), int.Parse(text[4])));
            else graph.Add(new Point(float.Parse(text[1]), float.Parse(text[2]), int.Parse(text[3]), int.Parse(text[4]), true));

            for (int i = 5; i < text.Length - 1; i += 5)
            {
                if ("p" == text[i])
                {
                    if (nextP)
                    {
                        nextP = false;
                        Point c = graph[crossP[crossP.Count - 1]];
                        graph.Add(new Point(float.Parse(text[i + 1]), float.Parse(text[i + 2]), int.Parse(text[i + 3]), ref c, int.Parse(text[i + 4])));

                        c = graph[graph.Count - 1];
                        graph[crossP[crossP.Count - 1]].SetRelPoint(graph[crossP[crossP.Count - 1]].GetRelPointInd(), ref c);

                        if (graph[crossP[crossP.Count - 1]].GetRelPointInd() == -1)
                        {
                            crossP.RemoveAt(crossP.Count - 1);
                        }

                        if (int.Parse(text[i + 3]) > 2)
                        {
                            crossP.Add(graph.Count - 1);
                        }
                        else if (int.Parse(text[i + 3]) == 1)
                        {
                            nextP = true;
                        }
                    }
                    else
                    {
                        Point c = graph[graph.Count - 1];
                        graph.Add(new Point(float.Parse(text[i + 1]), float.Parse(text[i + 2]), int.Parse(text[i + 3]), ref c, int.Parse(text[i + 4])));

                        c = graph[graph.Count - 1];
                        graph[graph.Count - 2].SetRelPoint(graph[graph.Count - 2].GetRelPointInd(), ref c);

                        if (int.Parse(text[i + 3]) > 2)
                        {
                            crossP.Add(graph.Count - 1);
                        }
                        else if (int.Parse(text[i + 3]) == 1)
                        {
                            nextP = true;
                        }
                    }
                }
                else if (text[i] == "s")
                {
                    if (nextP)
                    {
                        Point c = graph[crossP[crossP.Count - 1]];
                        graph.Add(new Point(float.Parse(text[i + 1]), float.Parse(text[i + 2]), int.Parse(text[i + 3]), ref c, int.Parse(text[i + 4]), true));

                        c = graph[graph.Count - 1];
                        graph[crossP[crossP.Count - 1]].SetRelPoint(graph[crossP[crossP.Count - 1]].GetRelPointInd(), ref c);

                        if (graph[crossP[crossP.Count - 1]].GetRelPointInd() == -1)
                        {
                            crossP.RemoveAt(crossP.Count - 1);
                        }
                    }
                    else
                    {
                        Point c = graph[graph.Count - 1];
                        graph.Add(new Point(float.Parse(text[i + 1]), float.Parse(text[i + 2]), int.Parse(text[i + 3]), ref c, int.Parse(text[i + 4]), true));

                        c = graph[graph.Count - 1];
                        graph[graph.Count - 2].SetRelPoint(graph[graph.Count - 2].GetRelPointInd(), ref c);
                        nextP = true;
                    }
                }
            }
            graphF = new Point[graph.Count];
            graph.CopyTo(graphF);

            relMatr = new double[graphF.Length][];
            for (int i = 0; i < graphF.Length; i++)
            {
                relMatr[i] = new double[graphF.Length];
                relMatr[i][i] = 0;
                for (int j = 0; j < graphF.Length; j++)
                {
                    relMatr[i][j] = double.MaxValue;
                    if (graph[i].GetRelPoints().Contains(graph[j]))
                    {
                        relMatr[i][j] = Point.DistBetween(graph[i], graph[j]);
                    }
                }
            }
        }
        public static void PrintGraph(Point[] p)
        {
            Console.WriteLine();
            foreach (var a in p)
            {
                Console.WriteLine(a.ToString());
            }
            Console.WriteLine();
        }
        
        // Прошлый вариант поиска, можно сносить
        public static Point[] FindPath(Point begP, Point toP)
        {
            List<Point> finPath = new List<Point>();
            List<int> crossP = new List<int>();
            List<bool[]> pRel = new List<bool[]>();
            Point ind = begP;

            finPath.Add(ind);
            if (ind.Equals(toP)) { return finPath.ToArray(); }
            if (ind.GetNumOfRel() > 2)
            {
                crossP.Add(finPath.Count);
                pRel.Add(new bool[ind.GetNumOfRel()]);
                ind = ind.GetRelPoint(Array.IndexOf(pRel[pRel.Count - 1], false));
                pRel[pRel.Count - 1][Array.IndexOf(pRel[pRel.Count - 1], false)] = true;
            }
            else
            {
                pRel.Add(new bool[ind.GetNumOfRel()]);
                ind = ind.GetRelPoint(Array.IndexOf(pRel[pRel.Count - 1], false));
                pRel[pRel.Count - 1][Array.IndexOf(pRel[pRel.Count - 1], false)] = true;
            }
            while (true)
            {
                finPath.Add(ind);
                if (ind.Equals(toP)) { break; }
                pRel.Add(new bool[ind.GetNumOfRel()]);
                for (int i = 0; i < ind.GetNumOfRel(); i++)
                {
                    if (finPath[finPath.Count - 2].Equals(ind.GetRelPoint(i))) { pRel[pRel.Count - 1][i] = true; break; }
                }

                if (ind.GetNumOfRel() > 2)
                {
                    crossP.Add(finPath.Count);
                }
                else if (ind.GetNumOfRel() == 1)
                {
                    while (true)
                    {
                        while (finPath.Count != crossP[crossP.Count - 1])
                        {
                            finPath.RemoveAt(finPath.Count - 1);
                            pRel.RemoveAt(pRel.Count - 1);
                        }
                        if (pRel[pRel.Count - 1].Contains(false)) { ind = finPath[finPath.Count - 1]; break; }
                        crossP.RemoveAt(crossP.Count - 1);
                    }
                }
                var a = Array.IndexOf(pRel[pRel.Count - 1], false);
                ind = ind.GetRelPoint(Array.IndexOf(pRel[pRel.Count - 1], false));
                pRel[pRel.Count - 1][Array.IndexOf(pRel[pRel.Count - 1], false)] = true;
            }
            return finPath.ToArray();
        }
        public static Point[] FindPathSameFloor(Point[] graph, double[][] relMatr, int beg, int fin)
        {
            return RecoverPath(beg, fin, relMatr, Algoriphm(beg, graph.Length, relMatr), graph);
        }
        public static Point[] FindPathDifFloors(Point[] BegF, Point[] FinF, double[][] relMatrBeg, double[][] relMatrFin, int beg, int fin)
        {
            double[] minDistBeg = Algoriphm(beg, BegF.Length, relMatrBeg);
            double[] minDistFin = Algoriphm(fin, FinF.Length, relMatrFin);

            double pathDist = double.MaxValue;
            int stairIndBeg = 0, stairIndFin = 0;
            for (int i = 0; i < BegF.Length; i++)
            {
                if (BegF[i].IsStair())
                {
                    for (int k = 0; k < FinF.Length; k++)
                    {
                        if (FinF[k].IsStair() && FinF[k].Equals(BegF[i]) && pathDist > minDistBeg[i] + minDistFin[k])
                        {
                            pathDist = minDistBeg[i] + minDistFin[k];
                            stairIndBeg = i; stairIndFin = k;
                        }
                    }
                }
            }

            Point[] a = RecoverPath(beg, stairIndBeg, relMatrBeg, minDistBeg, BegF);
            Point[] b = RecoverPath(fin, stairIndFin, relMatrFin, minDistFin, FinF);
            Point[] res = new Point[a.Length + b.Length];
            a.CopyTo(res, 0);
            Array.Reverse(b);
            b.CopyTo(res, a.Length);
            return res;
        }
        public static double[] Algoriphm(int beg, int graphLeng, double[][] relMatr)
        {
            double temp, min;
            int minindex;

            int begin_index = beg;
            double[] minDist = new double[graphLeng];
            bool[] visited = new bool[graphLeng];

            for (int i = 0; i < graphLeng; i++)
            {
                minDist[i] = double.MaxValue;
                visited[i] = false;
            }
            minDist[begin_index] = 0;

            do
            {
                minindex = int.MaxValue;
                min = double.MaxValue;
                for (int i = 0; i < graphLeng; i++)
                {
                    if (!visited[i] && minDist[i] < min)
                    {
                        min = minDist[i];
                        minindex = i;
                    }
                }

                if (minindex != int.MaxValue)
                {
                    for (int i = 0; i < graphLeng; i++)
                    {
                        if (relMatr[minindex][i] > 0)
                        {
                            temp = min + relMatr[minindex][i];
                            if (temp < minDist[i])
                            {
                                minDist[i] = temp;
                            }
                        }
                    }
                    visited[minindex] = true;
                }
            } while (minindex < int.MaxValue);

            return minDist;
        }
        public static Point[] RecoverPath(int begin_index, int final_index, double[][] relMatr, double[] minDist, Point[] graph)
        {
            List<int> ver = new List<int>(); 
            int end = final_index; 
            ver.Add(final_index); 
            int ind = 1; 
            double weight = minDist[end]; 

            while (end != begin_index) 
            {
                for (int i = 0; i < minDist.Length; i++)
                {
                    if (relMatr[i][end] != 0 && relMatr[i][end] != double.MaxValue)   
                    {
                        double t = weight - relMatr[i][end]; 
                        if (t == minDist[i] || (t + 0.01 > minDist[i] && t - 0.01 < minDist[i])) 
                        {
                            weight = t; 
                            end = i;    
                            ver.Add(i); 
                            ind++;
                            if (end == begin_index) break;
                        }
                    }
                }
            }
            List<Point> p = new List<Point>();
            foreach (var g in ver)
            {
                p.Add(graph[g]);
            }
            p.Reverse();
            return p.ToArray();
        }
        static void Main(string[] args)
        {
            Point[] graph1F;
            Point[] graph2F;
            Point[] graph3F;
            Point[] graph4F;
            double[][] relMatr1F;
            double[][] relMatr2F;
            double[][] relMatr3F;
            double[][] relMatr4F;

            GetAndBuild(out graph1F, out relMatr1F, "Points1F.txt");
            GetAndBuild(out graph2F, out relMatr2F, "Points2F.txt");
            GetAndBuild(out graph3F, out relMatr3F, "Points3F.txt");
            GetAndBuild(out graph4F, out relMatr4F, "Points4F.txt");
            
            PrintGraph(graph1F);
            PrintGraph(graph2F);
            PrintGraph(graph3F);
            PrintGraph(graph4F);
            var t = FindPathSameFloor(graph1F.ToArray(), relMatr1F, 6, 2);
            foreach (var a in t)
            {
                Console.WriteLine(a);
            }
            Console.WriteLine();

            t = FindPathDifFloors(graph1F.ToArray(), graph2F.ToArray(), relMatr1F, relMatr2F, 6, 6);
            foreach (var a in t)
            {
                Console.WriteLine(a);
            }

            Console.Read();
        }
    }
}
