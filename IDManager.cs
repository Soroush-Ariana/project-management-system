using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement
{
    public class IDManager
    {
        private string filePath;
        private int lastAssignedID;

        public IDManager(string fileName)
        {

            // Check if the folder exists, create it if not
            if (!Directory.Exists("ids"))
            {
                Directory.CreateDirectory("ids");
            }

            // Check if the file exists, create it if not
            if (!File.Exists(fileName))
            {
                using (File.CreateText(fileName))
                {

                }
            }

            filePath = fileName;
            // Read the last assigned ID from the file
            lastAssignedID = ReadLastAssignedID();
        }

        private int ReadLastAssignedID()
        {
            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                if (int.TryParse(content, out int id))
                {
                    return id;
                }
            }
            return 1; // Default value if file doesn't exist or parsing fails
        }

        private void SaveLastAssignedID()
        {
            File.WriteAllText(filePath, lastAssignedID.ToString());
        }

        public int GetNextID()
        {
            // Increment the ID counter
            lastAssignedID++;
            // Save the updated ID to the file
            SaveLastAssignedID();
            return lastAssignedID;
        }
    }
}
