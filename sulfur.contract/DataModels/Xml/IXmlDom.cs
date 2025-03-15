namespace Sulfur.Contract.DataModels.Xml
{
    public interface IXmlDom
    {
        IGameObjectXmlElement FindByXpath(string requestXPath);
        IGameObjectXmlElement[] FindAllByXpath(string requestXPath);
    }
}