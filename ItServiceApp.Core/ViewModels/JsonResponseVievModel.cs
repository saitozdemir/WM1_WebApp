using System;

namespace ITServiceApp.Core.ViewModels
{
    public class JsonResponseVievModel
    {
        public bool IsSuccess { get; set; } = true;
        public object Data { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ResponseTime { get; set; } = DateTime.UtcNow;
    }
}
