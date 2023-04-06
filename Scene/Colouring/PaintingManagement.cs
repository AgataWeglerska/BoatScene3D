using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Scene
{
    // class contains chosepixelcolor and filltriangle for every object
    // it also have ZBuffor, which is given as reference to every fillTriangle class
    // class give canvas to fill with colour and this canvas is returned always to ColourBox "Canvas" in Form1
    class PaintingManagement
    {
        private bool isFog;
        private Bitmap canvas;
        private LockBitMap canvasLocked;
        private ZBuffer zBuffer;
        // lists to actualize when boat has changed the position
        List<(double, double, double)> Normals;
        List<(double, double, double)> Vertices;
        List<(double, double, double)> LightNormals;
        List<(double, double, double)> LightVertices;

        // classes used to calculate RGB color in every triangle from Object
        private ChoosePixelColor BoatchoseColor; // class is used to calculate RGB color in single pixel
        private FillTriangle BoatfTriangle; // class is used to fill all triangles from Object

        private ChoosePixelColor LightchoseColor;
        private FillTriangle LightfTriangle;

        private ChoosePixelColor MainBuoychoseColor;
        private FillTriangle MainBuoyfTriangle;

        private ChoosePixelColor LeftBuoychoseColor;
        private FillTriangle LeftBuoyfTriangle;

        private ChoosePixelColor RightBuoychoseColor;
        private FillTriangle RightBuoyfTriangle;

        public PaintingManagement(int Width, int Height, Vector4[] MainBuoyV, Vector4[] MainBuoyN,
                Vector4[] LeftBuoyV, Vector4[] LeftBuoyN, Vector4[] RightBuoyV, Vector4[] RightBuoyN)
        {
            canvas = new Bitmap(Width, Height);
            canvasLocked = new LockBitMap(canvas);
            zBuffer = new ZBuffer(Width,Height);
            InitializeLists();
            InitializeChosePixelColorAndFillTriangle(MainBuoyV, MainBuoyN, LeftBuoyV, LeftBuoyN, RightBuoyV, RightBuoyN);
            isFog = false;
        }
        // ---------------------------------------------------------//
        // function used to change fogGradient - if camera has changed
        public void changedfogGradient(int grad)
        {
            BoatchoseColor.changedfogGradient(grad);
            LightchoseColor.changedfogGradient(grad);
            MainBuoychoseColor.changedfogGradient(grad);
            LeftBuoychoseColor.changedfogGradient(grad);
            RightBuoychoseColor.changedfogGradient(grad);
        }
        public void chanedisFog()
        {
            isFog = !isFog;
        }
        // initializing every list - lists will be importatnt while painting
        private void InitializeLists()
        {
            Normals = new List<(double, double, double)>();
            Vertices = new List<(double, double, double)>();
            LightNormals = new List<(double, double, double)>();
            LightVertices = new List<(double, double, double)>();
        }
        private void InitializeChosePixelColorAndFillTriangle(Vector4[] MainBuoyV, Vector4[] MainBuoyN, Vector4[] LeftBuoyV,
                        Vector4[] LeftBuoyN, Vector4[] RightBuoyV, Vector4[] RightBuoyN)
        {
            // Boat
            BoatchoseColor = new ChoosePixelColor(Vertices, Normals, null,0.7,0.8,10, Color.DimGray,false);
            BoatfTriangle = new FillTriangle(canvas.Height, BoatchoseColor, canvasLocked, zBuffer);
            // Light - Reflector
            LightchoseColor = new ChoosePixelColor(LightVertices, LightNormals, null, 0.7, 0.8, 10,Color.White,true);
            LightfTriangle = new FillTriangle(canvas.Height, LightchoseColor, canvasLocked, zBuffer);
            // MainBuoy - yellow
            List<(double, double, double)> Normalstemp = new List<(double, double, double)>();
            List<(double, double, double)> Verticestemp = new List<(double, double, double)>();
            Vector4 temp;
            foreach (var el in MainBuoyN)
            {
                temp = Vector4.Normalize(el);
                Normalstemp.Add((temp.X, temp.Y, temp.Z));
            }
            foreach (var el in MainBuoyV)
            {
                Verticestemp.Add((el.X,el.Y,el.Z));
            }
            
            MainBuoychoseColor = new ChoosePixelColor(Verticestemp, Normalstemp, null,0.3,0.7,3, Color.Yellow, false);
            MainBuoyfTriangle = new FillTriangle(canvas.Height, MainBuoychoseColor, canvasLocked, zBuffer);

            // LeftBuoy - green
            Normalstemp = new List<(double, double, double)>();
            Verticestemp = new List<(double, double, double)>();
            foreach (var el in LeftBuoyN)
            {
                temp = Vector4.Normalize(el);
                Normalstemp.Add((temp.X, temp.Y, temp.Z));
            }
            foreach (var el in LeftBuoyV)
            {
                Verticestemp.Add((el.X, el.Y, el.Z));
            }

            LeftBuoychoseColor = new ChoosePixelColor(Verticestemp, Normalstemp, null, 0.7,0.7,2,Color.Green,false);
            LeftBuoyfTriangle = new FillTriangle(canvas.Height, LeftBuoychoseColor, canvasLocked, zBuffer);

            // RightBouy - red
            Normalstemp = new List<(double, double, double)>();
            Verticestemp = new List<(double, double, double)>();
            foreach (var el in RightBuoyN)
            {
                temp = Vector4.Normalize(el);
                Normalstemp.Add((temp.X, temp.Y, temp.Z));
            }
            foreach (var el in RightBuoyV)
            {
                Verticestemp.Add((el.X, el.Y, el.Z));
            }

            RightBuoychoseColor = new ChoosePixelColor(Verticestemp, Normalstemp, null, 0.8,0.2,4,Color.Red,false);
            RightBuoyfTriangle = new FillTriangle(canvas.Height, RightBuoychoseColor, canvasLocked, zBuffer);
        }        
        // function recall after changing position of the boat, before viewChange
        public void ChangedPositionOfBoat(Vector4[] Vert, Vector4[] Norm, Vector4[] LightVert, Vector4[] LightNorm)
        {
            Vector4 temp;
            Normals.Clear();
            Vertices.Clear();
            LightNormals.Clear();
            LightVertices.Clear();

            foreach (var el in Norm)
            {
                temp = Vector4.Normalize(el);
                Normals.Add((temp.X, temp.Y, temp.Z));
            }
            foreach (var el in Vert)
            {
                Vertices.Add((el.X, el.Y, el.Z));
            }


            foreach (var el in LightNorm)
            {
                temp = Vector4.Normalize(el);
                LightNormals.Add((temp.X, temp.Y, temp.Z));
            }
            foreach (var el in LightVert)
            {
                LightVertices.Add((el.X, el.Y, el.Z));
            }
        }
        // filling every Object with colour
        public Bitmap FillObject(Vector4[] BoatV, List<(int, int, int)> BoatTriangles, Vector4[] LightV, List<(int, int, int)> LightTriangles, Vector4[] MainBuoyV, List<(int, int, int)> MainBuoyTriangles, Vector4[] LeftBuoyV,
            List<(int, int, int)> LeftBuoyTriangles, Vector4[] RightBuoyV, List<(int, int, int)> RightBuoyTriangles, Vector3 CameraPosition, Vector4 ReflectorNormal, Vector4 PR)
        {
            // creating the background - in fact covering last scene
            canvasLocked.LockBits();
            for (int i = 0; i < canvas.Width; i++)
                for (int j = 0; j < canvas.Height; j++)
                    canvasLocked.SetPixel(i, j, Color.Black);
            canvasLocked.UnlockBits();
            // first I have to inicjalize ZBuffer with Max_Value wich is 0 in my program,
            // because I logaritmize Z values for every vertice    
            zBuffer.RestartZBuffer(); // zBuffer is being filled with only ones values
            // filling Objects
            LightfTriangle.FillSpace(LightV, LightTriangles, CameraPosition,  ReflectorNormal,  PR, isFog);
            BoatfTriangle.FillSpace(BoatV, BoatTriangles, CameraPosition,  ReflectorNormal,  PR, isFog);  
            LeftBuoyfTriangle.FillSpace(LeftBuoyV, LeftBuoyTriangles, CameraPosition, ReflectorNormal, PR, isFog);
            RightBuoyfTriangle.FillSpace(RightBuoyV, RightBuoyTriangles, CameraPosition, ReflectorNormal, PR, isFog);
            MainBuoyfTriangle.FillSpace(MainBuoyV,MainBuoyTriangles, CameraPosition, ReflectorNormal, PR, isFog);

            return canvas;
        }
    }
}
