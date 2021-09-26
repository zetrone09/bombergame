using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ModelToObjectMapper
{
    private Dictionary<string, Action<JObject>> Deserializes;

    private readonly ClientController clientController;

    private Dictionary<string, Action<JObject>> CreateDeserializes() => new Dictionary<string, Action<JObject>>
    {
        [CreatePlayerModel.CLASS_NAME] = OnCreatePlayerModel,
        [InitDataModel.CLASS_NAME] = OnInitData,
        [UpdateModel.CLASS_NAME] = OnUpdateModel,

    };

    public ModelToObjectMapper(ClientController clientController)
    {
        Deserializes = CreateDeserializes();
        this.clientController = clientController;
    }

    public void DeserializeToFunction(string json)
    {
        var jObject = JObject.Parse(json);
        if (jObject.TryGetValue("ClassName", out var value))
        {
            var className = value.ToString();
            Deserializes[className](jObject);
        }
    }
    private void OnCreatePlayerModel(JObject jObject)
    {
        Debug.Log("OnCreatePlayerModel");
        var model = jObject.ToObject<CreatePlayerModel>();
        clientController.OnCreatePlayer(model);
    }
    private void OnInitData(JObject jObject)
    {
        Debug.Log("OnInitData");
        var model = jObject.ToObject<InitDataModel>();
        clientController.OnInitData(model);
    }
    private void OnUpdateModel(JObject jObject)
    {
        var model = jObject.ToObject<UpdateModel>();
        clientController.UpdatePlayerModel(model);

    }


}