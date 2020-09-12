using GTANetworkAPI;
using EternityRP.Utility;
using EternityRP.Global;
using EternityRP.Character;
using EternityRP.Models;
using BCrypt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EternityRP.Commands
{
    public class Connection : Script
    {     
        private void LoadCharacterData(Client client, PlayerModel character)
        {
            // string[] jail = character.Jailed.Split(',');

            client.SetData(EntityData.PLAYER_SQL_ID, character.Id);
            client.SetData(EntityData.PLAYER_NAME, character.Name);
            client.SetSharedData(EntityData.PLAYER_CASH, character.Cash);
            //  client.SetSharedData(EntityData.PLAYER_BANK, character.Bank);
            client.SetData(EntityData.PLAYER_SEX, character.Sex);
            client.SetData(EntityData.PLAYER_HEALTH, character.Health);
            client.SetData(EntityData.PLAYER_ARMOR, character.Armor);
            client.SetData(EntityData.PLAYER_AGE, character.Age);
            client.SetData(EntityData.PLAYER_ADMIN_RANK, character.Admin);
            client.SetData(EntityData.PLAYER_SPAWN_POS, character.Position);
            client.SetData(EntityData.PLAYER_SPAWN_ROT, character.Rotation);
            //client.SetData(EntityData.PLAYER_PHONE, character.Phone);
            //client.SetData(EntityData.PLAYER_RADIO, character.Radio);
            //client.SetData(EntityData.PLAYER_KILLED, character.Killed);
            // client.SetData(EntityData.PLAYER_JAIL_TYPE, int.Parse(jail[0]));
            // client.SetData(EntityData.PLAYER_JAILED, int.Parse(jail[1]));
            //client.SetData(EntityData.PLAYER_FACTION, character.Faction);
            // client.SetData(EntityData.PLAYER_JOB, character.Job);
            // client.SetData(EntityData.PLAYER_RANK, character.Rank);
            //client.SetData(EntityData.PLAYER_ON_DUTY, character.Duty);
            // client.SetData(EntityData.PLAYER_VEHICLE_KEYS, character.CarKeys);
            // client.SetData(EntityData.PLAYER_DOCUMENTATION, character.Documentation);
            // client.SetData(EntityData.PLAYER_LICENSES, character.Licenses);
            //client.SetData(EntityData.PLAYER_MEDICAL_INSURANCE, character.Insurance);
            // client.SetData(EntityData.PLAYER_WEAPON_LICENSE, character.WeaponLicense);
            // client.SetData(EntityData.PLAYER_RENT_HOUSE, character.HouseRent);
            //client.SetData(EntityData.PLAYER_HOUSE_ENTERED, character.HouseEntered);
            //client.SetData(EntityData.PLAYER_BUSINESS_ENTERED, character.BusinessEntered);
            //client.SetData(EntityData.PLAYER_EMPLOYEE_COOLDOWN, character.EmployeeCooldown);
            //client.SetData(EntityData.PLAYER_JOB_COOLDOWN, character.JobCooldown);
            // client.SetData(EntityData.PLAYER_JOB_DELIVER, character.JobDeliver);
            // client.SetData(EntityData.PLAYER_JOB_POINTS, character.JobPoints);
            // client.SetData(EntityData.PLAYER_ROLE_POINTS, character.rolePoints);
            // client.SetData(EntityData.PLAYER_PLAYED, character.Played);
            //client.SetData(EntityData.PLAYER_STATUS, character.Status);
        }
        [ServerEvent(Event.PlayerConnected)]
        public void Event_Connected(Client client)
        {
            //client.Transparency = 255;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (client.SocialClubName == null)
                    {
                        client.TriggerEvent("Connection_Player", "none", 3);
                    }
                    else
                    {
                        short status = Database.GetAccountSocial(client.SocialClubName);
                        switch (status)
                        {
                            case 0:
                                client.SendChatMessage(Constants.COLOR_INFO + Messages.INF_ACCOUNT_DISABLED);
                                client.TriggerEvent("Connection_Player", client.SocialClubName, status);
                                client.Kick(Messages.INF_ACCOUNT_DISABLED);
                                break;
                            case 1:
                                client.TriggerEvent("Connection_Player", client.SocialClubName, status);
                                break;
                            case 2:
                                client.TriggerEvent("Connection_Player", client.SocialClubName, status);
                                break;
                                //if (account.lastCharacter > 0)
                                // {
                                // PlayerModel character = Database.LoadCharacterInformationById(account.lastCharacter); еще не загрузил
                                // SkinModel skinModel = Database.GetCharacterSkin(account.lastCharacter);

                                // client.Name = character.realName;
                                //client.SetData(EntityData.PLAYER_SKIN_MODEL, skinModel);
                                // NAPI.Player.SetPlayerSkin(client, character.sex == 0 ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01);

                                // LoadCharacterData(client, character);
                                // Customization.ApplyPlayerCustomization(client, skinModel, character.sex);
                                //Customization.ApplyPlayerClothes(client);
                                //Customization.ApplyPlayerTattoos(client);
                                // }                   
                        }
                    }
                }
                catch (Exception ex)
                {
                    Globals.log.Trace(ex);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.StackTrace);
                }
            });
        }
        [Command("promo")]
        public void CMD_Test(Client client, string cod)
        {
            if (Database.ReturnCode(cod))
            {
                client.SendChatMessage("Ты получил бабло!");
            }
            else
            {
                client.SendChatMessage("Тaкого промокода нет!");
            }
            //NAPI.Ped.CreatePed(PedHash.Abner, client.Position, 100);         
        }
        [RemoteEvent("OnPlayerRegisterAttempt")]
        public void Event_Register(Client client, string login, string email, string password)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!Database.ReturnEmail(email))
                    {
                        string dbpassword = BCryptHelper.HashPassword(password, BCryptHelper.GenerateSalt());
                        Database.RegisterAccount(login, dbpassword, email, client.Address);
                        client.TriggerEvent("RegisterResultTrue");
                    }
                    else
                    {
                        NAPI.Util.ConsoleOutput("не рег");
                        client.TriggerEvent("RegisterResultFalse");
                    }
                }
                catch (Exception ex)
                {
                    Globals.log.Trace(ex);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.StackTrace);
                }
            });
        }
        [RemoteEvent("OnPlayerLoginAttempt")]
        public void Event_Login(Client client, string login, string password)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    AccountModel accountModel = Database.AuthorizationAccount(login, password);
                    // client.TriggerEvent(result ? "LoginResultTrue" : "LoginResultFalse");
                    if (accountModel.Id > 0)
                    {
                        client.SetData(EntityData.PLAYER_SQL_ID, accountModel.Id);
                        client.TriggerEvent("LoginResultTrue", accountModel.Bay_Pers);
                        List<ChangeMenuModel> playerModel = Database.GetPlayerIDEasy(accountModel.Id);
                        if (playerModel.Count > 0)
                        {
                            foreach (ChangeMenuModel player in playerModel)
                            {
                                switch (player.Place)
                                {
                                    case 1:
                                        client.TriggerEvent("СPlayer1", player.Id, player.Name, player.Cash, player.Age, player.Status);
                                        break;
                                    case 2:
                                        client.TriggerEvent("СPlayer2", player.Id, player.Name, player.Cash, player.Age, player.Status);
                                        break;
                                    case 3:
                                        client.TriggerEvent("СPlayer3", player.Id, player.Name, player.Cash, player.Age, player.Status);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            client.TriggerEvent("СPlayerDefault");
                        }
                    }
                    else
                    {
                        client.TriggerEvent("LoginResultFalse");
                    }
                }
                catch (Exception ex)
                {
                    Globals.log.Trace(ex);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.StackTrace);
                }
            });
        }
        [RemoteEvent("PlayToServer")]
        public void Event_PlayToServer(Client client, int ID)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    PlayerModel playerModel = Database.GetPlayerID(ID);
                    if (playerModel.Id != 0)
                    {
                        LoadCharacterData(client, playerModel);
                        
                        //SkinModel skinModel = Database.GetCharacterSkin(Id);                     
                        //client.SetData(EntityData.PLAYER_SKIN_MODEL ,skinModel);
                        //NAPI.Player.SetPlayerSkin(client, playerModel.Sex == false ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01);
                        //Customization.ApplyPlayerCustomization(client, skinModel, playerModel.Sex);
                        //Customization.ApplyPlayerClothes(client);
                        //Customization.ApplyPlayerTattoos(client);
                    }
                }
                catch (Exception ex)
                {
                    Globals.log.Trace(ex);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.StackTrace);
                }
            });
        }
        [RemoteEvent("ResetEmailToServer")]
        public void Event_SendToEmailCode(Client client, string email)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (Database.ReturnEmail(email))
                    {
                        Random rnd = new Random();
                        int cod = rnd.Next(10000, 99999);
                        var fromAddress = new MailAddress("xaxovichr@gmail.com", "*****");
                        var toAddress = new MailAddress(email, "*****");
                        const string fromPassword = "******";
                        const string subject = "Ваш код подтверждения";
                        string body = cod.ToString();
                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                            Timeout = 20000
                        };
                        using (var message = new MailMessage(fromAddress, toAddress)
                        {
                            Subject = subject,
                            Body = body
                        })
                        {
                            smtp.Send(message);
                            client.SetData("COD", cod);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Globals.log.Trace(ex);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.StackTrace);
                }
            });
        }
        [RemoteEvent("ResetCodeToServer")]
        public void Event_ResetCodeToServer(Client client, int enterCod)
        {
            Task.Factory.StartNew(() =>
            {
                if (client.GetData("COD") == enterCod)
                {
                    client.TriggerEvent("ReturnAnswer", 1);
                }
                else { client.TriggerEvent("ReturnAnswer", 0); }
            });
        }
        [RemoteEvent("ResetPassword")]
        public void Event_ResetPassword(Client client, string password)
        {
            Task.Factory.StartNew(() =>
            {
                password = BCryptHelper.HashPassword(password, BCryptHelper.GenerateSalt());
                if (!Database.ReturnPassword(password))
                {
                    Database.ResetPassword(client.SocialClubName, password);
                    client.TriggerEvent("AnswerResetPasswordTrue");
                }
                else
                {
                    client.TriggerEvent("AnswerResetPasswordFalse");
                }
            });
        }
        [RemoteEvent("createCharacter")]
        public void CreateCharacterEvent(Client client, string playerName, Int16 playerAge, Boolean playerSex, string skinJson)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    PlayerModel playerModel = new PlayerModel();
                    SkinModel skinModel = NAPI.Util.FromJson<SkinModel>(skinJson);
                    playerModel.Age = playerAge;
                    playerModel.Sex = playerSex;
                    // Apply the skin to the character
                    client.SetData(EntityData.PLAYER_SKIN_MODEL, skinModel);
                    Customization.ApplyPlayerCustomization(client, skinModel, playerSex);
                    int playerId = Database.CreateCharacter(client.SocialClubName, playerModel, skinModel);

                    if (playerId > 0)
                    {
                        // Database.UpdateLastCharacter(client.SocialClubName, playerId);
                        client.TriggerEvent("characterCreatedSuccessfully");
                    }
                }
                catch (Exception ex)
                {
                    Globals.log.Trace(ex);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.StackTrace);
                }
            });
        }
        /*[RemoteEvent("loadCharacter")]
        public void LoadCharacterEvent(Client player, string name)
        {
            Task.Factory.StartNew(() =>
            {
                PlayerModel playerModel = Database.LoadCharacterInformationByName(name);
                SkinModel skinModel = Database.GetCharacterSkin(playerModel.Id);

                // Load player's model
                player.Name = playerModel.Name;
                player.SetData(EntityData.PLAYER_SKIN_MODEL, skinModel);
                NAPI.Player.SetPlayerSkin(player, playerModel.Sex == 0 ? PedHash.FreemodeMale01 : PedHash.FreemodeFemale01);

                LoadCharacterData(player, playerModel);
                Customization.ApplyPlayerCustomization(player, skinModel, playerModel.Sex);
                Customization.ApplyPlayerClothes(player);
                Customization.ApplyPlayerTattoos(player);

                // Update last selected character
                Database.UpdateLastCharacter(player.SocialClubName, playerModel.Id);
            });
        }*/
        [RemoteEvent("setCharacterIntoCreator")]
        public void SetCharacterIntoCreatorEvent(Client client)
        {
            // Change player's skin
            NAPI.Player.SetPlayerSkin(client, PedHash.FreemodeMale01);

            // Remove clothes
            client.SetClothes(11, 15, 0);
            client.SetClothes(3, 15, 0);
            client.SetClothes(8, 15, 0);

            // Set player's position
            client.Transparency = 255;
            client.Rotation = new Vector3(0.0f, 0.0f, 180.0f);
            client.Position = new Vector3(152.3787f, -1000.644f, -99f);
        }
    }
}
