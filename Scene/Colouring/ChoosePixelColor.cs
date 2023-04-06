using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace Scene
{
    class ChoosePixelColor
    {
        float fogGradient; // for fog intensity 0.3

        Color C1, C2, C3; // colors in three vertices - from one triangle, which is coloured

        private bool isOne; // how many edges are in left side while colouring one triangle
        private bool isReflector; // if reflector then it's color is it's own

        private double kd,ks,ka; // parameters for Intensity in Phong Model
        private int m,P; // Power of cosinuses, first for I_s, second for I_Reflector
        private (double, double, double) V; // parameter from task V = (0,0,1)
        
        private int indexv1, indexv2, indexv3; // indexes, to read from Vertices and Norm  - from one triangle, which is coloured
        private (double, double) v1, v2, v3; // vertices - from one triangle, which is coloured
        

        private Color Objectcolor;
        private Color Lightcolor;

        private List<(double, double, double)> Norm;
        private List<(double, double, double)> Vertices;
        private (double, double, double) Sun; // position of light
        
        private Bitmap tTexture; // texture of Object

        public ChoosePixelColor(List<(double, double, double)> Vert, List<(double, double, double)> Normals,  Bitmap Texture, double kd, double ks, int m, Color ObjectC, bool IsItReflector)
        {
            this.Objectcolor = ObjectC;
            this.isReflector = IsItReflector;
            this.Vertices = Vert;
            this.Norm = Normals;
            this.tTexture = Texture;
            this.isOne = false;
            this.kd = kd;
            this.ks = ks;
            this.m = m;
            this.ka = 0.2; // every Object have the same value for I_ambient
            this.P = 10; // Power of cos for reflector
            this.Sun = (0, 400,0); // Position of "Sun"
            this.Lightcolor = Color.White; // Color of "Sun"
            this.fogGradient = 700;
        }
        // ---------------------------------------------------------//
        // function used to change fogGradient - if camera has changed
        public void changedfogGradient(int grad)
        {
            fogGradient = grad;
        }
        // ---------------------------------------------------------//
        // function used on the beginning when filling new triangle
        public void NewVertices(Vector3 CameraPosition, int index1, int index2, int index3, (double, double) v1, (double, double) v2, (double, double) v3, Vector4 ReflectorNormal, Vector4 PR, bool isFog, Vector3 cameraPosition)
        {
            CameraPosition = Vector3.Normalize(CameraPosition);
            V = (CameraPosition.X, CameraPosition.Y, CameraPosition.Z);
            // vertices sorting by y parameter
            (double, double) temp;
            int tempint;
            if (v1.Item2 > v2.Item2)
            {
                temp = v1;
                v1 = v2;
                v2 = temp;
                tempint = index2;
                index2 = index1;
                index1 = tempint;
            }

            if (v3.Item2 < v2.Item2)
            {
                temp = v3;
                v3 = v2;
                v2 = temp;
                tempint = index2;
                index2 = index3;
                index3 = tempint;
                if (v2.Item2 < v1.Item2)
                {
                    temp = v1;
                    v1 = v2;
                    v2 = temp;
                    tempint = index2;
                    index2 = index1;
                    index1 = tempint;
                }
            }

            // defining how many vertices will be on the side where colouring begin
            if (v2.Item1 > v3.Item1 && v2.Item1 > v1.Item1)
                isOne = true;
            else if (v2.Item1 < v1.Item1 && v2.Item1 < v3.Item1)
                isOne = false;
            else if (v1.Item1 == v2.Item1 && v2.Item1 < v3.Item1) 
                isOne = false;
            else if (v1.Item1 == v2.Item1 && v2.Item1 > v3.Item1)
                isOne = true;
            else if ((v1.Item2 - v3.Item2) / (v1.Item1 - v3.Item1) > (v1.Item2 - v2.Item2) / (v1.Item1 - v2.Item1))
                isOne = true;
            else
                isOne = false;

            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.indexv1 = index1;
            this.indexv2 = index2;
            this.indexv3 = index3;
            // defining C1, C2, C3
            // without fog: if Object is reflector then it is it's own colour - white here
            if (isReflector)
            {
                if (isFog)
                {
                    (double, double, double) L2;
                    L2.Item1 = Vertices[index1].Item1 - cameraPosition.X;
                    L2.Item2 = Vertices[index1].Item2 - cameraPosition.Y;
                    L2.Item3 = Vertices[index1].Item3 - cameraPosition.Z;
                    double length = Math.Sqrt(L2.Item1 * L2.Item1 + L2.Item2 * L2.Item2 + L2.Item3 * L2.Item3);
                    double fog = Math.Exp(-Math.Pow((length / fogGradient), 4));
                    int col = (int)(255 * fog);
                    C1 = C2 = C3 = Color.FromArgb(Convert.ToByte(col), Convert.ToByte(col), Convert.ToByte(col));
                }
                else
                {  C1 = C2 = C3 = Objectcolor;
                }
              
                return;
            }
            C1 = ColorInOneVertice(Vertices[index1], Norm[index1], ReflectorNormal, PR, isFog,cameraPosition);
            C2 = ColorInOneVertice(Vertices[index2], Norm[index2], ReflectorNormal, PR, isFog, cameraPosition);
            C3 = ColorInOneVertice(Vertices[index3], Norm[index3], ReflectorNormal, PR, isFog, cameraPosition);
        }
        // ---------------------------------------------------------//
        // calculating color in vertice when normal vector is known //
        private Color ColorInOneVertice((double, double, double) vert, (double, double, double) normal,
                                        Vector4 ReflectorNormal, Vector4 PR, bool isFog, Vector3 cameraPosition)
        {
         
            // at first calculations for light from reflector
            (double, double, double) L2;
            L2.Item1 = vert.Item1 - PR.X;
            L2.Item2 = vert.Item2 - PR.Y;
            L2.Item3 = vert.Item3 - PR.Z;
            double length = Math.Sqrt(L2.Item1 * L2.Item1 + L2.Item2 * L2.Item2 + L2.Item3 * L2.Item3);
            L2 = ((L2.Item1 / length, L2.Item2 / length, L2.Item3 / length));
            double cosDL = ReflectorNormal.X * L2.Item1 + ReflectorNormal.Y * L2.Item2 + ReflectorNormal.Z * L2.Item3;
            if (cosDL < 0)
                cosDL = 0;
            double IR = Math.Pow(cosDL, P);
            Color ReflectorColor = Color.FromArgb((int)(IR*255),(int)(IR*255),(int)(255*IR));
           
            // solid color
            double ro = Objectcolor.R/255.0;
            double go = Objectcolor.G/255.0;
            double bo = Objectcolor.B/255.0;

            (double, double, double) Temp =  CalculateI( vert, Sun, Lightcolor,normal,1,0,0);
            (double, double, double) Temp2 =  CalculateI( vert, (PR.X,PR.Y,PR.Z), ReflectorColor, normal,1,0,0.0001);
           
            int re = (int)((ka+Temp.Item1 + Temp2.Item1) * ro *255);
            int ge = (int)((ka+Temp.Item2 + Temp2.Item2) * go  * 255);
            int be = (int)((ka  + Temp.Item3 + Temp2.Item3)* bo *255);

            if (re > 255)
                re = 255;
            if (ge > 255)
                ge = 255;
            if (be > 255)
                be = 255;
            if (isFog)
            {
                L2.Item1 = vert.Item1 - cameraPosition.X;
                L2.Item2 = vert.Item2 - cameraPosition.Y;
                L2.Item3 = vert.Item3 - cameraPosition.Z;
                length = Math.Sqrt(L2.Item1 * L2.Item1 + L2.Item2 * L2.Item2 + L2.Item3 * L2.Item3);
                double fog = Math.Exp(-Math.Pow((length/fogGradient),4));
                re =  (int)(re*fog);
                ge = (int)(ge * fog);
                be = (int)(be * fog);
            }
            return Color.FromArgb(Convert.ToByte(re), Convert.ToByte(ge), Convert.ToByte(be));

        }

        // calculations of intensity for given position of vertice and it's normal vector, given light point and it's light
        // AC AL and AQ are variables used to calculate distance between point of vertice and light point
        private (double, double, double) CalculateI( (double, double, double) vert, (double, double, double) LightPoint, Color _Lightcolor, (double, double, double) normal,
            double AC, double AL, double AQ)
        {
            (double, double, double) L;
            double cosVR, cosNL, dist;

            L.Item1 = LightPoint.Item1 - vert.Item1;
            L.Item2 = LightPoint.Item2 - vert.Item2;
            L.Item3 = LightPoint.Item3 - vert.Item3;
            dist = Math.Sqrt(L.Item1 * L.Item1 + L.Item2 * L.Item2 + L.Item3 * L.Item3);
            // versor L
            L = ((L.Item1 / dist, L.Item2 / dist, L.Item3 / dist));

            (double, double, double) N = normal;
             cosNL = N.Item1 * L.Item1 + N.Item2 * L.Item2 + N.Item3 * L.Item3;
            if (cosNL <= 0)
            {
                cosNL = 0;
                cosVR = 0;

            }
            else {    
            // vector R
            (double, double, double) R = (2 * cosNL * N.Item1 - L.Item1, 2 * cosNL * N.Item2 - L.Item2, 2 * cosNL * N.Item3 - L.Item3);
             cosVR = V.Item1 * R.Item1 + V.Item2 * R.Item2 + V.Item3 * R.Item3;
          
            if (cosVR < 0)
                cosVR = 0;
            }

            // light color
            double rl = _Lightcolor.R / 255.0;
            double gl = _Lightcolor.G / 255.0;
            double bl = _Lightcolor.B / 255.0;
            double If = 1 / (AC+AL*dist+AQ*dist*dist);
            return ((kd * rl * cosNL + ks * rl * Math.Pow(cosVR, m))*If, (kd * gl * cosNL + ks * gl * Math.Pow(cosVR, m))*If, (kd * bl * cosNL + ks * bl * Math.Pow(cosVR, m))*If);
        }
        // ---------------------------------------------------------//
        // calculating normal vectors using interpolation //
        /*
        private (double, double, double) NormInOneVertice((double, double) point)
        {
            // calculating Normal vectors, which are inside the triangle, using interpolation
            double s = Heron(v1, v2, v3);
            double s1 = Heron(point, v3, v2);
            double s2 = Heron(point, v1, v3);
            double s3 = s - s1 - s2;

            if (s3 < 0) s3 = 0;

            double A = Norm[indexv1].Item1 * s1 / s + Norm[indexv2].Item1 * s2 / s + Norm[indexv3].Item1 * s3 / s;
            double B = Norm[indexv1].Item2 * s1 / s + Norm[indexv2].Item2 * s2 / s + Norm[indexv3].Item2 * s3 / s;
            double C = Norm[indexv1].Item3 * s1 / s + Norm[indexv2].Item3 * s2 / s + Norm[indexv3].Item3 * s3 / s;
            double length = Math.Sqrt(A*A+B*B+C*C);

            return (A/length, B/ length, C/ length);
        }
        
        private (double, double, double) NormInOneVerticeOnTheEdge((double, double) point)
        {
            // calculating Normal vectors, which are on the edge of triangle, using interpolation
            double dy, dyp;
            double A, B, C;
            if (isOne == true)
            {
                // when points are beetwen v3 and v1
                dy = v3.Item2 - v1.Item2;
                dyp = v3.Item2 - point.Item2;
                A = (Norm[indexv1].Item1 * dyp / dy + Norm[indexv3].Item1 * (dy - dyp) / dy);
                B = (Norm[indexv1].Item2 * dyp / dy + Norm[indexv3].Item2 * (dy - dyp) / dy);
                C = (Norm[indexv1].Item3 * dyp / dy + Norm[indexv3].Item3 * (dy - dyp) / dy);
            }
            else
            {
                // are points between v1-v2 or v2-v3?
                if (point.Item2 <= v2.Item2)
                {
                    // between v1 and v2
                    if (v2.Item2 == v1.Item2)
                    {
                        dy = v2.Item1 - v1.Item1;
                        dyp = point.Item1 - v1.Item1;
                        A = (Norm[indexv2].Item1 * dyp / dy + Norm[indexv1].Item1 * (dy - dyp) / dy);
                        B = (Norm[indexv2].Item2 * dyp / dy + Norm[indexv1].Item2 * (dy - dyp) / dy);
                        C = (Norm[indexv2].Item3 * dyp / dy + Norm[indexv1].Item3 * (dy - dyp) / dy);
                    }
                    else
                    {
                        dy = v2.Item2 - v1.Item2;
                        dyp = point.Item2 - v1.Item2;
                        A = (Norm[indexv2].Item1 * dyp / dy + Norm[indexv1].Item1 * (dy - dyp) / dy);
                        B = (Norm[indexv2].Item2 * dyp / dy + Norm[indexv1].Item2 * (dy - dyp) / dy);
                        C = (Norm[indexv2].Item3 * dyp / dy + Norm[indexv1].Item3 * (dy - dyp) / dy);

                    }
                }
                else
                {
                    // between v2 and v3
                    dy = v3.Item2 - v2.Item2;
                    dyp = point.Item2 - v2.Item2;
                    A = (Norm[indexv3].Item1 * dyp / dy + Norm[indexv2].Item1 * (dy - dyp) / dy);
                    B = (Norm[indexv3].Item2 * dyp / dy + Norm[indexv2].Item2 * (dy - dyp) / dy);
                    C = (Norm[indexv3].Item3 * dyp / dy + Norm[indexv2].Item3 * (dy - dyp) / dy);
                }
            }
            double length = Math.Sqrt(A * A + B * B + C * C);
            return (A / length, B / length, C / length);
        }
        */
        // ---------------------------------------------------------//
        // calculating colors using colors interpolation //
        public Color PixelColor((double,double) point)
        {
            if (point == v1) return C1;
            if( point == v2) return C2;
            if (point == v3) return C3;

            double s = Heron(v1, v2, v3);
            double s1 = Heron(point, v3, v2);
            double s2 = Heron(point, v1, v3);
            double s3 = s-s1-s2;
            if (s3 < 0) s3 = 0;
            int temp = (int)(C1.R * s1 / s + C2.R * s2 / s + C3.R * s3 / s);
            int A = Convert.ToByte(temp <= 255 ? temp: 255);
            temp = (int)(C1.G * s1 / s + C2.G * s2 / s + C3.G * s3 / s);
            int B = Convert.ToByte(temp <= 255 ? temp : 255);
            temp = (int)(C1.B * s1 / s + C2.B * s2 / s + C3.B * s3 / s);
            int C = Convert.ToByte(temp <= 255 ? temp : 255);
            return Color.FromArgb(A, B, C);
        }
        public Color PixelOnEdgeColor((double, double) point)
        {
            double dy, dyp;
            // calculating colors, which are on the edge of triangle
            (int, int, int) NewCol;
            if (isOne == true)
            {
                // when points are beetwen v3 and v1
                dy = v3.Item2 - v1.Item2;
                dyp = v3.Item2 - point.Item2;
                NewCol = CalculateRGB(dy,dyp, C3, C1, point);
            }
            else
            {
                // are points between v1-v2 or v2-v3?
                if (point.Item2 <= v2.Item2)
                    // between v1 and v2
                    if (v1.Item2 == v2.Item2)
                    {
                        dy = v2.Item1 - v1.Item1;
                        dyp = v2.Item1 - point.Item1;
                        NewCol = CalculateRGB(dy,dyp, C2, C1, point);
                    }
                    else
                    {
                        dy = v2.Item2 - v1.Item2;
                        dyp = v2.Item2 - point.Item2;
                        NewCol = CalculateRGB(dy,dyp, C2, C1, point);
                    }
                else
                {
                    // between v3 and v2
                    dy = v3.Item2 - v2.Item2;
                    dyp = v3.Item2 - point.Item2;
                    NewCol = CalculateRGB(dy, dyp, C3, C2, point);
                }
            }
            return Color.FromArgb(NewCol.Item1, NewCol.Item2, NewCol.Item3);
        }
        private (int,int,int) CalculateRGB(double dy, double dyp, Color Col1, Color Col2, (double, double) point)
        {
            // when on edge
            int A, B, C;
            int temp;
            temp = (int)(Col2.R * dyp / dy + Col1.R * (dy - dyp) / dy);
            A = Convert.ToByte(temp);
            temp = (int)(Col2.G * dyp / dy + Col1.G * (dy - dyp) / dy);
            B = Convert.ToByte(temp);
            temp = (int)(Col2.B * dyp / dy + Col1.B * (dy - dyp) / dy);
            C = Convert.ToByte(temp);
            return (A, B, C);
        }
        // ---------------------------------------------------------//
        // calculating colors using normals interpolation //
        /*
        public Color PixelColorwithNorm((double, double) point) 
        {
            if (point == v1) return C1;
            if (point == v2) return C2;
            if (point == v3) return C3;


            (double, double, double ) Normal;
            Normal = NormInOneVertice(point);

            (double, double, double) temppoint = ((center.X - point.Item1) / this.radius, (center.Y - point.Item2) / this.radius, 0);
            temppoint.Item3 = Math.Sqrt(1-temppoint.Item1*temppoint.Item1 - temppoint.Item2 * temppoint.Item2);

           // is texture checked or color?
            if(isColor)
                // if color
                return ColorInOneVertice(temppoint, Normal);
            else
                // if texture
                return ColorInOneVerticeTexture(point,temppoint, Normal);

        }
        public Color PixelOnTheEdgeColorwithNorm((double, double) point)
        {

            (double, double, double) Normal;
            Normal = NormInOneVerticeOnTheEdge(point);

            (double, double, double) temppoint = ((center.X - point.Item1) / this.radius, (center.Y - point.Item2) / this.radius, 0);
            double tempSqrt = 1 - temppoint.Item1 * temppoint.Item1 - temppoint.Item2 * temppoint.Item2;

            if (tempSqrt > 0) temppoint.Item3 = Math.Sqrt(tempSqrt);

            // is texture checked or color?
            if (isColor)
                // if color
                return ColorInOneVertice(temppoint, Normal);
            else
                // if texture
                return ColorInOneVerticeTexture(point, temppoint, Normal);

        }
        */
        // function used when texture is checked instead of color //
        private Color ColorInOneVerticeTexture((double,double) vertFromCanvas,(double, double, double) vert, (double, double, double) normal)
        {
            (double, double, double) L;
            L.Item1 = Sun.Item1 - vert.Item1;
            L.Item2 = Sun.Item2 - vert.Item2;
            L.Item3 = Sun.Item3 - vert.Item3;
            double length = Math.Sqrt(L.Item1 * L.Item1 + L.Item2 * L.Item2 + L.Item3 * L.Item3);
            // versor L
            L = ((L.Item1 / length, L.Item2 / length, L.Item3 / length));
            (double, double, double) N = normal;
            double cosNL = N.Item1 * L.Item1 + N.Item2 * L.Item2 + N.Item3 * L.Item3;
            if (cosNL < 0) cosNL = 0;
            // vector R
            (double, double, double) R = (2 * cosNL * N.Item1 - L.Item1, 2 * cosNL * N.Item2 - L.Item2, 2 * cosNL * N.Item3 - L.Item3);
            double cosVR = V.Item1 * R.Item1 + V.Item2 * R.Item2 + V.Item3 * R.Item3;
            if (cosVR < 0) cosVR = 0;

            Color TempCol = tTexture.GetPixel((int)vertFromCanvas.Item1, (int)vertFromCanvas.Item2);
            // solid color
            double ro = TempCol.R / 255.0;
            double go = TempCol.G / 255.0;
            double bo = TempCol.B / 255.0;
            // light color
            double rl = Lightcolor.R / 255.0;
            double gl = Lightcolor.G / 255.0;
            double bl = Lightcolor.B / 255.0;
            // color calculated in point
            int re = (int)((kd * ro * rl * cosNL + ks * ro * rl * Math.Pow(cosVR, m)) * 255);
            int ge = (int)((kd * go * gl * cosNL + ks * go * gl * Math.Pow(cosVR, m)) * 255);
            int be = (int)((kd * bo * bl * cosNL + ks * bo * bl * Math.Pow(cosVR, m)) * 255);

            return Color.FromArgb(Convert.ToByte(re), Convert.ToByte(ge), Convert.ToByte(be));
        }
        // ---------------------------------------------------------//
        // function to calculate field of a triangle v1v2v3 //
        private double Heron((double, double) v1, (double, double) v2, (double, double) v3)
        {            double a = Math.Sqrt((v1.Item1-v2.Item1)* (v1.Item1 - v2.Item1) + (v1.Item2 - v2.Item2)* (v1.Item2 - v2.Item2));
            double b = Math.Sqrt((v1.Item1 - v3.Item1) * (v1.Item1 - v3.Item1) + (v1.Item2 - v3.Item2) * (v1.Item2 - v3.Item2));
            double c = Math.Sqrt((v3.Item1 - v2.Item1) * (v3.Item1 - v2.Item1) + (v3.Item2 - v2.Item2) * (v3.Item2 - v2.Item2));
            double p = ( a + b + c) / 2;
            double P = p * (p - a) * (p - b) * (p - c);
            if (P < 0) return 0;
            return Math.Sqrt(P);
        }
    }
}
