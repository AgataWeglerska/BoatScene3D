using System.Numerics;

namespace Scene
{
    public static class Products
    {
        // class useful to calculate cross product or to multiply matrix with vector
        public static Vector4 ApplyMatrix(this Vector4 self, Matrix4x4 matrix)
        {
            return new Vector4(
                matrix.M11 * self.X + matrix.M12 * self.Y + matrix.M13 * self.Z + matrix.M14 * self.W,
                matrix.M21 * self.X + matrix.M22 * self.Y + matrix.M23 * self.Z + matrix.M24 * self.W,
                matrix.M31 * self.X + matrix.M32 * self.Y + matrix.M33 * self.Z + matrix.M34 * self.W,
                matrix.M41 * self.X + matrix.M42 * self.Y + matrix.M43 * self.Z + matrix.M44 * self.W
            );
        }

        public static Vector3 CrossProduct(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                v1.Y * v2.Z - v1.Z * v2.Y,
                -(v1.X * v2.Z - v2.X * v1.Z),
                v1.X * v2.Y - v2.X * v1.Y);
        }
        public static Vector4 CrossProduct(Vector4 v1, Vector4 v2)
        {
            return new Vector4(
                v1.Y * v2.Z - v1.Z * v2.Y,
                -(v1.X * v2.Z - v2.X * v1.Z),
                v1.X * v2.Y - v2.X * v1.Y,0);
        }

    }
}
