using GTANetworkAPI;
using EternityRP.Utility;
using EternityRP.Global;
using EternityRP.Character;
using EternityRP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System;

namespace EternityRP.Functions
{
    public class Vehicles : Script
    {
        private static Dictionary<int, Timer> gasTimerList = new Dictionary<int, Timer>();
        private static Dictionary<int, Timer> vehicleRespawnTimerList = new Dictionary<int, Timer>();

       /* public void LoadDatabaseVehicles()
        {
            List<VehicleModel> vehicleList = Database.LoadAllVehicles();
            Parking.parkedCars = new List<ParkedCarModel>();

            foreach (VehicleModel vehModel in vehicleList)
            {
                if (vehModel.parking == 0)
                {
                    // Create the vehicle ingame
                    CreateIngameVehicle(vehModel);
                }
                else
                {
                    // Link the car to the parking
                    ParkedCarModel parkedCarModel = new ParkedCarModel();
                    parkedCarModel.parkingId = vehModel.parking;
                    parkedCarModel.vehicle = vehModel;
                    
                    Parking.parkedCars.Add(parkedCarModel);
                }
            }
        }
        public static void CreateVehicle(Client player, VehicleModel vehModel, bool adminCreated)
        {
            Task.Factory.StartNew(() =>
            {
                NAPI.Task.Run(() =>
                {
                    // Add the vehicle to the database
                    vehModel.id = Database.AddNewVehicle(vehModel);

                    // Create the vehicle ingame
                    CreateIngameVehicle(vehModel);

                    if (!adminCreated)
                    {
                        int moneyLeft = player.GetSharedData(EntityData.PLAYER_BANK) - vehModel.price;
                        string purchaseMssage = string.Format(Messages.SUC_VEHICLE_PURCHASED, vehModel.model, vehModel.price);
                        player.SendChatMessage(Constants.COLOR_SUCCESS + purchaseMssage);
                        player.SetSharedData(EntityData.PLAYER_BANK, moneyLeft);
                    }
                });
            });
        }*/
    }
}