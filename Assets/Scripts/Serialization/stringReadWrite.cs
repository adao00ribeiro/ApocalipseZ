using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class stringTest
{
    public string text;

    public stringTest ( string _text)
    {
        text = _text;
    }
}
public static class stringReadWrite
{
    public static void WriteStringTest ( this NetworkWriter writer , stringTest value)
    {
        Debug.Log ("passo por aki write" );
        writer.WriteString ( value.text);
    }
    public static stringTest ReadStringTest ( this NetworkReader reader)
    {
        Debug.Log ( "passo por aki read" );
        return new stringTest ( reader.ReadString());
    }
}