using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ADTool.Serializer
{

    /// <summary>
    /// Clase para serialización/deserialización XML
    /// </summary>
    public class XmlSerializer
    {
        /// <summary>
        /// Función que recibirá un objeto (que servirá como plantilla) y un string en formato XML
        /// La función, intentará serializar el string XML en el objeto pasado por parámetros y retornarlo
        /// </summary>
        /// <param name="oToDeserializeObjectoType">Objeto plantilla donde se serializarán los datos</param>
        /// <param name="xmlString">String con los datos en XML</param>
        /// <returns>Retornará el objeto serializado</returns>
        public static object GetObjectDeserialized(Type oToDeserializeObjectoType, string xmlString)
        {
            //-- Deserializa Objetos con XML
            System.Xml.Serialization.XmlSerializer oSerializer = new System.Xml.Serialization.XmlSerializer(oToDeserializeObjectoType);
            XmlTextReader oReader = new XmlTextReader(new StringReader(xmlString));
            var oObject = oSerializer.Deserialize(oReader);
            return oObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T GetObjectDeserialized<T>(string xmlString)
        {
            //-- Deserializa Objetos con XML
            System.Xml.Serialization.XmlSerializer oSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            XmlTextReader oReader = new XmlTextReader(new StringReader(xmlString));
            var respo = oSerializer.Deserialize(oReader);
            return (T)respo;

        }

        #region "GetObjectSerialized"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oObjetoToSerialize"></param>
        /// <returns></returns>
        public static string Serialize(object oObjetoToSerialize)
        {
            return Serialize(oObjetoToSerialize, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oObjetoToSerialize"></param>
        /// <param name="omitXmlDeclaration"></param>
        /// <returns></returns>
        public static string Serialize(object oObjetoToSerialize, bool omitXmlDeclaration)
        {
            return Serialize(oObjetoToSerialize, omitXmlDeclaration, Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oObjetoToSerialize"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Serialize(object oObjetoToSerialize, Encoding encoding)
        {
            return Serialize(oObjetoToSerialize, true, encoding);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oObjetoToSerialize"></param>
        /// <param name="omitXmlDeclaration"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Serialize(object oObjetoToSerialize, bool omitXmlDeclaration, Encoding encoding)
        {
            var settings = new XmlWriterSettings() { OmitXmlDeclaration = omitXmlDeclaration, Encoding = encoding };
            return Serialize(oObjetoToSerialize, settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oObjetoToSerialize"></param>
        /// <param name="xmlWriterSettings"></param>
        /// <returns></returns>
        public static string Serialize(object oObjetoToSerialize, XmlWriterSettings xmlWriterSettings)
        {
            StringBuilder sb = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(sb, xmlWriterSettings))
            {
                //XML namespace
                XmlSerializerNamespaces nameSpace = new XmlSerializerNamespaces();
                nameSpace.Add(string.Empty, string.Empty);
                System.Xml.Serialization.XmlSerializer oXmlSerializer = new System.Xml.Serialization.XmlSerializer(oObjetoToSerialize.GetType());
                oXmlSerializer.Serialize(writer, oObjetoToSerialize, nameSpace);
            }
            var xmlSerialized = sb.ToString();
            return XmlBeautify(xmlSerialized);
        }

        public static String XmlBeautify(String XML)
        {
            String Result = "";

            MemoryStream mStream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode);
            XmlDocument document = new XmlDocument();

            try
            {
                // Load the XmlDocument with the XML.
                document.LoadXml(XML);

                writer.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                document.WriteContentTo(writer);
                writer.Flush();
                mStream.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                mStream.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader sReader = new StreamReader(mStream);

                // Extract the text from the StreamReader.
                String FormattedXML = sReader.ReadToEnd();

                Result = FormattedXML;
            }
            catch (XmlException)
            {
            }

            mStream.Close();
            writer.Close();

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dyn"></param>
        /// <returns></returns>
        public static string SerializeFromDyn(dynamic dyn)
        {
            return DynamicHelper.ToXml(dyn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataString"></param>
        /// <returns></returns>
        public static XmlNode[] GetCData(string dataString)
        {
            //return $"<![CDATA[{data}]]>";
            var xmlDoc = new XmlDocument();
            return new XmlNode[] { xmlDoc.CreateCDataSection(dataString) };
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string GetFirstXmlChild(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNode root = doc.FirstChild;
            return root.InnerXml;
        }

        internal static string GetXmlFirstTable(DataSet ds)
        {
            return ds.Tables.Count > 0 ? GetFirstXmlChild(ds.GetXml()) : string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static XmlCDataSection CreateCData(string text)
        {
            XmlDocument document = new XmlDocument();
            return document.CreateCDataSection(text);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class DynamicHelper
    { ///
      /// Defines the simple types that is directly writeable to XML.
        private static readonly Type[]
        _writeTypes = new[] { typeof(string), typeof(DateTime), typeof(Enum), typeof(decimal), typeof(Guid) };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSimpleType(this Type type)
        {
            return type.IsPrimitive || _writeTypes.Contains(type);
        } ///

        // Converts an anonymous type to an XElement.
        // The input./// Returns the object as it's XML representation in an XElement.
        public static XElement ToXml(this object input)
        {
            return input.ToXml(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static XElement ToXml(this object input, string element)
        {
            if (input == null) { return null; }
            if (String.IsNullOrEmpty(element)) { element = "object"; }

            element = XmlConvert.EncodeName(element);
            var ret = new XElement(element);
            if (input != null)
            {
                var type = input.GetType();
                var props = type.GetProperties();
                var elements =
                    from prop in props
                    let name = XmlConvert.EncodeName(prop.Name)
                    let val = prop.PropertyType.IsArray ? "array" : prop.GetValue(input, null)
                    let value = prop.PropertyType.IsArray
                        ? GetArrayElement(prop, (Array)prop.GetValue(input, null)) : (prop.PropertyType.IsSimpleType()
                            ? new XElement(name, val) : val.ToXml(name))
                    where value != null
                    select value;
                ret.Add(elements);
            }
            return ret;
        }

        // Gets the array element.
        // The property info.
        // The input object./
        // Returns an XElement with the array collection as child elements.
        private static XElement GetArrayElement(PropertyInfo info, Array input)
        {
            var name = XmlConvert.EncodeName(info.Name);
            XElement rootElement = new XElement(name);
            var arrayCount = input.GetLength(0);

            for (int i = 0; i < arrayCount; i++)
            {
                var val = input.GetValue(i);
                XElement childElement = val.GetType().IsSimpleType()
                    ? new XElement(name + "Child", val) : val.ToXml();
                rootElement.Add(childElement);
            }
            return rootElement;
        }
    }

}