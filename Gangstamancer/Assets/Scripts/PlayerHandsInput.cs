using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHandsInput", menuName = "Scriptables/PlayerHandsInput")]
public class PlayerHandsInput : ScriptableObject
{
    public List<KeyCode> LeftHandInputs = new List<KeyCode>();
    public List<KeyCode> RightHandInputs = new List<KeyCode>();
}
