/*Package must included UnityEngine*/
using UnityEngine;

namespace MyUnityUtility
{
    public static class SpaceConverter
    {
        /// <summary>
        /// Get Vector 2D from Angle Radian
        /// </summary>
        public static Vector2 fromRadianToVector2D(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }

        /// <summary>
        /// Get Angle Degree from Vector 2D
        /// </summary>
        public static float fromVector2DToDegree(Vector2 direction)
        {
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Get Vector 3D from Angle Direction with Floor on XY Axis
        /// </summary>
        public static Vector3 angleDegreeToVector3XY(float angleDegree)
        {
            float a = angleDegree * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0);
        }

        /// <summary>
        /// Get Vector 3D from Angle Direction with Floor on XY Axis
        /// </summary>
        public static Vector3 angleDegreeToVector3XZ(float angleDegree)
        {
            float a = angleDegree * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
        }

        /// <summary>
        /// Get Angle in Degree from Vector 3D with Floor on XZ Axis
        /// </summary>
        public static float Vector3XYtoAngleDegree(Vector3 direction)
        {
            direction = direction.normalized;
            float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return n;
        }

        /// <summary>
        /// Get Angle in Degree from Vector 3D with Floor on XZ Axis
        /// </summary>
        public static float Vector3XZtoAngleDegree(Vector3 direction)
        {
            direction = direction.normalized;
            float n = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            return n;
        }
    }

    public static class AdvanceMath
    {
        /// <summary>
        /// Phytagoras Formula to get slash
        /// </summary>
        /// <returns>float c</returns>
        public static float PhytagorasAB(float a, float b)
        {
            return Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));
        }

        /// <summary>
        /// Phytagoras Formula to get side
        /// </summary>
        /// <returns>float b</returns>
        public static float PhytagorasAC(float a, float c)
        {
            return Mathf.Sqrt(Mathf.Pow(c, 2) - Mathf.Pow(a, 2));
        }
    }

    public static class Randomness
    {
        private const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static System.Random rnd = new System.Random();
        private const float floatSampling = 10000f;

        public static int RandomInt(int min, int max)
        {
            return rnd.Next(min, max);
        }

        /// <summary>
        /// Random position from min to max in 3D
        /// </summary>
        public static Vector3 RandomPosition3D(Vector3 min, Vector3 max)
        {
            float x = rnd.Next((int)(min.x * floatSampling), (int)(max.x * floatSampling)) / floatSampling;
            float y = rnd.Next((int)(min.y * floatSampling), (int)(max.y * floatSampling)) / floatSampling;
            float z = rnd.Next((int)(min.z * floatSampling), (int)(max.z * floatSampling)) / floatSampling;
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Completely Random word from alphabets. Contains Uppercase and Lowercase letters.
        /// </summary>
        public static string RandomWord(int length)
        {
            string word = "";
            for (int i = 0; i < length; i++)
            {
                int index = rnd.Next(0, alphabet.Length);
                word = string.Concat(word, alphabet[index]);
            }
            return word;
        }

        /// <summary>
        /// Random Float.
        /// </summary>
        public static float RandomFloat(float min, float max)
        {
            float s = rnd.Next((int)(min * floatSampling), (int)(max * floatSampling)) / floatSampling;
            return s;
        }

        /// <summary>
        /// Get Random Color.
        /// </summary>
        public static Color RandomColorRGB(Vector3 RGBMinimum, Vector3 RGBMaximum)
        {
            Color result = new Color(
                RandomFloat(RGBMinimum.x, RGBMaximum.x),
                RandomFloat(RGBMinimum.y, RGBMaximum.y),
                RandomFloat(RGBMinimum.z, RGBMaximum.z)
            );
            return result;
        }
    }

    public static class CompareValues
    {

    }
}
