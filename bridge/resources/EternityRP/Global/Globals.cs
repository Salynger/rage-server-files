using GTANetworkAPI;
using NLog;
using EternityRP.Models;
using EternityRP.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System;

namespace EternityRP.Global
{
    public class Globals : Script
    {
        //private int fastFoodId = 1;
        //public static int orderGenerationTime;
       // public static List<FastFoodOrderModel> fastFoodOrderList;
        public static List<ClothesModel> clothesList;
        public static List<TattooModel> tattooList;
        public static List<ItemModel> itemList;
        // static List<ScoreModel> scoreList;
        //public static List<AdminTicketModel> adminTicketList;
        //private Timer minuteTimer;
        //private Timer playersCheckTimer;

        public static Logger log;
        public static void Logger()
        {
            try
            {
                log = LogManager.GetCurrentClassLogger();

                log.Trace("Version: {0}", Environment.Version.ToString());
                log.Trace("OS: {0}", Environment.OSVersion.ToString());
                log.Trace("Command: {0}", Environment.CommandLine.ToString());

                NLog.Targets.FileTarget tar = (NLog.Targets.FileTarget)LogManager.Configuration.FindTargetByName("run_log");
                tar.DeleteOldFileOnStartup = false;
            }
            catch (Exception e)
            {
                NAPI.Util.ConsoleOutput("Ошибка работы с логом!n" + e.Message);
            }
            // ловим все не обработанные исключения
        }
        public static Client GetPlayerById(int id)
        {
            Client target = null;
            foreach (Client player in NAPI.Pools.GetAllPlayers())
            {
                if (player.Value == id)
                {
                    target = player;
                    break;
                }
            }
            return target;
        }
        public static List<ClothesModel> GetPlayerClothes(int playerId)
        {
            // Get a list with the player's clothes
            return clothesList.Where(c => c.player == playerId).ToList();
        }

        public static ClothesModel GetDressedClothesInSlot(int playerId, int type, int slot)
        {
            // Get the clothes in the selected slot
            return clothesList.FirstOrDefault(c => c.player == playerId && c.type == type && c.slot == slot && c.dressed);
        }
        public static Vehicle GetClosestVehicle(Client player, float distance = 2.5f)
        {
            Vehicle vehicle = null;
            foreach (Vehicle veh in NAPI.Pools.GetAllVehicles())
            {
                Vector3 vehPos = veh.Position;
                float distanceVehicleToPlayer = player.Position.DistanceTo(vehPos);

                if (distanceVehicleToPlayer < distance && player.Dimension == veh.Dimension)
                {
                    distance = distanceVehicleToPlayer;
                    vehicle = veh;
                }
            }
            return vehicle;
        }
    }
}