public class MovePlayerModel : BaseModel
{
    public static readonly string CLASS_NAME = typeof(MovePlayerModel).Name;

    public VectorXZModel Target { get; set; }

    public MovePlayerModel() : base(CLASS_NAME)
    {
    }
}