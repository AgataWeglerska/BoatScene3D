using System;
using System.Numerics;

namespace Scene
{
    static class View
    {
        // static camera outside of the boat - directed in the center of the scene
        public static void Camera1(float Cx, float Cy, Vector3 Pv, ref Vector4[] Vertices, ref Vector4[] LightVertices,
            ref Vector4[] BuoyVertices, ref Vector4[] LeftBuoyVertices, ref Vector4[] RightBuoyVertices, int FarClipPlane)
        {
            Vector3 Uworld = new Vector3(0, 1, 0);

            Vector3 D = new Vector3(0, 0, 0); // the center of the scene
            Matrix4x4 View = Matrix4x4.CreateLookAt(Pv, D, Uworld);
            Matrix4x4 S = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI / 4, 1, 550, FarClipPlane);
            Matrix4x4 SXView = Matrix4x4.Multiply(View, S);

            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i] = CalculateVector(Vertices[i], SXView, Cx, Cy);

            for (int i = 0; i < BuoyVertices.Length; i++)
                BuoyVertices[i] = CalculateVector(BuoyVertices[i], SXView, Cx, Cy);

            for (int i = 0; i < LeftBuoyVertices.Length; i++)
                LeftBuoyVertices[i] = CalculateVector(LeftBuoyVertices[i], SXView, Cx, Cy);
            
            for (int i = 0; i < RightBuoyVertices.Length; i++)
                RightBuoyVertices[i] = CalculateVector(RightBuoyVertices[i],SXView, Cx, Cy);

            for (int i = 0; i < LightVertices.Length; i++)
                LightVertices[i] = CalculateVector(LightVertices[i], SXView, Cx, Cy);
        }

        // camera outside, looking on the center of the boat
        public static void Camera2(float Cx, float Cy, Vector3 Pv, ref Vector4[] Vertices, ref Vector4[] LightVertices, ref Vector4[] BuoyVertices,
            ref Vector4[] LeftBuoyVertices, ref Vector4[] RightBuoyVertices, Vector4 CenterOfBoat, int FarClipPlane)
        {
            Vector3 Uworld = new Vector3(0, 1, 0);
            Vector3 D = new Vector3(CenterOfBoat.X, CenterOfBoat.Y, CenterOfBoat.Z); // the center of the Boat
            Matrix4x4 View = Matrix4x4.CreateLookAt(Pv, D, Uworld);
            Matrix4x4 S = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI / 4, 1, 550, FarClipPlane);
            Matrix4x4 SXView = Matrix4x4.Multiply(View, S);

            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i] = CalculateVector(Vertices[i], SXView, Cx, Cy);

            for (int i = 0; i < BuoyVertices.Length; i++)
                BuoyVertices[i] = CalculateVector(BuoyVertices[i], SXView, Cx, Cy);

            for (int i = 0; i < LeftBuoyVertices.Length; i++)
                LeftBuoyVertices[i] = CalculateVector(LeftBuoyVertices[i], SXView, Cx, Cy);

            for (int i = 0; i < RightBuoyVertices.Length; i++)
                RightBuoyVertices[i] = CalculateVector(RightBuoyVertices[i], SXView, Cx, Cy);

            for (int i = 0; i < LightVertices.Length; i++)
                LightVertices[i] = CalculateVector(LightVertices[i], SXView, Cx, Cy);

        }

        // camera on the top of the boat, directed on the Main - yellow Buoy
        public static void Camera3(float Cx, float Cy, Vector3 Pv, ref Vector4[] Vertices, ref Vector4[] LightVertices, ref Vector4[] BuoyVertices,
            ref Vector4[] LeftBuoyVertices, ref Vector4[] RightBuoyVertices, int FarClipPlane)
        {
            Vector3 Uworld = new Vector3(0, 1, 0);
            Vector3 D = new Vector3(150, 0, 0); // the point where is the main Buoy
            Matrix4x4 View = Matrix4x4.CreateLookAt(Pv, D, Uworld);
            Matrix4x4 S = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI / 4, 1, FarClipPlane-370, FarClipPlane);
            Matrix4x4 SXView = Matrix4x4.Multiply(View, S);

            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i] = CalculateVector(Vertices[i], SXView, Cx, Cy);

            for (int i = 0; i < BuoyVertices.Length; i++)
                BuoyVertices[i] = CalculateVector(BuoyVertices[i], SXView, Cx, Cy);

            for (int i = 0; i < LeftBuoyVertices.Length; i++)
                LeftBuoyVertices[i] = CalculateVector(LeftBuoyVertices[i], SXView, Cx, Cy);

            for (int i = 0; i < RightBuoyVertices.Length; i++)
                RightBuoyVertices[i] = CalculateVector(RightBuoyVertices[i], SXView, Cx, Cy);

            for (int i = 0; i < LightVertices.Length; i++)
                LightVertices[i] = CalculateVector(LightVertices[i], SXView, Cx, Cy);
        }

        private static Vector4 CalculateVector(Vector4 Vert, Matrix4x4 SXView, float Cx, float Cy) 
        {
            Matrix4x4 T1, T2, temp;

            // View and Projection Matrixes apply
            Vert = Vector4.Transform(Vert, SXView);
            Vert.X = (Vert.X / Vert.W);
            Vert.Y = (Vert.Y / Vert.W);
            Vert.Z = (Vert.Z / Vert.W);
            Vert.W = (Vert.W / Vert.W);

            // appropriate adjust of view + scaling
            {
                T1 = new Matrix4x4
                (1, 0, 0, 1,
                0, -1, 0, 1,
                0, 0, 1, 0,
                0, 0, 0, 1);

                T2 = new Matrix4x4
                   (Cx, 0, 0, 0,
                   0, Cy, 0, 0,
                   0, 0, 1, 0,
                   0, 0, 0, 1);

                temp = Matrix4x4.Multiply(T2, T1);
                Vert = Products.ApplyMatrix(Vert, temp);
            }
            
            return Vert;
        }
    }
}
