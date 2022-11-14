using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Serializing;
public class stringTest
{
    public string text;

    public stringTest(string _text)
    {
        text = _text;
    }
}
public static class stringReadWrite
{
    public static void WriteStringTest(this Writer writer, stringTest value)
    {
        Debug.Log("passo por aki write");
        writer.WriteString(value.text);
    }
    public static stringTest ReadStringTest(this Reader reader)
    {
        Debug.Log("passo por aki read");
        return new stringTest(reader.ReadString());
    }
}