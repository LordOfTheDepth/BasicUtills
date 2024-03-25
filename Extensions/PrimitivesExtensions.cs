using System;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public static class PrimitivesExtensions
{
    public static bool Contains<T>(this NativeArray<T> array, T item) where T : struct
    {
        foreach (var element in array)
        {
            if (element.Equals(item))
            {
                return true;
            }
        }
        return false;
    }

    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static bool Contains<T>(this T[] array, T item)
    {
        return array.ToList().Contains(item);
    }

    public static float Magnitude(this float2 vector)
    {
        return math.sqrt(math.pow(vector.x, 2) + math.pow(vector.y, 2));
    }

    public static float SqrMagnitude(this float2 vector)
    {
        return math.pow(vector.x, 2) + math.pow(vector.y, 2);
    }

    public static float Magnitude(this int2 vector)
    {
        return math.sqrt(math.pow(vector.x, 2) + math.pow(vector.y, 2));
    }

    public static float SqrMagnitude(this int2 vector)
    {
        return math.pow(vector.x, 2) + math.pow(vector.y, 2);
    }

    public static Vector3Int ToVector3(this Vector2Int vector)
    {
        return new Vector3Int(vector.x, vector.y, 0);
    }

    public static Vector3 ToVector3(this Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }

    public static Vector3 ToVector3(this Vector2 vector, float z)
    {
        return new Vector3(vector.x, vector.y, z);
    }

    public static float2 Normalize(this float2 vector)
    {
        return math.normalize(vector);
    }

    public static Vector3 ToVector3(this float2 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }

    public static Vector2 ToVector2(this float2 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static float2 ToFloat2(this Vector3 vector)
    {
        return new float2(vector.x, vector.y);
    }

    public static float3 ToFloat3(this Vector3 vector)
    {
        return new float3(vector.x, vector.y, vector.z);
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

    public static Vector2 ToVector2Y(this int value)
    {
        return new Vector2(0, value);
    }

    public static Vector2 ToVector2X(this float value)
    {
        return new Vector2(value, 0);
    }

    public static Vector2 ToVector2Y(this float value)
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

    public static float2 ToFloat2(this float3 vector)
    {
        return new float2(vector.x, vector.y);
    }

    public static Vector3 ToVector3(this float3 vector)
    {
        return new Vector3(vector.x, vector.y, vector.z);
    }

    public static Vector3 ToRealPosition(this Vector3 vector)
    {
        return new Vector3(vector.x, vector.y, vector.y * 0.01f);
    }

    public static float2 ToFloat2(this Vector2 vector)
    {
        return new float2(vector.x, vector.y);
    }

    public static int2 ToMapPosition(this int index, int mapSizeX = 64, int mapSizeY = 64, int shift = 0)
    {
        int x = Mathf.Max(index % mapSizeX, 0);
        int y = (index - x) / mapSizeY;
        return new int2(x + shift, y + shift);
    }

    public static int2 ToInt2(this Vector2 vector)
    {
        return new int2((int)vector.x, (int)vector.y);
    }

    public static int2 ToInt2(this Vector3 vector)
    {
        return new int2((int)vector.x, (int)vector.y);
    }

    public static Vector3 ToRealPosition(this float2 vector)
    {
        return new Vector3(vector.x, vector.y, vector.y * 10);
    }

    public static Vector3 ToRealPosition(this int2 vector)
    {
        return new Vector3(vector.x, vector.y, vector.y * 10);
    }

    public static Vector2Int ToVector2(this int2 vector)
    {
        return new Vector2Int(vector.x, vector.y);
    }

    public static Vector2 ToVector3(this int2 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }

    public static int ToMapIndex(this int2 position, int width, int height)
    {
        if (position.x >= 0 && position.x < width && position.y >= 0 && position.y < height)
        {
            return position.y * width + position.x;
        }
        else
        {
            return -1;
        }
    }

    public static int ToMapIndex(this float2 vector)
    {
        int2 v = (int2)vector;
        return ToMapIndex(v);
    }

    public static int ToMapIndex(this int2 v)
    {
        return ToMapIndex(v, 64, 64);
    }

    public static bool Contains(this Bounds bounds, float2 vector)
    {
        return bounds.Contains(vector.ToVector3());
    }

    public static void Randomize(this float2 center, float range)
    {
        center.x += UnityEngine.Random.Range(-range, range);
        center.y += UnityEngine.Random.Range(-range, range);
    }

    public static float GetDistance(this int2 start, int2 target)
    {
        return (target - start).Magnitude();
    }

    public static float GetSqrDistance(this int2 start, int2 target)
    {
        return (target - start).SqrMagnitude();
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
        return Mathf.Abs(volume);

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
        var radians = math.radians(degrees);
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