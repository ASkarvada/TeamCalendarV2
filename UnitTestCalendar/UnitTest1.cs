using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamCalendar;

namespace UnitTestCalendar
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestLoginRandomUser()
        {
            //Arrange
            Random ran = new Random();
            string name = "";

            //Act
            for (int i = 0; i < 20; i++)
            {
                name += ran.Next();
            }

            StorageManager.storagePath = @"C:\Users\adams\OneDrive - Smíchovská střední průmyslová škola\MVOP\ZÁVĚREČNÝ PROJEKT\TeamCalendarV2\TeamCalendar\bin\Debug\storage.xml";

            User user = StorageManager.GetStorage().findUserByName(name);

            //Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void TestEncryptionClass()
        {
            //Arrange
            string password = "aduiabhduisaSDSDFAS45646SADAads";

            //Act
            string encrypted = Encrypce.Start(password, true);
            string decrypted = Encrypce.Start(encrypted, false);

            //Assert
            Assert.AreEqual(decrypted, password);
        }

        [TestMethod]
        public void TestCalendarIsNotEmpty()
        {
            //Arrange
            StorageManager.storagePath = @"C:\Users\adams\OneDrive - Smíchovská střední průmyslová škola\MVOP\ZÁVĚREČNÝ PROJEKT\TeamCalendarV2\TeamCalendar\bin\Debug\storage.xml";
            StorageManager.loggedUser = StorageManager.GetStorage().findUserByName("TEST_User");
            Win_Calendar window = new Win_Calendar();
            window.Show();
            window.CreateMeetingControl();

            //Act
            int x = 0;
            int y = 0;
            List<StackPanel> isNotNull = new List<StackPanel>();
            for (int i = 0; i < 42; i++) //procházení po dnech
            {
                string nameSPanel = ("sp_" + x + "_" + y);
                
                var resultOfSPanel = (StackPanel)window.FindName(nameSPanel);

                if (resultOfSPanel.Children != null) isNotNull.Add(resultOfSPanel);
                
                x++;
                if (x == 7) { x = 0; y++; }
            }


            //Assert
            Assert.IsTrue(isNotNull.Count > 0);
            
        }

        [TestMethod]
        public void TestMeetingsCreatingAndDeleting()
        {
            //Arrange
            StorageManager.storagePath = @"C:\Users\adams\OneDrive - Smíchovská střední průmyslová škola\MVOP\ZÁVĚREČNÝ PROJEKT\TeamCalendarV2\TeamCalendar\bin\Debug\storage.xml";
            List<Relation<User>> agreedByUser = new List<Relation<User>>();
            agreedByUser.Add(Relation<User>.Create(StorageManager.GetStorage().findUserByName("TEST_User")));
            List<Relation<User>> rejectedByUser = new List<Relation<User>>();

            //Act
            Meeting meeting = Meeting.Create("TEST__MMMMEEEEETTTIIINNGG", CreatingMeeting.loadInvitedUsers(""), "testPlace", Color.FromArgb(100, 20, 15, 35), new DateTime(2011, 1, 2, 8, 0, 0), new DateTime(2011, 1, 2, 9, 0, 0), StorageManager.GetStorage().findUserByName("TEST_User"), agreedByUser, rejectedByUser);
            StorageManager.storagePath = @"C:\Users\adams\OneDrive - Smíchovská střední průmyslová škola\MVOP\ZÁVĚREČNÝ PROJEKT\TeamCalendarV2\TeamCalendar\bin\Debug\storage.xml";


            //Z nějakého záhadného důvodu nefunguje
            //-------------------------------
            StorageManager.Save();
            //-------------------------------


            List<Meeting> found = StorageManager.GetStorage().FindMeetingsByDate(new DateTime(2011, 1, 1));

            //Assert
            Assert.AreEqual(meeting.Name, found[0].Name);

        }




    }
}
