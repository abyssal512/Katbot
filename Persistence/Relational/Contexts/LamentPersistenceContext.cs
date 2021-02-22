using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lament.Persistence.Document;
using Microsoft.EntityFrameworkCore;

namespace Lament.Persistence.Relational
{
    public class LamentPersistenceContext: DbContext
    {
        public DbSet<JsonRow<GuildConfig>> GuildConfigurations { get; set; }
        
        public DbSet<Reminder> Reminders { get; set; }

        public LamentPersistenceContext(DbContextOptions<LamentPersistenceContext> options) : base(options)
        {

        }

        public async Task<TJsonObject> GetJsonObjectAsync<TJsonObject>(
            Func<LamentPersistenceContext, DbSet<JsonRow<TJsonObject>>> accessor, ulong guildId) 
            where TJsonObject : JsonRootObject<TJsonObject>
        {
            var row = accessor(this);
            var rowResult = await row.FindAsync(guildId);
            if (rowResult != null) return rowResult.Data;
            rowResult = new JsonRow<TJsonObject>();
            row.Add(rowResult);
            await SaveChangesAsync();
            return rowResult.Data;
        }
        
        public async Task<TJsonObject> ModifyJsonObjectAsync<TJsonObject>(
            Func<LamentPersistenceContext, DbSet<JsonRow<TJsonObject>>> accessor, ulong guildId, Action<TJsonObject> modifier) 
            where TJsonObject : JsonRootObject<TJsonObject>
        {
            var row = accessor(this);
            var rowResult = await row.FindAsync(guildId);
            if (rowResult == null)
            {
                rowResult = new JsonRow<TJsonObject>();
                row.Add(rowResult);
            }

            modifier(rowResult.Data);
            Entry(rowResult).Property(d => d.Data).IsModified = true;
            await SaveChangesAsync();
            return rowResult.Data;
        }
    }
}