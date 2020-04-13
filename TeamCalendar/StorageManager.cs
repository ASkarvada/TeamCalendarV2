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

        public static Storage getStorage()
        {
            if (storage == null)
            {
                init();
            }
            return storage;
        }

        private static void init()
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
                save();
                init();
            }

        }

        public static void save()
        {

            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(Storage));


            FileStream file = File.Create(storagePath);
            Storage saveStorage;
            if (storage == null)
            {
                saveStorage = new Storage();
            }
            else
            {
                saveStorage = storage;
            }

            writer.Serialize(file, saveStorage);
            file.Close();
        }
    }
}
