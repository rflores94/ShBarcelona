using Audit.EntityFramework;
using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;
using System;
using Audit.Core;
using ShBarcelona.DAL.Entities;
using ShBarcelona.DAL.Configurations;

namespace ShBarcelona.DAL
{
    public class RecruitmentContext : AuditDbContext
    {
        //TODO: EF column encryption
        private readonly IEncryptionProvider _encryptionProvider;
        public RecruitmentContext(DbContextOptions options) : base(options)
        {
            //TODO: EF column encryption  (needs a 256-bit valid key from API config file)
            _encryptionProvider = new GenerateEncryptionProvider(Environment.GetEnvironmentVariable("ENCRYPTIONPROVIDER"));
            //For test mode
            //_encryptionProvider = new GenerateEncryptionProvider("J@NcRfUjXn2r5u8x/A?D(G+KbPdSgVkY");
        }

        public DbSet<AreaEntity> Areas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO: EF column encryption
            //modelBuilder.UseEncryption(_encryptionProvider);
            modelBuilder.ApplyConfiguration(new AreaEntityConfiguration());

            modelBuilder.UseEncryption(this._encryptionProvider);
        }

        public override void OnScopeSaving(IAuditScope auditScope)
        {
            base.OnScopeSaving(auditScope);

            var evt = auditScope.Event;

            evt.CustomFields.TryGetValue("User", out object username);

            //foreach (var entry in evt.GetEntityFrameworkEvent().Entries)
            //{
            //    this.AuditLog.Add(new AuditLog
            //    {
            //        AuditUser = (string)username ?? Environment.UserName,
            //        TablePk = entry.Table,
            //        AuditAction = entry.Action,
            //        AuditDate = DateTime.Now,
            //        EntityType = entry.Entity?.GetType()?.FullName ?? "Entity",
            //        AuditData = entry.ToJson()
            //    });
            //}
        }

        public override void OnScopeSaved(IAuditScope auditScope)
        {
            base.OnScopeSaved(auditScope);

            this.SaveChanges();
        }
    }
    
}
