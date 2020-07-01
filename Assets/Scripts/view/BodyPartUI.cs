using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BodyPartUI : MonoBehaviour
{
    [HideInInspector]
    public BodyPartMeasure BodyPart;
    [HideInInspector]
    public bool IsSet;

    [SerializeField]
    Text BodyText;
    [SerializeField]
    Text Measure;

    public Action<BodyPartUI> OnSelected;
    public Action<BodyPartUI> OnSaved;


    public void SetData(BodyPartMeasure bodyPart)
    {
        BodyPart = bodyPart;
        BodyText.text = bodyPart.Part.ToString();
        Measure.text = bodyPart.Cm.ToString();
    }

    public void UpdateMeasure(string measure) {
        Measure.text = measure;
    }

    public void OnPartSelected() {
        if (OnSelected != null) {
            OnSelected(this);
        }    
    }

    public void OnPartSaved() {
        if (OnSaved != null) {
            OnSaved(this);
            SetAsMeasured();
        }    
    }

    /// <summary>
    /// will highlight the row if the length is set
    /// </summary>
    void SetAsMeasured() {
        Debug.Log("set as measured "+IsSet);
        ColorBlock  color= gameObject.GetComponent<Button>().colors;
        color.normalColor = IsSet ? new Color32(46, 101, 111, 255) : new Color32(89, 89, 89, 255);
        gameObject.GetComponent<Button>().colors = color;
    }
}
