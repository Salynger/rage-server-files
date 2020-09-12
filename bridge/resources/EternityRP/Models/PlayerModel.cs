using GTANetworkAPI;
using System;

namespace EternityRP.Models
{

    public class PlayerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Int16 Admin { get; set; }
        public decimal Cash { get; set; }
        public Int16 Health { get; set; }
        public Int16 Armor { get; set; }
        public Int16 Age { get; set; }
        public Boolean Sex { get; set; }
        public Boolean Status { get; set; }
        public Int16 Place { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        public PlayerModel() { }

        public PlayerModel(int id, string name, Int16 admin, decimal cash, Int16 health, Int16 armor, Int16 age, Boolean sex, Boolean status, Int16 place, Vector3 position, Vector3 rotation)
        {
            this.Id = id;
            this.Name = name;
            this.Admin = admin;
            this.Cash = cash;
            this.Health = health;
            this.Armor = armor;
            this.Age = age;
            this.Sex = sex;
            this.Status = status;
            this.Place = place;
            this.Position = position;
            this.Rotation = rotation;
        }
    }
}
