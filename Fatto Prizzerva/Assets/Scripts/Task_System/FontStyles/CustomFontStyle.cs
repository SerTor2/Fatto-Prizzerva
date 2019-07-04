using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[CreateAssetMenu(fileName = "New Font Style", menuName = "CustomFontStyle/Crete New")]
public class CustomFontStyle : ScriptableObject
{
    [Header("Style Info")]
    [SerializeField] [TextArea] private string Description;

    [Header("Style atributes")]
    [SerializeField] private FontStyles fontStyle;
    [SerializeField] [ColorUsage(false)] private Color fontColor;


    /// <summary>
    /// Aplicamos este estilo a otro objeto
    /// </summary>
    /// <param name="_textObject"></param>
    public void ApplyStyleToText(TextMeshProUGUI _textObject)
    {
        _textObject.fontStyle = fontStyle;
        _textObject.color = fontColor;

    }

}
