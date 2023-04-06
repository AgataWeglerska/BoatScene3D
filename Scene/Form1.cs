using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using System.Numerics;

namespace Scene
{
    //------------- On the scene we can see Boat with some Buoys around --------------//
    public partial class Form1 : Form
    {
        // private variables
        // For Boat Object
        private Vector4[] Vertices; // positions of vertices change always when timer1_Tick() 
        private Vector4[] ConstVertices;
        private List<(int, int, int)> Triangles; // faces to fill with color - faces which are included in camera view
        private List<(int, int, int)> ConstTriangles; 
        private Vector4[] Normals; // positions of normals change always when timer1_Tick() 
        private Vector4[] ConstNormals;
        // For Reflector Object
        private Vector4[] LightVertices;
        private Vector4[] ConstLightVertices;
        private List<(int, int, int)> LightTriangles; // faces to fill with color
        private List<(int, int, int)> ConstLightTriangles;
        private Vector4[] LightNormals;
        private Vector4[] ConstLightNormals;
        // For Main - yellow Buoy Object
        private Vector4[] BuoyVertices;
        private Vector4[] BuoyNormals;
        private List<(int, int, int)> ConstBuoyTriangles; // faces to fill with color
        private List<(int, int, int)> BuoyTriangles; // faces to fill with color
        private Vector4[] BuoyConstVertices;
        // For Left - green Buoy Object
        private Vector4[] LeftBuoyVertices;
        private Vector4[] LeftBuoyNormals;
        private List<(int, int, int)> ConstLeftBuoyTriangles; // faces to fill with color
        private List<(int, int, int)> LeftBuoyTriangles; // faces to fill with color
        private Vector4[] LeftBuoyConstVertices;
        // For Right - red Buoy (Torus) Object
        private Vector4[] RightBuoyVertices;
        private Vector4[] RightBuoyNormals;
        private List<(int, int, int)> ConstRightBuoyTriangles; // faces to fill with color
        private List<(int, int, int)> RightBuoyTriangles; // faces to fill with color
        private Vector4[] RightBuoyConstVertices;

        // varaibles important to adjust Reflector position (PR) and it's normal vector
        private Vector4 ReflectorNormal, ConstReflectorNormal, PR, ConstPR;

        private Vector3 CameraPosition;
        private double alfa; // to create spiral movement
        private float scale; // scale of Canvas on witch full scene is painted
        // class usefull when painting surfaces of object on the scene
        private PaintingManagement PaintManager;

        public Form1()
        {
            InitializeComponent();
            InitializeListst();
            scale = Canvas.Width < Canvas.Height ? Canvas.Width / 12 : Canvas.Height / 12;
            alfa = 0;

            ConstReflectorNormal = new Vector4(0,0,1,0);// only for this scene
            ConstPR = new Vector4(0,10,85,1); // only for this scene
            StraightReflector.Checked = true;

            // reading vertices and normals of every Object
            ReadFile("./../../../ObjectsOnTheScene/myBoat7.obj",ref Vertices, ref ConstNormals, ConstTriangles,ref ConstVertices, (float)(scale*1.1));
            Normals = new Vector4[ConstNormals.Length];
            ReadFile("./../../../ObjectsOnTheScene/Reflector.obj", ref LightVertices, ref ConstLightNormals, ConstLightTriangles, ref ConstLightVertices, scale/5);
            LightNormals = new Vector4[ConstLightNormals.Length];
            ReadFile("./../../../ObjectsOnTheScene/Buoy.obj", ref BuoyVertices, ref BuoyNormals, ConstBuoyTriangles, ref BuoyConstVertices, scale/3);
            ReadFile("./../../../ObjectsOnTheScene/LeftBuoy1.obj", ref LeftBuoyVertices, ref LeftBuoyNormals, ConstLeftBuoyTriangles, ref LeftBuoyConstVertices, scale/3);
            ReadFile("./../../../ObjectsOnTheScene/FullTorus.obj", ref RightBuoyVertices, ref RightBuoyNormals, ConstRightBuoyTriangles, ref RightBuoyConstVertices, scale/3);

            // transformation of Buoys, which don't move
            BuoysTransformations.TransformConstBuoys(ref BuoyConstVertices, ref LeftBuoyConstVertices, ref RightBuoyConstVertices, ref ConstLightVertices);

            PaintManager = new PaintingManagement(Canvas.Width, Canvas.Height, BuoyConstVertices,
                           BuoyNormals, LeftBuoyVertices,LeftBuoyNormals,RightBuoyVertices,RightBuoyNormals);
        }
        private void InitializeListst()
        {
            Triangles = new List<(int, int, int)>();
            ConstTriangles = new List<(int, int, int)>();

            LightTriangles = new List<(int, int, int)>();
            ConstLightTriangles = new List<(int, int, int)>();

            BuoyTriangles = new List<(int, int, int)>();
            ConstBuoyTriangles = new List<(int, int, int)>();

            LeftBuoyTriangles = new List<(int, int, int)>();
            ConstLeftBuoyTriangles = new List<(int, int, int)>();

            RightBuoyTriangles = new List<(int, int, int)>();
            ConstRightBuoyTriangles = new List<(int, int, int)>();
        }

