using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARLineColorPicker : MonoBehaviour
{
    [Header("挑色用")] [SerializeField] private Color _colorPreview;
    [Header("輸入色碼(要帶#符號)")] [SerializeField] private List<string> _colorList = new List<string>();
    [SerializeField] private List<Material> _materials = new List<Material>();
    void Start()
    {
        foreach (var item in _colorList)
        {
            // Standard
            //Material material = new Material(Shader.Find("Standard"));

            //URP
            Material material = new Material(Shader.Find("Standard"));


            Color color = HexToColor(item);
            material.name = item;
            material.color = color;
            _materials.Add(material);
        }
    }

    public Material PickAColor()
    {
        int _index = Random.Range(0, _materials.Count);
        return _materials[_index];
    }

    public Material GetMaterial(string _name)
    {
        foreach (var item in _materials)
        {
            if (_name == item.name)
            {
                return item;
            }
        }

        // Standard
        // Material material = new Material(Shader.Find("Standard"));

        //URP
        Material material = new Material(Shader.Find("Standard"));
        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", Color.green);


        Color color = HexToColor(_name);
        material.name = _name;
        material.color = color;
        _materials.Add(material);

        return material;
    }

    Color HexToColor(string hex)
    {
        hex = hex.Replace("#", "");
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        Color color = new Color32(r, g, b, 255);
        return color;
    }
}
