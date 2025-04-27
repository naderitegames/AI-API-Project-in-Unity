using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using TMPro;
using UnityEngine;

public class MemoryInput : MonoBehaviour
{
    [SerializeField] TMP_InputField title, description;
    public string Description => description.text;
    public string Ttle => title.text;

    public void RefreshInputs()
    {
        title.text = description.text = "";
    }

    public void ClearDescription()
    {
        description.text = "";
    }

    public void ForceInputValuesTo()
    {
        
    }
}