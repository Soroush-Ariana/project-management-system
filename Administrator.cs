using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement
{
    public class Administrator : TeamLeader
    {
        //Constructors
        public Administrator() : base()
        {

        }

        public Administrator(int id, string name, string password, string email, string phoneNumber) : base(id, name, password, email, phoneNumber)
        {

        }

        //Methods
        public void createTeamMember()
        {
            Console.WriteLine("Please enter the name of the team member: ");
            string teamMemberName = Console.ReadLine().ToLower();

            Console.WriteLine("Please enter the email of the team member: ");
            string teamMemberEmail = Console.ReadLine().ToLower();

            Console.WriteLine("Please enter the phone number of the team member: ");
            string teamMemberPhoneNumber = Console.ReadLine().ToLower();

            Console.WriteLine("Please enter the password for this account: ");
            string teamMemberPassword = Console.ReadLine().ToLower();

            TeamMember teamMember = new TeamMember(0, teamMemberName, teamMemberPassword, teamMemberEmail, teamMemberPhoneNumber);

            // Check if the folder exists, create it if not
            if (!Directory.Exists("members"))
            {
                Directory.CreateDirectory("members");
            }

            // Check if the file exists, create it if not
            if (!File.Exists(Path.Combine("members", "teamMembers.txt")))
            {
                using (File.CreateText(Path.Combine("members", "teamMembers.txt")))
                {

                }
            }

            // Append a line to the existing file
            using (StreamWriter sw = File.AppendText(Path.Combine("members", "teamMembers.txt")))
            {
                sw.WriteLine(teamMember.getId() + "," + teamMember.getEmail() + "," + teamMember.getName() + "," + teamMember.getPassword() + "," + teamMember.getPhoneNumber());
            }

            Console.WriteLine("The team member account is created successfully.");

        }

        public void createTeamLeader()
        {
            Console.WriteLine("Please enter the name of the team leader: ");
            string teamLeaderName = Console.ReadLine().ToLower();

            Console.WriteLine("Please enter the email of the team leader: ");
            string teamLeaderEmail = Console.ReadLine().ToLower();

            Console.WriteLine("Please enter the phone number of the team leader: ");
            string teamLeaderPhoneNumber = Console.ReadLine().ToLower();

            Console.WriteLine("Please enter the password for this account: ");
            string teamLeaderPassword = Console.ReadLine().ToLower();

            TeamLeader teamLeader = new TeamLeader(0, teamLeaderName, teamLeaderPassword, teamLeaderEmail, teamLeaderPhoneNumber);

            // Check if the folder exists, create it if not
            if (!Directory.Exists("members"))
            {
                Directory.CreateDirectory("members");
            }

            // Check if the file exists, create it if not
            if (!File.Exists(Path.Combine("members", "teamLeaders.txt")))
            {
                using (File.CreateText(Path.Combine("members", "teamLeaders.txt")))
                {

                }
            }

            // Append a line to the existing file
            using (StreamWriter sw = File.AppendText(Path.Combine("members", "teamLeaders.txt")))
            {
                sw.WriteLine(teamLeader.getId() + "," + teamLeader.getEmail() + "," + teamLeader.getName() + "," + teamLeader.getPassword() + "," + teamLeader.getPhoneNumber());
            }

            Console.WriteLine("The team leader account is created successfully.");

        }

        public void createAdministrator()
        {
            Console.WriteLine("Please enter the name of the administrator: ");
            string administratorName = Console.ReadLine().ToLower();

            Console.WriteLine("Please enter the email of the administrator: ");
            string administratorEmail = Console.ReadLine().ToLower();

            Console.WriteLine("Please enter the phone number of the administrator: ");
            string administratorPhoneNumber = Console.ReadLine().ToLower();

            Console.WriteLine("Please enter the password for this account: ");
            string administratorPassword = Console.ReadLine().ToLower();

            Administrator administrator = new Administrator(0, administratorName, administratorPassword, administratorEmail, administratorPhoneNumber);

            // Check if the folder exists, create it if not
            if (!Directory.Exists("members"))
            {
                Directory.CreateDirectory("members");
            }

            // Check if the file exists, create it if not
            if (!File.Exists(Path.Combine("members", "administrators.txt")))
            {
                using (File.CreateText(Path.Combine("members", "administrators.txt")))
                {

                }
            }

            // Append a line to the existing file
            using (StreamWriter sw = File.AppendText(Path.Combine("members", "administrators.txt")))
            {
                sw.WriteLine(administrator.getId() + "," + administrator.getEmail() + "," + administrator.getName() + "," + administrator.getPassword() + "," + administrator.getPhoneNumber());
            }

            Console.WriteLine("The administrator account is created successfully.");

        }

        public void closeProject()
        {
            bool projectExists;
            string projectTitle;
            string folderPath;

            do
            {
                Console.WriteLine("Please enter the name of the project. Or type 'exit' to go back to the main menu: ");
                projectTitle = Console.ReadLine().ToLower();
                if (projectTitle == "exit") return;

                // Specify the folder path
                folderPath = Path.Combine("projects", projectTitle);

                // Check if the folder exists
                if (!Directory.Exists(folderPath))
                {
                    Console.WriteLine("Such project does not exist in the system!\nTry again.");
                    projectExists = false;
                }
                else projectExists = true;

            } while (!projectExists);

            // Specify the file path
            string projectDescriptionFileName = "description.txt";
            string projectDescriptionFilePath = Path.Combine(folderPath, projectDescriptionFileName);

            // Check if the file exists
            if (!File.Exists(projectDescriptionFilePath))
            {
                Console.WriteLine("Sorry, it seems that the project description text file does not exist. So we cannot proceed. ask an admin to fix this problem.");
                return;
            }

            try
            {
                // Read all lines of the description.txt file into an array
                string[] lines = File.ReadAllLines(projectDescriptionFilePath);

                // Iterate over each line in the array
                for (int i = 0; i < lines.Length; i++)
                {
                    // Split the line by comma to extract taskID
                    string[] parts = lines[i].Split(',');

                    // Update the projectState
                    parts[2] = State.completed.ToString();
                    parts = new string[] { parts[0], parts[1], parts[2], parts[3], parts[4], "closed" };

                    // Replace the current line in the array with the updated line
                    lines[i] = string.Join(",", parts);
                }

                // Write the modified lines back to the tasks.txt file
                File.WriteAllLines(projectDescriptionFilePath, lines);
                Console.WriteLine("Project is closed successfully.");

                string projectMembersFileName = "members.txt";
                string projectMembersFilePath = Path.Combine(folderPath, projectMembersFileName); ;

                // Read all lines from the file into an array
                string[] linesOfMembersFile = File.ReadAllLines(projectMembersFilePath);

                foreach (string line in linesOfMembersFile)
                {
                    // Split each line into email and name parts
                    string[] parts = line.Split(',');

                    // Ensure the line contains both email and name parts
                    if (parts.Length == 2)
                    {
                        string email = parts[0].Trim();

                        sendEmail(email, "The project '" + projectTitle + "' is completed and closed now.", 2);
                    }
                    else
                    {
                        Console.WriteLine("The notifying email could not be sent because of invalid line format in members.txt file.");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }

        public void deleteProject()
        {
            Console.WriteLine("Please enter the title of the project: ");
            string projectTitle = Console.ReadLine().ToLower();

            string folderPath = Path.Combine("projects", projectTitle);

            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
                Console.WriteLine("Directory deleted successfully.");
            }
            else
            {
                Console.WriteLine("Directory does not exist.");
            }
        }

    }
}
