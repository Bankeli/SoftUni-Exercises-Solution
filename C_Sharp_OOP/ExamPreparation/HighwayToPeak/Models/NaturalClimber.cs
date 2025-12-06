using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighwayToPeak.Models
{
    public class NaturalClimber : Climber
    {
        private const int DefaultStamina = 6;
        private const int RecoveryUnit = 2;
        public NaturalClimber(string name)
            : base(name, DefaultStamina)
        {
        }

        public override void Rest(int daysCount)
        {
            Stamina += daysCount * RecoveryUnit;
        }
    }
}
