﻿using AdminPanel.Application.Interfaces;
using AdminPanel.Application.Interfaces.Contexts;
using AdminPanel.Application.Interfaces.Shared;
using AdminPanel.Domain.Common;
using AdminPanel.Domain.Common.Models;
using AdminPanel.Domain.Entities;
using AdminPanel.Infrastructure.Identity.Models;
using AdminPanel.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdminPanel.Infrastructure.DbContexts
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public ApplicationDbContext(
            DbContextOptions options,
            IAuthenticatedUserService currentUserService,
            IDateTimeService dateTime) : base(options)
        {
            _authenticatedUser = currentUserService;
            _dateTime = dateTime;
        }

        public bool HasChanges => ChangeTracker.HasChanges();

        public DbSet<Community> Communities { get; set; }
        public DbSet<Correspondence> Correspondences { get; set; }
        public DbSet<Message> Messages { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        entry.Entity.Created = _dateTime.NowUtc;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                        entry.Entity.LastModified = _dateTime.NowUtc;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
