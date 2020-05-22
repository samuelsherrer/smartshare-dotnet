using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SmartShareClient.Model
{
    [Serializable]
    public class SmartShareException : Exception
    {
        public string RawJsonResponse { get; set; }
        public SmartShareError ReturnedError { get; set; }
        HttpStatusCode StatusCode { get; set; }

        public SmartShareException()
        {
        }

        public SmartShareException(string message) : base(message)
        {
        }

        public SmartShareException(string message, string jsonResponse) : base(message)
        {
            RawJsonResponse = jsonResponse;
        }

        public SmartShareException(string message, Exception innerException) : base(message, innerException)
        { }

        public SmartShareException(string message, SmartShareError error, string responseContent) : base(message)
        {
            ReturnedError = error;
            RawJsonResponse = responseContent;
        }

        public override string Message => base.Message + $", Response = {(string.IsNullOrWhiteSpace(RawJsonResponse) ? ReturnedError.ToString() : RawJsonResponse)}";
    }
}
