using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ZGTR_CROSPELLSpellingCheckerLib.SpellingCheckerEngine.UserPref
{
    public static class XmlSerializerHandler
    {
        public static void SerializeObject<T>(T serializableObject, string fileName) where T : class
        {
            if (serializableObject == null) { return; }
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }
        }

        public static T DeSerializeObject<T>(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(fileName);
            if (string.IsNullOrEmpty(fileName)) { return default(T); }
            T objectOut = default(T);

            try
            {
                string attributeXml = string.Empty;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }
            return objectOut;
        }

        #region OldSerializers
        //public static string SerializeAnObject(object obj)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
        //    MemoryStream stream = new System.IO.MemoryStream();
        //    try
        //    {
        //        serializer.Serialize(stream, obj);
        //        stream.Position = 0;
        //        doc.Load(stream);
        //        return doc.InnerXml;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        stream.Close();
        //        stream.Dispose();
        //    }
        //}

        //public static object DeSerializeAnObject(string xmlOfAnObject, ref object objectOfXml)
        //{
        //    StringReader read = new StringReader(xmlOfAnObject);
        //    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(objectOfXml.GetType());
        //    XmlReader reader = new XmlTextReader(read);
        //    try
        //    {
        //        objectOfXml = (object)serializer.Deserialize(reader);
        //        return objectOfXml;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        reader.Close();
        //        read.Close();
        //        read.Dispose();
        //    }
        //}
        #endregion
    }
}
