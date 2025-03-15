using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;
using Sulfur.Contract.DataModels;
using Sulfur.Contract.DataModels.Xml;
using Sulfur.Contract.Exceptions;
using UnityEngine;

namespace sulfur.unity_sdk.HierarchyInspector
{
    public sealed class GameObjectXmlElement : XmlElement, IGameObjectXmlElement, IXmlDom
    {
        public GameObject GameObject { get; set; }
        public int Id { get; set; }

        public SulfurObjectData Data => new SulfurObjectData()
        {
            Id = Id,
            Name = GameObject.name
        };

        public GameObjectXmlElement(GameObject gameObject, [NotNull] XmlDocument doc) : base(string.Empty, XmlNodeNameConverter.EscapeGameObjectName(gameObject.name), string.Empty, doc)
        {
            GameObject = gameObject;
            Id = gameObject.GetInstanceID();

            SetAttribute("name", gameObject.name);

            var types = new List<string>();
            foreach (var component in gameObject.GetComponents<Component>())
            {
                types.Add(component.GetType().ToString());
            }

            SetAttribute("type", string.Join(" ", types));
            SetAttribute("id", Id.ToString());
        }

        public IGameObjectXmlElement FindByXpath(string xpath)
        {
            var xmlDomAdoptedXpath = XmlNodeNameConverter.ProcessXPath(xpath);
            var node = SelectSingleNode(xmlDomAdoptedXpath);

            if (node == null) throw new ElementNotFoundException(xpath);

            return node as GameObjectXmlElement;
        }

        public IGameObjectXmlElement[] FindAllByXpath(string xpath)
        {
            var nodes = SelectNodes(xpath);

            if (nodes == null) throw new ElementNotFoundException(xpath);

            var objects = new IGameObjectXmlElement[nodes.Count];
            for (int i = 0; i < nodes.Count; i++)
            {
                objects[i] = nodes[i] as GameObjectXmlElement;
            }

            return objects;
        }
    }
}