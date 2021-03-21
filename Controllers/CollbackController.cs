using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace Queis
{
    [Route("vkapi")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IVkApi _vkApi;

        static List<string> response = new List<string>();

        public CallbackController(IVkApi vkApi, IConfiguration configuration)
        {
            _vkApi = vkApi;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Callback([FromBody]System.Text.Json.JsonElement message)
        {
            if (response.Count > 99)
                response.RemoveAt(0);

            response.Add(message.ToString());

            JsonParser parsedValue = System.Text.Json.JsonSerializer.Deserialize<JsonParser>(message.ToString());
            switch (parsedValue.type)
            {
                case "confirmation":
                    return Ok(_configuration["Config:Confirmation"]);
                case "message_new":
                    {
                        Console.WriteLine(parsedValue.Object);
                        //Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JToken.Parse(parsedValue.Object.ToString());
                        //var msg = Message.FromJson(new VkResponse(token));
                        //Console.WriteLine(msg.Text);
                        /*_vkApi.Messages.Send(new MessagesSendParams
                        {
                            RandomId = new DateTime().Millisecond,
                            PeerId = msg.UserId.Value,
                            Message = msg.Text
                        });*/
                        Console.WriteLine(2);
                        break;
                    }
            }
            return Ok("ok");
        }

        [HttpGet]
        public IActionResult Get()
        {
            string result = "";
            foreach (var res in response)
                result += res + "\n";
            return Ok(result);
        }
    }
}
