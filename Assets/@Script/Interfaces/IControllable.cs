using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public interface IControllable 
{
    public void Move(Vector2 mousePos, KeyCode clickType, bool isDown);
    public void KeyAction(KeyCode keyCode,Vector2 mousePos);
}
