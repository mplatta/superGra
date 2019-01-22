using System;
using SuperGra.Helpers;
using System.IO;

namespace SuperGra.Data
{
    class Repository
    {
        public void Save<T>(T obj)
        {
            var xml = Serialisation.SerializeObject<T>(obj);
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SavedCanvas.xml");
            File.WriteAllText(path, xml);
        }

        public T Load<T>()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SavedCanvas.xml");
            var xml = File.ReadAllText(path);
            var obj = (T)Serialisation.DeserializeObject<T>(xml);
            return obj;
        }
    }
}
