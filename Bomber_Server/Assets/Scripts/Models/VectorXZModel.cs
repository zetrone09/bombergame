using UnityEngine;

public class VectorXZModel
{
    public static readonly VectorXZModel ZERO = new VectorXZModel { X = 0, Z = 0 };
    public float X { get; set; }
    public float Z { get; set; }

    public void From(Vector3 value)
    {
        X = value.x;
        Z = value.z;
    }

    public Vector3 ToUnityVector3()
    {
        return new Vector3(X, 0, Z);
    }
}