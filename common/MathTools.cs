using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Revival.Common
{
    public class Matrix
    {
        public float F11, F12, F13, F14;
        public float F21, F22, F23, F24;
        public float F31, F32, F33, F34;
        public float F41, F42, F43, F44;

        public static Matrix Multiply(Matrix m1, Matrix m2)
        {
            Matrix ret = new Matrix
            {
                F11 = m1.F11 * m2.F11 + m1.F12 * m2.F21 + m1.F13 * m2.F31 + m1.F14 * m2.F41,
                F12 = m1.F11 * m2.F12 + m1.F12 * m2.F22 + m1.F13 * m2.F32 + m1.F14 * m2.F42,
                F13 = m1.F11 * m2.F13 + m1.F12 * m2.F23 + m1.F13 * m2.F33 + m1.F14 * m2.F43,
                F14 = m1.F11 * m2.F14 + m1.F12 * m2.F24 + m1.F13 * m2.F34 + m1.F14 * m2.F44,
                F21 = m1.F21 * m2.F11 + m1.F22 * m2.F21 + m1.F23 * m2.F31 + m1.F24 * m2.F41,
                F22 = m1.F21 * m2.F12 + m1.F22 * m2.F22 + m1.F23 * m2.F32 + m1.F24 * m2.F42,
                F23 = m1.F21 * m2.F13 + m1.F22 * m2.F23 + m1.F23 * m2.F33 + m1.F24 * m2.F43,
                F24 = m1.F21 * m2.F14 + m1.F22 * m2.F24 + m1.F23 * m2.F34 + m1.F24 * m2.F44,
                F31 = m1.F31 * m2.F11 + m1.F32 * m2.F21 + m1.F33 * m2.F31 + m1.F34 * m2.F41,
                F32 = m1.F31 * m2.F12 + m1.F32 * m2.F22 + m1.F33 * m2.F32 + m1.F34 * m2.F42,
                F33 = m1.F31 * m2.F13 + m1.F32 * m2.F23 + m1.F33 * m2.F33 + m1.F34 * m2.F43,
                F34 = m1.F31 * m2.F14 + m1.F32 * m2.F24 + m1.F33 * m2.F34 + m1.F34 * m2.F44,
                F41 = m1.F41 * m2.F11 + m1.F42 * m2.F21 + m1.F43 * m2.F31 + m1.F44 * m2.F41,
                F42 = m1.F41 * m2.F12 + m1.F42 * m2.F22 + m1.F43 * m2.F32 + m1.F44 * m2.F42,
                F43 = m1.F41 * m2.F13 + m1.F42 * m2.F23 + m1.F43 * m2.F33 + m1.F44 * m2.F43,
                F44 = m1.F41 * m2.F14 + m1.F42 * m2.F24 + m1.F43 * m2.F34 + m1.F44 * m2.F44
            };

            return ret;
        }

        public static Matrix Identity()
        {
            return new Matrix { F11 = 1.0f, F22 = 1.0f, F33 = 1.0f, F44 = 1.0f };
        }

        public Matrix Clone()
        {
            return new Matrix
            {
                F11 = F11, F12 = F12, F13 = F13, F14 = F14,
                F21 = F21, F22 = F22, F23 = F23, F24 = F24,
                F31 = F31, F32 = F32, F33 = F33, F34 = F34,
                F41 = F41, F42 = F42, F43 = F43, F44 = F44
            };
        }

        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}, {3}]\n[{4}, {5}, {6}, {7}]\n[{8}, {9}, {10}, {11}]\n[{12}, {13}, {14}, {15}]",
                F11.ToString("r"), F12.ToString("r"), F13.ToString("r"), F14.ToString("r"),
                F21.ToString("r"), F22.ToString("r"), F23.ToString("r"), F24.ToString("r"),
                F31.ToString("r"), F32.ToString("r"), F33.ToString("r"), F34.ToString("r"),
                F41.ToString("r"), F42.ToString("r"), F43.ToString("r"), F44.ToString("r"));
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3()
        {
            X = 0.0f;
            Y = 0.0f;
            Z = 0.0f;
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 Transform(Vector3 vector3, Matrix matrix)
        {
            float x = vector3.X;
            float y = vector3.Y;
            float z = vector3.Z;

            return new Vector3
            {
                X = x * matrix.F11 + y * matrix.F21 + z * matrix.F31 + matrix.F41,
                Y = x * matrix.F12 + y * matrix.F22 + z * matrix.F32 + matrix.F42,
                Z = x * matrix.F13 + y * matrix.F23 + z * matrix.F33 + matrix.F43
            };
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3
            {
                X = v1.X - v2.X,
                Y = v1.Y - v2.Y,
                Z = v1.Z - v2.Z
            };
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3
            {
                X = v1.X + v2.X,
                Y = v1.Y + v2.Y,
                Z = v1.Z + v2.Z
            };
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}", X.ToString("r"), Y.ToString("r"), Z.ToString("r"));
        }
    }

    public class Vector4
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public Vector4() { }

        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static Vector4 Transform(Vector4 vector4, Matrix matrix)
        {
            float x = vector4.X;
            float y = vector4.Y;
            float z = vector4.Z;
            float w = vector4.W;

            return new Vector4
            {
                X = x * matrix.F11 + y * matrix.F21 + z * matrix.F31 + w * matrix.F41,
                Y = x * matrix.F12 + y * matrix.F22 + z * matrix.F32 + w * matrix.F42,
                Z = x * matrix.F13 + y * matrix.F23 + z * matrix.F33 + w * matrix.F43,
                W = x * matrix.F14 + y * matrix.F24 + z * matrix.F34 + w * matrix.F44
            };
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}, {3}", X.ToString("r"), Y.ToString("r"), Z.ToString("r"), W.ToString("r"));
        }
    }

    public static class MathTools
    {
        public static void Vector3Transform(Vector4 vector4, Vector3 vector3, Matrix matrix)
        {
            float x = vector3.X;
            float y = vector3.Y;
            float z = vector3.Z;
            const float w = 1.0f;

            vector4.X = x * matrix.F11 + y * matrix.F21 + z * matrix.F31 + w * matrix.F41;
            vector4.Y = x * matrix.F12 + y * matrix.F22 + z * matrix.F32 + w * matrix.F42;
            vector4.Z = x * matrix.F13 + y * matrix.F23 + z * matrix.F33 + w * matrix.F43;
            vector4.W = x * matrix.F14 + y * matrix.F24 + z * matrix.F34 + w * matrix.F44;
        }
    }
}
