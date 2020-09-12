using GTANetworkAPI;
using EternityRP.Utility;
using EternityRP.Global;
using EternityRP.Character;
using EternityRP.Models;
using System;

namespace EternityRP.Events
{
    class Disconnect : Script
    {
        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(Client client, DisconnectionType type, string reason)
        {
            switch (type)
            {
                case DisconnectionType.Left:
                    Database.Disconnected(client.GetData(EntityData.PLAYER_SQL_ID));
                    break;

                case DisconnectionType.Timeout:
                    NAPI.Chat.SendChatMessageToAll("~b~" + client.SocialClubName + "~w~ has timed out.");
                    Database.Disconnected(client.GetData(EntityData.PLAYER_SQL_ID));
                    break;

                case DisconnectionType.Kicked:
                    NAPI.Chat.SendChatMessageToAll("~b~" + client.SocialClubName + "~w~ has been kicked for " + reason);
                    Database.Disconnected(client.GetData(EntityData.PLAYER_SQL_ID));
                    break;
            }
        }
    }
}
