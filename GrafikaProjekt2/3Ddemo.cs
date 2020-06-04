using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;
using System.Windows.Forms;




namespace GrafikaProjekt2

{
    class _3Ddemo
    {
                      
        public class Triangle
        {
            public Vector3 v1;
            public Vector3 v2;
            public Vector3 v3;


            public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
            {
                this.v1 = v1;
                this.v2 = v2;
                this.v3 = v3;
                List<Vector3> tri = new List<Vector3>() { this.v1, this.v2, this.v3 };
            }
        }

        //MultMatrixvector
        public Vector3 MultMatrixVect(Vector3 v1, Matrix4x4 M) {

            Vector3 v2 = new Vector3();
            v2.X = (v1.X * M.M11) + (v1.Y * M.M21) + (v1.Z * M.M31) + M.M41;
            v2.Y = (v1.X * M.M12) + (v1.Y * M.M22) + (v1.Z * M.M32) + M.M42;
            v2.Z = (v1.X * M.M13) + (v1.Y * M.M23) + (v1.Z * M.M33) + M.M43;
            float W = (v1.X * M.M14) + (v1.Y * M.M24) + (v1.Z * M.M34) + M.M44;

            if (W != 0.0f)
            {
                v2.X /= W;
                v2.Y /= W;
                v2.Z /= W;
            }

            return v2;

        }

        //fields here
        public Bitmap bp;
        public PictureBox pic;
        public Matrix4x4 projMatrix, matRotX, matRotZ = new Matrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        public float ftheta;
        public List<Triangle> cube = new List<Triangle>();

        //Constructor here, we will pass the pic in the constroctor when we will work with forms
        public _3Ddemo(PictureBox pic) {

            this.pic = pic;
            
            //create verticies of cibe

            cube = new List<Triangle>() {
            new Triangle(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector3(1.0f, 1.0f, 0.0f)),
            new Triangle(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f)),

            new Triangle(new Vector3(1.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f)),
            new Triangle(new Vector3(1.0f, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 0.0f, 1.0f)),

            new Triangle(new Vector3(1.0f, 0.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.0f, 1.0f, 1.0f)),
            new Triangle(new Vector3(1.0f, 0.0f, 1.0f), new Vector3(0.0f, 1.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f)),

            new Triangle(new Vector3(0.0f, 0.0f, 1.0f), new Vector3(0.0f, 1.0f, 1.0f), new Vector3(0.0f, 1.0f, 0.0f)),
            new Triangle(new Vector3(0.0f, 0.0f, 1.0f), new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f)),

