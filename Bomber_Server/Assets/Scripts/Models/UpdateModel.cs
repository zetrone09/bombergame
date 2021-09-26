using System.Collections.Generic;

public class UpdateModel : BaseModel
{
    public static readonly string CLASS_NAME = typeof(UpdateModel).Name;

    public List<PlayerPositionModel> PlayerPositionModels { get; set; } = new List<PlayerPositionModel>();

    public UpdateModel() : base(CLASS_NAME)
    {
    }
}