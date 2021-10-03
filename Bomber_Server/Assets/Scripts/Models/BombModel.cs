using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombModel : BaseModel
{
    public static readonly string CLASS_NAME = typeof(BombModel).Name;
    public float CurrnetTime { get; set; }

    public Vector3Model PositionModel { get; set; } = Vector3Model.ZERO;

    [JsonIgnore]
    public Vector3 Position
    {
        get
        {
            return PositionModel.ToUnityVector3();
        }
        set
        {
            PositionModel.From(value);
        }
    }

    public BombModel() : base(CLASS_NAME)
    {
    }
}