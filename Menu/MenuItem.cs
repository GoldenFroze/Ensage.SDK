// <copyright file="MenuItem.cs" company="Ensage">
//    Copyright (c) 2017 Ensage.
// </copyright>

namespace Ensage.SDK.Menu
{
    using Ensage.Common.Menu;

    public class MenuItem<TType>
    {
        public MenuItem(string displayName, string name, TType value)
        {
            this.Item = new MenuItem(name, displayName);
            this.Item.SetValue(value);
        }

        public MenuItem(MenuItem item)
        {
            this.Item = item;
        }

        public MenuItem Item { get; }

        public TType Value => this.Item.GetValue<TType>();
    }
}