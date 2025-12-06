using LegendsOfValor_TheGuildTrials.Models;
using LegendsOfValor_TheGuildTrials.Models.Contracts;
using LegendsOfValor_TheGuildTrials.Repositories.Contratcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsOfValor_TheGuildTrials.Repositories
{
    public class GuildRepository : IRepository<IGuild>
    {
        private readonly List<IGuild> guilds;
        public GuildRepository()
        {
            guilds = new List<IGuild>();
        }

        public IReadOnlyCollection<IGuild> Models => guilds.AsReadOnly();
        public void AddModel(IGuild entity)
        {
            guilds.Add(entity);
        }

        public IReadOnlyCollection<IGuild> GetAll() => guilds;
        

        public IGuild GetModel(string runeMarkOrGuildName)
        {
            return guilds.FirstOrDefault(g => g.Name == runeMarkOrGuildName);
        }
    }
}
