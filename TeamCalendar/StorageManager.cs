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
            try
            {
                System.Xml.Serialization.XmlSerializer reader =
             new System.Xml.Serialization.XmlSerializer(typeof(Storage));
                StreamReader file = new StreamReader(storagePath);
                StorageManager.storage = (Storage)reader.Deserialize(file);
                file.Close();
            }
            catch
            {
                Save();
                Init();
            }

        }

        public static void Save()
        {

            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Storage));


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
