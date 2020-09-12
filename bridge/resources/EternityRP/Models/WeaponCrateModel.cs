using GTANetworkAPI;
using System;

namespace EternityRP.Models
{
    public class WeaponCrateModel
    {
        public string ContentItem { get; set; }
        public int ContentAmount { get; set; }
        public Vector3 Position { get; set; }
        public string CarriedEntity { get; set; }
        public int CarriedIdentifier { get; set; }
        public GTANetworkAPI.Object CrateObject { get; set; }
        public WeaponCrateModel() { }
        public WeaponCrateModel(string contentItem, int contentAmount, Vector3 position, string carriedEntity, int carriedIdentifier, GTANetworkAPI.Object crateObject)
        {
            this.ContentItem = contentItem;
            this.ContentAmount = contentAmount;
            this.Position = position;
            this.CarriedEntity = carriedEntity;
            this.CarriedIdentifier = carriedIdentifier;
            this.CrateObject = crateObject;
        }
    }
}
