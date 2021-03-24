using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VkNet.Abstractions;
using System.Text.Json;
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
        public IActionResult Callback([FromBody]JsonElement message)
        {
            if (response.Count > 99)
                response.RemoveAt(0);
            response.Add(message.ToString());

            Dictionary<string, object> parsedValue = JsonSerializer.Deserialize<Dictionary<string, object>>(message.ToString());



            if (!parsedValue.ContainsKey("type"))
                return NotFound("Has not type");
            switch (parsedValue["type"].ToString())
            {
                case "confirmation":
                    return Ok(_configuration["Config:Confirmation"]);
                case "message_new":
                    {
                        if (!parsedValue.ContainsKey("object"))
                            return NotFound("Has not object");
                        new messageObjectHandlers(_vkApi, parsedValue["object"].ToString());
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
