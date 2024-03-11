using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARLine : MonoBehaviour
{
    private int positionCount = 0;
    private Vector3 prevPointDistance = Vector3.zero;
    private LineRenderer LineRenderer { get; set; }
    private LineSettings settings;

    public GameObject tempLine;

    public ARLine(LineSettings settings)
    {
        this.settings = settings;
    }

    public void AddPoint(Vector3 position)
    {
        if (prevPointDistance == null)
            prevPointDistance = position;

        if (prevPointDistance != null && Mathf.Abs(Vector3.Distance(prevPointDistance, position)) >= settings.minDistanceBeforeNewPoint)
        {
            prevPointDistance = position;
            positionCount++;

            LineRenderer.positionCount = positionCount;

            LineRenderer.SetPosition(positionCount - 1, position);

            if (LineRenderer.positionCount % settings.applySimplifyAfterPoints == 0 && settings.allowSimplification)
            {
                LineRenderer.Simplify(settings.tolerance);
            }
        }
    }

    public void AddNewLineRenderer(Transform parent, Vector3 position, int _lineCount)
    {
        positionCount = 2;
        GameObject go = new GameObject($"LineRenderer {_lineCount}");
        tempLine = go;

        go.AddComponent<ARLineDataHandler>();
        go.transform.parent = parent;
        go.transform.position = position;
        go.tag = settings.lineTagName;

        LineRenderer goLineRenderer = go.AddComponent<LineRenderer>();
        goLineRenderer.startWidth = settings.startWidth;
        goLineRenderer.endWidth = settings.endWidth;

        goLineRenderer.startColor = settings.startColor;
        goLineRenderer.endColor = settings.endColor;

        // goLineRenderer.material = settings.defaultMaterial;
        goLineRenderer.material = ARDrawManager.instance.SetMaterial();
        goLineRenderer.useWorldSpace = true;
        goLineRenderer.positionCount = positionCount;

        goLineRenderer.numCornerVertices = settings.cornerVertices;
        goLineRenderer.numCapVertices = settings.endCapVertices;

        goLineRenderer.SetPosition(0, position);
        goLineRenderer.SetPosition(1, position);

        LineRenderer = goLineRenderer;
    }

    public GameObject GetTempLine()
    {
        if (tempLine != null) return tempLine;
        else return null;
    }

    public void CleanTempLine()
    {
        tempLine = null;
    }
}
