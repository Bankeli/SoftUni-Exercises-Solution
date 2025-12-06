using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighwayToPeak.Models
{
    public class OxygenClimber : Climber
    {
        private const int DefaultStamina = 10;
        private const int RecoveryUnit = 1;
        public OxygenClimber(string name)
            : base(name, DefaultStamina)
        {
        }

        public override void Rest(int daysCount)
        {
            Stamina += daysCount * RecoveryUnit;
        }
    }
}
