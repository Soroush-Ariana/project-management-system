using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement
{
    public class Project
    {

        //Attributes
        private static IDManager idManager = new IDManager(Path.Combine("ids", "projectsIds.txt"));
        private int id;
        private string title;
        private State state;
        private string startDate;
        private string endDate;

        //Constructors
        public Project()
        {
            this.title = string.Empty;
            this.state = State.notStarted;
            this.id = idManager.GetNextID();
            this.startDate = string.Empty;
            this.endDate = string.Empty;
        }

        public Project(int id, string title, State state, string startDate, string endDate)
        {
            if (id == 0) this.id = idManager.GetNextID();
            else this.id = id;
            this.title = title;
            this.state = state;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        //Setters and Getters
        public void setTitle(string title)
        {
            this.title = title.ToLower();
        }
        public string getTitle()
        {
            return this.title;
        }

        public void setState(State state)
        {
            this.state = state;
        }
        public State getState()
        {
            return this.state;
        }

        public int getId()
        {
            return this.id;
        }

        public void setStartDate(string startDate)
        {
            this.startDate = startDate.ToLower();
        }
        public string getStartDate()
        {
            return this.startDate;
        }

        public void setEndDate(string endDate)
        {
            this.endDate = endDate.ToLower();
        }
        public string getEndDate()
        {
            return this.endDate;
        }

        //Methods
        public void addTask()
        {
            Console.WriteLine("Please Enter the title of the task: ");
            string taskTitle = Console.ReadLine().ToLower();

            Task task = new Task(taskTitle, this.getId(), State.notStarted);

            // Specify the folder path and file name
            string folderPath = Path.Combine("Projects", this.getTitle());
            string fileName = "tasks.txt";
            string filePathForAdding = Path.Combine(folderPath, fileName);

            // Check if the folder exists, create it if not
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Check if the file exists, create it if not
            if (!File.Exists(filePathForAdding))
            {
                using (File.CreateText(filePathForAdding))
                {

                }
            }

            // Append a line to the existing file
            using (StreamWriter sw = File.AppendText(filePathForAdding))
            {
                sw.WriteLine(task.getId() + "," + task.getTitle() + "," + task.getProjectId() + "," + task.getState());
            }

            Console.WriteLine("Task created successfully.");

        }

        public void addMember()
        {

            // Specify the folder path and file name
            string folderPath = Path.Combine("projects", this.getTitle());
            string fileName = "members.txt";
            string filePath = Path.Combine(folderPath, fileName);

            // Check if the folder exists, create it if not
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Check if the file exists, create it if not
            if (!File.Exists(filePath))
            {
                using (File.CreateText(filePath))
                {

                }
            }

            int roleNo = 0;
            bool invalidInput;

            do
            {

                Console.WriteLine("Please enter the number of the role you want to add to this projetc (1,2):\n" +
                    "1. Team Member\n" +
                    "2. Team Leader");

                try
                {
                    roleNo = int.Parse(Console.ReadLine());

                    if (roleNo == 1 || roleNo == 2) invalidInput = false;
                    else throw new Exception();
                }
                catch
                {
                    Console.WriteLine("Invalid input");
                    invalidInput = true;
                }
            } while (invalidInput);

            string filePathForSearching = "";

            switch (roleNo)
            {
                case 1:
                    filePathForSearching = Path.Combine("members", "teamMembers.txt");
                    break;

                case 2:
                    filePathForSearching = Path.Combine("members", "teamLeaders.txt");
                    break;

                default:
                    Console.WriteLine("An error occured!");
                    return;
            }

            string memberName = "";
            string enteredEmail;
            bool emailExists;

            do
            {

                Console.WriteLine("Please enter the email of the member you want to add to this project. Or type 'exit' to go back to the main menu: ");
                enteredEmail = Console.ReadLine().ToLower();
                if (enteredEmail == "exit") return;

                try
                {
                    // Read each line of the members.txt file
                    using (StreamReader sr = new StreamReader(filePathForSearching))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            // Split the line by comma to extract email and name
                            string[] parts = line.Split(',');
                            string email = parts[1].Trim();
                            string name = parts[2].Trim();

                            // Check if the entered email matches any email in the file
                            if (email == enteredEmail)
                            {
                                // Store the member's name
                                memberName = name;
                                break;
                            }
                        }
                    }

                    // Check if memberName is empty
                    if (string.IsNullOrEmpty(memberName))
                    {
                        Console.WriteLine("Member with entered email not found.");
                        emailExists = false;
                    }
                    else
                    {
                        emailExists = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    emailExists = false;
                }

            } while (!emailExists);

            // Append a line to the existing file
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(enteredEmail + "," + memberName);
            }

            Console.WriteLine("The member is added successfully.");

        }

    }
}
