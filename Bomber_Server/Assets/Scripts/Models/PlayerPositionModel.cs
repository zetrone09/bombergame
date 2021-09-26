using Newtonsoft.Json;
using UnityEngine;

public class PlayerPositionModel
{
    public int PlayerId { get; set; }

    public Vector3Model PositionModel { get; set; } = new Vector3Model();

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
}