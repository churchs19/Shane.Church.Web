//
//  This file is part of the Microsoft Windows Live SDK Code Samples.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
// 
// This source code is intended only as a supplement to Microsoft
// Development Tools and/or on-line documentation.  See these other
// materials for detailed information regarding Microsoft code samples.
// 
// THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//

using System;
using System.Collections.Generic;
using System.Xml;

namespace Windows.Live.SDK.Samples
{
    /// <summary>
    /// The Publish Plugin API communicates primarily via two XmlDocuments.
    /// The first XmlDocument is the sessionXml. This document will look 
    /// something like the following: (elements preceeded by *s are created
    /// and used soley by this sample plug-in, everything else is provided 
    /// by the Plugin Framework)
    /// 
    /// <?xml version="1.0"?>
    /// <PhotoGalleryPublishSession versionMajor="1" versionMinor="0">
    ///     <PublishParameters>
    ///         <MaxWidth>1024</MaxWidth>
    ///         <MaxHeight>1024</MaxHeight>
    ///     </PublishParameters>
    ///     <ItemSet>
    ///         <Item id="16">
    ///             <FullFilePath>C:\Users\Public\Pictures\Sample Pictures\Tree.jpg</FullFilePath>
    ///             <OriginalFileName>Tree.jpg</OriginalFileName>
    ///             <OriginalFileExtension>.jpg</OriginalFileExtension>
    ///             <PerceivedType>image</PerceivedType>
    ///             <Title>Sheep under a tree near Dorset, England.</Title>
    ///             <OriginalWidth>1024</OriginalWidth>
    ///             <OriginalHeight>768</OriginalHeight>
    ///             <LengthMS>0</LengthMS>
    ///             <FileSize>770042</FileSize>
    ///             <KeywordSet>
    ///                 <Keyword>Sample</Keyword>
    ///                 <Keyword>Landscape</Keyword>
    ///             </KeywordSet>
    ///         </Item>
    ///         <Item id="3">
    ///             <FullFilePath>C:\Users\Public\Videos\Sample Videos\Lake.wmv</FullFilePath>
    ///             <OriginalFileName>Lake.wmv</OriginalFileName>
    ///             <OriginalFileExtension>.wmv</OriginalFileExtension>
    ///             <PerceivedType>video</PerceivedType>
    ///             <Title/>
    ///             <OriginalWidth>720</OriginalWidth>
    ///             <OriginalHeight>480</OriginalHeight>
    ///             <LengthMS>0</LengthMS>
    ///             <FileSize>2981738</FileSize> 
    ///             <KeywordSet>
    ///                 <Keyword>Sample</Keyword>
    ///                 <Keyword>Landscape</Keyword>
    ///                 <Keyword>Wildlife</Keyword>
    ///             </KeywordSet>
    ///         </Item>
    ///     </ItemSet>
    /// </PhotoGalleryPublishSession>    
    /// 
    /// This XmlDocument will be preserved through the lifetime of the publish 
    /// session and you may make changes to it in any way you wish. This sample 
    /// plugin makes full use of the sessionXml XmlDocument. The static 
    /// functions that begin with Session... perform various operations on the 
    /// sessionXml XmlDocument.
    /// 
    /// The other XmlDocument is the persistXml and it will initially be empty. 
    /// The format you use for this document is up to you. All data stored in 
    /// the persistXml XmlDocument will be encrypted and stored on the local 
    /// machine between publish sessions. The XmlDocument will be loaded and 
    /// decrypted and presented to subsequent publish sessions. This sample 
    /// will store data in the persistXml XmlDocument something like the 
    /// following:
    /// 
    /// <?xml version="1.0"?>
    /// <Settings>
    ///     <Users>
    ///         <User Token="FlickrUserToken" Nsid="FlickrNsid" Username="FlickrUsername"/>
    ///     </Users>
    ///     <Defaults Token="CurrentFlickrUserToken"/>
    ///     <FlickrKeys Key="Flickr Application API Key" Secret="Flickr Application Shared Secret"/>
    /// </Settings>    
    /// 
    /// The static functions that begin with Persist... perform various 
    /// operations on the persistXml XmlDocument.
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// This class consists of only static helper functions
        /// </summary>
        private XmlHelper()
        {
        }

