using UnityEngine;

public class Vector3Model
{
    public static readonly Vector3Model ZERO = new Vector3Model { X = 0, Y = 0, Z = 0 };

    public Vector3Model()
    {
    }

    public Vector3Model(Vector3 position)
    {
        X = position.x;
        Y = position.y;
        Z = position.z;
    }

    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public void From(Vector3 value)
    {
        X = value.x;
        Y = value.y;
        Z = value.z;
    }

    public Vector3 ToUnityVector3()
    {
        return new Vector3(X, Y, Z);
    }
}