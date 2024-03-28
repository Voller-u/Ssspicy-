using UnityEngine;
using System.Collections;
using System.Xml;
using System;


public class CustomFontImportor : MonoBehaviour
{
    public Font font;
    public TextAsset textAsset;


    void Awake()
    {
        if (font == null || textAsset == null)
        {
            Debug.LogError("请设置font和textAsset.");
            return;
        }

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(textAsset.text);


        int totalWidth = Convert.ToInt32(xmlDocument["font"]["common"].Attributes["scaleW"].InnerText);
        int totalHeight = Convert.ToInt32(xmlDocument["font"]["common"].Attributes["scaleH"].InnerText);

        XmlElement xml = xmlDocument["font"]["chars"];
        ArrayList characterInfoList = new ArrayList();


        for (int i = 0; i < xml.ChildNodes.Count; ++i)
        {
            XmlNode node = xml.ChildNodes[i];
            if (node.Attributes == null)
            {
                continue;
            }
            int index = Convert.ToInt32(node.Attributes["id"].InnerText);
            int x = Convert.ToInt32(node.Attributes["x"].InnerText);
            int y = Convert.ToInt32(node.Attributes["y"].InnerText);
            int width = Convert.ToInt32(node.Attributes["width"].InnerText);
            int height = Convert.ToInt32(node.Attributes["height"].InnerText);
            int xOffset = Convert.ToInt32(node.Attributes["xoffset"].InnerText);
            int yOffset = Convert.ToInt32(node.Attributes["yoffset"].InnerText);
            int xAdvance = Convert.ToInt32(node.Attributes["xadvance"].InnerText);

            CharacterInfo info = new CharacterInfo();
            Rect uv = new Rect();
            uv.x = (float)x / totalWidth;
            uv.y = (float)(totalHeight - y - height) / totalHeight;
            uv.width = (float)width / totalWidth;
            uv.height = (float)height / totalHeight;


            info.index = index;
            info.uvBottomLeft = new Vector2(uv.xMin, uv.yMin);
            info.uvBottomRight = new Vector2(uv.xMax, uv.yMin);
            info.uvTopLeft = new Vector2(uv.xMin, uv.yMax);
            info.uvTopRight = new Vector2(uv.xMax, uv.yMax);
            info.minX = xOffset;
            info.maxX = xOffset + width;
            info.minY = -yOffset - height;
            info.maxY = -yOffset;
            info.advance = xAdvance;
            info.glyphWidth = width;
            info.glyphHeight = height;


            characterInfoList.Add(info);
        }
        font.characterInfo = characterInfoList.ToArray(typeof(CharacterInfo)) as CharacterInfo[];


        Debug.Log("生成成功.");
    }
}