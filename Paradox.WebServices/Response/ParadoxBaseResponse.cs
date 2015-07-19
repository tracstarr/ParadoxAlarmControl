namespace Paradox.WebServices.Response
{
    /// This is largly used for SmartThings app. It's not able to receive direct response from a call, instead we need to parse values and this is an easy way to check what call was made
    public class ParadoxBaseResponse
    {
        public string Action { get; set; }
    }
}
