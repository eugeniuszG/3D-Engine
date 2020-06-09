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
    public class Triangle
    {
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

        //
        float helperDotProd;

        //Constructor here, we will pass the pic in the constroctor when we will work with forms
        public _3Ddemo(PictureBox pic)
        {

            this.pic = pic;

            //create verticies of cibe

            cube = new List<Triangle>() {
            new Triangle(new Vector4(0.0f, 0.0f, 0.0f,1), new Vector4(0.0f, 1.0f, 0.0f,1), new Vector4(1.0f, 1.0f, 0.0f,1)),
            new Triangle(new Vector4(0.0f, 0.0f, 0.0f,1), new Vector4(1.0f, 1.0f, 0.0f,1), new Vector4(1.0f, 0.0f, 0.0f,1)),

            new Triangle(new Vector4(1.0f, 0.0f, 0.0f,1), new Vector4(1.0f, 1.0f, 0.0f,1), new Vector4(1.0f, 1.0f, 1.0f,1)),
            new Triangle(new Vector4(1.0f, 0.0f, 0.0f,1), new Vector4(1.0f, 1.0f, 1.0f,1), new Vector4(1.0f, 0.0f, 1.0f,1)),

            new Triangle(new Vector4(1.0f, 0.0f, 1.0f,1), new Vector4(1.0f, 1.0f, 1.0f,1), new Vector4(0.0f, 1.0f, 1.0f,1)),
            new Triangle(new Vector4(1.0f, 0.0f, 1.0f,1), new Vector4(0.0f, 1.0f, 1.0f,1), new Vector4(0.0f, 0.0f, 1.0f,1)),

            new Triangle(new Vector4(0.0f, 0.0f, 1.0f,1), new Vector4(0.0f, 1.0f, 1.0f,1), new Vector4(0.0f, 1.0f, 0.0f,1)),
            new Triangle(new Vector4(0.0f, 0.0f, 1.0f,1), new Vector4(0.0f, 1.0f, 0.0f,1), new Vector4(0.0f, 0.0f, 0.0f,1)),

            new Triangle(new Vector4(0.0f, 1.0f, 0.0f,1), new Vector4(0.0f, 1.0f, 1.0f,1), new Vector4(1.0f, 1.0f, 1.0f,1)),
            new Triangle(new Vector4(0.0f, 1.0f, 0.0f,1), new Vector4(1.0f, 1.0f, 1.0f,1), new Vector4(1.0f, 1.0f, 0.0f,1)),

            new Triangle(new Vector4(1.0f, 0.0f, 1.0f,1), new Vector4(0.0f, 0.0f, 1.0f,1), new Vector4(0.0f, 0.0f, 0.0f,1)),
            new Triangle(new Vector4(1.0f, 0.0f, 1.0f,1), new Vector4(0.0f, 0.0f, 0.0f,1), new Vector4(1.0f, 0.0f, 0.0f,1))
             };

            //Loadfigure figure = new Loadfigure();
            //figure.readVerticies();
            //cube = figure.circle;

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
            sipmleCamera = new Vector4();
            double timeTheta = elapsedTime.TotalMilliseconds / 1000;

            ftheta += 1.0f * (float)timeTheta;
            //Rotation by z
            matRotZ = MatrixRotateZ(ftheta);
            //Rotation by x
            matRotX = MatrixRotateX(ftheta * 0.5f);
            //тут все хорошо тоже

            Matrix4x4 matTrans;
            matTrans = MatrixTranslation(0, 0, 3);

            Matrix4x4 matWorld;
            matWorld = MatrixIdentity();
            //matWorld = matRotZ * matRotX;
            matWorld = matWorld * matTrans;


            List<Triangle> listTrianglesToRastr = new List<Triangle>();


            foreach (Triangle tri in cube)
            {
                //тут начинается херня
                Triangle triProject = new Triangle(new Vector4(), new Vector4(), new Vector4());
                Triangle triTransform = new Triangle(new Vector4(), new Vector4(), new Vector4());
                //Triangle triTranslated = new Triangle(new Vector4(), new Vector4(), new Vector4());
                //Triangle triRotatedZX = new Triangle(new Vector4(), new Vector4(), new Vector4());
                //Triangle triRotatedZ = new Triangle(new Vector4(), new Vector4(), new Vector4());

                //new code rotate zx
                triTransform.v1 = MatrixMultVector(tri.v1, matWorld);
                triTransform.v2 = MatrixMultVector(tri.v2, matWorld);
                triTransform.v3 = MatrixMultVector(tri.v3, matWorld);

                //noramlize 
                //triTransform.v1.

                ////rotate in the z axis
                //triRotatedZ.v1 = MatrixMultVector(tri.v1, matRotZ);
                //triRotatedZ.v2 = MatrixMultVector(tri.v2, matRotZ);
                //triRotatedZ.v3 = MatrixMultVector(tri.v3, matRotZ);

                ////rotate in the x axis
                //triRotatedZX.v1 = MatrixMultVector(triRotatedZ.v1, matRotX);
                //triRotatedZX.v2 = MatrixMultVector(triRotatedZ.v2, matRotX);
                //triRotatedZX.v3 = MatrixMultVector(triRotatedZ.v3, matRotX);

                //triTranslated = triRotatedZX;
                //triTranslated.v1.Z = triRotatedZX.v1.Z + 3.0f;
                //triTranslated.v2.Z = triRotatedZX.v2.Z + 3.0f;
                //triTranslated.v3.Z = triRotatedZX.v3.Z + 3.0f;

                //try to show cube without backside lines
                Vector4 normal, line1, line2 = new Vector4();
                line1 = triTransform.v2 - triTransform.v1;
                line2 = triTransform.v3 - triTransform.v1;

                normal = VectorCrossProd(line1, line2);

                normal = VectorNormalise(normal);


                //line1.X = triTransform.v2.X - triTransform.v1.X;
                //line1.Y = triTransform.v2.Y - triTransform.v1.Y;
                //line1.Z = triTransform.v2.Z - triTransform.v1.Z;

                //line2.X = triTransform.v3.X - triTransform.v1.X;
                //line2.Y = triTransform.v3.Y - triTransform.v1.Y;
                //line2.Z = triTransform.v3.Z - triTransform.v1.Z;

                //normal.X = (line1.Y * line2.Z) - (line1.Z * line2.Y);
                //normal.Y = (line1.Z * line2.X) - (line1.X * line2.Z);
                //normal.Z = (line1.X * line2.Y) - (line1.Y * line2.X);

                //float l = (float)Math.Sqrt(normal.X * normal.X + normal.Y * normal.Y + normal.Z * normal.Z);
                //normal.X /= l; normal.Y /= l; normal.Z /= l;

                //float dotProduct = normal.X * (triTransform.v1.X - sipmleCamera.X) +
                //                    normal.Y * (triTransform.v1.Y - sipmleCamera.Y) +
                //                    normal.Z * (triTransform.v1.Z - sipmleCamera.Z);

                Vector4 vcameraRay = triTransform.v1 - sipmleCamera;

                if (VectordotProduct(normal, vcameraRay) < 0)
                {

                    Vector4 light = new Vector4(0.0f, 1.0f, -1.0f, 1.0f);
                    //float tmp = (float)Math.Sqrt(light.X * light.X + light.Y * light.Y + light.Z * light.Z);
                    //light.X /= tmp;
                    //light.Y /= tmp;
                    //light.Z /= tmp;
                    light = VectorNormalise(light);


                    float dp = Math.Max(0.1f, VectordotProduct(light, normal));

                    helperDotProd = dp;

                    //Multiply vector kazdego trojkonta
                    triProject.v1 = MatrixMultVector(triTransform.v1, projMatrix);
                    triProject.v2 = MatrixMultVector(triTransform.v2, projMatrix);
                    triProject.v3 = MatrixMultVector(triTransform.v3, projMatrix);

                    //normalize
                    triProject.v1 = triProject.v1 / triProject.v1.W;
                    triProject.v2 = triProject.v2 / triProject.v2.W;
                    triProject.v3 = triProject.v3 / triProject.v3.W;

                    //scale into view
                    Vector4 vOffset = new Vector4(1, 1, 0, 1);
                    triProject.v1 = triProject.v1 + vOffset;
                    triProject.v2 = triProject.v2 + vOffset;
                    triProject.v3 = triProject.v3 + vOffset;

                    //triProject.v1.X += 1.0f; triProject.v1.Y += 1.0f;
                    //triProject.v2.X += 1.0f; triProject.v2.Y += 1.0f;
                    //triProject.v3.X += 1.0f; triProject.v3.Y += 1.0f;

                    triProject.v1.X *= 0.5f * bp.Width;
                    triProject.v1.Y *= 0.5f * bp.Height;

                    triProject.v2.X *= 0.5f * bp.Width;
                    triProject.v2.Y *= 0.5f * bp.Height;

                    triProject.v3.X *= 0.5f * bp.Width;
                    triProject.v3.Y *= 0.5f * bp.Height;

                    listTrianglesToRastr.Add(triProject);

                }

            }





            //end of foreach loop

            //algorytm malarski, sortujemy według osi z
            listTrianglesToRastr.Sort(delegate (Triangle t1, Triangle t2)
            {
                return (t1.v1.Z + t1.v2.Z + t1.v3.Z / 3.0f).CompareTo(t2.v1.Z + t2.v2.Z + t2.v3.Z / 3.0f);
            });




            foreach (var triProject in listTrianglesToRastr)
            {
                Console.WriteLine((triProject.v3.Z + triProject.v3.Z + triProject.v3.Z) / 3.0f);
                fillTriangle(triProject, bp, helperDotProd);
                DrawTriangle(triProject, bp);
            }

            pic.Image = bp;
            pic.Refresh();

        }




        //Matrix4x4 MatrixPointAt(Vector3 pos, Vector3 target, Vector3 up)
        //{
        //    Vector3 newForward = target - pos;
        //    newForward = VectorNormalise(newForward);

        //    Vector3 a = newForward * dotProduct(up, newForward);
        //    Vector3 newUp = up - a;
        //    newUp = VectorNormalise(newUp);

        //    Vector3 newRight = VectorCrossProd(newUp, newForward);

        //    Matrix4x4 matrix;

        //    matrix.M11 = newRight.X;
        //    matrix.M12 = newRight.Y;
        //    matrix.M13 = newRight.Y;

        //    matrix.m[0][0] = newRight.x; matrix.m[0][1] = newRight.y; matrix.m[0][2] = newRight.z; matrix.m[0][3] = 0.0f;
        //    matrix.m[1][0] = newUp.x; matrix.m[1][1] = newUp.y; matrix.m[1][2] = newUp.z; matrix.m[1][3] = 0.0f;
        //    matrix.m[2][0] = newForward.x; matrix.m[2][1] = newForward.y; matrix.m[2][2] = newForward.z; matrix.m[2][3] = 0.0f;
        //    matrix.m[3][0] = pos.x; matrix.m[3][1] = pos.y; matrix.m[3][2] = pos.z; matrix.m[3][3] = 1.0f;
        //    return matrix;
        //}






        //helper functions

        //fill triangle
        public void fillTriangle(Triangle tri, Bitmap bitmap, float dotProduct)
        {

            SolidBrush solidBrush = new SolidBrush(Color.FromArgb((int)(160 * dotProduct), 150, 233));

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
        public void DrawTriangle(Triangle tri, Bitmap bitmap)
        {
            DrawFunctions.Brsenham((int)tri.v1.X, (int)tri.v1.Y, (int)tri.v2.X, (int)tri.v2.Y, ref bitmap);
            DrawFunctions.Brsenham((int)tri.v2.X, (int)tri.v2.Y, (int)tri.v3.X, (int)tri.v3.Y, ref bitmap);
            DrawFunctions.Brsenham((int)tri.v3.X, (int)tri.v3.Y, (int)tri.v1.X, (int)tri.v1.Y, ref bitmap);
        }


        //helper functions 
                     

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

    }
}