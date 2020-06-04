﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
namespace _3Dengine
{
    class Engine
    {
        protected class point3D
        {
            public float x, y, z;
            public point3D(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }
        protected class triangle
        {
            List<point3D> tri;
            public triangle(point3D a, point3D b, point3D c)
            {
                tri = new List<point3D>();
                tri.Add(a);
                tri.Add(b);
                tri.Add(c);
            }
            public point3D this[int index]
            {
                get
                {
                    return tri[index];
                }
                set
                {
                    tri[index] = value;
                }
            }
        }

        List<triangle> meshCube;
        float[,] matProj = new float[4, 4];
        Bitmap bp;
        PictureBox mainPic;
        float fTheta;
        point3D Camera;
        private void MultiplyMatrix(point3D i, point3D o, float[,] mProj)
        {
            o.x = i.x * mProj[0, 0] + i.y * mProj[1, 0] + i.z * mProj[2, 0] + mProj[3, 0];
            o.y = i.x * mProj[0, 1] + i.y * mProj[1, 1] + i.z * mProj[2, 1] + mProj[3, 1];
            o.z = i.x * mProj[0, 2] + i.y * mProj[1, 2] + i.z * mProj[2, 2] + mProj[3, 2];
            float w = i.x * mProj[0, 3] + i.y * mProj[1, 3] + i.z * mProj[2, 3] + mProj[3, 3];
            if (w != 0.0f)
            {
                o.x /= w; o.y /= w; o.z /= w;
            }
        }
        public Engine(PictureBox mainPic)
        {
            this.mainPic = mainPic;
            Camera = new point3D(0, 0, 0);
            meshCube = new List<triangle>();
            // south
            meshCube.Add(new triangle(new point3D(0, 0, 0), new point3D(0, 1, 0), new point3D(1, 1, 0)));
            meshCube.Add(new triangle(new point3D(0, 0, 0), new point3D(1, 1, 0), new point3D(1, 0, 0)));
            // east
            meshCube.Add(new triangle(new point3D(1, 0, 0), new point3D(1, 1, 0), new point3D(1, 1, 1)));
            meshCube.Add(new triangle(new point3D(1, 0, 0), new point3D(1, 1, 1), new point3D(1, 0, 1)));
            // north 
            meshCube.Add(new triangle(new point3D(1, 0, 1), new point3D(1, 1, 1), new point3D(0, 1, 1)));
            meshCube.Add(new triangle(new point3D(1, 0, 1), new point3D(0, 1, 1), new point3D(0, 0, 1)));
            // west
            meshCube.Add(new triangle(new point3D(0, 0, 1), new point3D(0, 1, 1), new point3D(0, 1, 0)));
            meshCube.Add(new triangle(new point3D(0, 0, 1), new point3D(0, 1, 0), new point3D(0, 0, 0)));
            // top
            meshCube.Add(new triangle(new point3D(0, 1, 0), new point3D(0, 1, 1), new point3D(1, 1, 1)));
            meshCube.Add(new triangle(new point3D(0, 1, 0), new point3D(1, 1, 1), new point3D(1, 1, 0)));
            // botton
            meshCube.Add(new triangle(new point3D(1, 0, 1), new point3D(0, 0, 1), new point3D(0, 0, 0)));
            meshCube.Add(new triangle(new point3D(1, 0, 1), new point3D(0, 0, 0), new point3D(1, 0, 0)));
            float fNear = 0.1f;
            float fFar = 1000.0f;
            float fFov = 90.0f;
            float fAspectRatio = (float)mainPic.Height / (float)mainPic.Width;
            float fFovRad = 1.0f / (float)Math.Tan(fFov * 0.5f / 180.0f * 3.14159f);
            matProj[0, 0] = fAspectRatio * fFovRad;
            matProj[1, 1] = fFovRad;
            matProj[2, 2] = fFar / (fFar - fNear);
            matProj[3, 2] = (-fFar * fNear) / (fFar - fNear);
            matProj[2, 3] = 1.0f;
            matProj[3, 3] = 0.0f;
        }
        public void UserView(TimeSpan time)
        {
            float[,] matRotZ = new float[4, 4];
            float[,] matRotX = new float[4, 4];
            double a = time.TotalMilliseconds / 1000;
            bp = new Bitmap(mainPic.Width, mainPic.Height);
            fTheta += 1.0f * (float)a;
            matRotZ[0, 0] = (float)Math.Cos(fTheta);
            matRotZ[0, 1] = (float)Math.Sin(fTheta);
            matRotZ[1, 0] = -(float)Math.Sin(fTheta);
            matRotZ[1, 1] = (float)Math.Cos(fTheta);
            matRotZ[2, 2] = 1;
            matRotZ[3, 3] = 1;
            // Rotation X
            matRotX[0, 0] = 1;
            matRotX[1, 1] = (float)Math.Cos(fTheta * 0.5f);
            matRotX[1, 2] = (float)Math.Sin(fTheta * 0.5f);
            matRotX[2, 1] = -(float)Math.Sin(fTheta * 0.5f);
            matRotX[2, 2] = (float)Math.Cos(fTheta * 0.5f);
            matRotX[3, 3] = 1;
            foreach (triangle tri in meshCube)
            {
                triangle triaProject = new triangle(new point3D(0, 0, 0), new point3D(0, 0, 0), new point3D(0, 0, 0));
                triangle triTranslated = new triangle(new point3D(0, 0, 0), new point3D(0, 0, 0), new point3D(0, 0, 0));
                triangle triRotatedZ = new triangle(new point3D(0, 0, 0), new point3D(0, 0, 0), new point3D(0, 0, 0));
                triangle triRotatedZX = new triangle(new point3D(0, 0, 0), new point3D(0, 0, 0), new point3D(0, 0, 0));
                // Z
                MultiplyMatrix(tri[0], triRotatedZ[0], matRotZ);
                MultiplyMatrix(tri[1], triRotatedZ[1], matRotZ);
                MultiplyMatrix(tri[2], triRotatedZ[2], matRotZ);
                // X
                MultiplyMatrix(triRotatedZ[0], triRotatedZX[0], matRotX);
                MultiplyMatrix(triRotatedZ[1], triRotatedZX[1], matRotX);
                MultiplyMatrix(triRotatedZ[2], triRotatedZX[2], matRotX);
                // Offset into the screen
                triTranslated = triRotatedZX;
                triTranslated[0].z = triRotatedZX[0].z + 8.0f;
                triTranslated[1].z = triRotatedZX[1].z + 8.0f;
                triTranslated[2].z = triRotatedZX[2].z + 8.0f;
                point3D normal = new point3D(0, 0, 0);
                point3D line1 = new point3D(0, 0, 0);
                point3D line2 = new point3D(0, 0, 0);
                line1.x = triTranslated[1].x - triTranslated[0].x;
                line1.y = triTranslated[1].y - triTranslated[0].y;
                line1.z = triTranslated[1].z - triTranslated[0].z;
                line2.x = triTranslated[2].x - triTranslated[0].x;
                line2.y = triTranslated[2].y - triTranslated[0].y;
                line2.z = triTranslated[2].z - triTranslated[0].z;
                normal.x = line1.y * line2.z - line1.z * line2.y;
                normal.y = line1.z * line2.x - line1.x * line2.z;
                normal.z = line1.x * line2.y - line1.y * line2.x;
                // It's normally normal to normalise the 

                float l = (float)Math.Sqrt(normal.x * normal.x + normal.y * normal.y + normal.z * normal.z);
                normal.x /= l; normal.y /= l; normal.z /= l;
                if (normal.x * (triTranslated[0].x - Camera.x) +
                    normal.y * (triTranslated[0].y - Camera.y) +
                    normal.z * (triTranslated[0].z - Camera.z) < 0.0f)
                {
                    // Illumination
                    point3D light_direction = new point3D(0.0f, 0.0f, -1.0f);
                    float li = (float)Math.Sqrt(light_direction.x * light_direction.x + light_direction.y * light_direction.y + light_direction.z * light_direction.z);
                    light_direction.x /= li; light_direction.y /= li; light_direction.z /= li;
                    // How similar is normal to light direction
                    float dp = normal.x * light_direction.x + normal.y * light_direction.y + normal.z * light_direction.z;

                    MultiplyMatrix(triTranslated[0], triaProject[0], matProj);
                    MultiplyMatrix(triTranslated[1], triaProject[1], matProj);
                    MultiplyMatrix(triTranslated[2], triaProject[2], matProj);
                    //Scale
                    triaProject[0].x += 1.0f; triaProject[0].y += 1.0f;
                    triaProject[1].x += 1.0f; triaProject[1].y += 1.0f;
                    triaProject[2].x += 1.0f; triaProject[2].y += 1.0f;
                    triaProject[0].x *= (0.5f * (float)mainPic.Width);
                    triaProject[0].y *= (0.5f * (float)mainPic.Height);
                    triaProject[1].x *= (0.5f * (float)mainPic.Width);
                    triaProject[1].y *= (0.5f * (float)mainPic.Height);
                    triaProject[2].x *= (0.5f * (float)mainPic.Width);
                    triaProject[2].y *= (0.5f * (float)mainPic.Height);
                    float R = dp * 255;
                    float G = dp * 0;
                    float B = dp * 0;
                    // Draw
                    SolidBrush solidBrush = new SolidBrush(Color.FromArgb((int)R, (int)G, (int)B));
                    Point point1 = new Point((int)triaProject[0].x, (int)triaProject[0].y);
                    Point point2 = new Point((int)triaProject[1].x, (int)triaProject[1].y);
                    Point point3 = new Point((int)triaProject[2].x, (int)triaProject[2].y);
                    Point[] curvePoints = { point1, point2, point3 };

                    using (var graphics = Graphics.FromImage(bp))
                    {
                        graphics.FillPolygon(solidBrush, curvePoints);
                    }
                    //Line.BresenhamLine((int)triaProject[0].x, (int)triaProject[0].y, (int)triaProject[1].x, (int)triaProject[1].y, ref bp);
                    //Line.BresenhamLine((int)triaProject[1].x, (int)triaProject[1].y, (int)triaProject[2].x, (int)triaProject[2].y, ref bp);
                    //Line.BresenhamLine((int)triaProject[2].x, (int)triaProject[2].y, (int)triaProject[0].x, (int)triaProject[0].y, ref bp);
                }
            }
            mainPic.Image = bp;
            mainPic.Refresh();
        }
    }
}