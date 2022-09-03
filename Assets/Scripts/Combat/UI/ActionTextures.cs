using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ActionTextures : MonoBehaviour
{
    [Header("Attack")]
    public Texture2D Melee;
    public Texture2D Magic;
    public Texture2D Ranged;

    [Header("Defence")]
    public Texture2D Normal;
    public Texture2D MeleeImmune;
    public Texture2D MagicImmune;
    public Texture2D RangedImmune;

}
