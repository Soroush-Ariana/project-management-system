using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement
{
    public class User
    {

        // Attributes
        private static IDManager idManager = new IDManager(Path.Combine("ids", "usersIds.txt"));
        private int id;
        private string name;
        private string password;
        private string email;
        private string phoneNumber;

        //Constructors
        public User()
        {
            this.id = idManager.GetNextID();
            this.name = string.Empty;
            this.password = string.Empty;
            this.email = string.Empty;
            this.phoneNumber = string.Empty;
        }

        public User(int id, string name, string password, string email, string phoneNumber)
        {
            if (id == 0) this.id = idManager.GetNextID();
            else this.id = id;
            this.name = name;
            this.password = password;
            this.email = email;
            this.phoneNumber = phoneNumber;
        }

        //Setters and Getters
        public void setName(string name)
        {
            this.name = name.ToLower();
        }
        public string getName()
        {
            return this.name;
        }

        public void setPassword(string password)
        {
            this.password = password.ToLower();
        }
        public string getPassword()
        {
            return this.password;
        }

        public void setEmail(string email)
        {
            this.email = email.ToLower();
        }
        public string getEmail()
        {
            return this.email;
        }

        public void setPhoneNumber(string phoneNumber)
        {
            this.phoneNumber = phoneNumber.ToLower();
        }
        public string getPhoneNumber()
        {
            return this.phoneNumber;
        }

        public int getId()
        {
            return this.id;
        }

        //Methods
        public void taskIsDone()
        {

            bool invalidInput;
            string enteredProjectName = "";

            do
            {

                Console.WriteLine("Please enter the name of the project. Or type 'exit' to go back to the main menu: ");

                try
                {
                    enteredProjectName = Console.ReadLine().ToLower();
                    if (enteredProjectName == "exit") return;
                    invalidInput = false;
                }
                catch
                {
                    Console.WriteLine("Invalid input");
                    invalidInput = true;
                }
            } while (invalidInput);

            try
            {
                if (!Directory.Exists(Path.Combine("projects", enteredProjectName)))
                {
                    Console.WriteLine("Sorry, it seems that the project's folder does not exist. So we cannot proceed. ask an admin to fix this problem.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return;
            }

            // Specify the file path
            string tasksFileName = "tasks.txt";
            string tasksFilePath = Path.Combine("projects", enteredProjectName, tasksFileName);

            // Check if the file exists
            if (!File.Exists(tasksFilePath))
            {
                Console.WriteLine("No task is defined for this project!");
                return;
            }

            int enteredTaskId = 0;
            bool taskExists;

            do
            {

                taskExists = false;
                do
                {

                    Console.WriteLine("Please enter the ID of the task. Or enter '0' to go back to the main menu: ");

                    try
                    {
                        enteredTaskId = int.Parse(Console.ReadLine());
                        if (enteredTaskId == 0) return;
                        invalidInput = false;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid input");
                        invalidInput = true;
                    }
                } while (invalidInput);

                try
                {
                    // Read all lines of the tasks.txt file into an array
                    string[] lines = File.ReadAllLines(tasksFilePath);

                    // Iterate over each line in the array
                    for (int i = 0; i < lines.Length; i++)
                    {
                        // Split the line by comma to extract taskID
                        string[] parts = lines[i].Split(',');
                        int currentTaskID = int.Parse(parts[0].Trim());

                        // Check if the entered taskID matches any taskID in the file
                        if (currentTaskID == enteredTaskId)
                        {

                            if (parts.Length < 5)
                            {
                                Console.WriteLine("The task is not assigned to anyone yet!");
                                return;
                            }

                            if (parts[4] != this.getEmail())
                            {
                                Console.WriteLine("This task is not assigned to you!");
                                return;
                            }

                            // Update the taskState
                            parts[3] = State.completed.ToString();
                            parts = new string[] { parts[0], parts[1], parts[2], parts[3], parts[4], parts[5] };

                            // Replace the current line in the array with the updated line
                            lines[i] = string.Join(",", parts);
                            taskExists = true;
                            break; // Exit the loop since the task has been found and updated
                        }
                    }

                    // Write the modified lines back to the tasks.txt file
                    File.WriteAllLines(tasksFilePath, lines);

                    // Check if the taskID was found and updated
                    if (taskExists)
                    {
                        Console.WriteLine("The task is marked as completed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Task not found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    taskExists = false;
                }

            } while (!taskExists);

        }

        public void addComment(bool userHasFullAccess)
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

            bool emailExists = false;

            if (!userHasFullAccess)
            {

                //Check if the user is on the members list for this project
                // Specify the file path
                string projectMembersFileName = "members.txt";
                string projectMembersFilePath = Path.Combine(folderPath, projectMembersFileName);

                // Check if the file exists
                if (!File.Exists(projectMembersFilePath))
                {
                    Console.WriteLine("Sorry, it seems that the project members text file does not exist. So we cannot proceed. ask an admin to fix this problem.");
                    return;
                }

                try
                {
                    // Read all lines from the file into an array
                    string[] lines = File.ReadAllLines(projectMembersFilePath);

                    foreach (string line in lines)
                    {
                        // Split each line into email and name parts
                        string[] parts = line.Split(',');

                        // Ensure the line contains both email and name parts
                        if (parts.Length == 2)
                        {
                            string email = parts[0].Trim();

                            // Check if the email matches the one being searched for
                            if (email.Equals(this.email, StringComparison.OrdinalIgnoreCase))
                            {
                                emailExists = true; // Email found
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid line format: " + line);
                        }
                    }

                    if (!emailExists)
                    {
                        Console.WriteLine("You are not a member for this project!");
                        return;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return;
                }

            }

            Console.WriteLine("Please enter your comment (it should be in one line): ");
            string userComment = Console.ReadLine().ToLower();

            string data = "date and time: " + DateTime.Now.ToString() + "  user ID: " + this.id + "   user name: " + this.name + "    user email: " + this.email;

            // Specify the file path
            string projectProgressionMessagesFileName = "progressionMessages.txt";
            string projectProgressionMessagesFilePath = Path.Combine(folderPath, projectProgressionMessagesFileName);

            // Append a line to the file
            using (StreamWriter sw = File.AppendText(projectProgressionMessagesFilePath))
            {
                sw.WriteLine(data);
                sw.WriteLine(userComment);
                sw.WriteLine();
                Console.WriteLine("Comment is added successfully.");
            }

        }

        public void showProgressionMessages(bool userHasFullAccess)
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

            bool emailExists = false;

            if (!userHasFullAccess)
            {

                //Check if the user is on the members list for this project
                // Specify the file path
                string projectMembersFileName = "members.txt";
                string projectMembersFilePath = Path.Combine(folderPath, projectMembersFileName);

                // Check if the file exists
                if (!File.Exists(projectMembersFilePath))
                {
                    Console.WriteLine("Sorry, it seems that the project members text file does not exist. So we cannot proceed. ask an admin to fix this problem.");
                    return;
                }

                try
                {
                    // Read all lines from the file into an array
                    string[] lines = File.ReadAllLines(projectMembersFilePath);

                    foreach (string line in lines)
                    {
                        // Split each line into email and name parts
                        string[] parts = line.Split(',');

                        // Ensure the line contains both email and name parts
                        if (parts.Length == 2)
                        {
                            string email = parts[0].Trim();

                            // Check if the email matches the one being searched for
                            if (email.Equals(this.email, StringComparison.OrdinalIgnoreCase))
                            {
                                emailExists = true; // Email found
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid line format: " + line);
                        }
                    }

                    if (!emailExists)
                    {
                        Console.WriteLine("You are not a member for this project!");
                        return;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    return;
                }

            }

            // Specify the file path
            string projectProgressionMessagesFileName = "progressionMessages.txt";
            string projectProgressionMessagesFilePath = Path.Combine(folderPath, projectProgressionMessagesFileName);

            // Check if the file exists
            if (!File.Exists(projectProgressionMessagesFilePath))
            {
                Console.WriteLine("the progressionMessages.txt file doesn't exist!");
                return;
            }

            // Get information about the file
            FileInfo fileInfo = new FileInfo(projectProgressionMessagesFilePath);

            // Check if the file length is 0 (empty)
            if (fileInfo.Length == 0)
            {
                Console.WriteLine("No progression messages for this project!");
                return;
            }
            else
            {
                Console.WriteLine("Here are the progression messages on this project:");
                Console.WriteLine();
            }

            try
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(projectProgressionMessagesFilePath);

                // Display each line to the user
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return;
            }

        }

        public void showProjectStatus()
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

            // Get information about the file
            FileInfo fileInfo = new FileInfo(projectDescriptionFilePath);

            // Check if the file length is 0 (empty)
            if (fileInfo.Length == 0)
            {
                Console.WriteLine("No description for this project!");
                return;
            }
            else
            {
                Console.WriteLine("Here is the status of this project:");
            }

            try
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(projectDescriptionFilePath);

                // Display each line to the user
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return;
            }

        }

    }
}
