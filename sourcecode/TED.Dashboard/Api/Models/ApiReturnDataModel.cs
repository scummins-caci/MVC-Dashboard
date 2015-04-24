
namespace TED.Dashboard.Api.Models
{
    /// <summary>
    /// A view model class that handles all data returned by the 
    /// api.  Holds data, status and return codes
    /// </summary>
    public class ApiReturnDataModel
    {
        public ApiReturnDataModel(object data, int status, bool success)
        {
            Data = data;
            Status = status;
            Success = success;
        }

        public object Data { get; set; }
        public int Status { get; set; }
        public bool Success { get; set; }
    }
}