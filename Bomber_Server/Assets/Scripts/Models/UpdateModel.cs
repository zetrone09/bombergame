using System.Collections.Generic;

public class UpdateModel : BaseModel
{
    public static readonly string CLASS_NAME = typeof(UpdateModel).Name;

    public List<PlayerPositionModel> PlayerPositionModels { get; set; } = new List<PlayerPositionModel>();
    public List<int> PlayerRemoveIds { get; set; } = new List<int>();
    public List<BombModel> NewBombs { get; set; } = new List<BombModel>();
    public List<int> PlayerDeathIds { get; set; } = new List<int>();

    public Dictionary<int, Vector3Model> CreateCoins = new Dictionary<int, Vector3Model>();
    public List<int> DeletedCoins = new List<int>();

    public UpdateModel() : base(CLASS_NAME)
    {
    }
}