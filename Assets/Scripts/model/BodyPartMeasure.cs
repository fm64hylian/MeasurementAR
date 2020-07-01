using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BodyPart
{
    None,
    Neck,
    Shoulders,
    Hip,
    Waist
}

public class BodyPartMeasure
{
    public BodyPart Part;
    public float Cm { get; set; }
    public float Girth;
    float UnityUnit;

    public BodyPartMeasure(BodyPart part, float unityU)
    {
        Part = part;
        UnityUnit = unityU;
        Cm = unityU * 100;
    }

    public float GetGirth()
    {
        return UnityUnit * 100f * Mathf.PI;
    }
}
