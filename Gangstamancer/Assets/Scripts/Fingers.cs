using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fingers", menuName = "Scriptables/Fingers")]
public class Fingers : ScriptableObject
{
    public FingerType FingersTypes;

    [Serializable, Flags]
    public enum FingerType
    {
        None = 0,
        Thumb = 1 << 0,
        Index = 1 << 1,
        Middle = 1 << 2,
        Ring = 1 << 3,
        Pinky = 1 << 4
    }

    [ShowAssetPreview]
    public Sprite SpriteLeft;

    [ShowAssetPreview]
    public Sprite SpriteRight;

    [ShowAssetPreview]
    public Sprite SpriteToDoLeft;

    [ShowAssetPreview]
    public Sprite SpriteToDoRight;
}
