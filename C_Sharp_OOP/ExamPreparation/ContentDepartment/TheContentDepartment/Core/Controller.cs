using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheContentDepartment.Core.Contracts;
using TheContentDepartment.Models;
using TheContentDepartment.Models.Contracts;
using TheContentDepartment.Repositories;
using TheContentDepartment.Repositories.Contracts;
using TheContentDepartment.Utilities.Messages;

namespace TheContentDepartment.Core
{
    public class Controller : IController
    {
        private IRepository<IResource> resources = new ResourceRepository();
        private IRepository<ITeamMember> members = new MemberRepository();
        public string ApproveResource(string resourceName, bool isApprovedByTeamLead)
        {
            var resource = resources.TakeOne(resourceName);

            if (resource.IsTested == false)
                return string.Format(OutputMessages.ResourceNotTested, resourceName);

            ITeamMember teamLead = members.Models.FirstOrDefault(m => m.GetType().Name == nameof(TeamLead));

            if (isApprovedByTeamLead)
            {
                resource.Approve();
                teamLead!.FinishTask(resource.Name);
                return string.Format(OutputMessages.ResourceApproved, teamLead.Name, resource.Name);
            }
            else
            {
                resource.Test();
                return string.Format(OutputMessages.ResourceReturned, teamLead!.Name, resource.Name);
            } 
                
        }

        public string CreateResource(string resourceType, string resourceName, string path)
        {
            if (resourceType != nameof(Exam) && resourceType != nameof(Presentation) && resourceType != nameof(Workshop))
                return string.Format(OutputMessages.ResourceTypeInvalid, resourceType);

            var teamMember = members.Models.FirstOrDefault(m => m.Path == path);

            if (teamMember == null)
                return string.Format(OutputMessages.NoContentMemberAvailable, resourceName);
            if (teamMember.InProgress.Contains(resourceName))
                return string.Format(OutputMessages.ResourceExists, resourceName);

            IResource resource;

            if (resourceType == nameof(Exam))
            {
                resource = new Exam(resourceName, teamMember.Name);
            }
            else if (resourceType == nameof(Workshop))
            {
                resource = new Workshop(resourceName, teamMember.Name);
            }
            else
            {
                resource = new Presentation(resourceName, teamMember.Name);
            }

            resources.Add(resource);
            teamMember.WorkOnTask(resourceName);
            return string.Format(OutputMessages.ResourceCreatedSuccessfully, teamMember.Name, resource.GetType().Name, resourceName);
        }

        public string DepartmentReport()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Finished Tasks:");

            foreach (var resource in resources.Models.Where(m => m.IsApproved))
            {
                sb.AppendLine($"--{resource.ToString()}");
            }

            sb.AppendLine($"Team Report:");

            ITeamMember teamLead = members.Models.FirstOrDefault(m => m.GetType().Name == nameof(TeamLead));

            sb.AppendLine($"--{teamLead!.ToString()}");

            foreach (var member in members.Models.Where(m => m.GetType().Name != nameof(TeamLead)))
            {
                sb.AppendLine($"{member.ToString()}");
            }

            return sb.ToString().TrimEnd();
        }

        public string JoinTeam(string memberType, string memberName, string path)
        {
            if (memberType != nameof(TeamLead) && memberType != nameof(ContentMember))
                return string.Format(OutputMessages.MemberTypeInvalid, memberType);

            if (members.Models.Any(x => x.Path == path))
                return OutputMessages.PositionOccupied;

            if (members.Models.Any(x => x.Name == memberName))
                return string.Format(OutputMessages.MemberExists, memberName);

            ITeamMember member;

            if (memberType == nameof(ContentMember))
                member = new ContentMember(memberName, path);
            else
                member = new TeamLead(memberName, path);

            members.Add(member);
            return string.Format(OutputMessages.MemberJoinedSuccessfully, member.Name);
        }

        public string LogTesting(string memberName)
        {
            var teamMember = members.TakeOne(memberName);
            if (teamMember == null)
                return OutputMessages.WrongMemberName;

            IResource resource = resources.Models
                .Where(m => m.Creator == teamMember.Name).Where(m => m.IsTested == false)
                .OrderBy(m => m.Priority).FirstOrDefault();

            if (resource == null)
                return string.Format(OutputMessages.NoResourcesForMember, memberName);

            ITeamMember teamLead = members.Models.FirstOrDefault(m => m.GetType().Name == nameof(TeamLead));

            resource!.Test();

            teamMember.FinishTask(resource.Name);
            teamLead.WorkOnTask(resource.Name);

            return string.Format(OutputMessages.ResourceTested, resource.Name);


        }
    }
}
