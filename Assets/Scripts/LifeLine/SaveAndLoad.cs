using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SaveAndLoad : MonoBehaviour
{
    public static SaveAndLoad instance;
    [SerializeField] private GameObject _linePrefab;
    [Header("§ó·sÀW²v(¬í)")] [SerializeField] private float _updateRate;
    [SerializeField] private GameObject _drawUI;
    private List<GameObject> _tempLoadedLine = new List<GameObject>();
    private int _tempLoadedLineCount;
    [SerializeField] private List<LineDataModel> LineDataHandle = new List<LineDataModel>();
    public GameObject InstantiateLocation;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        InvokeRepeating("LoadData", 5f, _updateRate);
    }

    public void HandleLineData(LineDataModel _lineDataModel)
    {
        LineDataHandle.Add(_lineDataModel);
    }

    public void CheckHasDataToSave()
    {
        if (LineDataHandle.Count == 0)
        {
            Debug.Log("0");
            //_drawUI.SetActive(true);
        }
        else
        {
            SaveData();
        }
    }

    public void SaveData()
    {
        LineData _lineData = new LineData();

        for (int i = 0; i < LineDataHandle.Count; i++)
        {
            LineDataModel _data = new LineDataModel();
            _data.name = LineDataHandle[i].name;
            _data.position = LineDataHandle[i].position;
            _data.color = LineDataHandle[i].color;
            _data.linePathData = LineDataHandle[i].linePathData;

            _lineData.lineData.Add(_data);
        }

        StartCoroutine(SaveToSheet(_lineData));
    }

    public void LoadData()
    {
        StartCoroutine(LoadFromSheet());

        if (_tempLoadedLine.Count == 0) return;
        _tempLoadedLineCount = _tempLoadedLine.Count;
        StartCoroutine(DestoryTempLine());
    }

    IEnumerator SaveToSheet(LineData _json)
    {
        string json = JsonUtility.ToJson(_json, true);
        Debug.Log("Sent JSON: " + json);
        UnityWebRequest request = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbynAWLkML8ewD3iDMKj2GieNby50egPKSG5xB2T0KVsfTjT5hqVIeOp7b3DqNa5MdcjHA/exec", json);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            LineDataHandle.Clear();
            ARDrawManager.isDrawing = false;
        }
        else
        {
            Debug.LogError(request.error);
        }
    }

    IEnumerator LoadFromSheet()
    {
        UnityWebRequest request = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbynAWLkML8ewD3iDMKj2GieNby50egPKSG5xB2T0KVsfTjT5hqVIeOp7b3DqNa5MdcjHA/exec", "Read");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            ProcessJsonFormat(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError(request.error);
        }
    }

    void ProcessJsonFormat(string _json)
    {
        if (_json == "") return;

        DataPack _pack = JsonUtility.FromJson<DataPack>(_json);

        foreach (var _data in _pack.dataPack)
        {
            for (int i = 0; i < _data.lineData.Count; i++)
            {
                SetLoadedLine(_data.lineData[i].name + " " + i, _data.lineData[i].position, _data.lineData[i].rotation, _data.lineData[i].color, _data.lineData[i].linePathData.linePath);
            }
        }
    }

    void SetLoadedLine(string _name, Vector3 _position, Vector3 _rotation, string _color, List<Vector3> _linePath)
    {
        GameObject _go = Instantiate(_linePrefab);
        _go.transform.parent = InstantiateLocation.transform;
        _tempLoadedLine.Add(_go);
        //ARDrawManager.instance.AlignToWorldAnchor(_go);
        _go.name = "(loaded) " + _name;
        _go.transform.localPosition = _position;
        //_go.GetComponent<Renderer>().material = ARDrawManager.instance.SetLoadedLineMaterial(_color);
        LineRenderer _line = _go.GetComponent<LineRenderer>();

        _line.positionCount = _linePath.Count;
        _line.SetPositions(_linePath.ToArray());
    }

    IEnumerator ToggleUIDisplay(GameObject _go, bool _active, float _sec)
    {
        yield return new WaitForSeconds(_sec);
        _go.SetActive(_active);
    }

    IEnumerator DestoryTempLine()
    {
        yield return new WaitForSeconds(3);

        for (int i = 0; i < _tempLoadedLineCount; i++)
        {
            Destroy(_tempLoadedLine[0]);
            _tempLoadedLine.Remove(_tempLoadedLine[0]);
        }
    }
}
