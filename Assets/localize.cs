using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class localize : MonoBehaviour
{
    public Text AreaText;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "center")
        {
            AreaText.text = "����";
        }
        else if (other.gameObject.tag == "path")
        {
            AreaText.text = "���D";
        }
        else if (other.gameObject.tag == "wall")
        {
            AreaText.text = "��";
        }
        else
        {
            AreaText.text = "";
        }
    }
}