        /// <summary>
        /// This function stores the dimensions in the seesionXml XmlDocument.
        /// the sessionXml XmlDocument will look something like this:
        /// 
        /// <?xml version="1.0"?>
        /// <PhotoGalleryPublishSession versionMajor="12" versionMinor="0">
        ///     <PublishParameters>
        ///         <MaxWidth>1024</MaxWidth>
        ///         <MaxHeight>1024</MaxHeight>
        ///         ...
        ///     </PublishParameters>
        ///     ...
        /// </PhotoGalleryPublishSession>    
        /// 
        /// </summary>
        public static void SessionStoreDimensions(XmlDocument sessionXml, int maxWidth, int maxHeight, int resizeSize)
        {
            XmlNode node = sessionXml.SelectSingleNode("/PhotoGalleryPublishSession/PublishParameters");
            XmlNode paramNode = node.SelectSingleNode("MaxWidth");
            if (paramNode == null)
            {
                paramNode = sessionXml.CreateElement("MaxWidth");
                node.AppendChild(paramNode);
            }
            paramNode.InnerText = Convert.ToString(maxWidth);

            paramNode = node.SelectSingleNode("MaxHeight");
            if (paramNode == null)
            {
                paramNode = sessionXml.CreateElement("MaxHeight");
                node.AppendChild(paramNode);
            }
            paramNode.InnerText = Convert.ToString(maxHeight);

            paramNode = node.SelectSingleNode("ResizeSize");
            if (paramNode == null)
            {
                paramNode = sessionXml.CreateElement("ResizeSize");
                node.AppendChild(paramNode);
            }
            paramNode.InnerText = Convert.ToString(resizeSize);
        }

        public static bool SessionLoadDimensions(XmlDocument sessionXml, out int maxWidth, out int maxHeight, out int resizeSize)
        {
            maxWidth = 231;
            maxHeight = 173;
            resizeSize = 576;

            XmlNode node = sessionXml.SelectSingleNode("/PhotoGalleryPublishSession/PublishParameters");


            XmlNode paramNode = node.SelectSingleNode("MaxWidth");
            if (!int.TryParse(paramNode.InnerText, out maxWidth))
                return false;

            paramNode = node.SelectSingleNode("MaxHeight");
            if (!int.TryParse(paramNode.InnerText, out maxHeight))
                return false;

            paramNode = node.SelectSingleNode("ResizeSize");
            if (!int.TryParse(paramNode.InnerText, out resizeSize))
                return false;

            return true;
        }

        /// <summary>
        /// This function loads the title and description of the photo set to be created from the sessionXml XmlDocument.
        /// The sessionXml XmlDocument looks something like this:
        /// 
        /// <?xml version="1.0"?>
        /// <PhotoGalleryPublishSession versionMajor="12" versionMinor="0">
        ///     <PublishParameters>
        ///         <CreatePhotoSet Title="FlcikrPhotoSetTitle" Description="FlickrPhotoSetDescription"/>
        ///     </PublishParameters>
        ///     ...
        /// </PhotoGalleryPublishSession>    
        /// 
        /// </summary>
        public static bool SessionLoadUsePhotoSet(XmlDocument sessionXml, out string id, out string title, out string display_name, out string latitude, out string longitude)
        {
            title = string.Empty;
            display_name = string.Empty;
            latitude = string.Empty;
            longitude = string.Empty;
            id = string.Empty;
            XmlNode node = sessionXml.SelectSingleNode("/PhotoGalleryPublishSession/PublishParameters");
            XmlElement element = node.SelectSingleNode("UsePhotoSet") as XmlElement;
            if (element != null)
            {
                title = element.GetAttribute("Title");
                id = element.GetAttribute("Id");
                if (element.HasAttribute("DisplayName"))
                    display_name = element.GetAttribute("DisplayName");
                if (element.HasAttribute("Latitude"))
                    latitude = element.GetAttribute("Latitude");
                if (element.HasAttribute("Longitude"))
                    longitude = element.GetAttribute("Longitude");
                return true;
            }
            return false;
        }

        /// <summary>
        /// This function stores the title and description of the photo set to be created in the sessionXml XmlDocument.
        /// The sessionXml XmlDocument looks something like this:
        /// 
        /// <?xml version="1.0"?>
        /// <PhotoGalleryPublishSession versionMajor="12" versionMinor="0">
        ///     <PublishParameters>
        ///         <CreatePhotoSet Title="FlcikrPhotoSetTitle" Description="FlickrPhotoSetDescription"/>
        ///     </PublishParameters>
        ///     ...
        /// </PhotoGalleryPublishSession>    
        /// 
        /// </summary>
        public static void SessionStoreUsePhotoSet(XmlDocument sessionXml, string id, string title, string display_name, string latitude, string longitude)
        {
            XmlNode node = sessionXml.SelectSingleNode("/PhotoGalleryPublishSession/PublishParameters");
            XmlElement element = node.SelectSingleNode("UsePhotoSet") as XmlElement;
            if (element == null)
            {
                element = sessionXml.CreateElement("UsePhotoSet");
                node.AppendChild(element);
            }

            element.SetAttribute("Id", id);
            element.SetAttribute("Title", title);
            if (display_name != string.Empty)
            {
                element.SetAttribute("DisplayName", display_name);
            }
            if (latitude != string.Empty)
            {
                element.SetAttribute("Latitude", latitude);
            }
            if (longitude != string.Empty)
            {
                element.SetAttribute("Longitude", longitude);
            }
        }