        // maybe it will load textures - so far not
        /*  public void LoadTexture(string fileName)
          {
              using (Stream BitmapStream = System.IO.File.Open(fileName, System.IO.FileMode.Open))
              {
                  Image img = Image.FromStream(BitmapStream);
                  this.tTexture = new Bitmap(new Bitmap(img), new Size(Canvas.Width, Canvas.Height));
              }
          }*/
        //--------------------------------------------------------
        // function to read vertices, normals and faces from obj files
        private void ReadFile(string fileName, ref Vector4[] Vertices_, ref Vector4[] Normals_, List<(int, int, int)> Triangles_, ref Vector4[] ConstVertices_, float radius)
        {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    List<(double, double, double)> Vert = new List<(double, double, double)>(); // helpful variable
                    string line;
                    string[] strArray;
                    double x1, y1, z1;
                    int i;
                    int[] temp1 = new int[3];
                    double[] temp2 = new double[3];
                    var fmt = new NumberFormatInfo();
                    List<(double, double, double)> TempNormals = new List<(double, double, double)>();
                    fmt.NegativeSign = "-";
                    //at fisrt I add the location of vertices scaled and on Canvas.Image 
                    line = sr.ReadLine();
                    line = sr.ReadLine();
                    strArray = line.Split(' ');
                    while (strArray[0] != "vn")
                    {
                        x1 = double.Parse(strArray[1], fmt);
                        y1 = double.Parse(strArray[2], fmt);
                        z1 = double.Parse(strArray[3], fmt);
                        Vert.Add((x1, y1, z1));
                        line = sr.ReadLine();
                        strArray = line.Split(' ');
                    }
                    // then I add normal vectors and then the triangles between which vertices are
                    // normal vectors
                    i = 0;
                // reading normals, which are unstorted (there is a hash function in obj file)
                 (double, double, double)[] Norm = new (double, double, double)[Vert.Count]; // helpful variable
                    while (strArray[0] != "s")
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            temp2[j] = Double.Parse(strArray[j + 1], fmt);
                        }
                        TempNormals.Add((temp2[0], temp2[1], temp2[2]));
                        strArray = sr.ReadLine().Split(' ');
                        i++;
                    }
                    // reading triangles and sorting normals 
                    i = 0;
                    string[] tempstring;
                    while ((line = sr.ReadLine()) != null)
                    {
                        strArray = line.Split(' ');
                        for (int j = 0; j < 3; j++)
                        {
                            tempstring = strArray[j + 1].Split("//");
                            temp1[j] = Int32.Parse(tempstring[0]);
                            Norm[temp1[j] - 1] = TempNormals[Int32.Parse(tempstring[1]) - 1];
                        }

                        Triangles_.Add((temp1[0], temp1[1], temp1[2]));
                        i++;
                    }
                Vertices_ = new Vector4[Vert.Count];
                ConstVertices_ = new Vector4[Vert.Count];
                Normals_ = new Vector4[Vert.Count];
                for (i =0; i < Vert.Count; i++)
                {
                    Vertices_[i] = new Vector4((float)Vert[i].Item2 *-radius, (float)Vert[i].Item3 * radius, (float)Vert[i].Item1 * radius,1);
                    ConstVertices_[i] = new Vector4((float)Vert[i].Item2 * -radius, (float)Vert[i].Item3 * radius, (float)Vert[i].Item1 * radius, 1);

                    Normals_[i] = new Vector4((float)Norm[i].Item2 * -1, (float)Norm[i].Item3 , (float)Norm[i].Item1 , 0);
                }

                }
            }
        //--------------------------------------------------------
        // adding/deleting fog

        private void AddDeleteFog_Click(object sender, EventArgs e)
        {
            PaintManager.chanedisFog();
        }
        //--------------------------------------------------------
        // changing fog gradient
        private void Camera3_CheckedChanged(object sender, EventArgs e)
        {
            PaintManager.changedfogGradient(180);
        }

        private void Camera1_CheckedChanged(object sender, EventArgs e)
        {
            PaintManager.changedfogGradient(700);
        }

        private void Camera2_CheckedChanged(object sender, EventArgs e)
        {
            PaintManager.changedfogGradient(800);
        }

       

        //--------------------------------------------------------
        // function change the scene in every timer1 interval
        private void timer1_Tick(object sender, EventArgs e)
        {
            alfa += Math.PI / 64; // angle of rotation of the Boat
            FindPosition(); // finds the position of every element in the screen

            // FillObject function returns Bitmap with object on the scene
            Canvas.Image = (Image)(PaintManager.FillObject(Vertices,Triangles, LightVertices, LightTriangles, BuoyVertices,
                            BuoyTriangles, LeftBuoyVertices, LeftBuoyTriangles, RightBuoyVertices, RightBuoyTriangles, CameraPosition, ReflectorNormal, PR));
            Canvas.Invalidate();
        }
        // function evoke to change the position of Boat in the scene
        private void FindPosition()
        {
            int FarClipPlane; // far plane which camera can see
            float Cx = Canvas.Width / 2, Cy = Canvas.Height / 2; // center of the Canvas
            Vector3 ObservatedPoint; // point observated by camera
            Vector4 CameraOnBoat = new Vector4(0, 0, 1.17f * scale, 1); // in fact it is where is camera on the boat - on the right side of boat
            CameraPosition = new Vector3(2.5f * scale * 3.1f, 0.6f * scale * 12.5f, 1.17f * scale * 9.9f); // camera position depends on the screen size

            // initializnig vertices and normals with values from const ones
            {
                for (int i = 0; i < ConstVertices.Length; i++)
                    Vertices[i] = ConstVertices[i];
                for (int i = 0; i < ConstNormals.Length; i++)
                    Normals[i] = ConstNormals[i];

                for (int i = 0; i < ConstLightVertices.Length; i++)
                    LightVertices[i] = ConstLightVertices[i];
                for (int i = 0; i < ConstLightNormals.Length; i++)
                    LightNormals[i] = ConstLightNormals[i];

                for (int i = 0; i < BuoyConstVertices.Length; i++)
                    BuoyVertices[i] = BuoyConstVertices[i];

                for (int i = 0; i < LeftBuoyConstVertices.Length; i++)
                    LeftBuoyVertices[i] = LeftBuoyConstVertices[i];

                for (int i = 0; i < RightBuoyConstVertices.Length; i++)
                    RightBuoyVertices[i] = RightBuoyConstVertices[i];
                ReflectorNormal = ConstReflectorNormal;
                PR = ConstPR;
            }

            // transformation of boat - rotation 
            TransformBoat.Transformation(!StraightReflector.Checked,ref CameraOnBoat, ref ReflectorNormal,
                ref PR, alfa, ref Vertices, ref Normals, ref LightVertices, ref LightNormals);
            
            PaintManager.ChangedPositionOfBoat(Vertices, Normals, LightVertices, LightNormals);

            // it depends which camera has been chosen
            // if static camera outside
            if (Camera1.Checked == true)
            {
                ObservatedPoint = new Vector3(0, 0, 0);
                FarClipPlane = (int)((ObservatedPoint - CameraPosition).Length() * 1.3); // the view form camera contains more then the center of the plane
                ClipObjects(CameraPosition, ObservatedPoint, FarClipPlane);
                View.Camera1(Cx, Cy, CameraPosition, ref Vertices, ref BuoyVertices, ref LightVertices, ref LeftBuoyVertices, ref RightBuoyVertices, FarClipPlane);
            }
            // else if camera follow the boat
            else if (Camera2.Checked == true)
            {
                ObservatedPoint = new Vector3(CameraOnBoat.X, CameraOnBoat.Y, CameraOnBoat.Z);
                FarClipPlane = (int)((ObservatedPoint-CameraPosition).Length() * 1.3); // the view contains a bit more then only on the boat
                ClipObjects(CameraPosition, ObservatedPoint, FarClipPlane);
                View.Camera2(Cx, Cy, CameraPosition, ref Vertices, ref BuoyVertices, ref LightVertices, ref LeftBuoyVertices, ref RightBuoyVertices, CameraOnBoat, FarClipPlane);
            }
            // else camera on the boat
            else
            {
                int height = (int)(0.92 * scale); // camera is on the top of the boat
                CameraPosition = new Vector3(CameraOnBoat.X, CameraOnBoat.Y + height, CameraOnBoat.Z);
                ObservatedPoint = new Vector3(150, 0, 0);
                FarClipPlane = (int)((ObservatedPoint - CameraPosition).Length()+300); // x10 so as to see the rightBuoy
                ClipObjects(CameraPosition, ObservatedPoint, FarClipPlane);
                View.Camera3(Cx,Cy, CameraPosition, ref Vertices, ref LightVertices, ref BuoyVertices, ref LeftBuoyVertices, ref RightBuoyVertices, FarClipPlane);
            }
        }
        // function evoke to clip faces which are outside the view of camera and also back faces - which are behind the Object
        private void ClipObjects(Vector3 CameraPosition, Vector3 ObservatedPoint,int FarClipPlane)
        {
            Clip.Cliping(Triangles, ConstTriangles, Vertices, CameraPosition, ObservatedPoint, FarClipPlane);
            Clip.Cliping(LightTriangles, ConstLightTriangles, LightVertices, CameraPosition, ObservatedPoint, FarClipPlane);
            Clip.Cliping(BuoyTriangles, ConstBuoyTriangles, BuoyVertices, CameraPosition, ObservatedPoint, FarClipPlane);
            Clip.Cliping(LeftBuoyTriangles, ConstLeftBuoyTriangles, LeftBuoyVertices, CameraPosition, ObservatedPoint, FarClipPlane);
            Clip.Cliping(RightBuoyTriangles, ConstRightBuoyTriangles, RightBuoyVertices, CameraPosition, ObservatedPoint, FarClipPlane);
        }
    }   
}