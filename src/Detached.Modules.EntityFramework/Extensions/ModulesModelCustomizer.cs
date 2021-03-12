﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq;

namespace Detached.Modules.EntityFramework.DbContextExtension
{
    public class ModulesModelCustomizer : IModelCustomizer
    {
        readonly Application _app;

        public ModulesModelCustomizer(Application app)
        {
            _app = app;
        }

        public void Customize(ModelBuilder modelBuilder, DbContext context)
        {
            foreach (Module module in _app.Modules)
            {
                foreach (EFRepositoryComponent component in module.Components.OfType<EFRepositoryComponent>())
                {
                    component.ConfigureModel(context, modelBuilder);
                }
            }
        }
    }
}