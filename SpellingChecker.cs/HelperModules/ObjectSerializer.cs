using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ZGTR_CROSPELLSpellingCheckerLib.HelperModules
{
    public class ObjectSerializer
    {
        public static byte[] Serialize<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return ms.ToArray();
                //return (T)formatter.Deserialize(ms);
            }
        }

        public static T Deserialize<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var formatter = new BinaryFormatter();
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        public static T DeepCopy<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
