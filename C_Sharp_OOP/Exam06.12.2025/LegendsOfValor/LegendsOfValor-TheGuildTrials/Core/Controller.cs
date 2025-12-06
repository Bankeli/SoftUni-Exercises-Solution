using LegendsOfValor_TheGuildTrials.Core.Contracts;
using LegendsOfValor_TheGuildTrials.Models;
using LegendsOfValor_TheGuildTrials.Models.Contracts;
using LegendsOfValor_TheGuildTrials.Repositories;
using LegendsOfValor_TheGuildTrials.Repositories.Contratcs;
using LegendsOfValor_TheGuildTrials.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsOfValor_TheGuildTrials.Core
{
    public class Controller : IController
    {
        IRepository<IHero> heroes;
        IRepository<IGuild> guilds;
        public Controller()
        {
            heroes = new HeroRepository();
            guilds = new GuildRepository();
        }
        public string AddHero(string heroTypeName, string heroName, string runeMark)
        {
            if (heroTypeName != nameof(Warrior) && heroTypeName != nameof(Sorcerer) && heroTypeName != nameof(Spellblade))
                return string.Format(OutputMessages.InvalidHeroType, heroTypeName);

            IHero hero = heroes.GetModel(runeMark);
            if (hero != null)
                return string.Format(OutputMessages.HeroAlreadyExists, runeMark);

            if (heroTypeName == nameof(Warrior))
                hero = new Warrior(heroName, runeMark);
            else if (heroTypeName == nameof(Sorcerer))
                hero = new Sorcerer(heroName, runeMark);
            else
                hero = new Spellblade(heroName, runeMark);
            heroes.AddModel(hero);
            return string.Format(OutputMessages.HeroAdded, heroTypeName, heroName, runeMark);
        }

        public string CreateGuild(string guildName)
        {
            if (guilds.GetModel(guildName) != null)
                return string.Format(OutputMessages.GuildAlreadyExists, guildName);
            IGuild guild = new Guild(guildName);
            guilds.AddModel(guild);
            return string.Format(OutputMessages.GuildCreated, guildName);
        }

        public string RecruitHero(string runeMark, string guildName)
        {
            IHero hero = heroes.GetModel(runeMark);
            IGuild guild = guilds.GetModel(guildName);

            if (hero == null)
                return string.Format(OutputMessages.HeroNotFound, runeMark);

            if (guild == null)
                return string.Format(OutputMessages.GuildNotFound, guildName);

            if (!string.IsNullOrEmpty(hero.GuildName) && hero.GuildName != "None")
                return string.Format(OutputMessages.HeroAlreadyInGuild, hero.Name);
            if (guild.IsFallen == true)
                return string.Format(OutputMessages.GuildIsFallen, guild.Name);

            if (guild.Wealth < 500)
                return string.Format(OutputMessages.GuildCannotAffordRecruitment, guild.Name);

            bool compatible = false;
            switch (hero)
            {
                case Warrior _:
                    compatible = guildName == "WarriorGuild" || guildName == "ShadowGuild";
                    break;
                case Sorcerer _:
                    compatible = guildName == "SorcererGuild" || guildName == "ShadowGuild";
                    break;
                case Spellblade _:
                    compatible = guildName == "WarriorGuild" || guildName == "SorcererGuild";
                    break;
            }

            var heroTypeName = hero.GetType().Name;
            if (!compatible)
                return string.Format(OutputMessages.HeroTypeNotCompatible, heroTypeName, guild.Name);

            hero.JoinGuild(guild);
            guild.RecruitHero(hero);

            return string.Format(OutputMessages.HeroRecruited, hero.Name, guild.Name);

        }

        public string StartWar(string attackerGuildName, string defenderGuildName)
        {
            IGuild attacker = guilds.GetModel(attackerGuildName);
            IGuild defender = guilds.GetModel(defenderGuildName);

            if (attacker == null || defender == null)
                return OutputMessages.OneOfTheGuildsDoesNotExist;

            if (attacker.IsFallen == true || defender.IsFallen == true)
                return OutputMessages.OneOfTheGuildsIsFallen;

            int attackerPower = CalculateGuildCombatPower(attacker); 
            int deffenderPower = CalculateGuildCombatPower(defender);


            IGuild winner;
            IGuild loser;

            if (attackerPower > deffenderPower)
            {
                winner = attacker;
                loser = defender;
            }
            else
            {
                winner = defender;
                loser = attacker;
            }

           
            int goldAmount = loser.Wealth;

            
            winner.WinWar(goldAmount);   
            loser.LoseWar();

            if (winner == attacker)
                return string.Format(OutputMessages.WarWon, attacker.Name, defender.Name, goldAmount);
            else
                return 
                    string.Format (OutputMessages.WarLost, defender.Name, goldAmount, attacker.Name);


        }

        public string TrainingDay(string guildName)
        {
          IGuild guild = guilds.GetModel(guildName);

            if (guild == null)
                return string.Format(OutputMessages.GuildNotFound, guildName);

            if (guild.IsFallen == true)
                return string.Format(OutputMessages.GuildTrainingDayIsFallen, guildName);

            int count = guild.Legion.Count;
            int totalCost = 200 * count;

            if (totalCost > guild.Wealth)
                return string.Format(OutputMessages.TrainingDayFailed, guild.Name);

            List<IHero> heroesToTrain = new List<IHero>();

            foreach (string runeMark in guild.Legion)
            {
                IHero hero = heroes.GetModel(runeMark);

                if (hero != null)
                {
                    heroesToTrain.Add(hero);
                }
            }

            guild.TrainLegion(heroesToTrain);

           
            return string.Format(OutputMessages.TrainingDayStarted, guildName, count, totalCost);
        }

        public string ValorState()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Valor State:");

            
            var orderedGuilds = guilds.GetAll()
                .OrderByDescending(g => g.Wealth)
                .ToList();

            foreach (var guild in orderedGuilds)
            {
                sb.AppendLine($"{guild.Name} (Wealth: {guild.Wealth})");

                // Order heroes by Name alphabetically
                var heroes = guild.Legion
                    .Select(runeMark => this.heroes.GetModel(runeMark))
                        
                    .Where(h => h != null)
                    .OrderBy(h => h.Name)
                    .ToList();

                foreach (var hero in heroes)
                {
                    sb.AppendLine($"-Hero: [{hero.Name}] of the Guild '{guild.Name}' - RuneMark: {hero.RuneMark}");
                    sb.AppendLine($"--Essence Revealed - Power [{hero.Power}] Mana [{hero.Mana}] Stamina [{hero.Stamina}]");
                }
            }

            return sb.ToString().TrimEnd();
        }

        private int CalculateGuildCombatPower(IGuild guild)
        {
            int totalPower = 0;

            foreach (string runeMark in guild.Legion)
            {
                IHero hero = heroes.GetModel(runeMark);
                   
                if (hero != null)
                {
                    totalPower += hero.Power + hero.Mana + hero.Stamina;
                }
            }

            return totalPower;
        }

    }
}
