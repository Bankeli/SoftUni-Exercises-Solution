using HighwayToPeak.Core.Contracts;
using HighwayToPeak.Models;
using HighwayToPeak.Models.Contracts;
using HighwayToPeak.Repositories;
using HighwayToPeak.Repositories.Contracts;
using HighwayToPeak.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighwayToPeak.Core
{
    public class Controller : IController
    {
        private IBaseCamp baseCamp = new BaseCamp();
        private IRepository<IPeak> peaks =new PeakRepository();
        private IRepository<IClimber> climbers = new ClimberRepository();
        public string AddPeak(string name, int elevation, string difficultyLevel)
        {
            if (peaks.Get(name) != null)
                return string.Format(OutputMessages.PeakAlreadyAdded, name);
            if (difficultyLevel != "Extreme" && difficultyLevel != "Hard" && difficultyLevel != "Moderate")
                return string.Format(OutputMessages.PeakDiffucultyLevelInvalid, difficultyLevel);

            IPeak peak = new Peak(name, elevation, difficultyLevel);
            peaks.Add(peak);

            return string.Format(OutputMessages.PeakIsAllowed, name, nameof(PeakRepository));

        }

        public string AttackPeak(string climberName, string peakName)
        {
            if (climbers.Get(climberName) == null)
                return string.Format(OutputMessages.ClimberNotArrivedYet, climberName);

            if (peaks.Get(peakName) == null)
                return string.Format(OutputMessages.PeakIsNotAllowed, peakName);

            if (!baseCamp.Residents.Contains(climberName))
                return string.Format(OutputMessages.ClimberNotFoundForInstructions, climberName, peakName);

            var climber = climbers.Get(climberName);
            var peak = peaks.Get(peakName);

            if (peak.DifficultyLevel == "Extreme" && climber.GetType().Name == "NaturalClimber")
                return string.Format(OutputMessages.NotCorrespondingDifficultyLevel, climber.Name, peak.Name);

            baseCamp.LeaveCamp(climber.Name);
            climber.Climb(peak);
            if (climber.Stamina <= 0)
                return string.Format(OutputMessages.NotSuccessfullAttack, climber.Name);
            else 
                baseCamp.ArriveAtCamp(climber.Name);

            return string.Format(OutputMessages.SuccessfulAttack, climber.Name, peak.Name);


        }

        public string BaseCampReport()
        {
            StringBuilder sb = new StringBuilder();

            if (baseCamp.Residents.Any() == false)
            {
                sb.AppendLine("BaseCamp is currently empty.");
            }
            else
            {
                sb.AppendLine("BaseCamp residents:");

                foreach (var climberName in baseCamp.Residents)
                {
                    var climber = climbers.Get(climberName);
                    sb.AppendLine($"Name: {climber.Name}, Stamina: {climber.Stamina}, Count of Conquered Peaks: {climber.ConqueredPeaks.Count}");
                }
            }

            return sb.ToString().TrimEnd();

        }

        public string CampRecovery(string climberName, int daysToRecover)
        {
            if (!baseCamp.Residents.Contains(climberName))
                return string.Format(OutputMessages.ClimberIsNotAtBaseCamp, climberName);
            var climber = climbers.Get(climberName);
            if (climber.Stamina == 10)
                return string.Format(OutputMessages.NoNeedOfRecovery, climber.Name);

            climber.Rest(daysToRecover);
            return string.Format(OutputMessages.ClimberRecovered, climber.Name, daysToRecover);
        }

        public string NewClimberAtCamp(string name, bool isOxygenUsed)
        {
            if (climbers.Get(name) != null)
                return string.Format(OutputMessages.ClimberCannotBeDuplicated, name, nameof(ClimberRepository));
            IClimber climber;
            if (isOxygenUsed)
                climber = new OxygenClimber(name);
            else 
                climber = new NaturalClimber(name);

            climbers.Add(climber);
            baseCamp.ArriveAtCamp(climber.Name);

            return string.Format(OutputMessages.ClimberArrivedAtBaseCamp, climber.Name);
        }

        public string OverallStatistics()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("***Highway-To-Peak***");

            var sortedClimbers = climbers.All
                .OrderByDescending(c => c.ConqueredPeaks.Count) 
                .ThenBy(c => c.Name);                           

            foreach (var climber in sortedClimbers)
            {
                sb.AppendLine(climber.ToString());

                
                foreach (var peakName in climber.ConqueredPeaks
                                                .Select(p => peaks.Get(p))
                                                .OrderByDescending(p => p.Elevation))
                {
                    sb.AppendLine(peakName.ToString());
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}
