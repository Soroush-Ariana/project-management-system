using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace ProjectManagement
{
    public class TeamLeader : User
    {
        //Constructors
        public TeamLeader() : base()
        {

        }

        public TeamLeader(int id, string name, string password, string email, string phoneNumber) : base(id, name, password, email, phoneNumber)
        {

        }

        //Methods
        public void createProject()
        {
            bool invalidInput;
            string folderPath;
            string projectTitle;

            do
            {
                Console.WriteLine("Please enter the title of the project: ");
                projectTitle = Console.ReadLine().ToLower();

                folderPath = Path.Combine("projects", projectTitle);

                try
                {
                    if (Directory.Exists(folderPath)) throw new Exception();
                    else
                    {
                        invalidInput = false;
                        Directory.CreateDirectory(folderPath);
                    }
                }
                catch
                {
                    Console.WriteLine("A project with this name already exists. You can choose another name.");
                    invalidInput = true;
                }
            } while (invalidInput);

            try
            {
                // Create a file stream
                using (FileStream fs = File.Create(Path.Combine(folderPath, "tasks.txt")))
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating file: {ex.Message}");
            }

            try
            {
                // Create a file stream
                using (FileStream fs = File.Create(Path.Combine(folderPath, "members.txt")))
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating file: {ex.Message}");
            }


            string desiredFormat = "dd/MM/yyyy"; // Desired date format
            DateTime parsedDate;
            string projectStartDate;

            do
            {
                Console.WriteLine("Please enter the project's start date in the format dd/mm/yyyy:");
                projectStartDate = Console.ReadLine();

                // Try parsing the user input as a DateTime object using the desired format
                if (DateTime.TryParseExact(projectStartDate, desiredFormat, null, System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    // User input is in the desired format
                    invalidInput = false;
                }
                else
                {
                    // User input is not in the desired format
                    Console.WriteLine("Invalid date format.");
                    invalidInput = true;
                }

            } while (invalidInput);

            string projectEndDate;

            do
            {
                Console.WriteLine("Please enter the project's end date in the format dd/mm/yyyy:");
                projectEndDate = Console.ReadLine();

                // Try parsing the user input as a DateTime object using the desired format
                if (DateTime.TryParseExact(projectEndDate, desiredFormat, null, System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    // User input is in the desired format
                    invalidInput = false;
                }
                else
                {
                    // User input is not in the desired format
                    Console.WriteLine("Invalid date format.");
                    invalidInput = true;
                }

            } while (invalidInput);

            Project project = new Project(0, projectTitle, State.notStarted, projectStartDate, projectEndDate);

            try
            {
                // Append the line to the file (first creates the file if it doesn't already exist).
                using (StreamWriter sw = File.AppendText(Path.Combine(folderPath, "description.txt")))
                {
                    sw.WriteLine(project.getId() + "," + project.getTitle() + "," + project.getState() + "," + project.getStartDate() + "," + project.getEndDate());
                    Console.WriteLine("Project is created successfully.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing into the text file: {ex.Message}");
            }



        }

        public void assignTask()
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
            string membersFileName = "members.txt";
            string membersFilePath = Path.Combine(folderPath, membersFileName);

            // Check if the file exists
            if (!File.Exists(membersFilePath))
            {
                Console.WriteLine("No member is added for this project!");
                return;
            }

            string enteredEmail = "";
            string memberName = "";
            bool invalidInput;
            bool emailExists = false;

            do
            {

                do
                {

                    Console.WriteLine("Please enter the email of the member, to who you want to assign a task. Or type 'exit' to go back to the main menu: ");

                    try
                    {
                        enteredEmail = Console.ReadLine().ToLower();
                        if (enteredEmail == "exit") return;
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
                    // Read each line of the members.txt file
                    using (StreamReader sr = new StreamReader(membersFilePath))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            // Split the line by comma to extract email
                            string[] parts = line.Split(',');
                            string memberEmail = parts[0].Trim();
                            memberName = parts[1].Trim();

                            // Check if the entered email matches any email in the file
                            if (memberEmail == enteredEmail)
                            {
                                emailExists = true;
                                break;
                            }
                        }
                    }

                    // Check if the email is found
                    if (!emailExists)
                    {
                        Console.WriteLine("member with such email is not found!\nTry again.");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    emailExists = false;
                }

            } while (!emailExists);


            // Specify the file path
            string tasksFileName = "tasks.txt";
            string tasksFilePath = Path.Combine(folderPath, tasksFileName);

            // Check if the file exists
            if (!File.Exists(tasksFilePath))
            {
                Console.WriteLine("No task is defined for this project!");
                return;
            }

            int enteredTaskId = 0;
            bool idExists = false;

            do
            {

                idExists = false;

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
                    string theChosenLine = "";

                    // Iterate over each line in the array
                    for (int i = 0; i < lines.Length; i++)
                    {
                        // Split the line by comma to extract taskID
                        string[] parts = lines[i].Split(',');
                        int currentTaskID = int.Parse(parts[0].Trim());

                        // Check if the entered taskID matches any taskID in the file
                        if (currentTaskID == enteredTaskId)
                        {

                            if (parts.Length > 4)
                            {
                                Console.WriteLine("The task is already assigned!");
                                return;
                            }

                            // Update the taskState, memberEmail, and memberName
                            parts[3] = State.inProgress.ToString();
                            parts = new string[] { parts[0], parts[1], parts[2], parts[3], enteredEmail, memberName };

                            // Replace the current line in the array with the updated line
                            lines[i] = string.Join(",", parts);
                            idExists = true;
                            theChosenLine = lines[i];
                            break; // Exit the loop since the task has been found and updated
                        }
                    }

                    // Write the modified lines back to the tasks.txt file
                    File.WriteAllLines(tasksFilePath, lines);

                    // Check if the taskID was found and updated
                    if (idExists)
                    {
                        Console.WriteLine("Task assigned successfully.");
                        string emailSubject = sendEmail(enteredEmail, theChosenLine, 1);
                    }
                    else
                    {
                        Console.WriteLine("Task not found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    idExists = false;
                }

            } while (!idExists);

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
                    parts[2] = State.inProgress.ToString();
                    parts = new string[] { parts[0], parts[1], parts[2], parts[3], parts[4] };

                    // Replace the current line in the array with the updated line
                    lines[i] = string.Join(",", parts);
                }

                // Write the modified lines back to the tasks.txt file
                File.WriteAllLines(projectDescriptionFilePath, lines);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }

        public string sendEmail(string receiverEmail, string taskInfo, int state)
        {
            // Sender's email address and credentials
            string senderEmail = "soroush.project.management@gmail.com";
            string senderPassword = "klgc ihgs srzz rqkb";

            // Recipient's email address
            string recipientEmail = receiverEmail;

            // Mail message details
            string subject = "";
            string body = "";

            if (state == 1)
            {
                subject = "Task Assignment";
                body = "The following task is assigned to you. You can check your profile for more details.\n" + taskInfo;
            }
            else if (state == 2)
            {
                subject = "Project completion";
                body = taskInfo;
            }

            try
            {
                // Create a new MailMessage
                MailMessage mail = new MailMessage(senderEmail, recipientEmail, subject, body);

                // Create SmtpClient
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true
                };

                // Send the email
                smtpClient.Send(mail);

                Console.WriteLine("A notifying email sent successfully to the member.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("The notifying email could not be sent to the member because of the following exception error!");
                Console.WriteLine();
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
            return subject;
        }

        public void seeMembersOfProject()
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
            string projectMembersFileName = "members.txt";
            string projectMembersFilePath = Path.Combine(folderPath, projectMembersFileName);

            // Check if the file exists
            if (!File.Exists(projectMembersFilePath))
            {
                Console.WriteLine("Sorry, it seems that the project members text file does not exist. So we cannot proceed. ask an admin to fix this problem.");
                return;
            }

            // Get information about the file
            FileInfo fileInfo = new FileInfo(projectMembersFilePath);

            // Check if the file length is 0 (empty)
            if (fileInfo.Length == 0)
            {
                Console.WriteLine("No member is added for this project!");
                return;
            }
            else
            {
                Console.WriteLine("Here are the members of this project: ");
                Console.WriteLine();
            }

            try
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(projectMembersFilePath);

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

        public void seeTasksOfProject()
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
            string projectTasksFileName = "tasks.txt";
            string projectTasksFilePath = Path.Combine(folderPath, projectTasksFileName);

            // Check if the file exists
            if (!File.Exists(projectTasksFilePath))
            {
                Console.WriteLine("Sorry, it seems that the project tasks text file does not exist. So we cannot proceed. ask an admin to fix this problem.");
                return;
            }

            // Get information about the file
            FileInfo fileInfo = new FileInfo(projectTasksFilePath);

            // Check if the file length is 0 (empty)
            if (fileInfo.Length == 0)
            {
                Console.WriteLine("No task is added to this project!");
                return;
            }
            else
            {
                Console.WriteLine("Here are the tasks and the members who are assigned to the tasks (if the task is assigned) for this project: ");
                Console.WriteLine();
            }

            try
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(projectTasksFilePath);

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
