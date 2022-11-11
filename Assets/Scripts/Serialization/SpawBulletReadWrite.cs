using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public struct SpawBulletTransform
{
    public string NameBullet;
    public Vector3 Position;
    public Quaternion Rotation;

    public SpawBulletTransform ( string _nameBullet , Vector3 _position, Quaternion _rotation)
    {
        NameBullet = _nameBullet;
        Position = _position;
        Rotation = _rotation;
    }
}
public static class SpawBulletReadWrite
{
    public static void WriteSpawBulletTransform ( this NetworkWriter writer , SpawBulletTransform value )
    {
        writer.WriteString ( value.NameBullet );
        writer.WriteVector3 ( value .Position);
        writer.WriteQuaternion ( value.Rotation);
    }

    public static SpawBulletTransform ReadSpawBulletTransform ( this NetworkReader reader )
    {
        return new SpawBulletTransform (reader.ReadString(), reader.ReadVector3(), reader.ReadQuaternion());
    }

}