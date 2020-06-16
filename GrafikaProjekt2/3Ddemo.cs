using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;
using System.Drawing.Drawing2D;





namespace GrafikaProjekt2

{
    public class Triangle : IComparable<Triangle>
    {
        internal  float dp;
        public Vector4 v1;
        public Vector4 v2;
        public Vector4 v3;


        public Triangle(Vector4 v1, Vector4 v2, Vector4 v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            List<Vector4> tri = new List<Vector4>() { this.v1, this.v2, this.v3 };
        }


        public int CompareTo(Triangle other)
        {
            float first = (v1.Z + v2.Z + v3.Z) / 3.0f;
            float second = (other.v1.Z + other.v2.Z + other.v3.Z) / 3.0f;

            return second.CompareTo(first);

            throw new NotImplementedException();
        }

    }

    class _3Ddemo 
    {


        //fields here
        public Bitmap bp;
        public PictureBox pic;
        public Matrix4x4 projMatrix, matRotX, matRotZ = new Matrix4x4(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        public float ftheta;
        public List<Triangle> cube = new List<Triangle>();

        public Vector4 sipmleCamera;

        //unit vector
        Vector4 lookDir;

        float fYaw;

        //
        float helperDotProd;

        //Constructor here, we will pass the pic in the constroctor when we will work with forms
        public _3Ddemo(PictureBox pic)
        {

            this.pic = pic;

            Loadfigure figure = new Loadfigure();
            figure.readVerticies();
            cube = figure.circle;

            //cube = new List<Triangle>() {
            //new Triangle(new Vector4(0.0f, 0.0f, 0.0f,1), new Vector4(0.0f, 1.0f, 0.0f,1), new Vector4(1.0f, 1.0f, 0.0f,1)),
            //new Triangle(new Vector4(0.0f, 0.0f, 0.0f,1), new Vector4(1.0f, 1.0f, 0.0f,1), new Vector4(1.0f, 0.0f, 0.0f,1)),

            //new Triangle(new Vector4(1.0f, 0.0f, 0.0f,1), new Vector4(1.0f, 1.0f, 0.0f,1), new Vector4(1.0f, 1.0f, 1.0f,1)),
            //new Triangle(new Vector4(1.0f, 0.0f, 0.0f,1), new Vector4(1.0f, 1.0f, 1.0f,1), new Vector4(1.0f, 0.0f, 1.0f,1)),

            //new Triangle(new Vector4(1.0f, 0.0f, 1.0f,1), new Vector4(1.0f, 1.0f, 1.0f,1), new Vector4(0.0f, 1.0f, 1.0f,1)),
            //new Triangle(new Vector4(1.0f, 0.0f, 1.0f,1), new Vector4(0.0f, 1.0f, 1.0f,1), new Vector4(0.0f, 0.0f, 1.0f,1)),

            //new Triangle(new Vector4(0.0f, 0.0f, 1.0f,1), new Vector4(0.0f, 1.0f, 1.0f,1), new Vector4(0.0f, 1.0f, 0.0f,1)),
            //new Triangle(new Vector4(0.0f, 0.0f, 1.0f,1), new Vector4(0.0f, 1.0f, 0.0f,1), new Vector4(0.0f, 0.0f, 0.0f,1)),

            //new Triangle(new Vector4(0.0f, 1.0f, 0.0f,1), new Vector4(0.0f, 1.0f, 1.0f,1), new Vector4(1.0f, 1.0f, 1.0f,1)),
            //new Triangle(new Vector4(0.0f, 1.0f, 0.0f,1), new Vector4(1.0f, 1.0f, 1.0f,1), new Vector4(1.0f, 1.0f, 0.0f,1)),

            //new Triangle(new Vector4(1.0f, 0.0f, 1.0f,1), new Vector4(0.0f, 0.0f, 1.0f,1), new Vector4(0.0f, 0.0f, 0.0f,1)),
            //new Triangle(new Vector4(1.0f, 0.0f, 1.0f,1), new Vector4(0.0f, 0.0f, 0.0f,1), new Vector4(1.0f, 0.0f, 0.0f,1))
            // };



            float fNear = 0.1f;
            float fFar = 1000.0f;
            float fFov = 90.0f;
            float fAspR = (float)pic.Height / (float)pic.Width;

            //Projection matrix
            projMatrix = MatrixMakeProjection(fFov, fAspR, fNear, fFar);


            //тут все хорошо

        }


        public void viewOnUpdate(TimeSpan elapsedTime)
        {
            bp = new Bitmap(pic.Width, pic.Height);
            double timeV = elapsedTime.TotalMilliseconds / 1000;

            ftheta += 1.0f * (float)timeV;

            if ((Keyboard.GetKeyStates(Key.Up) & KeyStates.Down) > 0)
            {
                sipmleCamera.Y += 0.01f + (float)timeV;
            }
            if ((Keyboard.GetKeyStates(Key.Down) & KeyStates.Down) > 0)
            {
                sipmleCamera.Y -= 0.01f + (float)timeV;
            }

            if ((Keyboard.GetKeyStates(Key.Left) & KeyStates.Down) > 0)
            {
                sipmleCamera.X += 0.01f + (float)timeV;
            }
            if ((Keyboard.GetKeyStates(Key.Right) & KeyStates.Down) > 0)
            {
                sipmleCamera.X -= 0.01f + (float)timeV;
            }

            Vector4 vForaward = lookDir * (8.0f * (float)timeV);


            if ((Keyboard.GetKeyStates(Key.W) & KeyStates.Down) > 0)
            {
                sipmleCamera = addVector(sipmleCamera, vForaward);
            }
            if ((Keyboard.GetKeyStates(Key.S) & KeyStates.Down) > 0)
            {
                sipmleCamera = subVector( sipmleCamera, vForaward);
            }

            if ((Keyboard.GetKeyStates(Key.A) & KeyStates.Down) > 0)
            {
                fYaw += 0.05f + (float)timeV;
            }
            if ((Keyboard.GetKeyStates(Key.D) & KeyStates.Down) > 0)
            {
                fYaw -= 0.05f + (float)timeV;
            }



            //Rotation by z
            matRotZ = MatrixRotateZ(ftheta);
            //Rotation by x
            matRotX = MatrixRotateX(ftheta * 0.5f);
            //тут все хорошо тоже

            Matrix4x4 matTrans;
            matTrans = MatrixTranslation(0, 0, 6);

            Matrix4x4 matWorld;
            matWorld = MatrixIdentity();
            matWorld = matRotZ;
            matWorld = matWorld * matTrans;


            Vector4 vUp = new Vector4(0, 1, 0, 1);
            Vector4 vTarget = new Vector4(0,0,1,1);
            Matrix4x4 matCameraR = MatrixRotateY(fYaw);
            lookDir = MatrixMultVector(vTarget, matCameraR);
            vTarget = addVector(sipmleCamera , lookDir);

            Matrix4x4 matCamera = MatrixPointAt(sipmleCamera, vTarget, vUp);

            Matrix4x4 matView = Matrix_QuickInverse(matCamera);


            List<Triangle> listTrianglesToRastr = new List<Triangle>();

            foreach (Triangle tri in cube)
            {
                //тут начинается херня
                Triangle triProject = new Triangle(new Vector4(), new Vector4(), new Vector4());
                Triangle triTransform = new Triangle(new Vector4(), new Vector4(), new Vector4());
                Triangle triView = new Triangle(new Vector4(), new Vector4(), new Vector4());
                //Triangle triTranslated = new Triangle(new Vector4(), new Vector4(), new Vector4());
                //Triangle triRotatedZX = new Triangle(new Vector4(), new Vector4(), new Vector4());
                //Triangle triRotatedZ = new Triangle(new Vector4(), new Vector4(), new Vector4());

                //new code rotate zx
                triTransform.v1 = MatrixMultVector(tri.v1, matWorld);
                triTransform.v2 = MatrixMultVector(tri.v2, matWorld);
                triTransform.v3 = MatrixMultVector(tri.v3, matWorld);


                


                //try to show cube without backside lines
                Vector4 normal, line1, line2 = new Vector4();
                line1 = subVector( triTransform.v2, triTransform.v1);
                line2 = subVector(triTransform.v3, triTransform.v1);

                normal = VectorCrossProd(line1, line2);

                normal = VectorNormalise(normal);


                Vector4 vcameraRay = subVector( triTransform.v1,  sipmleCamera);

                if (VectordotProduct(normal, vcameraRay) < 0)
                {

                    Vector4 light = new Vector4(0.0f, -1.0f, -1.0f, 1.0f);
                    //float length = (float)Math.Sqrt(light.X * light.X + light.Y * light.Y + light.Z * light.Z);
                    //light.X /= length;
                    //light.Y /= length;
                    //light.Z /= length;

                    light = VectorNormalise(light);

                    //// How similar is normal to light direction
                    float dp = normal.X * light.X + normal.Y * light.Y + normal.Z * light.Z;

                    Console.WriteLine(dp);


                    


                    //World space into view
                    triView.v1 = MatrixMultVector(triTransform.v1, matView);
                    triView.v2 = MatrixMultVector(triTransform.v2, matView);
                    triView.v3 = MatrixMultVector(triTransform.v3, matView);

                    //project triangles 3d -2d
                    triProject.v1 = MatrixMultVector(triView.v1, projMatrix);
                    triProject.v2 = MatrixMultVector(triView.v2, projMatrix);
                    triProject.v3 = MatrixMultVector(triView.v3, projMatrix);

                    //normalize
                    triProject.v1 = triProject.v1 / triProject.v1.W;
                    triProject.v2 = triProject.v2 / triProject.v2.W;
                    triProject.v3 = triProject.v3 / triProject.v3.W;

                    //scale into view
                    Vector4 vOffset = new Vector4(1, 1, 0, 1);
                    triProject.v1 = triProject.v1 + vOffset;
                    triProject.v2 = triProject.v2 + vOffset;
                    triProject.v3 = triProject.v3 + vOffset;


                    triProject.v1.X *= 0.5f * bp.Width;
                    triProject.v1.Y *= 0.5f * bp.Height;

                    triProject.v2.X *= 0.5f * bp.Width;
                    triProject.v2.Y *= 0.5f * bp.Height;

                    triProject.v3.X *= 0.5f * bp.Width;
                    triProject.v3.Y *= 0.5f * bp.Height;

                    triProject.dp = dp;
                    listTrianglesToRastr.Add(triProject);

                }

            }
            //end of foreach loop

            //algorytm malarski, sortujemy według osi z
            listTrianglesToRastr.Sort();


            foreach (var triProject in listTrianglesToRastr)
            {


                fillTriangle(triProject, bp);
                //DrawTriangle(triProject, bp);
                
                
            }

            pic.Image = bp;
            pic.Refresh();

        }





        //helper functions

        //fill triangle
        public void fillTriangle(Triangle tri, Bitmap bitmap)
        {
            
            float dp = tri.dp;
            float R = dp * 255;
            float G = dp * 255;
            float B = dp * 255;
            //Console.WriteLine(dp);
            if (R < 0)
            {
                R = 0;
            }
            if (G < 0)
            {
                G = 0;
            }
            if (B < 0)
            {
                B = 0;
            }



            SolidBrush solidBrush = new SolidBrush(Color.FromArgb((int)R,(int)G, (int)B));

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
        //public void DrawTriangle(Triangle tri, Bitmap bitmap)
        //{
        //    DrawFunctions.Brsenham((int)tri.v1.X, (int)tri.v1.Y, (int)tri.v2.X, (int)tri.v2.Y, ref bitmap);
        //    DrawFunctions.Brsenham((int)tri.v2.X, (int)tri.v2.Y, (int)tri.v3.X, (int)tri.v3.Y, ref bitmap);
        //    DrawFunctions.Brsenham((int)tri.v3.X, (int)tri.v3.Y, (int)tri.v1.X, (int)tri.v1.Y, ref bitmap);
        //}


        //helper functions 


        public Vector4 addVector(Vector4 v1, Vector4 v2)
        {
            Vector4 v = new Vector4();
            v.X = v1.X + v2.X;
            v.Y = v1.Y + v2.Y;
            v.Z = v1.Z + v2.Z;
            return v;
        }

        public Vector4 subVector(Vector4 v1, Vector4 v2)
        {
            Vector4 v = new Vector4();
            v.X = v1.X - v2.X;
            v.Y = v1.Y - v2.Y;
            v.Z = v1.Z - v2.Z;
            return v;
        }

        public float VectordotProduct(Vector4 v1, Vector4 v2)
        {

            return (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);
        }

        public float VectorLength(Vector4 v)
        {
            return (float)Math.Sqrt(VectordotProduct(v, v));
        }
        public Vector4 VectorNormalise(Vector4 v)
        {

            float l = VectorLength(v);
            return new Vector4(v.X / l, v.Y / l, v.Z / l, 1);
        }

        public Vector4 VectorCrossProd(Vector4 v1, Vector4 v2)
        {
            Vector4 v = new Vector4();
            v.X = v1.Y * v2.Z - v1.Z * v2.Y;
            v.Y = v1.Z * v2.X - v1.X * v2.Z;
            v.Z = v1.X * v2.Y - v1.Y * v2.X;
            return v;
        }

        public Matrix4x4 MatrixIdentity()
        {
            Matrix4x4 mat = new Matrix4x4();
            mat.M11 = 1.0f;
            mat.M22 = 1.0f;
            mat.M33 = 1.0f;
            mat.M44 = 1.0f;
            return mat;
        }

        public Matrix4x4 MatrixRotateX(float angle)
        {
            Matrix4x4 mat = new Matrix4x4();
            mat.M11 = 1.0f;
            mat.M22 = (float)Math.Cos(angle);
            mat.M23 = (float)Math.Sin(angle);
            mat.M32 = -(float)Math.Sin(angle);
            mat.M33 = (float)Math.Cos(angle);
            mat.M44 = 1.0f;
            return mat;
        }

        public Matrix4x4 MatrixRotateY(float angle)
        {
            Matrix4x4 mat = new Matrix4x4();
            mat.M11 = (float)Math.Cos(angle);
            mat.M13 = (float)Math.Sin(angle);
            mat.M31 = -(float)Math.Sin(angle);
            mat.M22 = 1.0f;
            mat.M33 = (float)Math.Cos(angle); ;
            mat.M44 = 1.0f;
            return mat;
        }

        public Matrix4x4 MatrixRotateZ(float angle)
        {
            Matrix4x4 mat = new Matrix4x4();
            mat.M11 = (float)Math.Cos(angle);
            mat.M12 = (float)Math.Sin(angle);
            mat.M21 = -(float)Math.Sin(angle);
            mat.M22 = (float)Math.Cos(angle);
            mat.M33 = 1.0f;
            mat.M44 = 1.0f;
            return mat;
        }

        public Matrix4x4 MatrixTranslation(float x, float y, float z)
        {
            Matrix4x4 mat = new Matrix4x4(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
            mat.M11 = 1.0f;
            mat.M22 = 1.0f;
            mat.M33 = 1.0f;
            mat.M44 = 1.0f;
            mat.M41 = x;
            mat.M42 = y;
            mat.M43 = z;

            return mat;
        }

        public Matrix4x4 MatrixMakeProjection(float fFovDegrees, float fAspectRatio, float fNear, float fFar)
        {
            Matrix4x4 mat = new Matrix4x4(0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
            float fFovRad = 1.0f / (float)Math.Tan(fFovDegrees * 0.5f / 180.0f * 3.14159f);
            mat.M11 = fAspectRatio * fFovRad;
            mat.M22 = fFovRad;
            mat.M33 = fFar / (fFar - fNear);
            mat.M43 = (-fFar * fNear) / (fFar - fNear);
            mat.M34 = 1.0f;
            mat.M44 = 0.0f;

            return mat;
        }


        public Vector4 MatrixMultVector(Vector4 v1, Matrix4x4 M)
        {

            Vector4 v2 = new Vector4();
            v2.X = (v1.X * M.M11) + (v1.Y * M.M21) + (v1.Z * M.M31) + M.M41;
            v2.Y = (v1.X * M.M12) + (v1.Y * M.M22) + (v1.Z * M.M32) + M.M42;
            v2.Z = (v1.X * M.M13) + (v1.Y * M.M23) + (v1.Z * M.M33) + M.M43;
            v2.W = (v1.X * M.M14) + (v1.Y * M.M24) + (v1.Z * M.M34) + M.M44;

             return v2;

        }

        Matrix4x4 MatrixPointAt(Vector4 pos, Vector4 target, Vector4 up)
        {
            Vector4 newForward = target - pos;
            newForward = VectorNormalise(newForward);

            Vector4 a = newForward * VectordotProduct(up, newForward);
            Vector4 newUp = up - a;
            newUp = VectorNormalise(newUp);

            Vector4 newRight = VectorCrossProd(newUp, newForward);

            Matrix4x4 matrix;

            matrix.M11 = newRight.X;
            matrix.M12 = newRight.Y;
            matrix.M13 = newRight.Y;

            matrix.M11 = newRight.X;
            matrix.M12 = newRight.Y;
            matrix.M13 = newRight.Z;
            matrix.M14 = 0.0f;
            matrix.M21 = newUp.X;
            matrix.M22 = newUp.Y;
            matrix.M23 = newUp.Z;
            matrix.M24 = 0.0f;
            matrix.M31 = newForward.X;
            matrix.M32 = newForward.Y;
            matrix.M33 = newForward.Z;
            matrix.M34 = 0.0f;
            matrix.M41 = pos.X;
            matrix.M42 = pos.Y;
            matrix.M43 = pos.Z;
            matrix.M44 = 1.0f;
            return matrix;
        }

        Matrix4x4 Matrix_QuickInverse(Matrix4x4 m) // Only for Rotation/Translation Matrices
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.M11 = m.M11;
            matrix.M12 = m.M21;
            matrix.M13 = m.M31;
            matrix.M14 = 0.0f;
            matrix.M21 = m.M12;
            matrix.M22 = m.M22;
            matrix.M23 = m.M32;
            matrix.M24 = 0.0f;
            matrix.M31 = m.M13;
            matrix.M32 = m.M23;
            matrix.M33 = m.M33;
            matrix.M34 = 0.0f;
            matrix.M41 = -(m.M41 * matrix.M11 + m.M42 * matrix.M21 + m.M43 * matrix.M31);
            matrix.M42 = -(m.M41 * matrix.M12 + m.M42 * matrix.M22 + m.M43 * matrix.M32);
            matrix.M43 = -(m.M41 * matrix.M13 + m.M42 * matrix.M23 + m.M43 * matrix.M33);
            matrix.M44 = 1.0f;
            return matrix;
        }

    }
}