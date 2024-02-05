using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Promrub.Services.API.Models.ResponseModels.Common;

namespace Promrub.Services.API.Handlers
{
    public static class ResponseHandler
    {
        public static StatusResponseModel ResponseMessage(string code,string? header)
        {
            string message = string.Empty; 
            switch (code)
            {
                case "1000":
                    message = $"Success";
                    break;
                case "1101":
                    message = $"Missing required parameters";
                    break;
                case "1102":
                    message = $"Invalid parameters entered";
                    break;
                case "1111":
                    message = $"Data entry duplicated with existing";
                    break;
                case "4101":
                    message = $"Current channel is not supported";
                    break;
                case "9100":
                    message = $"Required standard headers {header} invalid";
                    break;
                case "9500":
                    message = $"Invalid apikey provided";
                    break;
            }
            return new StatusResponseModel { Code = code, Message = message };
        }
    }
}
