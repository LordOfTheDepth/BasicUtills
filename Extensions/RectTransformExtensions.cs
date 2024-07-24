using UnityEngine;

public static class RectTransformExtensions
{
    public static Vector2 FitInsideRectTransform(this RectTransform child, Vector2 position, RectTransform parent)
    {
        var childSize = child.rect.size;
        var childPivot = child.pivot;

        var corners = new Vector3[4];
        parent.GetWorldCorners(corners);



        var leftBound = corners[0].x;
        var rightBound = corners[3].x;

        var lowerBound = corners[0].y;
        var upperBound = corners[1].y;

        var leftOffset = childSize.x * childPivot.x;
        var rightOffset = - childSize.x * (1 - childPivot.x);
        var lowerOffset = childSize.y * childPivot.y;
        var upperOffset = - childSize.y * (1 - childPivot.y);

        leftBound += leftOffset;
        rightBound += rightOffset;
        lowerBound += lowerOffset;
        upperBound += upperOffset;


        var x = Mathf.Clamp(position.x, leftBound, rightBound);
        var y = Mathf.Clamp(position.y, lowerBound, upperBound);

        

        return new Vector2(x, y);
    }
}
