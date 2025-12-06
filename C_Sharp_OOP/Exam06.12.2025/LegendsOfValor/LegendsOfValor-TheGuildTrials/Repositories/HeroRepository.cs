using LegendsOfValor_TheGuildTrials.Models.Contracts;
using LegendsOfValor_TheGuildTrials.Repositories.Contratcs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsOfValor_TheGuildTrials.Repositories
{
    public class HeroRepository : IRepository<IHero>
    {
        private readonly List<IHero> heroes;
        public HeroRepository()
        {
            heroes = new List<IHero>();
        }
        public IReadOnlyCollection<IHero> Models => heroes.AsReadOnly();
        public void AddModel(IHero entity)
        {
            heroes.Add(entity);
        }

        public IReadOnlyCollection<IHero> GetAll() => heroes;
       

        public IHero GetModel(string runeMarkOrGuildName)
        {
            return heroes.FirstOrDefault(h => h.RuneMark == runeMarkOrGuildName /*|| h.GuildName == runeMarkOrGuildName*/);
        }
    }
}
