using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using EternityRP.Utility;
using EternityRP.Global;
using EternityRP.Functions;
using EternityRP.Character;
using EternityRP.Models;

namespace EternityRP.Events
{
    public class Admin : Script
    {
        [RemoteEvent("Command|CreateVehicle")]
        public static void VehicleCommand(Client client, string model, string FirstColor , string SecondColor)
        {           
            if (client.GetData(EntityData.PLAYER_ADMIN_RANK) > Constants.ADMIN_RANG_NONE)
            {               
                NAPI.Vehicle.CreateVehicle(NAPI.Util.VehicleNameToModel(model), client.Position.Around(5), 124, 25, 100);            
            }
        }
        [RemoteEvent("Command|TpPlayers")]
        public static void CommandTpPlayers(Client client, string targetString)
        {
            if (client.GetData(EntityData.PLAYER_ADMIN_RANK) > Constants.ADMIN_RANG1)
            {
                Client target = int.TryParse(targetString, out int targetId) ? Globals.GetPlayerById(targetId) : NAPI.Player.GetPlayerFromName(targetString);

                if (target != null)
                {
                    string message = string.Format(Messages.ADM_GOTO_PLAYER, target.Name);

                    int targetHouse = target.GetData(EntityData.PLAYER_HOUSE_ENTERED);
                    int targetBusiness = target.GetData(EntityData.PLAYER_BUSINESS_ENTERED);

                    client.Position = target.Position;
                    client.Dimension = target.Dimension;
                    client.SetData(EntityData.PLAYER_HOUSE_ENTERED, targetHouse);
                    client.SetData(EntityData.PLAYER_BUSINESS_ENTERED, targetBusiness);
                    client.SendChatMessage(Constants.COLOR_ADMIN_INFO + message);
                }
                else
                {
                    client.SendChatMessage(Constants.COLOR_ERROR + Messages.ERR_PLAYER_NOT_FOUND);
                }
            }
        }
        [RemoteEvent("Command|TpPlace")]
        public static void CommandTpPlace(Client client, Vector3 coor)
        {
            if (client.GetData(EntityData.PLAYER_ADMIN_RANK) > Constants.ADMIN_RANG1)
            {             
                client.Position = coor;
            }
        }
        [RemoteEvent("Command|GetСoord")]
        public static void CheckCoordinateCommand(Client client)
        {
            if (client.GetData(EntityData.PLAYER_ADMIN_RANK) > Constants.ADMIN_RANG1)
            {                           
                client.SendChatMessage(" Position: "+ client.Position.ToString() + "Dimension: " + client.Dimension.ToString());
            }
        }
        [RemoteEvent("Command|GiveGun")]
        public void GunCommand(Client player, string targetString, string weaponName, int ammo)
        {
            if (player.GetData(EntityData.PLAYER_ADMIN_RANK) > Constants.ADMIN_RANG4)
            {
                // We get the player from the input string
                Client target = int.TryParse(targetString, out int targetId) ? Globals.GetPlayerById(targetId) : NAPI.Player.GetPlayerFromName(targetString);

                if (target != null)
                {
                    WeaponHash weapon = NAPI.Util.WeaponNameToModel(weaponName);
                    if (weapon == 0)
                    {
                        player.SendChatMessage(Constants.COLOR_HELP + Messages.GEN_GUN_COMMAND);
                    }
                    else
                    {
                        // Give the weapon to the player
                        //Weapons.GivePlayerNewWeapon(target, weapon, ammo, false);
                    }
                }
                else
                {
                    player.SendChatMessage(Constants.COLOR_ERROR + Messages.ERR_PLAYER_NOT_FOUND);
                }
            }
        }
        [RemoteEvent("Command|GiveMoney")]
        public static void GiveMoney(Client client)
        {
            if (client.GetData(EntityData.PLAYER_ADMIN_RANK) > Constants.ADMIN_RANG4)
            {
                client.SendChatMessage(" Position: " + client.Position.ToString() + "Dimension: " + client.Dimension.ToString());
            }
        }
        [RemoteEvent("Command|TakeMoney")]
        public static void TakeMoney(Client client)
        {
            if (client.GetData(EntityData.PLAYER_ADMIN_RANK) > Constants.ADMIN_RANG4)
            {

                client.SendChatMessage(" Position: " + client.Position.ToString() + "Dimension: " + client.Dimension.ToString());
            }
        }
        [Command("cpromo")]
        public static void CMD_Test(Client client, string cod, int days)
        {
            DateTime date = DateTime.Today;                 
            Database.CreatePromo(cod, date.AddDays(days));
        }
    }
}
