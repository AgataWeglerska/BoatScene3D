using System;
using System.Numerics;

namespace Scene
{
    public static class BuoysTransformations
    {
        // function adjust the positions of Buoys in the scene
        public static void TransformConstBuoys(ref Vector4[] BuoyConstVertices, ref Vector4[] LeftBuoyConstVertices, ref Vector4[] RightBuoyConstVertices, ref Vector4[] LightConstVertices)
        {
            double alfa;
            Matrix4x4 TB, RyLight;
            // main - yellow Buoy transformation //
            TB = new Matrix4x4
            (1, 0, 0, 150,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1);
            for (int i = 0; i < BuoyConstVertices.Length; i++)
                BuoyConstVertices[i] = Products.ApplyMatrix(BuoyConstVertices[i], TB);
            

            // left - green Buoy transformation //
            TB = new Matrix4x4
            (1, 0, 0, 100,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1);
            for (int i = 0; i < LeftBuoyConstVertices.Length; i++)
                LeftBuoyConstVertices[i] = Products.ApplyMatrix(LeftBuoyConstVertices[i], TB);
            
            // red - right Buoy transformation //
            TB = new Matrix4x4
            (1, 0, 0, -190,
            0, 1, 0, 0,
            0, 0, 1, 200,
            0, 0, 0, 1);
            for (int i = 0; i < RightBuoyConstVertices.Length; i++)
                RightBuoyConstVertices[i] = Products.ApplyMatrix(RightBuoyConstVertices[i], TB);
            

            // Light transformation  - Reflector on the Boat transformation //
            TB = new Matrix4x4
           (1, 0, 0, 0,
           0, 1, 0, 10,
           0, 0, 1, 85,
           0, 0, 0, 1);

            alfa = Math.PI / 2;

            RyLight = new Matrix4x4(
           (float)Math.Cos(alfa), 0, (float)(-1 * Math.Sin(alfa)), 0,
            0, 1, 0, 0,
           (float)Math.Sin(alfa), 0, (float)Math.Cos(alfa), 0,
            0, 0, 0, 1);

            for (int i = 0; i < LightConstVertices.Length; i++)
            {
                LightConstVertices[i] = Products.ApplyMatrix(LightConstVertices[i], RyLight);
                LightConstVertices[i] = Products.ApplyMatrix(LightConstVertices[i], TB);
            }
        }
    }
}
