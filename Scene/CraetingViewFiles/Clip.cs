using System;
using System.Collections.Generic;
using System.Numerics;

namespace Scene
{
    // function evoke to clip faces which are outside the view of camera and also back faces - which are behind the Object
    public static class Clip
    {
        // function should remove some faces from the Buoy's list of faces
        public static void Cliping(List<(int, int, int)> Triangles, List<(int, int, int)> ConstTriangles, Vector4[] Vertices, Vector3 CameraPosition, Vector3 ObservatedPoint, int FarClipPlane)
        {
            // initialzie Triangles with const ones, because of new scene
            Triangles.Clear();
            for (int i = 0; i < ConstTriangles.Count; i++)
                Triangles.Add( ConstTriangles[i]);

            // cutting faces which are invisible
            CutEdges(Triangles,Vertices, new Vector4(CameraPosition.X, CameraPosition.Y, CameraPosition.Z,0));

            // cutting faces which intersect faces of camera view
            List<Vector4> NewPolygon = new List<Vector4>();
            for (int i= Triangles.Count - 1; i >=0 ;i--)
            {
                NewPolygon.Clear();
                NewPolygon.Add(Vertices[Triangles[i].Item1 - 1]);
                NewPolygon.Add(Vertices[Triangles[i].Item2 - 1]);
                NewPolygon.Add(Vertices[Triangles[i].Item3 - 1]);

                if (ClipEdgeB(NewPolygon, 'e', CameraPosition, ObservatedPoint, FarClipPlane))//east - camera's left clip plane
                    Triangles.RemoveAt(i);
                else if(ClipEdgeB(NewPolygon, 'w', CameraPosition, ObservatedPoint, FarClipPlane)) // west - camera's right clip plane
                    Triangles.RemoveAt(i);
                else if(ClipEdgeB(NewPolygon, 'n', CameraPosition, ObservatedPoint, FarClipPlane)) // north - camera's far clip plane
                    Triangles.RemoveAt(i);
                else if (ClipEdgeB(NewPolygon, 'd', CameraPosition, ObservatedPoint, FarClipPlane)) // down - camera's  down clip plane
                    Triangles.RemoveAt(i);
                else if (ClipEdgeB(NewPolygon, 'u', CameraPosition, ObservatedPoint, FarClipPlane)) // up - camera's up clip plane
                    Triangles.RemoveAt(i);

            }

        }
        // finding intersection by comparing scalar products of clip plane's normal vector and vectors from clip plane to ends of edge (edge is from triangle face)
        private static bool ClipEdgeB(List<Vector4> OldPolygon, char side, Vector3 CameraPosition, Vector3 ObservatedPoint, int FarClipPlane)
        {
            double Xlength;
            Vector3 Uworld, CP, U, X, Nx, G, CG, Norm;
            Vector3 Normal = Vector3.Normalize(ObservatedPoint - CameraPosition);
            Vector3 P = CameraPosition + Normal * FarClipPlane;
            double APosition, BPosition;
            List<Vector4> NewPolygon = new List<Vector4>();
            Vector3 AP, BP;
            switch (side)
            {
                case 'n':
                    {
                        // already done
                        break;
                    }
                case 's':
                    {
                        // the view is very close
                    }
                    break;

                case 'e':
                    Uworld = new Vector3(0, 1, 0);
                    CP = Normal * FarClipPlane;
                    U = Products.CrossProduct(CP, Products.CrossProduct(Uworld, CP));
                    Nx = Vector3.Normalize(Products.CrossProduct(CP, U));
                    Xlength = Math.PI / 8 * CP.Length();
                    X = (int)Xlength * Nx;
                    G = P + X;
                    CG = G - CameraPosition;
                    Norm = Vector3.Normalize(Products.CrossProduct(U, CG));

                    P = G;
                    Normal = -Norm; // Normal is inside

                    break;

                case 'w':
                    Uworld = new Vector3(0, 1, 0);
                    CP = Normal * FarClipPlane;
                    U = Products.CrossProduct(CP, Products.CrossProduct(Uworld, CP));
                    Nx = Vector3.Normalize(Products.CrossProduct(U, CP));
                    Xlength = Math.PI / 8 * CP.Length();
                    X = (int)Xlength * Nx;
                    G = P + X;
                    CG = G - CameraPosition;
                    Norm = Vector3.Normalize(Products.CrossProduct(U, CG));

                    P = G;
                    Normal = Norm; // Normal is inside

                    break;
                case 'd':
                    Uworld = new Vector3(0, 1, 0);
                    CP = Normal * FarClipPlane;
                    U = Products.CrossProduct(Uworld, CP);
                    Nx = Vector3.Normalize(Products.CrossProduct(U, CP));
                    Xlength = Math.PI / 8 * CP.Length();
                    X = (int)Xlength * Nx;
                    G = P + X;
                    CG = G - CameraPosition;
                    Norm = Vector3.Normalize(Products.CrossProduct(U, CG));

                    P = G;
                    Normal = Norm; // Normal is inside

                    break;
                case 'u':
                    Uworld = new Vector3(0, 1, 0);
                    CP = Normal * FarClipPlane;
                    U = Products.CrossProduct(Uworld, CP);
                    Nx = Vector3.Normalize(Products.CrossProduct(CP, U));
                    Xlength = Math.PI / 8 * CP.Length();
                    X = (int)Xlength * Nx;
                    G = P + X;
                    CG = G - CameraPosition;
                    Norm = Vector3.Normalize(Products.CrossProduct(U, CG));

                    P = G;
                    Normal = -Norm; // Normal is inside

                    break;
            }
            for (int i = 0; i < OldPolygon.Count; i++)
            {
                AP = new Vector3(-OldPolygon[i].X + P.X, -OldPolygon[i].Y + P.Y, -OldPolygon[i].Z + P.Z);
                BP = new Vector3(-OldPolygon[(i + 1) % OldPolygon.Count].X + P.X, -OldPolygon[(i + 1) % OldPolygon.Count].Y + P.Y, -OldPolygon[(i + 1) % OldPolygon.Count].Z + P.Z);
                APosition = (Normal.X * AP.X + Normal.Y * AP.Y + Normal.Z * AP.Z);
                BPosition = (Normal.X * BP.X + Normal.Y * BP.Y + Normal.Z * BP.Z);
                if(FindIntersectionB( APosition, BPosition)) return true;
            }
            return false;
        }
        private static bool FindIntersectionB(double APosition, double BPosition)
        {
            if ((APosition < 0 && BPosition > 0) || (APosition > 0 && BPosition < 0) || (APosition < 0 && BPosition < 0) ) // B inside, A outside
                return true;
        
            return false;
        }

