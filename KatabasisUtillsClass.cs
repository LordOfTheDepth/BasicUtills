using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public struct RayRange
{
    public readonly Direction Direction;
    public readonly Vector2 Start, End;

    public RayRange(float x1, float y1, float x2, float y2, Direction direction)
    {
        Start = new Vector2(x1, y1);
        End = new Vector2(x2, y2);
        Direction = direction;
    }
}

public enum Direction
{
    Null,
    Up,
    Down,
    Left,
    Right,
}

public enum AxisRestriction
{
    None,
    Vertical,
    Horizontal,
}

public static class KatabasisUtillsClass
{
    public static bool IsPointerOverUIElement(Vector2 position)
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults(position));
    }

    public static float AngleBetween(Vector2 vector1, Vector2 vector2)
    {
        float sin = vector1.x * vector2.y - vector2.x * vector1.y;
        float cos = vector1.x * vector2.x + vector1.y * vector2.y;

        return Mathf.Atan2(sin, cos) * (180 / Mathf.PI);
    }

    public static Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }

    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == 5)
            {
                return true;
            }
        }
        return false;
    }

    public static float CalculateArea2D(this Mesh m)
    {
        Vector3[] mVertices = m.vertices;
        Vector3 result = Vector3.zero;
        for (int p = mVertices.Length - 1, q = 0; q < mVertices.Length; p = q++)
        {
            result += Vector3.Cross(mVertices[q], mVertices[p]);
        }
        result *= 0.5f;
        return result.magnitude;
    }

    public static List<RaycastResult> GetEventSystemRaycastResults(Vector2 position)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = position;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    public static float Percent(float value, float percent)
    {
        return (value / 100f) * percent;
    }

    public static bool Chance(float percent)
    {
        if (percent == 0) return false;
        if (percent == 100) return true;
        if (percent >= UnityEngine.Random.Range(0, 10000) * 0.01) return true;
        return false;
    }

    public static void SaveTextureToFile(Texture2D texture, string path)
    {
        var bytes = texture.EncodeToPNG();
        var file = File.Open(path, FileMode.Create);
        var binary = new BinaryWriter(file);
        binary.Write(bytes);
        file.Close();
    }

    public static float GenerateNormalRandom(float mu, float sigma)
    {
        float rand1 = UnityEngine.Random.Range(0.0f, 1.0f);
        float rand2 = UnityEngine.Random.Range(0.0f, 1.0f);

        float n = Mathf.Sqrt(-2.0f * Mathf.Log(rand1)) * Mathf.Cos((2.0f * Mathf.PI) * rand2);

        return (mu + sigma * n);
    }

    public static bool ChanceThreadSafe(Unity.Mathematics.Random random, float percent)
    {
        if (percent <= 0) return false;
        if (percent >= 100) return true;
        if (percent >= random.NextFloat(0, 100)) return true;
        return false;
    }

    public static List<string> romanNumerals = new List<string>() { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
    public static List<int> numerals = new List<int>() { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };

    public static string IntToRomanNumeral(int number)
    {
        var romanNumeral = string.Empty;
        while (number > 0)
        {
            // find biggest numeral that is less than equal to number
            var index = numerals.FindIndex(x => x <= number);
            // subtract it's value from your number
            number -= numerals[index];
            // tack it onto the end of your roman numeral
            romanNumeral += romanNumerals[index];
        }
        return romanNumeral;
    }

    public static Direction[] Directions => new Direction[]
    {
    Direction.Up,
    Direction.Down,
    Direction.Left,
    Direction.Right,
    };

    public static T GetRandomEnum<T>()
    {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    public static bool AxisRestrictionAlowsDirection(AxisRestriction axisRestriction, Direction direction)
    {
        if (axisRestriction == AxisRestriction.None || direction == Direction.Null)
        {
            return true;
        }
        else
        {
            if (axisRestriction == AxisRestriction.Vertical)
            {
                return direction == Direction.Up || direction == Direction.Down;
            }
            else
            {
                return direction == Direction.Left || direction == Direction.Right;
            }
        }
    }

    public static Vector2 Turn(Direction direction, Vector2 vector)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Vector2(vector.x, vector.y);

            case Direction.Down:
                return new Vector2(vector.x * -1, vector.y * -1);

            case Direction.Left:
                return new Vector2(vector.y * -1, vector.x);

            case Direction.Right:
                return new Vector2(vector.y, vector.x * -1);

            default:
                break;
        }

        throw new System.ArgumentOutOfRangeException();
    }

    public static Direction Turn(Direction direction, Direction directionToTurn)
    {
        return VectorToDirection(Turn(direction, DirectionToVector(directionToTurn)));
    }

    public static Direction VectorToDirection(Vector2 vector)
    {
        if (vector == Vector2.up) return Direction.Up;
        if (vector == Vector2.down) return Direction.Down;
        if (vector == Vector2.left) return Direction.Left;
        if (vector == Vector2.right) return Direction.Right;
        if (vector == Vector2.zero) return Direction.Null;

        float angle = Vector2.SignedAngle(vector, new Vector2(-1, 1));

        if (angle >= 0 && angle <= 90)
        {
            return Direction.Up;
        }
        if (angle > 90)
        {
            return Direction.Right;
        }
        if (angle < 0 && angle > -90)
        {
            return Direction.Left;
        }
        if (angle <= -90)
        {
            return Direction.Down;
        }

        throw new System.ArgumentOutOfRangeException();
    }

    public static Vector2 DirectionToVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector2.up;

            case Direction.Down:
                return Vector2.down;

            case Direction.Left:
                return Vector2.left;

            case Direction.Right:
                return Vector2.right;

            default:
                break;
        }

        throw new System.ArgumentOutOfRangeException();
    }

    public static Direction ReverseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;

            case Direction.Down:
                return Direction.Up;

            case Direction.Left:
                return Direction.Right;

            case Direction.Right:
                return Direction.Left;

            default: return Direction.Null;
        }

        throw new System.ArgumentOutOfRangeException();
    }

    public static T GetRandomWeightedItem<T>(Dictionary<T, float> itemsAndWeights)
    {
        var weightsSum = itemsAndWeights.Values.Sum();
        var r = UnityEngine.Random.Range(0, weightsSum);
        foreach (var item in itemsAndWeights.Keys)
        {
            r -= itemsAndWeights[item];
            if (r <= 0)
            {
                return item;
            }
        }
        throw new System.ArgumentOutOfRangeException();
    }

    public static Vector2Int[] GetCellsInRect(Vector2Int corner1, Vector2Int corner2)
    {
        var result = new List<Vector2Int>();
        for (int x = corner1.x; x < corner2.x; x++)
        {
            for (int y = corner1.y; y < corner2.y; y++)
            {
                result.Add(new Vector2Int(x, y));
            }
        }
        return result.ToArray();
    }

    public static Vector2Int[] GetTilesInRadius(Vector2Int center, int radius)
    {
        var cornerVector = new Vector2Int(radius, radius);
        var cellsInRect = GetCellsInRect(center - cornerVector, center + cornerVector);
        var sqrRad = radius * radius;

        return cellsInRect.Where(c => (c - center).sqrMagnitude <= sqrRad).ToArray();
    }
}