using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsOfValor_TheGuildTrials.Models
{
    public class Warrior : Hero
    {
        private const int WarriorPower = 60;
        private const int WarriorMana = 0;
        private const int WarriorStamina = 100;
        private const int TrainingPower = 30;
        private const int TrainingStamina = 10;
        public Warrior(string name, string runeMark)
            : base(name, runeMark, WarriorPower, WarriorMana, WarriorStamina)
        {
        }

        public override void Train()
        {
            Power += TrainingPower;
            Stamina += TrainingStamina;
        }
    }
}
