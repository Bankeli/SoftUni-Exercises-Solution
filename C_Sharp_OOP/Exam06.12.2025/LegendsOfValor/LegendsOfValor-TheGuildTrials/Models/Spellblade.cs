using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsOfValor_TheGuildTrials.Models
{
    public class Spellblade : Hero
    {
        private const int BladePower = 50;
        private const int BladeMana = 60;
        private const int BladeStamina = 60;
        private const int TrainingPower = 15;
        private const int TrainingMana = 10;
        private const int TrainingStamina = 10;
        public Spellblade(string name, string runeMark)
            : base(name, runeMark, BladePower, BladeMana, BladeStamina)
        {
        }

        public override void Train()
        {
            Power += TrainingPower;
            Mana += TrainingMana;
            Stamina += TrainingStamina;
        }
    }
}
