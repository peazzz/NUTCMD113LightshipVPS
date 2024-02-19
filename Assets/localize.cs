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
            AreaText.text = "中央";
        }
        else if (other.gameObject.tag == "path")
        {
            AreaText.text = "走道";
        }
        else if (other.gameObject.tag == "wall")
        {
            AreaText.text = "牆面";
        }
        else
        {
            AreaText.text = "";
        }
    }
}
