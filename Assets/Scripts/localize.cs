using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class localize : MonoBehaviour
{
    public Text AreaText;
    public static bool canDraw;

    void Updata()
    {
        transform.up = Vector3.up;//�O������
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "center")
        {
            AreaText.text = "����";
            canDraw = true;
        }
        else if (other.gameObject.tag == "path")
        {
            AreaText.text = "���D";
            canDraw = false;
        }
        else if (other.gameObject.tag == "wall")
        {
            AreaText.text = "��";
            canDraw = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "center")
        {
            AreaText.text = " ";
            canDraw = false;
        }
        else if (other.gameObject.tag == "path")
        {
            AreaText.text = " ";
            canDraw = false;
        }
        else if (other.gameObject.tag == "wall")
        {
            AreaText.text = " ";
            canDraw = false;
        }
    }
}
