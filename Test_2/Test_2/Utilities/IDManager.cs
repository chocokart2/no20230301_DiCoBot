using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Test_2.Utilities
{
    internal class IDManager
    {
        private HashSet<int> ids = new HashSet<int>();
        private readonly string filePath = "ids.json";

        public IDManager()
        {
            LoadFromFile();
        }

        public void AddID(int id)
        {
            ids.Add(id);
            UpdateFile();
        }

        public bool FindID(int id) => ids.Contains(id);

        public void RemoveID(int id)
        {
            if (ids.Remove(id))
            {
                UpdateFile();
            }
        }

        public void UpdateFile()
        {
            string json = JsonSerializer.Serialize(ids, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        private void LoadFromFile()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                ids = JsonSerializer.Deserialize<HashSet<int>>(json) ?? new HashSet<int>();
            }
        }
    }
}