        public static bool SessionLoadConnectionString(XmlDocument sessionXml, out string conn)
        {
            conn = string.Empty;
            XmlNode node = sessionXml.SelectSingleNode("/PhotoGalleryPublishSession/PublishParameters");
            XmlElement element = node.SelectSingleNode("ConnectionString") as XmlElement;
            if (element != null)
            {
                conn = element.GetAttribute("Value");
                return true;
            }
            return false;
        }

        public static void SessionStoreConnectionString(XmlDocument sessionXml, string conn)
        {
            XmlNode node = sessionXml.SelectSingleNode("/PhotoGalleryPublishSession/PublishParameters");
            XmlElement element = node.SelectSingleNode("ConnectionString") as XmlElement;
            if (element == null)
            {
                element = sessionXml.CreateElement("ConnectionString");
                node.AppendChild(element);
            }

            element.SetAttribute("Value", conn);
        }

        public static void SessionRemoveConnectionString(XmlDocument sessionXml)
        {
            XmlNode node = sessionXml.SelectSingleNode("/PhotoGalleryPublishSession/PublishParameters/ConnectionString");
            if (node != null)
            {
                node.ParentNode.RemoveChild(node);
            }
        }

        /// <summary>
        /// This function removes the photo set to be created from the sessionXml XmlDocument.
        /// The sessionXml XmlDocument looks something like this:
        /// 
        /// <?xml version="1.0"?>
        /// <PhotoGalleryPublishSession versionMajor="12" versionMinor="0">
        ///     <PublishParameters>
        ///         <CreatePhotoSet Title="FlcikrPhotoSetTitle" Description="FlickrPhotoSetDescription"/>
        ///     </PublishParameters>
        ///     ...
        /// </PhotoGalleryPublishSession>    
        /// 
        /// </summary>
        public static void SessionRemoveUsePhotoSet(XmlDocument sessionXml)
        {
            XmlNode node = sessionXml.SelectSingleNode("/PhotoGalleryPublishSession/PublishParameters/UsePhotoSet");
            if (node != null)
            {
                node.ParentNode.RemoveChild(node);
            }
        }

        /// <summary>
        /// This function gets the count of items in the item set from the sessionXml XmlDocument.
        /// The sessionXml XmlDocument looks something like this:
        /// 
        /// <?xml version="1.0"?>
        /// <PhotoGalleryPublishSession versionMajor="12" versionMinor="0">
        ///     ...
        ///     <ItemSet>
        ///         <Item id="1">
        ///             ...
        ///         </Item>
        ///         <Item id="2">
        ///             ...
        ///         </Item>
        ///     </ItemSet>
        ///     ...
        /// </PhotoGalleryPublishSession>    
        /// 
        /// </summary>
        public static void SessionGetItemCount(XmlDocument sessionXml, out int count)
        {
            XmlNode node = sessionXml.SelectSingleNode("/PhotoGalleryPublishSession/ItemSet");
            XmlNodeList list = node.SelectNodes("Item");
            count = list.Count;
        }

        /// <summary>
        /// This function gets the count of videos in the item set from the sessionXml XmlDocument.
        /// The sessionXml XmlDocument looks something like this:
        /// 
        /// <?xml version="1.0"?>
        /// <PhotoGalleryPublishSession versionMajor="12" versionMinor="0">
        ///     ...
        ///     <ItemSet>
        ///         <Item id="1">
        ///             ...
        ///             <PerceivedType>video</PerceivedType>
        ///             ...
        ///         </Item>
        ///         <Item id="2">
        ///             ...
        ///             <PerceivedType>image</PerceivedType>
        ///             ...
        ///         </Item>
        ///     </ItemSet>
        ///     ...
        /// </PhotoGalleryPublishSession>    
        /// 
        /// </summary>
        public static void SessionGetVideoCount(XmlDocument sessionXml, out int count)
        {
            XmlNode node = sessionXml.SelectSingleNode("/PhotoGalleryPublishSession/ItemSet");
            XmlNodeList list = node.SelectNodes("Item[PerceivedType='video']");
            count = list.Count;
        }

