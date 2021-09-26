using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerModel : BaseModel
{
    public static readonly string CLASS_NAME = typeof(UpdatePlayerModel).Name;

    public int Score { get; set; }

    public UpdatePlayerModel() : base(CLASS_NAME)
    {
    }
}