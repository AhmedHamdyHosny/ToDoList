

namespace Classes.Utilities
{
    public class JsonResponse<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
        public string RedirectTo { get; set; }
        public System.Net.HttpStatusCode HttpStatusCode { get; set; }

        public JsonResponse<T> GetResponse(bool success, T model, string message, string redirectTo, AlertMessage alertMsg = null)
        {
            Result = model;
            RedirectTo = redirectTo;
            if (success)
            {
                Status = 1;
                HttpStatusCode = System.Net.HttpStatusCode.OK;
                alertMsg.MessageType = Common.Enums.AlertMessageType.Success;
            }
            else
            {
                Status = 0;
                HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
                alertMsg.MessageType = Common.Enums.AlertMessageType.Error;
            }
            Message = !string.IsNullOrEmpty(message) || (alertMsg == null) ? message : alertMsg.GetAlertMessage();
            return this;
        }
    }
}