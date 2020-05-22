using System;
namespace SmartShareClient.Model
{
    using System.Net;
    using System.Runtime.Serialization;

    [DataContract]
    public class SmartShareError
    {
        [DataMember(Name = "error")]
        public string ErrorCode { get; set; }

        [DataMember(Name = "error_description")]
        public string Description { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public SmartShareError() { }

        public SmartShareError(string code, string desc)
        {
            ErrorCode = code;
            Description = desc;
        }
        public SmartShareError(HttpStatusCode code, string desc)
        {
            Description = desc;
            StatusCode = code;
            ErrorCode = code.ToString();
        }

        public override string ToString()
            => $"Status: {(string.IsNullOrWhiteSpace(ErrorCode) ? "No Code" : ErrorCode)}; Description: {(string.IsNullOrWhiteSpace(Description) ? "No Description" : Description)}";
    }
}