            new Triangle(new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f)),
            new Triangle(new Vector3(0.0f, 1.0f, 0.0f), new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 0.0f)),

            new Triangle(new Vector3(1.0f, 0.0f, 1.0f), new Vector3(0.0f, 0.0f, 1.0f), new Vector3(0.0f, 0.0f, 0.0f)),
            new Triangle(new Vector3(1.0f, 0.0f, 1.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f))
             };

            float fNear = 0.1f;
            float fFar = 1000.0f;
            float fFov = 90.0f;
            float fAspR = (float)pic.Height / (float)pic.Width;
            float fFovR = 1.0f / (float)Math.Tan(fFov * 0.5f / 180.0f * 3.14159f);


            //Projection matrix
            projMatrix.M11 = fAspR * fFovR;
            projMatrix.M22 = fFovR;
            projMatrix.M33 = fFar / (fFar - fNear);
            projMatrix.M43 = (-fFar * fNear) / (fFar - fNear);
            projMatrix.M34 = 1.0f;
            projMatrix.M44 = 0.0f;



        }

        public void viewOnUpdate (TimeSpan elapsedTime)
        {
            bp = new Bitmap(pic.Width, pic.Height);
            double timeTheta = elapsedTime.TotalMilliseconds / 1000;

            ftheta += 1.0f * (float)timeTheta;
            //Rotation by z
            matRotX.M11 = (float)Math.Cos(ftheta);
            matRotX.M12 = (float)Math.Sin(ftheta);
            matRotX.M21 = -(float)Math.Sin(ftheta);
            matRotX.M22 = (float)Math.Cos(ftheta);
            matRotX.M33 = 1;
            matRotX.M44 = 1;

            //Rotation by x
            matRotZ.M11 = 1;
            matRotZ.M22 = (float)Math.Cos(ftheta * 0.5f);
            matRotZ.M23 = (float)Math.Sin(ftheta * 0.5f);
            matRotZ.M32 = -(float)Math.Sin(ftheta * 0.5f);
            matRotZ.M33 = (float)Math.Cos(ftheta * 0.5f); ;
            matRotZ.M44 = 1;


            foreach (Triangle tri in cube)
            {
                Triangle triProject = new Triangle(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));
                Triangle triTranslated = new Triangle(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));
                Triangle triRotatedZX = new Triangle(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));
                Triangle triRotatedZ = new Triangle(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f));


                //rotate in the z axis
                triRotatedZ.v1 = MultMatrixVect(tri.v1, matRotZ);
                triRotatedZ.v2 = MultMatrixVect(tri.v2, matRotZ);
                triRotatedZ.v3 = MultMatrixVect(tri.v3, matRotZ);

                //rotate in the x axis
                triRotatedZX.v1 = MultMatrixVect(triRotatedZ.v1, matRotX);
                triRotatedZX.v2 = MultMatrixVect(triRotatedZ.v2, matRotX);
                triRotatedZX.v3 = MultMatrixVect(triRotatedZ.v3, matRotX);

                triTranslated = triRotatedZX;
                triTranslated.v1.Z = triRotatedZX.v1.Z + 4.9f;
                triTranslated.v2.Z = triRotatedZX.v2.Z + 4.9f;
                triTranslated.v3.Z = triRotatedZX.v3.Z + 4.9f;

                //Multiply vector kazdego trojkonta
                triProject.v1 = MultMatrixVect(triTranslated.v1, projMatrix);
                triProject.v2 = MultMatrixVect(triTranslated.v2, projMatrix);
                triProject.v3 = MultMatrixVect(triTranslated.v3, projMatrix);

                //scale into view
                triProject.v1.X += 1.0f; triProject.v1.Y += 1.0f;
                triProject.v2.X += 1.0f; triProject.v2.Y += 1.0f;
                triProject.v3.X += 1.0f; triProject.v3.Y += 1.0f;

                triProject.v1.X *= 0.5f * bp.Width;
                triProject.v1.Y *= 0.5f * bp.Height;

                triProject.v2.X *= 0.5f * bp.Width;
                triProject.v2.Y *= 0.5f * bp.Height;

                triProject.v3.X *= 0.5f * bp.Width;
                triProject.v3.Y *= 0.5f * bp.Height;

                //here we spawn and fill our triangles
                fillTriangle(triProject, bp);
                DrawTriangle(triProject, bp);


                
            }

            pic.Image = bp;
            pic.Refresh();

        }



        //fill triangle
        public static void fillTriangle(Triangle tri, Bitmap bitmap)
        {
          
            SolidBrush solidBrush = new SolidBrush(Color.FromArgb(160, 150,233));

            Point point1 = new Point((int)tri.v1.X, (int)tri.v1.Y);
            Point point2 = new Point((int)tri.v2.X, (int)tri.v2.Y);
            Point point3 = new Point((int)tri.v3.X, (int)tri.v3.Y);

            Point[] curvePoints = { point1, point2, point3 };


            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillPolygon(solidBrush, curvePoints);
            }

        }

        //draw triangle
        public void DrawTriangle(Triangle tri, Bitmap bitmap) {
            DrawFunctions.AlgorytmPrzyrostowy((int)tri.v1.X, (int)tri.v1.Y, (int)tri.v2.X, (int)tri.v2.Y, ref bitmap);
            DrawFunctions.AlgorytmPrzyrostowy((int)tri.v2.X, (int)tri.v2.Y, (int)tri.v3.X, (int)tri.v3.Y, ref bitmap);
            DrawFunctions.AlgorytmPrzyrostowy((int)tri.v3.X, (int)tri.v3.Y, (int)tri.v1.X, (int)tri.v1.Y, ref bitmap);
        }



    }
}
