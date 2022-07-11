namespace ServIT.Business.GenericResponse
{
    public class BaseResponse <T>
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public T Data { get; set; }
    }
}
