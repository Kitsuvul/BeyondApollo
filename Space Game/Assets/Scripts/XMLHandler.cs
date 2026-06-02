using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class XMLHandler : MonoBehaviour
{
    XmlDocument levelDataXml;

    public struct PlanetToLoad
    {
        public GameObject planetPrefab;
        public Vector3 Position;
        public bool isSatellite;
    };

    List<PlanetToLoad> planetToLoad;
    [SerializeField] private GameObject PlanetSmallPrefab;
    [SerializeField] private GameObject PlanetMediumPrefab;
    [SerializeField] private GameObject PlanetLargePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitXML();
    }

    private void InitXML()
    {
        levelDataXml = new XmlDocument();
        levelDataXml.LoadXml(Resources.Load<TextAsset>("XML/LevelsNew").text);
    }

    public bool AddEntry(List<GameObject> planets, int id, SortedDictionary<int, int> satelliteID)
    {
        XmlNode level = levelDataXml.SelectSingleNode("/Levels/Level[@ID='" + id + "']");
        if (level == null)
        {
            XmlAttribute attr = levelDataXml.CreateAttribute("ID");
            attr.Value = id.ToString();
            XmlElement newLevel = levelDataXml.CreateElement("Level");
            newLevel.SetAttributeNode(attr);

            foreach(KeyValuePair<int, int> set in satelliteID)
            {
                AddPlanet(planets[set.Key], newLevel, false);
                AddPlanet(planets[set.Value], newLevel, true);
            }

            for(int i = 0; i < planets.Count; ++i)
            {
                if(!satelliteID.ContainsKey(i) && !satelliteID.ContainsValue(i))
                {
                    AddPlanet(planets[i], newLevel, false);
                }
            }

            levelDataXml.DocumentElement.AppendChild(newLevel);
            XmlNodeList list = levelDataXml.SelectNodes("/Levels/Level");
            XmlNode totalLevels = levelDataXml.SelectSingleNode("/Levels/TotalLevels"); 
            totalLevels.InnerText = list.Count.ToString();
            levelDataXml.Save("C://Users//danie//Documents//GitHub//BeyondApollo//Space Game//Assets//Resources//XML//LevelsNew.xml");

            Debug.Log("Complete");
            return true;
        }
        return false;
    }

    void AddPlanet(GameObject planet, XmlElement element, bool isSatellite)
    {
        XmlElement newPlanet = levelDataXml.CreateElement("Planet");
        XmlElement size = levelDataXml.CreateElement("Size");
        if (planet.tag == "Tiny")
        {
            size.InnerText = "Tiny";
        }
        if (planet.tag == "Small")
        {
            size.InnerText = "Small";
        }
        else if (planet.tag == "Medium")
        {
            size.InnerText = "Medium";
        }
        else if (planet.tag == "Large")
        {
            size.InnerText = "Large";
        }
        if (planet.tag == "ExtraLarge")
        {
            size.InnerText = "ExtraLarge";
        }
        newPlanet.AppendChild(size);
        XmlElement pos = levelDataXml.CreateElement("Position");
        XmlElement x = levelDataXml.CreateElement("x");
        x.InnerText = GetRoundedPosition(planet.transform.position.x).ToString();
        XmlElement y = levelDataXml.CreateElement("y");
        y.InnerText = GetRoundedPosition(planet.transform.position.y).ToString();
        XmlElement z = levelDataXml.CreateElement("z");
        z.InnerText = GetRoundedPosition(planet.transform.position.z).ToString();
        pos.AppendChild(x);
        pos.AppendChild(y);
        pos.AppendChild(z);
        newPlanet.AppendChild(pos);
        if(isSatellite)
        {
            XmlElement sata = levelDataXml.CreateElement("Satellite");
            sata.InnerText = 1.ToString();
            newPlanet.AppendChild(sata);
        }

        element.AppendChild(newPlanet);
    }

    private float GetRoundedPosition(float pos)
    {
        float roundedToTwoDB = Mathf.Round((pos * 100) / 100);
        return roundedToTwoDB;
    }
}
