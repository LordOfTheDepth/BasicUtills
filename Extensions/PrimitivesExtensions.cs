using System;
using System.Linq;
using UnityEngine;

public static class PrimitivesExtensions
{

    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static bool Contains<T>(this T[] array, T item)
    {
        return array.ToList().Contains(item);
    }

    public static Vector3Int ToVector3(this Vector2Int vector)
    {
        return new Vector3Int(vector.x, vector.y, 0);
    }

    public static Vector3 ToVector3(this Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }

    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector2Int ToVector2(this Vector3Int vector)
    {
        return new Vector2Int(vector.x, vector.y);
    }

    public static Vector2 ToVector2X(this int value)
    {
        return new Vector2(value, 0);
    }


    public static Vector2 ToVector2X(this float value)
    {
        return new Vector2(value, 0);
    }

    public static Vector2 ToVector2Y(this int value)
    {
        return new Vector2(0, value);
    }

    public static Vector2 ToVector2X(this Vector2 value)
    {
        return new Vector2(value.x, 0);
    }

    public static Vector2 ToVector2Y(this Vector2 value)
    {
        return new Vector2(0, value.y);
    }

    public static Vector2 ToVector2X(this Vector3 value)
    {
        return new Vector2(value.x, 0);
    }

    public static Vector2 ToVector2Y(this Vector3 value)
    {
        return new Vector2(0, value.y);
    }

    public static Vector3 ToRealPosition(this Vector3 vector)
    {
        return new Vector3(vector.x, vector.y, vector.y * 0.01f);
    }

    public static void Clear(this Texture2D texture)
    {
        var Colors = new Color[texture.width * texture.height];
        texture.SetPixels(0, 0, texture.width, texture.height, Colors);
        texture.Apply();
    }

    public static void SetAlpha(this Texture2D texture, float a)
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                UnityEngine.Color color = texture.GetPixel(x, y);
                texture.SetPixel(x, y, new UnityEngine.Color(color.r, color.g, color.b, a));
            }
        }
        texture.Apply();
    }

    public static GameObject[] GetChildren(this Transform transform)
    {
        var children = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
        }

        return children;
    }

    public static float CalculateVolume(this Mesh mesh)
    {
        float volume = 0;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        Vector3 o = new Vector3(0f, 0f, 0f);
        // Computing the center mass of the polyhedron as the fourth element of each mesh
        for (int i = 0; i < triangles.Length; i++)
        {
            o += vertices[triangles[i]];
        }
        o = o / mesh.triangles.Length;

        // Computing the sum of the volumes of all the sub-polyhedrons
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3, o);
        }
        return Math.Abs(volume);

        float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 o)
        {
            Vector3 v1 = p1 - o;
            Vector3 v2 = p2 - o;
            Vector3 v3 = p3 - o;

            return Vector3.Dot(Vector3.Cross(v1, v2), v3) / 6f; ;
        }
    }
}

public static class Vector2Extensions
{
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        var radians = Mathf.Deg2Rad * degrees;
        return new Vector2(
            (float)(v.x * Math.Cos(radians) - v.y * Math.Sin(radians)),
            (float)(v.x * Math.Sin(radians) + v.y * Math.Cos(radians))
        );
    }

    public static float Avg(this Vector2 vector)
    {
        return (vector.x + vector.y) / 2f;
    }
}

public static class Vector3Extensions
{
    public static Vector3 RotatePointAroundPivot(this Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    public static Vector3 RotatePointAroundPivot(this Vector3 point, Vector3 pivot, Quaternion angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = angles * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    public static float Summ(this Vector3 vector)
    {
        return vector.x + vector.y + vector.z;
    }

    public static float Avg(this Vector3 vector)
    {
        return (vector.x + vector.y + vector.z) / 3f;
    }

    public static float Volume(this Vector3 vector)
    {
        return vector.x * vector.y * vector.z;
    }
}

public static class UIExtensions
{
    public static RectTransform GetRectTransform(this Canvas canvas)
    {
        return canvas.transform as RectTransform;
    }
}