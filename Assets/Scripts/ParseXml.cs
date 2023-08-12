using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class XmlElement
{
    public string Name { get; set; }
    public string Value { get; set; }
    public List<XmlElement> Children { get; set; }
    public Dictionary<string, string> Attributes { get; set; }

    public XmlElement(string name)
    {
        Name = name;
        Value = "";
        Children = new List<XmlElement>();
        Attributes = new Dictionary<string, string>();
    }
}

public class ParseXml : MonoBehaviour
{

    [ContextMenu("parse")]
    public void Parse()
    {
        string xml = File.ReadAllText("C:\\Users\\zhou\\Desktop\\mianshi\\example.xml");
        XmlElement root = DoParse(xml);
        Debug.Log(PrintElement(root, 0));
    }

    private string PrintElement(XmlElement element, int indentationLevel)
    {
        string indentation = new string(' ', indentationLevel * 4);
        StringBuilder result = new StringBuilder(indentation + "Element: " + element.Name + "\n");

        if (!string.IsNullOrEmpty(element.Value))
        {
            result.Append(indentation + "  Value: " + element.Value + "\n");
        }

        foreach (var attribute in element.Attributes)
        {
            result.Append(indentation + "  Attribute: " + attribute.Key + " = " + attribute.Value + "\n");
        }

        foreach (var child in element.Children)
        {
            result.Append(PrintElement(child, indentationLevel + 1));
        }

        return result.ToString();
    }

    private XmlElement DoParse(string xml)
    {

        XmlElement root = null;
        Stack<XmlElement> elementStack = new Stack<XmlElement>();

        int index = 0;
        while (index < xml.Length)
        {
            if (xml[index] == '<')
            {
                if (xml[index + 1] == '/')
                {
                    // 结束标签
                    int endIndex = xml.IndexOf('>', index + 1);
                    string tagName = xml.Substring(index + 2, endIndex - index - 2);
                    if (elementStack.Count > 0 && elementStack.Peek().Name == tagName)
                    {
                        elementStack.Pop();
                    }
                    index = endIndex + 1;
                }
                else
                {
                    // 开始标签
                    int endIndex = xml.IndexOf('>', index + 1);
                    int spaceIndex = xml.IndexOf(' ', index + 1);
                    int endTagIndex = xml.IndexOf('/', index + 1);
                    string tagName = spaceIndex > 0 && spaceIndex < endIndex ? xml.Substring(index + 1, spaceIndex - index - 1) : xml.Substring(index + 1, endIndex - index - 1);

                    XmlElement element = new XmlElement(tagName);

                    // 提取属性
                    while (spaceIndex > 0 && spaceIndex < endIndex)
                    {
                        //a="123" bb="2222222"  
                        int equalIndex = xml.IndexOf('=', spaceIndex + 1);
                        int attrEndIndex = xml.IndexOf(' ', equalIndex + 2);
                        attrEndIndex = Math.Min(attrEndIndex, Math.Min(endTagIndex, endIndex));
                        // 减去=号
                        string attrName = xml.Substring(spaceIndex + 1, equalIndex - spaceIndex - 1);
                        // 排除多余符号占用的长度
                        string attrValue = xml.Substring(equalIndex + 2, attrEndIndex - equalIndex - 3);
                        element.Attributes[attrName] = attrValue;
                        spaceIndex = xml.IndexOf(' ', attrEndIndex);
                    }

                    if (root == null)
                    {
                        root = element;
                    }
                    else if (elementStack.Count > 0)
                    {
                        elementStack.Peek().Children.Add(element);
                    }

                    elementStack.Push(element);
                    index = endIndex + 1;
                }
            }
            else
            {
                // 文本内容
                int endIndex = xml.IndexOf('<', index);
                if (elementStack.Count > 0)
                {
                    elementStack.Peek().Value += xml.Substring(index, endIndex - index);
                }
                index = endIndex;
            }
        }

        return root;
    }
}
