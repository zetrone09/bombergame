using System.Collections.Generic;

public class UpdateModel : BaseModel
{
    public static readonly string CLASS_NAME = typeof(UpdateModel).Name;

    public List<PlayerPositionModel> PlayerPositionModels { get; set; } = new List<PlayerPositionModel>();
    public Dictionary<int, Vector3Model> CreateCoins = new Dictionary<int, Vector3Model>();
    public List<int> DeletedCoins = new List<int>();

    public UpdateModel() : base(CLASS_NAME)
    {
    }
}