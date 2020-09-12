using GTANetworkAPI;
using EternityRP.Utility;
using EternityRP.Global;
using EternityRP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace EternityRP.weapons
{
    public class Weapons : Script
    {
        private static Timer weaponTimer;
        private static List<Timer> vehicleWeaponTimer;
        public static List<WeaponCrateModel> weaponCrateList;

        public static void GivePlayerWeaponItems(Client player)
        {
            int itemId = 0;
            int playerId = player.GetData(EntityData.PLAYER_SQL_ID);
            foreach (ItemModel item in Globals.itemList)
            {
                if (!int.TryParse(item.hash, out itemId) && item.ownerIdentifier == playerId && item.ownerEntity == Constants.ITEM_ENTITY_WHEEL)
                {
                    WeaponHash weaponHash = NAPI.Util.WeaponNameToModel(item.hash);
                    player.GiveWeapon(weaponHash, 0);
                    player.SetWeaponAmmo(weaponHash, item.amount);
                }
            }
        }

        public static void GivePlayerNewWeapon(Client player, WeaponHash weapon, int bullets, bool licensed)
        {
            // Create weapon model
            ItemModel weaponModel = new ItemModel();
            weaponModel.hash = weapon.ToString();
            weaponModel.amount = bullets;
            weaponModel.ownerEntity = Constants.ITEM_ENTITY_WHEEL;
            weaponModel.ownerIdentifier = player.GetData(EntityData.PLAYER_SQL_ID);
            weaponModel.position = new Vector3(0.0f, 0.0f, 0.0f);
            weaponModel.dimension = 0;

            Task.Factory.StartNew(() =>
            {
                //weaponModel.id = Database.AddNewItem(weaponModel);
                Globals.itemList.Add(weaponModel);
            });

            player.GiveWeapon(weapon, 0);
            player.SetWeaponAmmo(weapon, bullets);
            
            if (licensed)
            {
                Task.Factory.StartNew(() =>
                {
                    // We add the weapon as a registered into database
                   // Database.AddLicensedWeapon(weaponModel.id, player.Name);
                });
            }
        }

        public static string GetGunAmmunitionType(WeaponHash weapon)
        {
            string type = string.Empty;
            foreach (GunModel gun in Constants.GUN_LIST)
            {
                if (weapon == gun.Weapon)
                {
                    type = gun.Ammunition;
                    break;
                }
            }
            return type;
        }

        public static int GetGunAmmunitionCapacity(WeaponHash weapon)
        {
            int amount = 0;
            foreach (GunModel gun in Constants.GUN_LIST)
            {
                if (weapon == gun.Weapon)
                {
                    amount = gun.Capacity;
                    break;
                }
            }
            return amount;
        }
    }
}
