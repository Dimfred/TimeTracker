using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace TimeTracker
{

    public class ConfigHandler
    {


        private readonly XmlDocument configDoc = new XmlDocument();
        private readonly XmlDocument trackedItemsDoc = new XmlDocument();

        private readonly XmlNode RootNode_Config;
        private readonly XmlNode PathNode_Config;
        private readonly XmlNode RootNode_Tracked;

        public readonly string Config_Path = "Config.xml";
        private readonly string Config_RootName = "Configuration";
        private readonly string Config_PathName = "Path";

        public string TrackedItems_Path = "";
        private readonly string TrackedItems_FileName = "TrackedItems.xml";
        private readonly string TrackedItems_RootName = "Tracked_Items";

        public ConfigHandler()
        {
            try
            {
                configDoc.Load(Config_Path);
            }
            catch (Exception)
            {
                CreateConfig();
            }
            RootNode_Config = configDoc.SelectSingleNode(Config_RootName);
            PathNode_Config = RootNode_Config.SelectSingleNode(Config_PathName);

            try
            {
                string pathToLoad = GetConfigPath() == null ? TrackedItems_FileName : GetConfigPath() + TrackedItems_FileName;
                TrackedItems_Path = GetConfigPath() == null ? TrackedItems_Path : GetConfigPath();
                trackedItemsDoc.Load(pathToLoad);
            }
            catch (Exception)
            {
                CreateTracked();
            }
            RootNode_Tracked = trackedItemsDoc.SelectSingleNode(TrackedItems_RootName);
        }

        public void AddNewConfigPath(string path)
        {
            PathNode_Config.InnerText = path;
            configDoc.Save(Config_Path);
        }

        public string GetConfigPath()
        {
            return PathNode_Config.InnerText == "" ? null : PathNode_Config.InnerText;
        }

        public void AddItem(TrackedItem item)
        {
            XmlNode newItem = trackedItemsDoc.CreateElement(item.Name);
            newItem.InnerText = item.Time.ToString();
            RootNode_Tracked.AppendChild(newItem);
            trackedItemsDoc.Save(TrackedItems_Path + TrackedItems_FileName);
        }

        public void UpdateItem(TrackedItem item)
        {
            var toUpd = GetItem(item);
            if (toUpd != null)
            {
                toUpd.InnerText = item.Time.ToString();
                trackedItemsDoc.Save(TrackedItems_Path + TrackedItems_FileName);
            }
        }

        public void DeleteItem(TrackedItem item)
        {
            var toDel = GetItem(item);
            if (toDel != null)
            {
                RootNode_Tracked.RemoveChild(toDel);
                trackedItemsDoc.Save(TrackedItems_FileName);
            }
        }

        public List<TrackedItem> GetTrackedItems()
        {
            var trackedList = new List<TrackedItem>();
            foreach (XmlNode node in RootNode_Tracked.ChildNodes)
            {
                trackedList.Add(new TrackedItem(node.Name, node.InnerText));
            }
            return trackedList;
        }

        public void MoveTrackedItemsFile(string to)
        {
            string from = TrackedItems_Path == "" ? TrackedItems_FileName : TrackedItems_Path + TrackedItems_FileName;
            to = to + TrackedItems_FileName;

            File.Move(from, to);
            TrackedItems_Path = to.Replace(TrackedItems_FileName, "");
        }


        //PRIVATE
        public XmlNode GetItem(TrackedItem item)
        {
            foreach (XmlNode node in RootNode_Tracked.ChildNodes)
            {
                if (node.Name == item.Name)
                    return node;
            }
            return null;
        }

        private void CreateConfig()
        {
            XmlNode node = configDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            configDoc.AppendChild(node);

            XmlNode rootElement = configDoc.CreateElement(Config_RootName);
            configDoc.AppendChild(rootElement);

            XmlNode pathElement = configDoc.CreateElement(Config_PathName);
            rootElement.AppendChild(pathElement);

            configDoc.Save(Config_Path);
        }

        private void CreateTracked()
        {
            XmlNode node = trackedItemsDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            trackedItemsDoc.AppendChild(node);

            XmlNode rootElement = trackedItemsDoc.CreateElement(TrackedItems_RootName);
            trackedItemsDoc.AppendChild(rootElement);

            trackedItemsDoc.Save(TrackedItems_FileName);
        }
    }
}


