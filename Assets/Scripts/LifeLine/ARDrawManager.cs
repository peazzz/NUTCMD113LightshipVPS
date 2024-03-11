using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class ARDrawManager : MonoBehaviour
{
    public static ARDrawManager instance;
    [SerializeField] private LineSettings _lineSettings;
    [SerializeField] private UnityEvent OnDraw;
    [SerializeField] private Camera _aRCamera;
    [SerializeField] private ARLineColorPicker _aRLineColorPicker;
    private Dictionary<int, ARLine> _lines = new Dictionary<int, ARLine>();
    [SerializeField] private bool _canDraw;
    private int lineCount;
    //[SerializeField] private Transform _worldAnchor;
    public Transform DrawLocation;
    [SerializeField] private string _currentColor;
    [SerializeField] private List<LineDataModel> LineDataHandle = new List<LineDataModel>();
    public GameObject DrawButton;
    public GameObject SaveButton;
    public static bool isDrawing;

    //void OnEnable()
    //{
    //    WayspotAnchorStatue.OnWaySpotFound += WayspotAnchorFounded;
    //}

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Update()
    {
        if (localize.canDraw && _lines.Count == 0 && !isDrawing)
        {
            DrawButton.SetActive(true);
        }

#if !UNITY_EDITOR
        DrawOnTouch();
#else
        DrawOnMouse();
#endif
    }

    void DrawOnTouch()
    {
        if (!_canDraw || Input.touchCount == 0) return;
        isDrawing = true;
        int tapCount = Input.touchCount > 1 && _lineSettings.allowMultiTouch ? Input.touchCount : 1;

        for (int i = 0; i < tapCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector3 touchPosition = _aRCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, _lineSettings.distanceFromCamera));

            if (touch.phase == TouchPhase.Began)
            {
                OnDraw?.Invoke();
                ARLine line = new ARLine(_lineSettings);
                _lines.Add(touch.fingerId, line);
                line.AddNewLineRenderer(DrawLocation, touchPosition, lineCount);//_worldAnchor
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                _lines[touch.fingerId].AddPoint(touchPosition);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (_lines[0].GetTempLine() != null)
                {
                    _lines[0].GetTempLine().GetComponent<ARLineDataHandler>().HoldLineData();
                    _lines[0].CleanTempLine();
                }
                _lines.Remove(touch.fingerId);
            }
        }
    }

    void DrawOnMouse()
    {
        if (!_canDraw) return;
        isDrawing = true;
        Vector3 mousePosition = _aRCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _lineSettings.distanceFromCamera));

        if (Input.GetMouseButtonDown(0))
        {
            lineCount++;
        }

        if (Input.GetMouseButton(0))
        {
            OnDraw?.Invoke();

            if (_lines.Keys.Count == 0)
            {
                ARLine line = new ARLine(_lineSettings);
                _lines.Add(0, line);

                line.AddNewLineRenderer(DrawLocation, mousePosition, lineCount);//_worldAnchor
            }
            else
            {
                _lines[0].AddPoint(mousePosition);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (_lines[0].GetTempLine() != null)
            {
                _lines[0].GetTempLine().GetComponent<ARLineDataHandler>().HoldLineData();
                _lines[0].CleanTempLine();
            }
            _lines.Remove(0);
        }
    }

    //void WayspotAnchorFounded()
    //{
    //    _worldAnchor = GameObject.FindObjectOfType<WayspotAnchorStatue>().transform;
    //}

    public void DrawActive(bool _active)
    {
        StartCoroutine(DelayDrawActive(_active));       
    }
    IEnumerator DelayDrawActive(bool _active)
    {
        yield return new WaitForSeconds(.2f);
        _canDraw = _active;
    }

    public void _draw()
    {
        isDrawing = true;
        DrawButton.SetActive(false);
        SaveButton.SetActive(true);
    }

    public void _save()
    {
        isDrawing = false;
        SaveButton.SetActive(false);
    }

    public void AlignToWorldAnchor(GameObject _go)
    {
        _go.transform.parent = DrawLocation;//_worldAnchor
    }

    public Material SetMaterial()
    {
        return _aRLineColorPicker.PickAColor();
    }

    public Material SetLoadedLineMaterial(string _name)
    {
        return _aRLineColorPicker.GetMaterial(_name);
    }
}
