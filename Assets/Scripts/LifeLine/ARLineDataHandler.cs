using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARLineDataHandler : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private string _color;
    private LineRenderer _lineRenderer;
    private int _pointCount;
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _name = gameObject.name;
        _position = gameObject.transform.localPosition;
        _rotation = gameObject.transform.localEulerAngles;
        _color = GetComponent<Renderer>().material.name.Replace(" (Instance)", "");
    }

    public void HoldLineData()
    {
        if (_lineRenderer.positionCount <= 3)
        {
            Destroy(this.gameObject);
            Debug.Log("²¾°£»~ÂI");
            return;
        }

        LineDataModel _save = new LineDataModel();
        _save.name = _name;
        _save.position = _position;
        _save.rotation = _rotation;
        _save.color = _color;
        _save.linePathData = new LinePathData();

        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            _save.linePathData.linePath.Add(transform.InverseTransformPoint(_lineRenderer.GetPosition(i)));
        }
        Debug.Log($"Save Object {_name}");

        SaveAndLoad.instance.HandleLineData(_save);
    }
}
