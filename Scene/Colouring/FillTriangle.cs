using System;
using System.Collections.Generic;
using System.Numerics;

namespace Scene
{
    // calculations are optimalized for triangular faces
    // ---------------------------------------------------------//
    // node representing the element in ET and AET list
    class Node
    {
        public int ymax;
        public double xmin;
        public double m;
        public Node next;
        
        public Node(int ymax1, int ymax2, int xmin1, int xmin2, double m)
        {
            if(ymax2 < ymax1)
            {
                this.ymax = ymax1;
                this.xmin = xmin2;
            }
            else
            {
                this.ymax = ymax2;
                this.xmin = xmin1;
            }
            this.m = m;
            next = null;
        }
    }
    // ---------------------------------------------------------//

    class FillTriangle
    {
        private Node[] buckets;
        ChoosePixelColor chosecolor;
        LockBitMap locked;
        private ZBuffer zBuffer;
        // ---------------------------------------------------------//
        // creation and update //
        public FillTriangle(int count, ChoosePixelColor choose, LockBitMap locked, ZBuffer zBuffer)// the count of buckets is the same as height in Canvas
        {
            buckets = new Node[count];
            chosecolor = choose;
            this.locked = locked;
            this.zBuffer = zBuffer;
        }
        // ---------------------------------------------------------//
        // SCAN-LINE ALGORITHM //
        // buckets sorting //
        public void FillSpace(Vector4[]  Vertices, List<(int, int, int)> Triangles, Vector3 CameraPosition, Vector4 ReflectorNormal, Vector4 PR, bool isFog)
        {
            locked.LockBits();
            // logaritmic transformation of Z value
            for (int i =0; i< Vertices.Length; i++)
            {
                Vertices[i].Z = (float)Math.Log2((double)Vertices[i].Z);
            }

            (int, int, float) v1, v2, v3;
            int min, temp;
            List<int> index = new List<int>();
           
            foreach (var f in Triangles)
            {
                index.Clear();
                // ---------------------------- //
                // this code only to read vertices
                v1 = ((int)Vertices[f.Item1 - 1].X, (int)Vertices[f.Item1 - 1].Y, Vertices[f.Item1 - 1].Z);
                v2 = ((int)Vertices[f.Item2 - 1].X, (int)Vertices[f.Item2 - 1].Y, Vertices[f.Item2 - 1].Z);
                v3 = ((int)Vertices[f.Item3 - 1].X, (int)Vertices[f.Item3 - 1].Y, Vertices[f.Item3 - 1].Z);
                // ---------------------------- //
                if (!(v1.Item1 == v2.Item1 && v1.Item2 == v2.Item2) && !(v1.Item1 == v3.Item1 && v1.Item2 == v3.Item2) && !(v3.Item1 == v2.Item1 && v3.Item2 == v2.Item2))
                {
                    chosecolor.NewVertices(CameraPosition,f.Item1 - 1, f.Item2 - 1, f.Item3 - 1, (v1.Item1, v1.Item2), (v2.Item1, v2.Item2), (v3.Item1, v3.Item2),  ReflectorNormal,  PR, isFog, CameraPosition);
                    // SCAN-LINE ALGORITHM //
                    // adding edges to ET - AddEdge function add edges
                    temp = AddEdge(v1, v2);
                    if (temp != -1) index.Add(temp);
                    temp = AddEdge(v2, v3);
                    if (!index.Contains(temp) && temp != -1) index.Add(temp);
                    temp = AddEdge(v1, v3);
                    if (!index.Contains(temp) && temp != -1) index.Add(temp);

                    // AET
                    Node tempnode, AET = null;
                    if (index.Count != 0)
                    {
                        min = index[0];
                        foreach (var i in index)
                            min = Math.Min(min, i);
                        // AET is empty in the beginning
                        {
                            AET = buckets[min];
                            AET = SortAET(AET);
                            buckets[min] = null;
                            //filling the pixels
                            FillPixels(AET, min, locked, Vertices[f.Item1 - 1], Vertices[f.Item2 - 1], Vertices[f.Item3 - 1]);
                            // next scan-line
                            min++;
                            tempnode = AET;
                            while (tempnode != null)
                            {
                                tempnode.xmin += tempnode.m;
                                tempnode = tempnode.next;
                            }
                            AET = SortAET(AET);
                        }

                        do
                        {
                            if (buckets[min] != null)
                            {
                                // adding ET to the list AET
                                AET = AddToAET(AET, min);
                                // sorting by x parameter
                                AET = SortAET(AET);
                            }
                            
                            //filling the pixels
                            FillPixels(AET, min, locked, Vertices[f.Item1 - 1], Vertices[f.Item2 - 1], Vertices[f.Item3 - 1]);

                            // removing edges for witch min == ymax - for triangles this is finish of calculations
                            if (AET.ymax == min)
                                break;
                                
                            // next scan-line
                            min++;
                            tempnode = AET;
                            while (tempnode != null)
                            {
                                tempnode.xmin += tempnode.m;
                                tempnode = tempnode.next;
                            }

                        } while (AET != null);
                    }
                }
            }
            locked.UnlockBits();
            return;
        }
        private int AddEdge((int, int, double) v1, (int, int, double) v2)
        {
            if (v1.Item2 != v2.Item2)
            {
                double m = (double)(v1.Item1 - v2.Item1) / (double)(v1.Item2 - v2.Item2);
                Node n = new Node(v1.Item2, v2.Item2, v1.Item1, v2.Item1, m);

                if (v1.Item2 < v2.Item2)
                {
                    if (buckets[v1.Item2] != null)
                        buckets[v1.Item2].next = n;
                    else
                        buckets[v1.Item2] = n;

                    return v1.Item2;
                }
                else
                {
                    if (buckets[v2.Item2] != null)
                        buckets[v2.Item2].next = n;
                    else
                        buckets[v2.Item2] = n;
                    return v2.Item2;
                }
            }
            return -1;
        }
        private Node AddToAET(Node AET, int min)
        {
            // only for triangles!
            if(Math.Abs(AET.xmin - buckets[min].xmin) < 1 && AET.ymax == min)
            {
                buckets[min].next = AET.next;
                AET = buckets[min];
            }
            else
            {
                AET.next = buckets[min];
            }
            buckets[min] = null; 
            
            return AET;
           
        }
        private Node SortAET(Node AET)
        {
            // only for triangles, NEW CODE!
            Node head = AET;
            Node tempnode;
            if (AET.next != null)
            {
                if (AET.next.xmin < AET.xmin)
                {
                    tempnode = AET.next;
                    AET.next = null;
                    tempnode.next = AET;
                    head = tempnode;
                }
            }
            return head;
        }
        private void FillPixels(Node AET, int min, LockBitMap locked, Vector4 v1, Vector4 v2, Vector4 v3)
        {
            Vector4 v;
            double temp;
            double s, s1, s2, s3;
            // calculation of Z value in Z-Buffer for pixels inside traingle
            s = Heron(v1, v2, v3); // it doesn't change
            v = new Vector4((float)AET.xmin, min, 0, 1);
            s1 = Heron(v, v3, v2);
            s2 = Heron(v, v1, v3);
            s3 = Heron(v, v1, v2);

            // this if clause is because of the fact that triangles are small
            {
                if (s1 >= s)
                    temp = v1.Z;
                else if (s2 >= s)
                    temp = v2.Z;
                else if (s3 >= s)
                    temp = v3.Z;
                else if (s1 + s2 > s)
                {
                    s2 = s - s1;
                    temp = (v1.Z * s1 / s + v2.Z * s2 / s);
                }
                else if (s1 + s3 > s)
                {
                    s1 = s - s3;
                    temp = (v1.Z * s1 / s + v3.Z * s3 / s);
                }
                else if (s3 + s2 > s)
                {
                    s3 = s - s2;
                    temp = (v2.Z * s2 / s + v3.Z * s3 / s);
                }
                else
                    temp = (v1.Z * s1 / s + v2.Z * s2 / s + v3.Z * s3 / s);
            }
            // at first I am checking is pixel visible
            if (zBuffer.IsInFront(((int)AET.xmin, min, temp)))
                locked.SetPixel((int)AET.xmin, min, chosecolor.PixelOnEdgeColor(((int)AET.xmin, min)));

            for (int i = (int)AET.xmin+1; i <= AET.next.xmin; i++)
            {
                // calculation of Z value in Z-Buffer for pixels inside traingle
                v.X += 1;
                s1 = Heron(v, v3, v2);
                s2 = Heron(v, v1, v3);  
                s3 = Heron(v, v1, v2);
                // this if clause is because of the fact that triangles are small
                {
                    if (s1 > s)
                        temp = v1.Z;
                    else if (s2 > s)
                        temp = v2.Z;
                    else if (s3 > s)
                        temp = v3.Z;
                    else if (s1 + s2 > s)
                    {
                        s2 = s - s1;
                        temp = (v1.Z * s1 / s + v2.Z * s2 / s);
                    }
                    else if (s1 + s3 > s)
                    {
                        s1 = s - s3;
                        temp = (v1.Z * s1 / s + v3.Z * s3 / s);
                    }
                    else if (s3 + s2 > s)
                    {
                        s3 = s - s2;
                        temp = (v2.Z * s2 / s + v3.Z * s3 / s);
                    }
                    else
                        temp = (v1.Z * s1 / s + v2.Z * s2 / s + v3.Z * s3 / s);
                }
                // at first I am checking is pixel visible
                if (zBuffer.IsInFront((i,min,temp)))
                    locked.SetPixel(i, min, chosecolor.PixelColor((i,min)));
            }
        }
        // ---------------------------------------------------------//
        // function to calculate field of a triangle v1v2v3 //

        private double Heron(Vector4 v1, Vector4 v2, Vector4 v3)
        {
            double a = Math.Sqrt((v1.X - v2.X) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v1.Y - v2.Y));
            double b = Math.Sqrt((v1.X - v3.X) * (v1.X - v3.X) + (v1.Y - v3.Y) * (v1.Y - v3.Y));
            double c = Math.Sqrt((v3.X - v2.X) * (v3.X - v2.X) + (v3.Y - v2.Y) * (v3.Y - v2.Y));
            double p = (a + b + c) / 2;
            double P = p * (p - a) * (p - b) * (p - c);
            if (P < 0) return 0;
            return Math.Sqrt(P);
        }
    }
}
