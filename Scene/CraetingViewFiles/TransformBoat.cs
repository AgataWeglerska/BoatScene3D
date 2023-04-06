using System;
using System.Numerics;

namespace Scene
{
    public static class TransformBoat
    {
        // function is used to change position of Boat, CenterPoint and Reflector in the scene
        public static void Transformation(bool DirectedOnBuoy, ref Vector4 CenterOfBoat, ref Vector4 ReflectorNorm, ref Vector4 PR, double alfa, ref Vector4[] Vertices, ref Vector4[] Normals, ref Vector4[] LightVertices, ref Vector4[] LightNormals)
        {
            Matrix4x4 T2 = new Matrix4x4
               (1, 0, 0, 0,
               0, 1, 0, 0,
               0, 0, 1, -200,
               0, 0, 0, 1);

            Matrix4x4 T3 = new Matrix4x4
               (1, 0, 0, 70,
               0, 1, 0, 0,
               0, 0, 1, 0,
               0, 0, 0, 1);

            Matrix4x4 Ry2 = new Matrix4x4(
              (float)Math.Cos(alfa), 0, (float)(-1 * Math.Sin(alfa)), 0,
               0, 1, 0, 0,
              (float)Math.Sin(alfa), 0, (float)Math.Cos(alfa), 0,
               0, 0, 0, 1);

            Matrix4x4 temp = Matrix4x4.Multiply(T3, Matrix4x4.Multiply(Ry2, T2));
            // Boat's vertices and normals transformation
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] = Products.ApplyMatrix(Vertices[i], temp);
            }
            for (int i = 0; i < Normals.Length; i++)
            {
                Normals[i] = Products.ApplyMatrix(Normals[i], Ry2);
            }
            CenterOfBoat = Products.ApplyMatrix(CenterOfBoat, temp);

            // light which is on the side od boat is translated and rotated in the same way as a Boat
            for (int i = 0; i < LightVertices.Length; i++)
            {
                LightVertices[i] = Products.ApplyMatrix(LightVertices[i], temp);
            }
            for (int i = 0; i < LightNormals.Length; i++)
            {
                LightNormals[i] = Products.ApplyMatrix(LightNormals[i], Ry2);
            }
            
            // PR - Point where is Reflector on the Boat istranslated and rotated in the same way as a Boat
            PR = Products.ApplyMatrix(PR, temp);
            // ReflectorNorm - normal vector of PR
            if (DirectedOnBuoy) // if Reflector is directed on yellow Buoy (150,0,0,1)
                ReflectorNorm = Vector4.Normalize(new Vector4(150,0,0,1)-PR);
            else // else if directed perpendicularly to Boat
                ReflectorNorm = Products.ApplyMatrix(ReflectorNorm, Ry2);
        }
    }
}
