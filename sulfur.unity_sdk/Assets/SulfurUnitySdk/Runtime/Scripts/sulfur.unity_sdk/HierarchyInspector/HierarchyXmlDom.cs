using System.Xml;
using Sulfur.Contract.DataModels.Xml;
using Sulfur.Contract.Exceptions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace sulfur.unity_sdk.HierarchyInspector
{
    public class HierarchyXmlDom : IXmlDom
    {
        public XmlDocument GetXmlDom()
        {
            var xmlDoc = new XmlDocument();

            var root = xmlDoc.CreateElement(Application.productName);
            xmlDoc.AppendChild(root);

            foreach (var go in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                var objElement = CreateXmlElementForGameObject(xmlDoc, go);
                root.AppendChild(objElement);
            }

            return xmlDoc;
        }

        public IGameObjectXmlElement FindByXpath(string xpath)
        {
            var xmlDoc = GetXmlDom();
            var xmlDomAdoptedXpath = XmlNodeNameConverter.ProcessXPath(xpath);
            var node = xmlDoc.SelectSingleNode(xmlDomAdoptedXpath);

            if (node == null) throw new ElementNotFoundException(xpath);

            return node as GameObjectXmlElement;
        }

        public IGameObjectXmlElement[] FindAllByXpath(string xpath)
        {
            var xmlDoc = GetXmlDom();
            var nodes = xmlDoc.SelectNodes(xpath);

            if (nodes == null) throw new ElementNotFoundException(xpath);

            var objects = new IGameObjectXmlElement[nodes.Count];
            for (int i = 0; i < nodes.Count; i++)
            {
                objects[i] = nodes[i] as GameObjectXmlElement;
            }

            return objects;
        }

        private GameObjectXmlElement CreateXmlElementForGameObject(XmlDocument doc, GameObject obj)
        {
            var element = doc.CreateElement(obj);
            foreach (Transform child in obj.transform)
            {
                var childElement = CreateXmlElementForGameObject(doc, child.gameObject);
                element.AppendChild(childElement);
            }

            return element;
        }
    }
}