        /// <summary>
        /// This function marks all of the videos in the item set from the sessionXml XmlDocument
        /// with a flag not to upload them. The sessionXml XmlDocument looks something like this:
        /// 
        /// <?xml version="1.0"?>
        /// <PhotoGalleryPublishSession versionMajor="12" versionMinor="0">
        ///     ...
        ///     <ItemSet>
        ///         <Item id="1" upload="0">
        ///             ...
        ///             <PerceivedType>video</PerceivedType>
        ///             ...
        ///         </Item>
        ///         <Item id="2">
        ///             ...
        ///             <PerceivedType>image</PerceivedType>
        ///             ...
        ///         </Item>
        ///     </ItemSet>
        ///     ...
        /// </PhotoGalleryPublishSession>    
        /// 
        /// </summary>
        public static void SessionFlagVideos(XmlDocument sessionXml)
        {
            XmlNode node = sessionXml.SelectSingleNode("/PhotoGalleryPublishSession/ItemSet");
            XmlNodeList items = node.SelectNodes("Item[PerceivedType='video']");
            
            XmlAttribute uploadAttribute = sessionXml.CreateAttribute("upload");
            uploadAttribute.Value = "0";

            for (int i = 0; i < items.Count; i++)
            {
                // Set an attribute on the <item> tag so that we don't upload it!
                items[i].Attributes.Append(uploadAttribute);
            }
        }

