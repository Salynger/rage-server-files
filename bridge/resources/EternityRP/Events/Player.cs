using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using EternityRP.Utility;
using EternityRP.Global;
using EternityRP.Character;
using EternityRP.Models;
using EternityRP.Functions;

namespace EternityRP.Events
{
    public class Player : Script
    {
        [Command("money", "usage: /money")]
        public static void MoneyCommand(Client client)
        {
            client.SendChatMessage(client.GetData(EntityData.PLAYER_CASH));
        }
    }
}
