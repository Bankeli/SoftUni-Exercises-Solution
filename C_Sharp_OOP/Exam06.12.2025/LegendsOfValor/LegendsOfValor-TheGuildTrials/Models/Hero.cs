using LegendsOfValor_TheGuildTrials.Models.Contracts;
using LegendsOfValor_TheGuildTrials.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsOfValor_TheGuildTrials.Models
{
    public abstract class Hero : IHero
    {
        private string name;
        private string runeMark;
        private int power;
        private int mana;
        private int stamina;

        protected Hero(string name, string runeMark, int power, int mana, int stamina)
        {
            Name = name;
            RuneMark = runeMark;
            Power = power;
            Mana = mana;
            Stamina = stamina;
        }
        public string Name
        {
            get => this.name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(ErrorMessages.InvalidHeroName);
                this.name = value;
            }
        }
        public string RuneMark
        {
            get => this.runeMark;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(ErrorMessages.InvalidHeroRuneMark);
                this.runeMark = value;
            }
        }

        public string GuildName { get; private set; }

        public int Power { get; protected set; }

        public int Mana { get; protected set; }

        public int Stamina { get; protected set; }

        public string Essence()
        {
            return $"Essence Revealed - Power [{Power}] Mana [{Mana}] Stamina [{Stamina}]";
        }

        public override string ToString()
       => $"Hero: [{Name}] of the Guild '{GuildName}' - RuneMark: {RuneMark}";

        public void JoinGuild(IGuild guild)
        {
           GuildName = guild.Name;
        }

        public abstract void Train();
        
    }
}