        /// <summary>
        /// This function loads the information associated with the specified item in the item set from the sessionXml XmlDocument.
        /// The sessionXml XmlDocument looks something like this:
        /// 
        /// <?xml version="1.0"?>
        /// <PhotoGalleryPublishSession versionMajor="12" versionMinor="0">
        ///     ...
        ///     <ItemSet>
        ///         <Item id="1">
        ///             <OriginalFileName>sample1.jpg</OriginalFileName>
        ///             <Title>sample1</Title>
        ///             <KeywordSet>
        ///                 <Keyword>Keyword 1</Keyword>
        ///                 <Keyword>Keyword 2</Keyword>
        ///             </KeywordSet>
        ///             ...
        ///         </Item>
        ///         <Item id="2">
        ///             <OriginalFileName>sample2.jpg</OriginalFileName>
        ///             <Title>sample2</Title>
        ///             <KeywordSet>
        ///                 <Keyword>Keyword 1</Keyword>
        ///                 <Keyword>Keyword 2</Keyword>
        ///             </KeywordSet>
        ///             ...
        ///         </Item>
        ///     </ItemSet>
        ///     ...
        /// </PhotoGalleryPublishSession>    
        /// 
        /// </summary>
        public static bool SessionLoadItemInfo(XmlDocument sessionXml, string mediaObjectId, out string fullFilePath, out string filename, out string title, out List<string> tags)
        {
            fullFilePath = string.Empty;
            filename = string.Empty;
            title = string.Empty;
            tags = new List<string>();

            XmlNode node = sessionXml.SelectSingleNode(string.Format("/PhotoGalleryPublishSession/ItemSet/Item[@id=\'{0}\']", mediaObjectId));
            if (node != null)
            {
                XmlElement element = node.SelectSingleNode("FullFilePath") as XmlElement;
                fullFilePath = element.InnerText;

                element = node.SelectSingleNode("OriginalFileName") as XmlElement;
                filename = element.InnerText;

                element = node.SelectSingleNode("Title") as XmlElement;
                title = element.InnerText;

                element = node.SelectSingleNode("KeywordSet") as XmlElement;
                XmlNodeList list = element.SelectNodes("Keyword");
                foreach (XmlNode keyword in list)
                {
                    tags.Add(keyword.InnerText);
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// This function gets the log entries in the the sessionXml XmlDocument.
        /// The sessionXml XmlDocument looks something like this:
        /// 
        /// <?xml version="1.0"?>
        /// <PhotoGalleryPublishSession versionMajor="12" versionMinor="0">
        ///     ...
        ///     <Log>
        ///         <Action>This is log entry of a successful publish action</Action>
        ///         <Error>This is a log entry of a publish error</Error>
        ///     </Log>
        /// </PhotoGalleryPublishSession>    
        /// 
        /// </summary>
        public static void SessionLoadPhotoIds(XmlDocument sessionXml, out List<string> photoids)
        {
            photoids = new List<string>();
            XmlNodeList list = sessionXml.SelectNodes("/PhotoGalleryPublishSession/ItemSet/Item/PhotoId");
            foreach (XmlNode node in list)
            {
                photoids.Add(node.InnerText);
            }
        }

        /// <summary>
        /// This function stores the photo id in the item of the sessionXml XmlDocument.
        /// The sessionXml XmlDocument looks something like this:
        /// 
        /// <?xml version="1.0"?>
        /// <PhotoGalleryPublishSession versionMajor="12" versionMinor="0">
        ///     ...
        ///     <ItemSet>
        ///         <Item id="1">
        ///         <PhotoId>FlickrPhotoId</PhotoId>
        ///             ...
        ///         </Item>
        ///         ...
        ///     </ItemSet>
        ///     ...
        /// </PhotoGalleryPublishSession>    
        /// 
        /// </summary>
        public static void SessionStorePhotoId(XmlDocument sessionXml, string mediaObjectId, string photoId)
        {
            XmlNode node = sessionXml.SelectSingleNode(string.Format("/PhotoGalleryPublishSession/ItemSet/Item[@id=\'{0}\']", mediaObjectId));
            XmlElement element = node.SelectSingleNode("PhotoId") as XmlElement;
            if (element == null)
            {
                element = sessionXml.CreateElement("PhotoId");
                node.AppendChild(element);
            }

            element.InnerText = photoId;
        }

        /// <summary>
        /// This function gets the count of error log entries in the the sessionXml XmlDocument.
        /// The sessionXml XmlDocument looks something like this:
        /// 
        /// <?xml version="1.0"?>
        /// <PhotoGalleryPublishSession versionMajor="12" versionMinor="0">
        ///     ...
        ///     <Log>
        ///         <Action>This is log entry of a successful publish action</Action>
        ///         <Error>This is a log entry of a publish error</Error>
        ///     </Log>
        /// </PhotoGalleryPublishSession>    
        /// 
        /// </summary>
        public static void SessionGetErrorLogCount(XmlDocument sessionXml, out int count)
        {
            XmlNode node = sessionXml.SelectSingleNode("/PhotoGalleryPublishSession/Log");
            XmlNodeList list = node.SelectNodes("Error");
            count = list.Count;
        }

        /// <summary>
        /// This function gets the log entries in the the sessionXml XmlDocument.
        /// The sessionXml XmlDocument looks something like this:
        /// 
        /// <?xml version="1.0"?>
        /// <PhotoGalleryPublishSession versionMajor="12" versionMinor="0">
        ///     ...
        ///     <Log>
        ///         <Action>This is log entry of a successful publish action</Action>
        ///         <Error>This is a log entry of a publish error</Error>
        ///     </Log>
        /// </PhotoGalleryPublishSession>    
        /// 
        /// </summary>
        public static void SessionLoadLog(XmlDocument sessionXml, out List<string> log)
        {
            log = new List<string>();
            XmlElement element = sessionXml.SelectSingleNode("/PhotoGalleryPublishSession/Log") as XmlElement;
            if (element != null)
            {
                foreach (XmlNode childNode in element.ChildNodes)
                {
                    log.Add(childNode.InnerText);
                }
            }
        }

        /// <summary>
        /// This function stores a log entry in the sessionXml XmlDocument.
        /// The sessionXml XmlDocument looks something like this:
        /// 
        /// <?xml version="1.0"?>
        /// <PhotoGalleryPublishSession versionMajor="12" versionMinor="0">
        ///     ...
        ///     <Log>
        ///         <Action>This is log entry of a successful publish action</Action>
        ///         <Error>This is a log entry of a publish error</Error>
        ///     </Log>
        /// </PhotoGalleryPublishSession>    
        /// 
        /// </summary>
        public static void SessionLog(XmlDocument sessionXml, bool fError, string message)
        {
            XmlNode node = sessionXml.SelectSingleNode("/PhotoGalleryPublishSession");
            XmlElement element = node.SelectSingleNode("Log") as XmlElement;
            if (element == null)
            {
                element = sessionXml.CreateElement("Log");
                node.AppendChild(element);
            }

            string name = fError ? "Error" : "Action";
            XmlElement entry = sessionXml.CreateElement(name);
            element.AppendChild(entry);
            entry.InnerText = message;
        }

        /// <summary>
        /// Checks if there is an attribute flag signaling the plug-in not
        /// to upload an item.
        /// </summary>
        /// <param name="sessionXml">The session data in xml format.</param>
        /// <param name="mediaObjectId">The xml id of the item.</param>
        /// <returns>True if no flag is present, false otherwise.</returns>
        public static bool SessionShouldUploadItem(XmlDocument sessionXml, string mediaObjectId)
        {
            XmlNode itemNode = sessionXml.SelectSingleNode(
                string.Format("/PhotoGalleryPublishSession/ItemSet/Item[@id=\'{0}\' and @upload='0']", mediaObjectId));
            return itemNode == null;
        }
    }
}
