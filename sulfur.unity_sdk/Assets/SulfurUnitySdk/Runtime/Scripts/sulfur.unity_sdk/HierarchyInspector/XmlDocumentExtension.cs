using System.Xml;
using UnityEngine;

namespace sulfur.unity_sdk.HierarchyInspector
{
    public static class XmlDocumentExtension
    {
        public static GameObjectXmlElement CreateElement(this XmlDocument doc, GameObject gameObject)
        {
            var node = new GameObjectXmlElement(gameObject, doc);

            return node;
        }
    }
}