
namespace Scene
{
    // class storing Z-Buffer
    class ZBuffer
    {
        private double[][] zBuffer;
        public ZBuffer(int Width, int Height)
        {
            zBuffer = new double[Width][];
            for (int i = 0; i < Width; i++)
                zBuffer[i] = new double[Height];
            RestartZBuffer();
        }
        // fillling Z-Buffer with Max_Value when starting calculations for new scene
        public void RestartZBuffer()
        {
            for (int i = 0; i < zBuffer.Length; i++)
                for (int j = 0; j < zBuffer[0].Length; j++)
                    zBuffer[i][j] = 1;
        }
        // if pixel is in front is true then function returns true
        public bool IsInFront((int,int,double) v)
        {
            if(v.Item3 < zBuffer[v.Item1][v.Item2])
            {
                zBuffer[v.Item1][v.Item2] = v.Item3;
                return true;
            }
            return false;
        }
    }
}
