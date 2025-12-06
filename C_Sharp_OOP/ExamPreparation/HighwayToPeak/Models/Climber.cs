using HighwayToPeak.Models.Contracts;
using HighwayToPeak.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighwayToPeak.Models
{
    public abstract class Climber : IClimber
    {
        private string name;
        private int stamina;
        private int MinStamina = 0;
        private int MaxStamina = 10;
        private readonly List<string> conqueredPeaks;

        protected Climber(string name, int stamina)
        {
            Name = name;
            Stamina = stamina;
            conqueredPeaks = new List<string>();
        }
        public string Name
        {
            get => name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(ExceptionMessages.ClimberNameNullOrWhiteSpace);
                name = value;
            }
        }

        public int Stamina
        {
            get => stamina;
            protected set
            {
                if (value < MinStamina)
                    stamina = MinStamina;
                else if (value > MaxStamina)
                    stamina = MaxStamina;
                else
                    stamina = value;
            }
        }

        public IReadOnlyCollection<string> ConqueredPeaks => conqueredPeaks.AsReadOnly();

        public void Climb(IPeak peak)
        {


            if (peak.DifficultyLevel == "Extreme")
                Stamina -= 6;
            else if (peak.DifficultyLevel == "Hard")
                Stamina -= 4;
            else if (peak.DifficultyLevel == "Moderate")
                Stamina -= 2;

            if (!conqueredPeaks.Contains(peak.Name))
                conqueredPeaks.Add(peak.Name);

        }

        public abstract void Rest(int daysCount);

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{this.GetType().Name} - Name: {this.Name}, Stamina: {this.Stamina}");

            if (this.ConqueredPeaks.Count == 0)
                sb.Append("Peaks conquered: no peaks conquered");
            else
                sb.Append($"Peaks conquered: {this.ConqueredPeaks.Count}");

            return sb.ToString();
        }



    }
}
