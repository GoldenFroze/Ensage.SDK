// <copyright file="HeroTable.cs" company="Ensage">
//    Copyright (c) 2017 Ensage.
// </copyright>

namespace Ensage.SDK.Plugins.DebugTables
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

    using Ensage.SDK.Helpers;

    [Export(typeof(Table))]
    public sealed class HeroTable : Table
    {
        public HeroTable()
            : base("Hero")
        {
        }

        internal override void OnUpdate()
        {
            var hero = ObjectManager.LocalHero;
            var data = new Dictionary<string, string>();

            foreach (var property in hero.GetType().GetProperties().OrderBy(e => e.Name))
            {
                var value = property.GetValue(hero)?.ToString();

                if (!string.IsNullOrEmpty(value))
                {
                    data[property.Name] = value;
                }
            }

            this.Columns =
                new List<TableColumn>
                {
                    new TableColumn("Name", data.Keys),
                    new TableColumn("Value", data.Values)
                };
        }
    }
}