using System.Threading;
using Sulfur.Contract;
using sulfur.unity_sdk.Connection;
using sulfur.unity_sdk.HierarchyInspector;
using UnityEngine;

namespace sulfur.unity_sdk
{
    public class SulfurUnitySdk : MonoBehaviour
    {
        private static SulfurUnitySdk _instance;
        private CancellationTokenSource _cts;

        public static SulfurUnitySdk Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SulfurUnitySdk>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<SulfurUnitySdk>();
                        singletonObject.name = typeof(SulfurUnitySdk) + " (Singleton)";
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }

        public HierarchyXmlDom XmlDom;
        private HttpService _httpService;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            XmlDom = new HierarchyXmlDom();
            _httpService = new HttpService(Const.DEFAULT_PORT);
            _httpService.StartServer();

            // Debug.Log("DOM\n\n" + XmlNodeNameConverter.UnescapeGameObjectName(XmlDom.GetXmlDom().OuterXml));
            //
            // var _1 = XmlDom.FindByXpath("//ProxySetupUI");
            // Debug.Log("_1 RESULT :\n" + ((GameObjectXmlElement)_1).GameObject.name + "\n" + ((GameObjectXmlElement)_1).InnerXml);
            // var _2 = XmlDom.FindByXpath("//*[contains(@type, 'TextMeshProUGUI')]");
            // Debug.Log("_2 RESULT :\n" + ((GameObjectXmlElement)_2).GameObject.name + "\n" + ((GameObjectXmlElement)_2).InnerXml);
            // var _3 = XmlDom.FindAllByXpath("//*[contains(@type, 'TextMeshProUGUI')]");
            // Debug.Log("_3 RESULT :\n" + string.Join(",", _3.Select(i => ((GameObjectXmlElement)i).GameObject.name)) + "\n" + string.Join("\n", _3.Select(i => ((GameObjectXmlElement)i).InnerXml)));
            // var _4 = XmlDom.FindByXpath("//*[contains(@name, 'Canvas')]");
            // Debug.Log("_4 RESULT :\n" + ((GameObjectXmlElement)_4).GameObject.name + "\n" + ((GameObjectXmlElement)_4).OuterXml);
            //
            // var _5 = ((GameObjectXmlElement)_1).FindByXpath("//3_child");
            //
            // Debug.Log("_5 RESULT :\n" + ((GameObjectXmlElement)_5).GameObject.name + "\n" + ((GameObjectXmlElement)_5).OuterXml);
        }

        private void OnDestroy()
        {
            _cts.Dispose();
            _httpService?.StopServer();
        }
    }
}