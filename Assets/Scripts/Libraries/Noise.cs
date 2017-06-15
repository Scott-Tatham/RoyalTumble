using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoiseLib
{
    public class Noise
    {
        #region Hash Tables

        static int[] hash =
        {
            142, 89, 207, 253, 51, 137, 103, 1, 229, 124, 65, 8, 227, 157, 78, 10, 239, 131, 174, 5, 234, 155, 33, 115, 212, 109, 163, 60, 22, 182, 165, 255, 161, 92, 6, 218, 141, 57, 121, 30, 243, 77, 0, 192, 101, 97, 69, 200, 20, 127, 48, 248, 116, 100, 9, 66, 130, 58, 252, 151, 17, 177, 136, 80, 150, 179, 250, 71, 187, 28, 215, 111, 34, 133, 223, 36, 120, 75, 246, 175, 86, 35, 162, 3, 160, 50, 189, 195, 254, 123, 44, 93, 190, 166, 61, 245, 25, 128, 16, 172, 210, 83, 148, 95, 56, 206, 24, 197, 113, 63, 132, 13, 188, 242, 106, 26, 119, 2, 198, 168, 41, 140, 64, 21, 238, 216, 15, 49, 181, 230, 153, 146, 67, 143, 31, 221, 233, 42, 125, 102, 38, 73, 199, 231, 169, 184, 204, 88, 185, 96, 53, 14, 241, 214, 170, 118, 236, 220, 46, 176, 104, 23, 139, 72, 45, 203, 194, 91, 171, 208, 117, 55, 99, 213, 202, 18, 62, 85, 191, 224, 244, 122, 27, 81, 164, 235, 4, 237, 59, 149, 183, 112, 70, 228, 7, 68, 178, 217, 105, 138, 47, 173, 84, 129, 40, 12, 209, 145, 52, 186, 90, 29, 219, 193, 108, 226, 82, 251, 147, 79, 232, 211, 98, 154, 32, 180, 240, 144, 201, 74, 11, 222, 54, 247, 196, 126, 158, 110, 43, 114, 135, 156, 19, 76, 167, 87, 249, 152, 39, 159, 94, 225, 134, 37, 107, 205, 142, 89, 207, 253, 51, 137, 103, 1, 229, 124, 65, 8, 227, 157, 78, 10, 239, 131, 174, 5, 234, 155, 33, 115, 212, 109, 163, 60, 22, 182, 165, 255, 161, 92, 6, 218, 141, 57, 121, 30, 243, 77, 0, 192, 101, 97, 69, 200, 20, 127, 48, 248, 116, 100, 9, 66, 130, 58, 252, 151, 17, 177, 136, 80, 150, 179, 250, 71, 187, 28, 215, 111, 34, 133, 223, 36, 120, 75, 246, 175, 86, 35, 162, 3, 160, 50, 189, 195, 254, 123, 44, 93, 190, 166, 61, 245, 25, 128, 16, 172, 210, 83, 148, 95, 56, 206, 24, 197, 113, 63, 132, 13, 188, 242, 106, 26, 119, 2, 198, 168, 41, 140, 64, 21, 238, 216, 15, 49, 181, 230, 153, 146, 67, 143, 31, 221, 233, 42, 125, 102, 38, 73, 199, 231, 169, 184, 204, 88, 185, 96, 53, 14, 241, 214, 170, 118, 236, 220, 46, 176, 104, 23, 139, 72, 45, 203, 194, 91, 171, 208, 117, 55, 99, 213, 202, 18, 62, 85, 191, 224, 244, 122, 27, 81, 164, 235, 4, 237, 59, 149, 183, 112, 70, 228, 7, 68, 178, 217, 105, 138, 47, 173, 84, 129, 40, 12, 209, 145, 52, 186, 90, 29, 219, 193, 108, 226, 82, 251, 147, 79, 232, 211, 98, 154, 32, 180, 240, 144, 201, 74, 11, 222, 54, 247, 196, 126, 158, 110, 43, 114, 135, 156, 19, 76, 167, 87, 249, 152, 39, 159, 94, 225, 134, 37, 107, 205
        };

        static int[][] grad3 = new int[][]
        {
            new int[] { 1, 1, 0 }, new int[] { -1, 1, 0 }, new int[] { 1, -1, 0 }, new int[] { -1, -1, 0 },
            new int[] { 1, 0, 1 }, new int[] { -1, 0, 1 }, new int[] { 1, 0, -1 }, new int[] { -1, 0, -1 },
            new int[] { 0, 1, 1 }, new int[] { 0, -1, 1 }, new int[] { 0, 1, -1 }, new int[] { 0, -1, -1 }
        };

        static int[][] grad4 = new int[][]
        {
            new int[] { 0, 1, 1, 1 }, new int[] { 0, 1, 1, -1 }, new int[] { 0, 1, -1, 1 }, new int[] { 0, 1, -1, -1 },
            new int[] { 0, -1, 1, 1 }, new int[] { 0, -1, 1, -1 }, new int[] { 0, -1, -1, 1 }, new int[] { 0, -1, -1, -1 },
            new int[] { 1, 0, 1, 1 }, new int[] { 1, 0, 1, -1 }, new int[] { 1, 0, -1, 1 }, new int[] { 1, 0, -1, -1 },
            new int[] { -1, 0, 1, 1 }, new int[] { -1, 0, 1, -1 }, new int[] { -1, 0, -1, 1 }, new int[] { -1, 0, 1, -1 },
            new int[] { 1, 1, 0, 1 }, new int[] { 1, 1, 0, -1 }, new int[] { 1, -1, 0, 1 }, new int[] { 1, -1, 0, -1 },
            new int[] { -1, 1, 0, 1 }, new int[] { -1, 1, 0, -1 }, new int[] { -1, -1, 0, 1 }, new int[] { -1, -1, 0, -1 },
            new int[] { 1, 1, 1, 0 }, new int[] { 1, 1, -1, 0 }, new int[] { 1, 1, 1, 0 }, new int[] { 1, -1, -1, 0 },
            new int[] { -1, 1, 1, 0 }, new int[] { -1, 1, -1, 0 }, new int[] { -1, -1, 1, 0 }, new int[] { -1, -1, -1, 0 }
        };

        #endregion

        #region Funcs

        static int FastFloor(float _x)
        {
            return (_x > 0) ? ((int)_x) : (((int)_x) - 1);
        }

        static float Dot(int[] _g, float _x, float _y)
        {
            return _g[0] * _x + _g[1] * _y;
        }

        static float Dot(int[] _g, float _x, float _y, float _z)
        {
            return _g[0] * _x + _g[1] * _y + _g[2] * _z;
        }

        static float Dot(int[] _g, float _x, float _y, float _z, float _w)
        {
            return _g[0] * _x + _g[1] * _y + _g[2] * _z + _g[3] * _w;
        }

        #endregion

        #region Value Noise

        public static float Value1D(Vector3 _p, int _f)
        {
            _p.x %= 4;
            _p.x *= _f;
            int x1 = Mathf.FloorToInt(_p.x);
            float t = _p.x - x1;
            x1 &= 255;
            int x2 = x1 + 1;
            int h1 = hash[x1];
            int h2 = hash[x2];

            return Mathf.Lerp(h1, h2, t) / 16;
        }

        public static float Value2D(Vector3 _p, int _f)
        {
            _p *= _f;
            int x = Mathf.FloorToInt(_p.x);
            int z = Mathf.FloorToInt(_p.z);
            x &= 255;
            z &= 255;

            return hash[(hash[x] + z)] / 16;
        }

        public static float Value3D(Vector3 _p, int _f)
        {
            _p *= _f;
            int x = Mathf.FloorToInt(_p.x);
            int y = Mathf.FloorToInt(_p.z);
            int z = Mathf.FloorToInt(_p.z);
            x &= 255;
            y &= 255;
            z &= 255;

            return hash[(hash[(hash[x] + y)] + z)] / 16;
        }

        #endregion

        #region Octaves

        public static float OctaveSimplex2D(int _o, float _per, float _f, float _bA, Vector2 _p, Vector3 _y)
        {
            float total = 0;
            float mA = 0;

            for (int i = 0; i < _o; i++)
            {
                total += (Simplex2D(new Vector2(_p.x * _f, _p.y * _f)) * _bA + (_y.x + _y.y - _y.z));
                _f *= 2;
                mA += _bA;
                _bA *= _per;
            }

            return total / mA;
        }

        public static float OctaveSimplex3D(int _o, float _per, float _f, float _bA, Vector3 _p)
        {
            float total = 0;
            float mA = 0;

            for (int i = 0; i < _o; i++)
            {
                total += Simplex3D(new Vector3(_p.x * _f, _p.y * _f, _p.z * _f) * _bA);
                _f *= 2;
                mA += _bA;
                _bA *= _per;
            }

            return total / mA;
        }

        public static float OctaveSimplex4D(int _o, float _per, float _f, float _bA, Vector4 _p)
        {
            float total = 0;
            float mA = 0;

            for (int i = 0; i < _o; i++)
            {
                total += Simplex3D(new Vector4(_p.x * _f, _p.y * _f, _p.z * _f, _p.w * _f) * _bA);
                _f *= 2;
                mA += _bA;
                _bA *= _per;
            }

            return total / mA;
        }

        #endregion

        #region Simplex Noise

        static float Simplex2D(Vector2 _p)
        {
            float F2 = 0.366025404f;
            float G2 = 0.211324865f;
            float n0, n1, n2;

            float s = (_p.x + _p.y) * F2;
            int i = FastFloor(_p.x + s);
            int j = FastFloor(_p.y + s);

            float t = (i + j) * G2;
            float m = i - t;
            float n = j - t;
            float x0 = _p.x - m;
            float y0 = _p.y - n;

            int i0, j0;

            if (x0 > y0)
            {
                i0 = 1;
                j0 = 0;
            }

            else
            {
                i0 = 0;
                j0 = 1;
            }

            float x1 = x0 - i0 + G2;
            float y1 = y0 - j0 + G2;
            float x2 = x0 - 1 + 2 * G2;
            float y2 = y0 - 1 + 2 * G2;

            int ii = i & 255;
            int jj = j & 255;
            int gi0 = hash[ii + hash[jj]] % 12;
            int gi1 = hash[ii + i0 + hash[jj + j0]] % 12;
            int gi2 = hash[ii + 1 + hash[jj + 1]] % 12;

            float a0 = 0.5f - x0 * x0 - y0 * y0;

            if (a0 < 0)
            {
                n0 = 0;
            }

            else
            {
                a0 *= a0;
                n0 = a0 * a0 * Dot(grad3[gi0], x0, y0);
            }

            float a1 = 0.5f - x1 * x1 - y1 * y1;

            if (a1 < 0)
            {
                n1 = 0;
            }

            else
            {
                a1 *= a1;
                n1 = a1 * a1 * Dot(grad3[gi1], x1, y1);
            }

            float a2 = 0.5f - x2 * x2 - y2 * y2;

            if (a2 < 0)
            {
                n2 = 0;
            }

            else
            {
                a2 *= a2;
                n2 = a2 * a2 * Dot(grad3[gi2], x2, y2);
            }

            return 40.0f * (n0 + n1 + n2);
        }

        static float Simplex3D(Vector3 _p)
        {
            float F3 = 0.333333333f;
            float G3 = 0.166666667f;
            float n0, n1, n2, n3; // To be used for the noise contributions for the 4 corners.

            // Skewing input for determination.
            float s = (_p.x + _p.y + _p.z) * F3; // Skew Factor.
            int i = FastFloor(_p.x + s);
            int j = FastFloor(_p.y + s);
            int k = FastFloor(_p.z + s);

            float t = (i + j + k) * G3;
            float m = i - t; // Unskew the cell origin.
            float n = j - t;
            float o = k - t;
            float x0 = _p.x - m; // Distance from cell origin.
            float y0 = _p.y - n;
            float z0 = _p.z - o;

            // Determination of Simplex we are in.
            int i0, j0, k0; // Offsets for the second corner of the simplex.
            int i1, j1, k1; // Offsets for the third corner of the simplex.

            if (x0 >= y0)
            {
                if (y0 >= z0)
                {
                    i0 = 1; // X Y Z
                    j0 = 0;
                    k0 = 0;
                    i1 = 1;
                    j1 = 1;
                    k1 = 0;
                }

                else if (x0 >= z0)
                {
                    i0 = 1; // X Z Y
                    j0 = 0;
                    k0 = 0;
                    i1 = 1;
                    j1 = 0;
                    k1 = 1;
                }

                else
                {
                    i0 = 0; // Z X Y
                    j0 = 0;
                    k0 = 1;
                    i1 = 1;
                    j1 = 0;
                    k1 = 1;
                }
            }

            else
            {
                if (y0 < z0)
                {
                    i0 = 0; // Z Y X
                    j0 = 0;
                    k0 = 1;
                    i1 = 0;
                    j1 = 1;
                    k1 = 1;
                }

                else if (x0 < z0)
                {
                    i0 = 0; // Y Z X
                    j0 = 1;
                    k0 = 0;
                    i1 = 0;
                    j1 = 1;
                    k1 = 1;
                }

                else
                {
                    i0 = 0; // Y X Z
                    j0 = 1;
                    k0 = 0;
                    i1 = 1;
                    j1 = 1;
                    k1 = 0;
                }
            }

            float x1 = x0 - i0 + G3; // Offsets for the second corner.
            float y1 = y0 - j0 + G3;
            float z1 = z0 - k0 + G3;
            float x2 = x0 - i1 + 2 * G3; // Offsets for the third corner.
            float y2 = y0 - j1 + 2 * G3;
            float z2 = z0 - k1 + 2 * G3;
            float x3 = x0 - 1 + 3 * G3; // Offsets for the fourth corner.
            float y3 = y0 - 1 + 3 * G3;
            float z3 = z0 - 1 + 3 * G3;

            int ii = i & 255; // Hash wrap for gradients.
            int jj = j & 255;
            int kk = k & 255;
            int gi0 = hash[ii + hash[jj + hash[kk]]] % 12;
            int gi1 = hash[ii + i0 + hash[jj + j0 + hash[kk + k0]]] % 12;
            int gi2 = hash[ii + i1 + hash[jj + j1 + hash[kk + k1]]] % 12;
            int gi3 = hash[ii + 1 + hash[jj + 1 + hash[kk + 1]]] % 12;

            // Four corner contribution.
            float a0 = 0.6f - x0 * x0 - y0 * y0 - z0 * z0;

            if (a0 < 0)
            {
                n0 = 0;
            }

            else
            {
                a0 *= a0;
                n0 = a0 * a0 * Dot(grad3[gi0], x0, y0, z0);
            }

            float a1 = 0.6f - x1 * x1 - y1 * y1 - z1 * z1;

            if (a1 < 0)
            {
                n1 = 0;
            }

            else
            {
                a1 *= a1;
                n1 = a1 * a1 * Dot(grad3[gi1], x1, y1, z1);
            }

            float a2 = 0.6f - x2 * x2 - y2 * y2 - z2 * z2;

            if (a2 < 0)
            {
                n2 = 0;
            }

            else
            {
                a2 *= a2;
                n2 = a2 * a2 * Dot(grad3[gi2], x2, y2, z2);
            }

            float a3 = 0.6f - x3 * x3 - y3 * y3 - z3 * z3;

            if (a3 < 0)
            {
                n3 = 0;
            }

            else
            {
                a3 *= a3;
                n3 = a3 * a3 * Dot(grad3[gi3], x3, y3, z3);
            }

            return 32 * (n0 + n1 + n2 + n3);
        }

        static float Simplex4D(Vector4 _p)
        {
            float F4 = 0.309016994f;
            float G4 = 0.138196601f;
            float n0, n1, n2, n3, n4; // To be used for the noise contributions for the 5 corners.
            
            // Skewing input for determination of which cell of 24 simplices.
            float s = (_p.x + _p.y + _p.z + _p.w) * F4;
            int i = FastFloor(_p.x + s);
            int j = FastFloor(_p.y + s);
            int k = FastFloor(_p.z + s);
            int l = FastFloor(_p.w + s);

            float t = (i + j + k + l) * G4;
            float m = i - t; // Unskew the cell origin.
            float n = j - t;
            float o = k - t;
            float p = l - t;

            float x0 = _p.x - m; // Distance from cell origin.
            float y0 = _p.y - n;
            float z0 = _p.z - o;
            float w0 = _p.w - p;

            // For the 4D case, the simplex is a 4D shape.
            // To find out which of the 24 possible simplices we're in, we need to determine the magnitude ordering of x0, y0, z0 and w0.
            // Six pair-wise comparisons are performed between each possible pair of the four coordinates, and the results are used to rank the numbers.
            int rankx = 0;
            int ranky = 0;
            int rankz = 0;
            int rankw = 0;

            if (x0 > y0)
            {
                rankx++;
            }

            else
            {
                ranky++;
            }

            if (x0 > z0)
            {
                rankx++;
            }

            else
            {
                rankz++;
            }

            if (x0 > w0)
            {
                rankx++;
            }

            else
            {
                rankw++;
            }

            if (y0 > z0)
            {
                ranky++;
            }

            else
            {
                rankz++;
            }

            if (y0 > w0)
            {
                ranky++;
            }

            else
            {
                rankw++;
            }

            if (z0 > w0)
            {
                rankz++;
            }

            else
            {
                rankw++;
            }

            int i1, j1, k1, l1; // Offsets for the second corner of the simplex.
            int i2, j2, k2, l2; // Offsets for the third corner of the simplex.
            int i3, j3, k3, l3; // Offsets for the fourth corner of the simplex.

            // Rank 3 denotes the largest coordinate.
            i1 = rankx >= 3 ? 1 : 0;
            j1 = ranky >= 3 ? 1 : 0;
            k1 = rankz >= 3 ? 1 : 0;
            l1 = rankw >= 3 ? 1 : 0;

            // Rank 2 denotes the second largest coordinate.
            i2 = rankx >= 2 ? 1 : 0;
            j2 = ranky >= 2 ? 1 : 0;
            k2 = rankz >= 2 ? 1 : 0;
            l2 = rankw >= 2 ? 1 : 0;

            // Rank 1 denotes the second smallest coordinate.
            i3 = rankx >= 1 ? 1 : 0;
            j3 = ranky >= 1 ? 1 : 0;
            k3 = rankz >= 1 ? 1 : 0;
            l3 = rankw >= 1 ? 1 : 0;

            // The fifth corner has all coordinate offsets equal 1.
            float x1 = x0 - i1 + G4; // Offsets for the second corner.
            float y1 = y0 - j1 + G4;
            float z1 = z0 - k1 + G4;
            float w1 = w0 - l1 + G4;
            float x2 = x0 - i2 + 2 * G4; // Offsets for the third corner.
            float y2 = y0 - j2 + 2 * G4;
            float z2 = z0 - k2 + 2 * G4;
            float w2 = w0 - l2 + 2 * G4;
            float x3 = x0 - i3 + 3 * G4; // Offsets for the fourth corner.
            float y3 = y0 - j3 + 3 * G4;
            float z3 = z0 - k3 + 3 * G4;
            float w3 = w0 - l3 + 3 * G4;
            float x4 = x0 - 1 + 4 * G4; // Offsets for the fifth corner.
            float y4 = y0 - 1 + 4 * G4;
            float z4 = z0 - 1 + 4 * G4;
            float w4 = w0 - 1 + 4 * G4;
            
            int ii = i & 255; // Hash wrap for gradients.
            int jj = j & 255;
            int kk = k & 255;
            int ll = l & 255;
            int gi0 = hash[ii + hash[jj + hash[kk + hash[ll]]]] % 32;
            int gi1 = hash[ii + i1 + hash[jj + j1 + hash[kk + k1 + hash[ll + l1]]]] % 32;
            int gi2 = hash[ii + i2 + hash[jj + j2 + hash[kk + k2 + hash[ll + l2]]]] % 32;
            int gi3 = hash[ii + i3 + hash[jj + j3 + hash[kk + k3 + hash[ll + l3]]]] % 32;
            int gi4 = hash[ii + 1 + hash[jj + 1 + hash[kk + 1 + hash[ll + 1]]]] % 32;

            // Calculate the contribution from the five corners.
            float t0 = 0.6f - x0 * x0 - y0 * y0 - z0 * z0 - w0 * w0;

            if (t0 < 0)
            {
                n0 = 0;
            }

            else
            {
                t0 *= t0;
                n0 = t0 * t0 * Dot(grad4[gi0], x0, y0, z0, w0);
            }

            float t1 = 0.6f - x1 * x1 - y1 * y1 - z1 * z1 - w1 * w1;

            if (t1 < 0)
            {
                n1 = 0;
            }

            else
            {
                t1 *= t1;
                n1 = t1 * t1 * Dot(grad4[gi1], x1, y1, z1, w1);
            }

            float t2 = 0.6f - x2 * x2 - y2 * y2 - z2 * z2 - w2 * w2;

            if (t2 < 0)
            {
                n2 = 0;
            }

            else
            {
                t2 *= t2;
                n2 = t2 * t2 * Dot(grad4[gi2], x2, y2, z2, w2);
            }

            float t3 = 0.6f - x3 * x3 - y3 * y3 - z3 * z3 - w3 * w3;

            if (t3 < 0)
            {
                n3 = 0;
            }

            else
            {
                t3 *= t3;
                n3 = t3 * t3 * Dot(grad4[gi3], x3, y3, z3, w3);
            }

            float t4 = 0.6f - x4 * x4 - y4 * y4 - z4 * z4 - w4 * w4;

            if (t4 < 0)
            {
                n4 = 0;
            }

            else
            {
                t4 *= t4;
                n4 = t4 * t4 * Dot(grad4[gi4], x4, y4, z4, w4);
            }
            
            return 27.0f * (n0 + n1 + n2 + n3 + n4);
        }

        #endregion
    }
}