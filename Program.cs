using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

                int roleNo = 0;
                bool invalidInput;

                // choose which role they have (admin or team member or team leader)
                do
                {

                    Console.WriteLine("Welcome to the project management app.");
                    Console.WriteLine("Please log in the system.");
                    Console.WriteLine("Please enter the number of your role (1,2,3), or press'4' to Exit:\n" +
                        "1. Administrator\n" +
                        "2. Team Member\n" +
                        "3. Team Leader\n" +
                        "4. Exit");

                    try
                    {
                        roleNo = int.Parse(Console.ReadLine());

                        if (roleNo == 1 || roleNo == 2 || roleNo == 3 || roleNo == 4) invalidInput = false;
                        else throw new Exception();
                    }
                    catch
                    {
                        Console.WriteLine("Invalid input.\nTry again.");
                        invalidInput = true;
                    }
                } while (invalidInput);

                // Specify the file path
                string filePath = Path.Combine("members", "teamMembers.txt"); //Just something for the default value

                switch (roleNo)
                {
                    case 1:
                        filePath = Path.Combine("members", "administrators.txt");
                        break;

                    case 2:
                        filePath = Path.Combine("members", "teamMembers.txt");
                        break;

                    case 3:
                        filePath = Path.Combine("members", "teamLeaders.txt");
                        break;

                    case 4:
                        return;

                    default:
                        Console.WriteLine("You did not select any of the available options");
                        continue;
                }

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("No member is added to the system for this role yet.\nTry Again.");
                    continue;
                }

                bool emailExists = false;
                bool passwordCorrect = false;
                int id = 0;
                string email = "";
                string name = "";
                string password = "";
                string phoneNumber = "";

                do 
                {

                    Console.Write("Enter your email. Or type 'exit' to go exit the system: ");
                    string userEmail = Console.ReadLine().ToLower();
                    if (userEmail == "exit") return;

                    Console.Write("Enter your password (case-insensitive): ");
                    string userPassword = Console.ReadLine().ToLower();

                    // Read the text file
                    string[] lines = File.ReadAllLines(filePath);

                    emailExists = false;
                    passwordCorrect = false;

                    // Check if the entered email exists and if the password matches
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 5)
                        {
                            try
                            {
                                id = int.Parse(parts[0]);
                            }
                            catch
                            {
                                Console.WriteLine("The id could not be converted to integer!");
                                continue;
                            }

                            email = parts[1];
                            name = parts[2];
                            password = parts[3];
                            phoneNumber = parts[4];

                            if (email == userEmail)
                            {
                                emailExists = true;
                                if (password == userPassword)
                                {
                                    passwordCorrect = true;
                                    break; // No need to continue checking if password matches
                                }
                            }
                        }
                    }

                    // Notify the user based on the validation results
                    if (!emailExists)
                    {
                        Console.WriteLine("Invalid email.");
                        Console.WriteLine("Try again!");
                    }
                    else if (!passwordCorrect)
                    {
                        Console.WriteLine("Incorrect password.");
                        Console.WriteLine("Try again!");
                    }
                    else
                    {
                        Console.WriteLine("Login successful!");
                    }

                } while ((!passwordCorrect) || (!emailExists));

                switch (roleNo)
                {
                    case 1:
                        administrator(id, name, password, email, phoneNumber);
                        break;

                    case 2:
                        teamMember(id, name, password, email, phoneNumber);
                        break;

                    case 3:
                        teamLeader(id, name, password, email, phoneNumber);
                        break;

                    default:
                        continue;
                }

            }

        }

        public static void administrator(int id, string name, string password, string email, string phoneNumber)
        {
            Administrator administrator = new Administrator(id, name, password, email, phoneNumber);

            Console.WriteLine("You have logged in the system as an administrator");

            int optionNo;

            while (true)
            {
                Console.WriteLine("Enter the number of the action you want to do:\n" +
                "1. See the status of a project\n" +
                "2. See progression messages for a project\n" +
                "3. Add a comment on a project\n" +
                "4. Create a Project\n" +
                "5. Assign a task\n" +
                "6. See members working on a project\n" +
                "7. See tasks of a project (and task assignments)\n" +
                "8. Create a team member account\n" +
                "9. Create a team leader account\n" +
                "10. Create an administrator account\n" +
                "11. Mark a project as 'completed' and close it\n" +
                "12. Add a member to a project\n" +
                "13. Add a task to a project\n" +
                "14. Delete a project\n" +
                "15. Log out");

                try
                {
                    optionNo = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }

                switch (optionNo)
                {
                    case 1:
                        administrator.showProjectStatus();
                        break;

                    case 2:
                        administrator.showProgressionMessages(true);
                        break;

                    case 3:
                        administrator.addComment(true);
                        break;

                    case 4:
                        administrator.createProject();
                        break;

                    case 5:
                        administrator.assignTask();
                        break;

                    case 6:
                        administrator.seeMembersOfProject();
                        break;

                    case 7:
                        administrator.seeTasksOfProject();
                        break;

                    case 8:
                        administrator.createTeamMember();
                        break;

                    case 9:
                        administrator.createTeamLeader();
                        break;

                    case 10:
                        administrator.createAdministrator();
                        break;

                    case 11:
                        administrator.closeProject();
                        break;

                    case 12:
                        Project projectForAddingMember = findProject();
                        if (projectForAddingMember != null)
                        {
                            projectForAddingMember.addMember();
                            break;
                        }
                        else
                        {
                            continue;
                        }

                    case 13:
                        Project projectForAddingTask = findProject();
                        if (projectForAddingTask != null)
                        {
                            projectForAddingTask.addTask();
                            break;
                        }
                        else
                        {
                            continue;
                        }

                    case 14:
                        administrator.deleteProject();
                        break;

                    case 15:
                        Console.WriteLine("You have successfully logged out.");
                        return;

                    default:
                        Console.WriteLine("You did not select any of the available options");
                        continue;
                }

            }
        }

        public static void teamMember(int id, string name, string password, string email, string phoneNumber)
        {
            TeamMember teamMember = new TeamMember(id, name, password, email, phoneNumber);

            Console.WriteLine("You have logged in the system as a team member");

            int optionNo;

            while (true)
            {
                Console.WriteLine("Enter the number of the action you want to do:\n" +
                "1. See the status of a project\n" +
                "2. See progression messages for a project\n" +
                "3. Announce your task is done\n" +
                "4. Add a comment on a project\n" +
                "5. Log out");

                try
                {
                    optionNo = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }

                switch (optionNo)
                {
                    case 1:
                        teamMember.showProjectStatus();
                        break;

                    case 2:
                        teamMember.showProgressionMessages(false);
                        break;

                    case 3:
                        teamMember.taskIsDone();
                        break;

                    case 4:
                        teamMember.addComment(false);
                        break;

                    case 5:
                        Console.WriteLine("You have successfully logged out.");
                        return;

                    default:
                        Console.WriteLine("You did not select any of the available options");
                        continue;
                }

            }

        }

        public static void teamLeader(int id, string name, string password, string email, string phoneNumber)
        {
            TeamLeader teamLeader = new TeamLeader(id, name, password, email, phoneNumber);

            Console.WriteLine("You have logged in the system as a team leader");

            int optionNo;

            while (true)
            {
                Console.WriteLine("Enter the number of the action you want to do:\n" +
                "1. See the status of a project\n" +
                "2. See progression messages for a project\n" +
                "3. Announce your task is done\n" +
                "4. Add a comment on a project\n" +
                "5. Create a Project\n" +
                "6. Assign a task\n" +
                "7. See members working on a project\n" +
                "8. See tasks of a project (and task assignments)\n" +
                "9. Add a member to a project\n" +
                "10. Add a task to a project\n" +
                "11. Log out");

                try
                {
                    optionNo = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }

                switch (optionNo)
                {
                    case 1:
                        teamLeader.showProjectStatus();
                        break;

                    case 2:
                        teamLeader.showProgressionMessages(false);
                        break;

                    case 3:
                        teamLeader.taskIsDone();
                        break;

                    case 4:
                        teamLeader.addComment(false);
                        break;

                    case 5:
                        teamLeader.createProject();
                        break;

                    case 6:
                        teamLeader.assignTask();
                        break;

                    case 7:
                        teamLeader.seeMembersOfProject();
                        break;

                    case 8:
                        teamLeader.seeTasksOfProject();
                        break;

                    case 9:
                        Project projectForAddingMember = findProject();
                        if (projectForAddingMember != null)
                        {
                            projectForAddingMember.addMember();
                            break;
                        }
                        else
                        {
                            continue;
                        }

                    case 10:
                        Project projectForAddingTask = findProject();
                        if (projectForAddingTask != null)
                        {
                            projectForAddingTask.addTask();
                            break;
                        }
                        else
                        {
                            continue;
                        }

                    case 11:
                        Console.WriteLine("You have successfully logged out.");
                        return;

                    default:
                        Console.WriteLine("You did not select any of the available options");
                        continue;
                }

            }
        }

        public static Project findProject()
        {
            bool projectExists;
            string projectTitle;
            string folderPath;

            do
            {
                Console.WriteLine("Please enter the name of the project. Or type 'exit' to go back to the previous menu: ");
                projectTitle = Console.ReadLine().ToLower();
                if (projectTitle == "exit") return null;

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
                return null;
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

                    int id = int.Parse(parts[0]);
                    string title = parts[1];
                    State state = State.inProgress;
                    string startDate = parts[3];
                    string endDate = parts[4];

                    Project project = new Project(id, title, state, startDate, endDate);
                    return project;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }

            Console.WriteLine("An error occurred!");
            return null;

        }

    }
}
