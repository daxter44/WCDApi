using System;
namespace WCDApi.Worker.HTML
{
    public class HTMLCompareResult
    {
        public int Type { get; set; }
        public string Message { get; set; }
        public HTMLCompareResult(int type, string message)
        {
            Type = type;
            Message = message;
        }
    }
}
