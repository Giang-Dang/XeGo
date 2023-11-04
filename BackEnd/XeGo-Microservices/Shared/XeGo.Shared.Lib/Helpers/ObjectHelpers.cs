using Newtonsoft.Json;

namespace XeGo.Shared.Lib.Helpers
{
    public class ObjectHelpers
    {
        public static T DeepCopy<T>(T obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(json)!;
        }

        public static bool Equals<T>(T obj1, T obj2)
        {
            var json1 = JsonConvert.SerializeObject(obj1);
            var json2 = JsonConvert.SerializeObject(obj2);
            return json1 == json2;
        }
    }
}
