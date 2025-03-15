using Sulfur.Contract.Communication.Command;
using Sulfur.Contract.DataModels;

namespace Sulfur.Driver
{
    public class SulfurObject : SulfurObjectData
    {
        private SulfurDriver _driver;

        public SulfurObject(SulfurDriver driver, SulfurObjectData data)
        {
            _driver = driver;

            Id = data.Id;
            Name = data.Name;
        }

        public SulfurObject FindChild(string childXpath)
        {
            var resp = _driver.Client.SendRequest<FindObjectRequest, FindObjectResponse>(new FindObjectRequest($"//*[@id={Id}]{childXpath}"));

            return new SulfurObject(_driver, resp.Data);
        }
    }
}