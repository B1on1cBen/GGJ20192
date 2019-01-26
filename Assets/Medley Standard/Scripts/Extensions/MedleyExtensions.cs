// Ben Gordon 2017

using System.Collections.Generic;
using UnityEngine;

namespace Medley.Extensions
{
    public static class MedleyExtensions
    {
        #region Transform
        public static void SetX(this Transform transform, float x)
        {
            transform.position = new Vector3(
                x,
                transform.position.y,
                transform.position.z
            );
        }

        public static void SetY(this Transform transform, float y)
        {
            transform.position = new Vector3(
                transform.position.x,
                y,
                transform.position.z
            );
        }

        public static void SetZ(this Transform transform, float z)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                z
            );
        }
        #endregion Transform

        #region Vector3
        public static Vector3[] ToVector3(this Vector2[] array, bool yIsZ, float thirdValue)
        {
            Vector3[] newArray = new Vector3[array.Length];

            for (int i = 0; i < newArray.Length; i++)
            {
                if (yIsZ)
                    newArray[i] = new Vector3(array[i].x, thirdValue, array[i].y);
                else
                    newArray[i] = new Vector3(array[i].x, array[i].y, thirdValue);
            }

            return newArray;
        }
        #endregion Vector3

        #region Vector2
        /// <summary>
        /// Rotates a vector by degrees
        /// </summary>
        /// <param name="v">The vector to be rotated</param>
        /// <param name="degrees">How many degrees to rotate</param>
        /// <returns>Rotated vector</returns>
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }
        #endregion Vector2

        #region List<T>
        // from https://stackoverflow.com/questions/273313/randomize-a-listt
        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        #endregion List<T>

        #region T[]

        // from https://stackoverflow.com/questions/273313/randomize-a-listt
        public static void Shuffle<T>(this T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                T value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
        }

        public static T[] Add<T>(this T[] array, T item)
        {
            T[] tempArray = new T[array.Length + 1];
            int index = 0;
            foreach (T t in array)
            {
                tempArray[index] = array[index];
                index++;
            }
            tempArray[index] = item;

            return tempArray;
        }

        public static T Random<T>(this T[] array)
        {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        #endregion

        #region bool
        /// <summary>
        /// Returns 1 if true, 0 if false
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this bool value)
        {
            return value == true ? 1 : 0;
        }

        /// <summary>
        /// Returns 1 if true, 0 if false.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="zeroToOne">Makes false return -1 instead of 0.</param>
        /// <returns></returns>
        public static int ToInt(this bool value, bool zeroToOne)
        {
            if (zeroToOne)
                return ToInt(value);

            if (value)
                return 1;

            return -1;
        }
        #endregion bool

        #region AudioSource
        public static void PlayRandomClip(this AudioSource source, AudioClip[] clips)
        {
            AudioClip clip = clips[UnityEngine.Random.Range(0, clips.Length)];

            if (clip != null)
                source.PlayOneShot(clip);
        }
        #endregion AudioSource

        #region Bounds
        /// <summary>
        /// Gets the bounds of the largest box that will fit inside of a given capsule collider.
        /// </summary>
        /// <param name="capsule">The capsule collider to fit a box inside of.</param>
        /// <returns></returns>
        public static Bounds CapsuleBox(this CapsuleCollider2D capsule)
        {
            Vector2 size;

            if (capsule.direction == CapsuleDirection2D.Horizontal)
            {
                if (capsule.size.y >= capsule.size.x)
                    size.x = 0;
                else
                    size.x = capsule.size.x - capsule.size.y;

                size.y = capsule.size.y;
            }
            else
            {
                size.x = capsule.size.x;

                if (capsule.size.x >= capsule.size.y)
                    size.y = 0;
                else
                    size.y = capsule.size.y - capsule.size.x;
            }

            return new Bounds(capsule.transform.position + (Vector3)capsule.offset, Vector3.Scale(size, capsule.transform.localScale));
        }

        /// <summary>
        /// Gets the bounds of the largest box that will fit inside of a given circle collider.
        /// </summary>
        /// <param name="circle">The circle collider to fit a box inside of.</param>
        /// <returns></returns>
        public static Bounds CircleBox(this CircleCollider2D circle)
        {
            float sideLength = Mathf.Sqrt(Mathf.Pow(2 * circle.radius, 2) / 2);

            return new Bounds(circle.transform.position + (Vector3)circle.offset, Vector3.Scale(Vector3.one * sideLength, circle.transform.localScale));
        }
        #endregion Bounds

        #region Mesh
        public static Mesh ReverseNormals(this MeshFilter meshFilter)
        {
            Mesh mesh = meshFilter.mesh;

            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
                normals[i] = -normals[i];
            mesh.normals = normals;

            for (int m = 0; m < mesh.subMeshCount; m++)
            {
                int[] triangles = mesh.GetTriangles(m);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    int temp = triangles[i + 0];
                    triangles[i + 0] = triangles[i + 1];
                    triangles[i + 1] = temp;
                }
                mesh.SetTriangles(triangles, m);
            }

            return mesh;
        }
        #endregion Mesh
    }
}
