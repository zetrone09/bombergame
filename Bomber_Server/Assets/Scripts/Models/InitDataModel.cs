using System.Collections.Generic;
public class InitDataModel : BaseModel
{
    public static readonly string CLASS_NAME = typeof(InitDataModel).Name;

    public InitDataModel() : base(CLASS_NAME)
    {

    }

    public int PlayerId { get; set; }
    public List<CreatePlayerModel> CreatePlayerModels { get; set; } = new List<CreatePlayerModel>();
}