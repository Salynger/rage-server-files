using GTANetworkAPI;
using System;

namespace EternityRP.Models
{
    public class GunModel
    {
        public WeaponHash Weapon { get; set; }
        public string Ammunition { get; set; }
        public int Capacity { get; set; }

        public GunModel() { }
        public GunModel(WeaponHash weapon, string ammunition, int capacity)
        {
            this.Weapon = weapon;
            this.Ammunition = ammunition;
            this.Capacity = capacity;
        }
    }
}
