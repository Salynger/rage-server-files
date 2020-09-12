using GTANetworkAPI;
using System;

namespace EternityRP.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Owner { get; set; }
        public string Plate { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public int ColorType { get; set; }
        public string FirstColor { get; set; }
        public string SecondColor { get; set; }
        public int Pearlescent { get; set; }
        public uint Dimension { get; set; }
        public int Faction { get; set; }
        public int Engine { get; set; }
        public int Locked { get; set; }
        public int Price { get; set; }
        public int Parking { get; set; }
        public int Parked { get; set; }
        public float Gas { get; set; }
        public float Kms { get; set; }
        public VehicleModel() { }

        public VehicleModel(int id, string model, string owner, string plate, Vector3 position, Vector3 rotation, int colorType, string firstColor, string secondColor, int pearlescent, uint dimension, int faction, int engine, int locked, int price, int parking, int parked, float gas, float kms)
        {
            this.Id = id;
            this.Model = model;
            this.Owner = owner;
            this.Plate = plate;
            this.Position = position;
            this.ColorType = colorType;
            this.FirstColor = firstColor;
            this.SecondColor = secondColor;
            this.Pearlescent = pearlescent;
            this.Dimension = dimension;
            this.Faction = faction;
            this.Engine = engine;
            this.Locked = locked;
            this.Price = price;
            this.Parking = parking;
            this.Gas = gas;
            this.Kms = kms;
            this.Engine = engine;
        }
    }
}
