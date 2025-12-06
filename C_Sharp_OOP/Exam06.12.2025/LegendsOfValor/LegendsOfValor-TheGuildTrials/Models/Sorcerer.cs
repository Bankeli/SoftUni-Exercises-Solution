using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsOfValor_TheGuildTrials.Models
{
    public class Sorcerer : Hero
    {
        private const int SorcerPower = 40;
        private const int SorcerMana = 120;
        private const int SorcerStamina = 0;
        private const int TrainingPower = 20;
        private const int TrainingMana = 25;
        public Sorcerer(string name, string runeMark) 
            : base(name, runeMark, SorcerPower, SorcerMana, SorcerStamina)
        {
        }

        public override void Train()
        {
            Power += TrainingPower;
            Mana += TrainingMana;
        }
    }
}
