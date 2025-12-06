using LegendsOfValor_TheGuildTrials.Models.Contracts;
using LegendsOfValor_TheGuildTrials.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LegendsOfValor_TheGuildTrials.Models
{
    public class Guild : IGuild
    {
        private string[] validGuilds = { "WarriorGuild", "SorcererGuild", "ShadowGuild" };
        private string name;
        private int wealth;
        private readonly List<string> legion;
        private bool isFallen;
        private int RecruitingCost = 500;
        public Guild(string name)
        {
            Name = name;
            wealth = 5000;
            legion = new List<string>();
            isFallen = false;
        }
        public string Name
        {
            get => name;
            private set
            {
                if (!validGuilds.Contains(value))
                    throw new ArgumentException(ErrorMessages.InvalidGuildName);
                name = value;
            }
        }

        public int Wealth => wealth;
        

        public IReadOnlyCollection<string> Legion => legion.AsReadOnly();

        public bool IsFallen => isFallen;

        public void RecruitHero(IHero hero)
        {

            legion.Add(hero.RuneMark);
            wealth -= RecruitingCost;
        }

        public void TrainLegion(ICollection<IHero> heroesToTrain)
        {
            foreach (var hero in heroesToTrain)
                hero.Train();

            wealth -= 200 * heroesToTrain.Count;
        }

        public void WinWar(int goldAmount)
        {
            wealth += goldAmount;
        }

        public void LoseWar()
        {
            wealth = 0;
            isFallen = true;
        }
    }
}
