using System;
using System.Collections.Generic;
using System.Text;

namespace Hydra.Such.Data.ViewModel
{
    public class ErrorHandler
    {
        public int eReasonCode { get; set; }
        public string eMessage { get; set; }

        public List<TraceInformation> eMessages { get; set; }

        public ErrorHandler()
        {
            eMessages = new List<TraceInformation>();
        }
    }
    public struct TraceInformation
    {
        public TraceInformation(TraceType type, string message)
        {
            Type = type;
            Message = message;
        }

        public TraceType Type { get; private set; }
        public string Message { get; private set; }
    }

    public enum TraceType
    { Error, Success /*, Warning, Information*/}

    public class FileActionResult : ErrorHandler
    {
        public string Base64FileContent { get; set; }
    }
}
