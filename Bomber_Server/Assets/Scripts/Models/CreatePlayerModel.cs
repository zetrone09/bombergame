using Newtonsoft.Json;
using UnityEngine;

public class CreatePlayerModel : BaseModel
{
    public static readonly string CLASS_NAME = typeof(CreatePlayerModel).Name;
    public int Id { get; set; }

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

    public CreatePlayerModel() : base(CLASS_NAME)
    {
    }
}