using System.Collections.Generic;

namespace ToolsManager.DataModel.Common
{
    public class Machine
    {
        public string Name { get; set; }
        public string Owner { get; set; }
    }

    public class Machines
    {
        public List<Machine> Items { get; set; }

        public Machine this[string name]
        {
            get { return this.ItemAt(name); }
        }

        public Machines()
        {
            this.Items = new List<Machine>();
        }

        public void Add(Machine item)
        {
            this.Items.Add(item);
        }

        public void Remove(Machine item)
        {
            this.Items.Remove(item);
        }

        private Machine ItemAt(string name)
        {
            foreach (Machine item in this.Items)
                if (item.Name == name)
                    return item;

            return null;
        }
    }
}
