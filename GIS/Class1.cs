using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GIS
{
    class Class1
    {
        class PointsClass
        {
            const int MaxSize = 100;
            public int[] vers;
            public int length;
            public double sum = 0;
            public PointsClass()
            {
                vers = new int[MaxSize];
                length = 0;


            }

            public bool CreateList(string[] split)
            {
                int i;
                for (i = 0; i < split.Length; i++)
                    try
                    { vers[i] = Convert.ToInt32(split[i]); }
                    catch (Exception err)
                    { return false; }
                length = i;
                return true;
            }
            public void Addver(int v, double edgelen)
            {
                vers[length] = v;
                length++;
                sum += edgelen;


            }
            public void Declength(double edgelen)
            {
                length--;
                sum -= edgelen;
            }
            public void Copy(PointsClass ps)
            {
                int i;
                for (i = 0; i < length; i++)
                    ps.vers[i] = vers[i];
                ps.length = length;
                ps.sum = sum;
            }
            public string Disp()
            {
                string mystr = ""; int i;
                if (length > 0)
                    mystr += vers[0].ToString();
                for (i = 1; i < length; i++)
                    mystr += "》》" + vers[i].ToString();
                mystr += "   总长度：" + sum.ToString();
                return mystr;
            }
            public bool allIn(PointsClass p)
            {
                int i, j;
                for (i = 0; i < p.length; i++)
                {
                    for (j = 0; j < length; j++)
                        if (p.vers[i] == vers[j])
                            break;
                    if (j == length)
                        return false;
                }
                return true;
            }
            public bool allnotIn(PointsClass p)
            {
                int i, j;
                for (i = 0; i < p.length; i++)
                {
                    for (j = 0; j < length; j++)
                        if (p.vers[i] == vers[j])
                            return false;
                }
                return true;
            }
        }
        class ArcNode
        {
            public int adjvex;
            public ArcNode nextarc;
            public double edgelength;

        };
        struct VNode
        {
            public string data;
            public ArcNode firstarc;
        };
        struct MAP
        {
            public VNode[] adjlist;
            public int n, e;
        };

        class GISClass
        {
            const double INF = 32767;
            const int MAXV = 100;
            MAP map = new MAP();
            PointsClass apath = new PointsClass();
            PointsClass minpath = new PointsClass();
            int[] visited;
            double[,] a;
            string pathstr;
            int count = 0;

            public GISClass()
            {   int i,j;
                minpath.sum = INF;
                map.adjlist=new VNode[MAXV];
                a = new double[MAXV,MAXV];
                visited=new int[MAXV];
                for (i=0;i<MAXV;i++)
                    for(j=0;j<MAXV;j++)
                        if(i==j) a[i,j]= 0;
                        else a[i,j] = INF;
                a[0, 2] = a[2, 0] = 2.5;
                a[0, 7] = a[7, 0] = 3.2;
                a[1, 2] = a[2, 1] = 2.0;
                a[1, 3] = a[3, 1] = 2.1;
                a[2, 6] = a[6, 2] = 2.6;
                a[3, 4] = a[4, 3] = 1.2;
                a[4, 6] = a[6, 4] = 0.6;
                a[4, 5] = a[5, 4] = 1.8;
                a[5, 10] = a[10, 5] = 1.8;
                a[5, 9] = a[9, 5] = 2.0;
                a[6, 8] = a[8, 6] = 0.8;
                a[7, 8] = a[8, 7] = 1.2;
                a[8, 9] = a[9, 8] = 1.2;
                a[9, 11] = a[11, 9] = 1.8;
                a[10, 12] = a[12, 10] = 2.2;
                a[10, 11] = a[11, 10] = 2.0;
                a[11, 16] = a[16, 11] = 1.8;
                a[12, 13] = a[13, 12] = 2.5;
                a[12, 15] = a[15, 12] = 2.5;
                a[13, 14] = a[14, 13] = 1.6;
                a[14, 15] = a[15, 14] = 2.2;
                a[15, 17] = a[17, 15] = 2.4;
                a[15, 18] = a[18, 15] = 2.6;
                a[16, 17] = a[17, 16] = 1.2;
                a[16, 19] = a[19, 16] = 2.3;
                a[17, 18] = a[18, 17] = 2.5;
                a[17, 20] = a[20, 17] = 2.7;
                a[19, 20] = a[20, 19] = 2.9;
                map.n = 21; map.e = 56;
                CreateMap();
                for (i = 0; i < map.n; i++)
                    visited[i] = 0;


            }
            private void CreateMap()
            {
                int i, j; ArcNode p;
                for (i = 0; i < map.n; i++)
                    map.adjlist[i].firstarc = null;
                for (i = 0; i < map.n; i++)
                    for (j = map.n - 1; j >= 0; j--)
                        if (a[i, j] != 0 && a[i, j] != INF)
                        {
                            p = new ArcNode();
                            p.adjvex = j;
                            p.edgelength = a[i, j];
                            p.nextarc = map.adjlist[i].firstarc;
                            map.adjlist[i].firstarc = p;
                        }
            }
            public string DispAdj()
            {
                string mystr = ""; int i;
                ArcNode p;
                for (i = 0; i < map.n; i++)
                {
                    p = map.adjlist[i].firstarc;
                    mystr += string.Format("{0,3}:", i);
                    if (p != null)
                    {
                        mystr += string.Format("{0,3}({1:f1})", p.adjvex, p.edgelength);
                        p = p.nextarc;
                    }
                    while (p != null)
                    {
                        mystr += string.Format("》》{0,3}({1:f1})", p.adjvex, p.edgelength);
                        p = p.nextarc;
                    }
                    mystr += "\r\n";
                }

                return mystr;
            }

            private bool Cond(PointsClass ps1, PointsClass ps2)
            {
                return (apath.allIn(ps1) && apath.allIn(ps2));
            }
            public string TravPath(int start, int end, PointsClass ps1, PointsClass ps2)
            {
                pathstr = "";
                TravPath1(start, end, 0, ps1, ps2);
                return pathstr;
            }
            private void TravPath1(int start, int end, double edgelen, PointsClass ps1, PointsClass ps2)
            {
                int v, i; ArcNode p;
                visited[start] = 1;
                apath.Addver(start, edgelen);
                if (start == end && Cond(ps1, ps2))
                {
                    count++;
                    pathstr += "路径" + count.ToString() + ":";
                    pathstr += apath.Disp() + "\r\n";
                    if (apath.sum < minpath.sum)
                        apath.Copy(minpath);
                }
                p = map.adjlist[start].firstarc;
                while (p != null)
                {
                    v = p.adjvex;
                    if (visited[v] == 0)
                    {
                        TravPath1(v, end, p.edgelength, ps1, ps2);
                        apath.Declength(p.edgelength);
                    }
                    p = p.nextarc;
                }
                visited[start] = 0;
            }
            public string DispminPath()
            {
                if (count > 0)
                    return minpath.Disp();
                else
                    return "找不到任何路径";
            }

        }

    }
}
