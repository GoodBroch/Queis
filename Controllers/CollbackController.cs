using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Queis
{
    [Route("vkapi")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly Bot _vkApi;

        static List<string> response = new List<string>();

        public CallbackController(Bot vkApi, IConfiguration configuration)
        {
            _vkApi = Bot.instance(configuration["Config:AccessToken"]);
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