using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement
{
    public class TeamMember : User
    {

        //Attributes
        private List<Task> assignedTasks;

        //Constructors
        public TeamMember() : base()
        {
            this.assignedTasks = new List<Task>();
        }

        public TeamMember(int id, string name, string password, string email, string phoneNumber) : base(id, name, password, email, phoneNumber)
        {
            this.assignedTasks = new List<Task>();
        }

        //Setters and Getters
        public void setAssignedTasks(List<Task> assignedTasks)
        {
            this.assignedTasks = assignedTasks;
        }
        public List<Task> getAssignedTasks()
        {
            return this.assignedTasks;
        }

        //Mehtods
        public void addTask(Task task)
        {
            this.assignedTasks.Add(task);
        }

    }
}