        // removing the triangles which are behind the Object
        private static void CutEdges(List<(int, int, int)> Triangles, Vector4[] Vertices, Vector4 CameraPosition)
        {
            Vector4 FromCamera, v0, v1, v;
            float k;
            for (int i = Triangles.Count - 1; i >= 0; i--)
            {
                v0 = Vertices[Triangles[i].Item2-1] - Vertices[Triangles[i].Item1 - 1];
                v1 = Vertices[Triangles[i].Item3 - 1] - Vertices[Triangles[i].Item2 - 1];
                FromCamera = Vertices[Triangles[i].Item2 - 1] - CameraPosition;

                v = Products.CrossProduct(v0,v1);
                k = v.X * FromCamera.X + v.Y * FromCamera.Y + v.Z * FromCamera.Z;
                if (k<=0)
                {
                    Triangles.RemoveAt(i);
                }
            }
        }

       /* // function should cut the boat, not only remove some triangles - so far useless
        public static void ClipingBoat(List<(int,int,int)> Triangles, List<(int, int, int)>  TrianglesNew, Vector4[] Vertices, List<Vector4> TempVert, Vector3 CameraPosition, Vector3 ObservatedPoint, int FarClipPlane)
        {

            TempVert.Clear();
            TrianglesNew.Clear();
            List<(int, int, int)> TrianglesTemp = new List<(int, int, int)>();
            foreach (var T in Triangles)
                TrianglesTemp.Add(T);

            Vector3 FromCameraVector = ObservatedPoint - CameraPosition;
            List<Vector4> NewPolygon = new List<Vector4>();

            //CutEdges(TrianglesTemp, Vertices, FromCameraVector);

            int trianglecount = 0;
            foreach (var T in TrianglesTemp)
             {
                 NewPolygon.Clear();
                 NewPolygon.Add(Vertices[T.Item1 - 1]);
                 NewPolygon.Add(Vertices[T.Item2 - 1]);
                 NewPolygon.Add(Vertices[T.Item3 - 1]);


                 NewPolygon = ClipEdge(NewPolygon, 'e', CameraPosition, ObservatedPoint, FarClipPlane); //east

                 if (NewPolygon.Count != 0)
                     NewPolygon = ClipEdge(NewPolygon, 'w', CameraPosition, ObservatedPoint, FarClipPlane); // west

                 if (NewPolygon.Count != 0)
                     NewPolygon = ClipEdge(NewPolygon, 'n', CameraPosition, ObservatedPoint, FarClipPlane); // north

                 if (NewPolygon.Count != 0)
                     NewPolygon = ClipEdge(NewPolygon, 'd', CameraPosition, ObservatedPoint, FarClipPlane); // down

                 if (NewPolygon.Count != 0)
                     NewPolygon = ClipEdge(NewPolygon, 'u', CameraPosition, ObservatedPoint, FarClipPlane); // up

                 // now list NewPolygon inculde every polygon inside

                 if (NewPolygon.Count != 0)
                 {
                     TempVert.AddRange(NewPolygon);
                     for (int i = 0; i < NewPolygon.Count - 2; i++)
                     {
                         TrianglesNew.Add((trianglecount, trianglecount + i + 1, trianglecount + i + 2));
                     }
                     trianglecount += NewPolygon.Count;
                 }
             }


        }
        private static List<Vector4> ClipEdge(List<Vector4> OldPolygon, char side, Vector3 CameraPosition, Vector3 ObservatedPoint, int FarClipPlane)
        {
            double Xlength;
            Vector3 Uworld, CP, U, X, Nx, G, CG, Norm;
            Vector3 Normal = Vector3.Normalize(ObservatedPoint - CameraPosition);
            Vector3 P = CameraPosition + Normal * FarClipPlane;
            double APosition,BPosition;
            List<Vector4> NewPolygon = new List<Vector4>();
            Vector3 AP, BP;
            switch (side)
            {
                case 'n':
                    {
                        // already done
                        break;
                    }
                case 's':
                    {
                        // the view is very close
                    }
                    break;

                case 'e':
                        Uworld = new Vector3(0, 1, 0);
                        CP = Normal * FarClipPlane;
                        U = Products.CrossProduct(CP, Products.CrossProduct(Uworld, CP));
                        Nx = Vector3.Normalize(Products.CrossProduct(CP, U));
                        Xlength = Math.PI / 8 * CP.Length();
                        X = (int)Xlength * Nx;
                        G = P + X;
                        CG = G - CameraPosition;
                        Norm = Vector3.Normalize(Products.CrossProduct(U, CG));

                        P = G;
                        Normal = -Norm; // Normal is inside
                    
                    break;

                case 'w':
                        Uworld = new Vector3(0, 1, 0);
                        CP = Normal * FarClipPlane;
                        U = Products.CrossProduct(CP, Products.CrossProduct(Uworld, CP));
                        Nx = Vector3.Normalize(Products.CrossProduct(U,CP));
                        Xlength = Math.PI / 8 * CP.Length();
                        X = (int)Xlength * Nx;
                        G = P + X;
                        CG = G - CameraPosition;
                        Norm = Vector3.Normalize(Products.CrossProduct(U, CG));

                        P = G;
                        Normal = Norm; // Normal is inside
                    
                    break;
                case 'd':
                    Uworld = new Vector3(0, 1, 0);
                    CP = Normal * FarClipPlane;
                    U =  Products.CrossProduct(Uworld, CP);
                    Nx = Vector3.Normalize(Products.CrossProduct(U, CP));
                    Xlength = Math.PI / 8 * CP.Length();
                    X = (int)Xlength * Nx;
                    G = P + X;
                    CG = G - CameraPosition;
                    Norm = Vector3.Normalize(Products.CrossProduct(U, CG));

                    P = G;
                    Normal = Norm; // Normal is inside

                    break;
                case 'u':
                    Uworld = new Vector3(0, 1, 0);
                    CP = Normal * FarClipPlane;
                    U = Products.CrossProduct(Uworld, CP);
                    Nx = Vector3.Normalize(Products.CrossProduct(CP, U));
                    Xlength = Math.PI / 8 * CP.Length();
                    X = (int)Xlength * Nx;
                    G = P + X;
                    CG = G - CameraPosition;
                    Norm = Vector3.Normalize(Products.CrossProduct(U, CG));

                    P = G;
                    Normal = -Norm; // Normal is inside

                    break;
            }


            for (int i = 0; i < OldPolygon.Count; i++)
            {
                AP = new Vector3(-OldPolygon[i].X + P.X, -OldPolygon[i].Y + P.Y, -OldPolygon[i].Z + P.Z);
                BP = new Vector3(-OldPolygon[(i + 1) % OldPolygon.Count].X + P.X, -OldPolygon[(i + 1) % OldPolygon.Count].Y + P.Y, -OldPolygon[(i + 1) % OldPolygon.Count].Z + P.Z);
                APosition = (Normal.X * AP.X + Normal.Y * AP.Y + Normal.Z * AP.Z) / Normal.Length() / AP.Length();
                BPosition = (Normal.X * BP.X + Normal.Y * BP.Y + Normal.Z * BP.Z) / Normal.Length() / BP.Length();
                FindIntersection(i, APosition, BPosition, NewPolygon, OldPolygon);
            }
            return NewPolygon;
        }
        private static void FindIntersection(int i, double APosition, double BPosition, List<Vector4> NewPolygon, List<Vector4> OldPolygon)
        {
            double length;
            Vector4 ClipPoint;
            if (APosition <= 0 && BPosition > 0) // B inside, A outside
            {
                length = Math.Abs(APosition) + BPosition;
                ClipPoint = new Vector4(OldPolygon[i].X + (float)APosition / (float)length * (OldPolygon[i].X - OldPolygon[(i + 1) % OldPolygon.Count].X), OldPolygon[i].Y + (float)APosition / (float)length * (OldPolygon[i].Y - OldPolygon[(i + 1) % OldPolygon.Count].Y), OldPolygon[i].Z + (float)APosition / (float)length * (OldPolygon[i].Z - OldPolygon[(i + 1) % OldPolygon.Count].Z), 1);
                NewPolygon.Add(ClipPoint);
                NewPolygon.Add(OldPolygon[(i + 1) % OldPolygon.Count]); // add B
            }
            else if (APosition >= 0 && BPosition < 0) // A inside, B outside
            {
                length = Math.Abs(BPosition) + APosition;
                ClipPoint = new Vector4(OldPolygon[i].X - (float)APosition / (float)length * (OldPolygon[i].X - OldPolygon[(i + 1) % OldPolygon.Count].X), OldPolygon[i].Y - (float)APosition / (float)length * (OldPolygon[i].Y - OldPolygon[(i + 1) % OldPolygon.Count].Y), OldPolygon[i].Z - (float)APosition / (float)length * (OldPolygon[i].Z - OldPolygon[(i + 1) % OldPolygon.Count].Z), 1);
                NewPolygon.Add(ClipPoint); // add only clipping point
            }
            else if (BPosition >= 0)
            {
                NewPolygon.Add(OldPolygon[(i + 1) % OldPolygon.Count]);
            }

        }
       */
     
    }
}
