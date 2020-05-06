using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TeamCalendar
{
    public class StorageManager
    {
        private static Storage storage;

        public static User loggedUser;

        const string storagePath = "storage.xml";

        public static Storage GetStorage()
        {
            if (storage == null)
            {
                Init();
            }
            return storage;
        }

        private static void Init()
        {
                System.Xml.Serialization.XmlSerializer reader =
             new System.Xml.Serialization.XmlSerializer(typeof(Storage));
                StreamReader file = new StreamReader(storagePath);
                storage = (Storage)reader.Deserialize(file);
                file.Close();

        }

        public static void Save()
        {
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Storage));

            FileStream file = File.Create(storagePath);
            Storage SaveStorage;
            if (storage == null)
            {
                SaveStorage = new Storage();
            }
            else
            {
                SaveStorage = storage;
            }

            writer.Serialize(file, SaveStorage);
            file.Close();
        }
    }
}
