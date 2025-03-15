using Sulfur.Contract.DataModels;
using Sulfur.Contract.DataModels.Xml;
using Sulfur.Contract.Helpers;

namespace Sulfur.Contract.Communication.Command
{
    [MessageContract("/findObject", typeof(FindObjectResponse))]
    public class FindObjectRequest : RequestMessage
    {
        public FindObjectRequest(string xPath)
        {
            XPath = xPath;
        }

        public string XPath { get; set; }
    }

    public class FindObjectResponse : ResponseMessage
    {
        public SulfurObjectData Data { get; set; }
    }
    
    [MessageHandler(typeof(FindObjectRequest))]
    public class FindObjectHandler : MessageHandler<FindObjectRequest, FindObjectResponse>
    {
        public override FindObjectResponse HandleRequest(FindObjectRequest request, ILogger logger, IXmlDom xmlDom)
        {
            var data = xmlDom.FindByXpath(request.XPath).Data;

            return new FindObjectResponse()
            {
                Data = data
            };
        }
    }
}