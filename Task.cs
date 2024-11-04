using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement
{
    public class Task
    {

        // Attributes
        private static IDManager idManager = new IDManager(Path.Combine("ids", "tasksIds.txt"));
        private int id;
        private string title;
        private State state;
        private int projectId;

        //Constructors
        public Task()
        {
            this.title = string.Empty;
            this.state = State.notStarted;
            this.id = idManager.GetNextID();
            this.projectId = 0;
        }

        public Task(string title, int projectId, State state)
        {
            this.id = idManager.GetNextID();
            this.title = title;
            this.projectId = projectId;
            this.state = state;
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

        public void setProjectId(int projectId)
        {
            this.projectId = projectId;
        }
        public int getProjectId()
        {
            return this.projectId;
        }

    }
}
