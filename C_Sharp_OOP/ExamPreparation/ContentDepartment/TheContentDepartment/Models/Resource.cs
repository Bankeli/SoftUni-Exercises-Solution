using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheContentDepartment.Models.Contracts;
using TheContentDepartment.Utilities.Messages;

namespace TheContentDepartment.Models
{
    public abstract class Resource : IResource
    {

        private string name;
        private bool isTested;
        private bool isApproved;

        protected Resource(string name, string creator, int priority)
        {
            Name = name;
            Creator = creator;
            Priority = priority;
            isApproved = false;
            isTested = false;
        }

        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(ExceptionMessages.NameNullOrWhiteSpace);
                name = value;
            }
        }

        public string Creator { get;private set; }

        public int Priority { get; private set; }

        public bool IsTested => isTested;

        public bool IsApproved => isApproved;

        public override string ToString() => $"{Name} ({GetType().Name}), Created By: {Creator}";



        public void Approve()
        {
            isApproved = true;
        }

        public void Test()
        {
            isTested = !isTested;
        }
    }
}
