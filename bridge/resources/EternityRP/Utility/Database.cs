using GTANetworkAPI;
using EternityRP.Models;
using EternityRP.Global;
using Npgsql;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using BCrypt;

namespace EternityRP.Utility
{
    public class Database : Script
    {
        private static string connectionString;
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart()
        {
            string host = NAPI.Resource.GetSetting<string>(this, "host");
            string user = NAPI.Resource.GetSetting<string>(this, "username");
            string pass = NAPI.Resource.GetSetting<string>(this, "password");
            string db = NAPI.Resource.GetSetting<string>(this, "database");
            connectionString = "SERVER=" + host + "; DATABASE=" + db + "; UID=" + user + "; PASSWORD=" + pass + ";";
            //NAPI.Server.SetGlobalServerChat(false);
            ///NAPI.Server.SetAutoSpawnOnConnect(false);
            //NAPI.Server.SetAutoRespawnAfterDeath(false);
        }
        // Server 
        public static void Disconnected(Client client)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("UPDATE server.account SET ip_last = @ip_last::inet, posx = @posX, posy = @posY, posz = @posZ, time_last = CURRENT_TIMESTAMP(0) WHERE id = @id;", conn);
                command.Parameters.AddWithValue("@ip_last", client.Address);
                command.Parameters.AddWithValue("@posX", client.Position.X);
                command.Parameters.AddWithValue("@posY", client.Position.Y);
                command.Parameters.AddWithValue("@posZ", client.Position.Z);
                command.Parameters.AddWithValue("@id", client.GetData("PLAYER_SQL_ID"));
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        public static short GetAccountSocial(string socialName)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    NpgsqlCommand command = conn.CreateCommand();
                    command.CommandText = "SELECT status FROM server.accounts WHERE login = @login LIMIT 1;";
                    command.Parameters.AddWithValue("@login", socialName);
                    command.ExecuteNonQuery();
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            short status = reader.GetInt16(0);
                            return status;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Globals.log.Trace(ex);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.StackTrace);
                }
                return 1;
            }
        }
        public static bool RegisterAccount(string login, string password, string email, string player_ip)
        {        
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    NpgsqlCommand command = new NpgsqlCommand("INSERT INTO server.accounts (login, password, mail, status, status_mail, ip_reg, date_reg) VALUES (@login, @password, @mail, @status, @status_mail, @ip_reg::inet, CURRENT_DATE);", conn);
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);
                    NAPI.Util.ConsoleOutput(password);
                    command.Parameters.AddWithValue("@mail", email);
                    command.Parameters.AddWithValue("@status", 2);
                    command.Parameters.AddWithValue("@status_mail", true);
                    command.Parameters.AddWithValue("@ip_reg", player_ip);
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    Globals.log.Trace(ex);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.StackTrace);
                }
                return false;
            }           
        }
        public static AccountModel AuthorizationAccount(string login, string password)
        {
            AccountModel accountModel = new AccountModel();
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    NpgsqlCommand command = new NpgsqlCommand("SELECT id, password, buy_pers FROM server.accounts WHERE login = @login LIMIT 1;", conn);
                    command.Parameters.AddWithValue("@login", login);
                    command.ExecuteNonQuery();
                    NpgsqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    if (BCryptHelper.CheckPassword(password, reader.GetString(1)))
                    {
                        NAPI.Util.ConsoleOutput("Заебись");
                        accountModel.Id = reader.GetInt32(0);
                        accountModel.Bay_Pers = reader.GetBoolean(2);
                        return accountModel;
                    }
                }
                catch (Exception ex)
                {
                    Globals.log.Trace(ex);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.StackTrace);
                }
                return accountModel;
            }
        }
        public static bool ReturnEmail(string email)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    NpgsqlCommand command = new NpgsqlCommand("SELECT mail FROM server.accounts WHERE mail = @mail LIMIT 1;", conn);
                    command.Parameters.AddWithValue("@mail", email);
                    command.ExecuteNonQuery();
                    NpgsqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        conn.Close();
                        return true;
                    }
                    conn.Close();
         
                }
                catch (Exception ex)
                {

                    Globals.log.Trace(ex);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.StackTrace);                 
                }
                return false;
            }         
        }
        public static bool ReturnPassword(string password)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    NpgsqlCommand command = new NpgsqlCommand("SELECT password FROM server.accounts WHERE password = @password LIMIT 1;", conn);
                    command.Parameters.AddWithValue("@password", password);
                    command.ExecuteNonQuery();
                    NpgsqlDataReader reader = command.ExecuteReader();                   
                    if (reader.Read())
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {

                    Globals.log.Trace(ex);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.StackTrace);
                }
                return false;
            }
        }       
        public static void ResetPassword(string socialName, string password)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    NpgsqlCommand command = new NpgsqlCommand("UPDATE server.accounts SET password = @password WHERE login = @login;", conn);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@login", socialName);
                    command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {

                    Globals.log.Trace(ex);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.StackTrace);
                }
            }
        }
        public static List<ChangeMenuModel> GetPlayerIDEasy(int accountID)
        {
            List<ChangeMenuModel> players = new List<ChangeMenuModel>();
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    NpgsqlCommand command = new NpgsqlCommand("SELECT status, id, name, cash, age, place, admin FROM server.players WHERE id_account = @accountID LIMIT 3;", conn);
                    command.Parameters.AddWithValue("@accountID", accountID);
                    command.ExecuteNonQuery();
                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ChangeMenuModel player = new ChangeMenuModel();
                        player.Status = reader.GetBoolean(0);
                        player.Id = reader.GetInt32(1);
                        player.Name = reader.GetString(2);
                        player.Cash = reader.GetDecimal(3);
                        player.Age = reader.GetInt16(4);
                        player.Place = reader.GetInt16(5);
                        player.Admin = reader.GetInt16(6);
                        players.Add(player);
                    }
                    return players;
                }
                catch (Exception ex)
                {
                    Globals.log.Trace(ex);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.StackTrace);
                }
                return players;
            }
        }
        public static PlayerModel GetPlayerID(int ID)
        {
            PlayerModel playerModel = new PlayerModel();
            playerModel.Id = 0;
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    NpgsqlCommand command = new NpgsqlCommand("SELECT name, cash, admin, age, status, place, sex, health, armor, posx, posy, posz, rotation FROM server.players WHERE id = @ID LIMIT 1;", conn);
                    command.Parameters.AddWithValue("@ID", ID);
                    command.ExecuteNonQuery();
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        float posX = reader.GetFloat(9);
                        float posY = reader.GetFloat(10);
                        float posZ = reader.GetFloat(11);
                        float rot = reader.GetFloat(12);
                        playerModel.Id = ID;
                        playerModel.Name = reader.GetString(0);
                        playerModel.Cash = reader.GetDecimal(1);
                        playerModel.Admin = reader.GetInt16(2);
                        playerModel.Age = reader.GetInt16(3);
                        playerModel.Status = reader.GetBoolean(4);
                        playerModel.Place = reader.GetInt16(5);
                        playerModel.Sex = reader.GetBoolean(6);
                        playerModel.Health = reader.GetInt16(7);
                        playerModel.Armor = reader.GetInt16(8);
                        playerModel.Position = new Vector3(posX, posY, posZ);
                        playerModel.Rotation = new Vector3(0.0, 0.0, rot);
                        return playerModel;
                    }
                }
                catch (Exception ex)
                {
                    Globals.log.Trace(ex);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.StackTrace);
                }
                return playerModel;
            }
        }
        public static SkinModel GetCharacterSkin(int characterId)
        {
            SkinModel skin = new SkinModel();

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                NpgsqlCommand command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM skins WHERE characterId = @characterId LIMIT 1";
                command.Parameters.AddWithValue("@characterId", characterId);

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        skin.firstHeadShape = reader.GetInt32(0);
                        skin.secondHeadShape = reader.GetInt32(1);
                        skin.firstSkinTone = reader.GetInt32(2);
                        skin.secondSkinTone = reader.GetInt32(3);
                        skin.headMix = reader.GetFloat(4);
                        skin.skinMix = reader.GetFloat(5);
                        skin.hairModel = reader.GetInt32(6);
                        skin.firstHairColor = reader.GetInt32(7);
                        skin.secondHairColor = reader.GetInt32(8);
                        skin.beardModel = reader.GetInt32(9);
                        skin.beardColor = reader.GetInt32(10);
                        skin.chestModel = reader.GetInt32(11);
                        skin.chestColor = reader.GetInt32(12);
                        skin.blemishesModel = reader.GetInt32(13);
                        skin.ageingModel = reader.GetInt32(14);
                        skin.complexionModel = reader.GetInt32(15);
                        skin.sundamageModel = reader.GetInt32(16);
                        skin.frecklesModel = reader.GetInt32(17);
                        skin.noseWidth = reader.GetFloat(18);
                        skin.noseHeight = reader.GetFloat(19);
                        skin.noseLength = reader.GetFloat(20);
                        skin.noseBridge = reader.GetFloat(21);
                        skin.noseTip = reader.GetFloat(22);
                        skin.noseShift = reader.GetFloat(23);
                        skin.browHeight = reader.GetFloat(24);
                        skin.browWidth = reader.GetFloat(25);
                        skin.cheekboneHeight = reader.GetFloat(26);
                        skin.cheekboneWidth = reader.GetFloat(27);
                        skin.cheeksWidth = reader.GetFloat(28);
                        skin.eyes = reader.GetFloat(29);
                        skin.lips = reader.GetFloat(30);
                        skin.jawWidth = reader.GetFloat(31);
                        skin.jawHeight = reader.GetFloat(32);
                        skin.chinLength = reader.GetFloat(33);
                        skin.chinPosition = reader.GetFloat(34);
                        skin.chinWidth = reader.GetFloat(35);
                        skin.chinShape = reader.GetFloat(36);
                        skin.neckWidth = reader.GetFloat(37);
                        skin.eyesColor = reader.GetInt32(38);
                        skin.eyebrowsModel = reader.GetInt32(39);
                        skin.eyebrowsColor = reader.GetInt32(40);
                        skin.makeupModel = reader.GetInt32(41);
                        skin.blushModel = reader.GetInt32(42);
                        skin.blushColor = reader.GetInt32(43);
                        skin.lipstickModel = reader.GetInt32(44);
                        skin.lipstickColor = reader.GetInt32(45);
                    }
                }
            }

            return skin;
        }
        public static int CreateCharacter(string SocialClubName, PlayerModel playerModel, SkinModel skin)
        {
            int accountID = 0;
            int playerID = 0;
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    NpgsqlCommand command = new NpgsqlCommand("SELECT id FROM server.accounts WHERE login = @SocialClubName LIMIT 1;");
                    command.Parameters.AddWithValue("@SocialClubName", SocialClubName);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        accountID = reader.GetInt32(0);
                        command.ExecuteNonQuery();
                    }
                    command.CommandText = "INSERT INTO server.players (name, age, sex, id_account) VALUES (@playerName, @playerAge, @playerSex, @accountID";
                    command.Parameters.AddWithValue("@playerName", playerModel.Name);
                    command.Parameters.AddWithValue("@playerAge", playerModel.Age);
                    command.Parameters.AddWithValue("@playerSex", playerModel.Sex);
                    command.Parameters.AddWithValue("@accountID", accountID);
                    command.ExecuteNonQuery();

                    command.CommandText = "SELECT CURRVAL(pg_get_serial_sequence('players','id')) AS last_insert_id;";
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        playerID = reader.GetInt32(0);
                        command.ExecuteNonQuery();
                    }
                    // Store player's skin
                    command.CommandText = "INSERT INTO skins VALUES (@playerId, @firstHeadShape, @secondHeadShape, @firstSkinTone, @secondSkinTone, @headMix, @skinMix, ";
                    command.CommandText += "@hairModel, @firstHairColor, @secondHairColor, @beardModel, @beardColor, @chestModel, @chestColor, @blemishesModel, @ageingModel, ";
                    command.CommandText += "@complexionModel, @sundamageModel, @frecklesModel, @noseWidth, @noseHeight, @noseLength, @noseBridge, @noseTip, @noseShift, @browHeight, ";
                    command.CommandText += "@browWidth, @cheekboneHeight, @cheekboneWidth, @cheeksWidth, @eyes, @lips, @jawWidth, @jawHeight, @chinLength, @chinPosition, @chinWidth, ";
                    command.CommandText += "@chinShape, @neckWidth, @eyesColor, @eyebrowsModel, @eyebrowsColor, @makeupModel, @blushModel, @blushColor, @lipstickModel, @lipstickColor)";
                    command.Parameters.AddWithValue("@playerId", playerID);
                    command.Parameters.AddWithValue("@firstHeadShape", skin.firstHeadShape);
                    command.Parameters.AddWithValue("@secondHeadShape", skin.secondHeadShape);
                    command.Parameters.AddWithValue("@firstSkinTone", skin.firstSkinTone);
                    command.Parameters.AddWithValue("@secondSkinTone", skin.secondSkinTone);
                    command.Parameters.AddWithValue("@headMix", skin.headMix);
                    command.Parameters.AddWithValue("@skinMix", skin.skinMix);
                    command.Parameters.AddWithValue("@hairModel", skin.hairModel);
                    command.Parameters.AddWithValue("@firstHairColor", skin.firstHairColor);
                    command.Parameters.AddWithValue("@secondHairColor", skin.secondHairColor);
                    command.Parameters.AddWithValue("@beardModel", skin.beardModel);
                    command.Parameters.AddWithValue("@beardColor", skin.beardColor);
                    command.Parameters.AddWithValue("@chestModel", skin.chestModel);
                    command.Parameters.AddWithValue("@chestColor", skin.chestColor);
                    command.Parameters.AddWithValue("@blemishesModel", skin.blemishesModel);
                    command.Parameters.AddWithValue("@ageingModel", skin.ageingModel);
                    command.Parameters.AddWithValue("@complexionModel", skin.complexionModel);
                    command.Parameters.AddWithValue("@sundamageModel", skin.sundamageModel);
                    command.Parameters.AddWithValue("@frecklesModel", skin.frecklesModel);
                    command.Parameters.AddWithValue("@noseWidth", skin.noseWidth);
                    command.Parameters.AddWithValue("@noseHeight", skin.noseHeight);
                    command.Parameters.AddWithValue("@noseLength", skin.noseLength);
                    command.Parameters.AddWithValue("@noseBridge", skin.noseBridge);
                    command.Parameters.AddWithValue("@noseTip", skin.noseTip);
                    command.Parameters.AddWithValue("@noseShift", skin.noseShift);
                    command.Parameters.AddWithValue("@browHeight", skin.browHeight);
                    command.Parameters.AddWithValue("@browWidth", skin.browWidth);
                    command.Parameters.AddWithValue("@cheekboneHeight", skin.cheekboneHeight);
                    command.Parameters.AddWithValue("@cheekboneWidth", skin.cheekboneWidth);
                    command.Parameters.AddWithValue("@cheeksWidth", skin.cheeksWidth);
                    command.Parameters.AddWithValue("@eyes", skin.eyes);
                    command.Parameters.AddWithValue("@lips", skin.lips);
                    command.Parameters.AddWithValue("@jawWidth", skin.jawWidth);
                    command.Parameters.AddWithValue("@jawHeight", skin.jawHeight);
                    command.Parameters.AddWithValue("@chinLength", skin.chinLength);
                    command.Parameters.AddWithValue("@chinPosition", skin.chinPosition);
                    command.Parameters.AddWithValue("@chinWidth", skin.chinWidth);
                    command.Parameters.AddWithValue("@chinShape", skin.chinShape);
                    command.Parameters.AddWithValue("@neckWidth", skin.neckWidth);
                    command.Parameters.AddWithValue("@eyesColor", skin.eyesColor);
                    command.Parameters.AddWithValue("@eyebrowsModel", skin.eyebrowsModel);
                    command.Parameters.AddWithValue("@eyebrowsColor", skin.eyebrowsColor);
                    command.Parameters.AddWithValue("@makeupModel", skin.makeupModel);
                    command.Parameters.AddWithValue("@blushModel", skin.blushModel);
                    command.Parameters.AddWithValue("@blushColor", skin.blushColor);
                    command.Parameters.AddWithValue("@lipstickModel", skin.lipstickModel);
                    command.Parameters.AddWithValue("@lipstickColor", skin.lipstickColor);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.StackTrace);
                }
            }

            return playerID;
        }
        //Admin
        public static bool SetAdmin(string socialName, int rang)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    NpgsqlCommand command = new NpgsqlCommand("UPDATE server.players SET admin = @rang WHERE login = @socialName;", conn);
                    command.Parameters.AddWithValue("@rang", rang);
                    command.Parameters.AddWithValue("@socialName", socialName);
                    command.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Globals.log.Trace(ex);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.Message);
                    NAPI.Util.ConsoleOutput("[EXCEPTION CreateCharacter] " + ex.StackTrace);
                }
                return true;
            }
        }
        public static int AuthAdmin(string socialName, string password)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("SELECT admin, password FROM server.players WHERE login = @socialName LIMIT 1;", conn);
                command.Parameters.AddWithValue("@socialName", socialName);
                command.ExecuteNonQuery();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    if (password == reader.GetString(1))
                    {
                        conn.Close();
                        return reader.GetInt16(0);
                    }
                    conn.Close();
                    return 0;
                }
            }
        }
        public static void CreatePromo(string cod, DateTime date)
        {          
            using ( NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("INSERT INTO server.promo_code (p_cod, end_date, state) VALUES (@p_cod, @end_date, @state); SELECT UPDATE server.promo_code SET state = @state pg_sleep_until('tomorrow 00:00');", conn);
                command.Parameters.AddWithValue("@p_cod", cod);
                command.Parameters.AddWithValue("@end_date", date);
                command.Parameters.AddWithValue("@state", true);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        public static void ChangePromo(string cod, bool status)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("UPDATE server.promo_code SET state = @state WHERE p_cod = @cod;", conn);
                command.Parameters.AddWithValue("@state", status);            
                command.Parameters.AddWithValue("@cod", cod);
            }
        }         
        //Смертные
        public static bool ReturnCode(string cod)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("SELECT state FROM server.promo_code WHERE p_cod = @cod LIMIT 1;", conn);
                command.Parameters.AddWithValue("@cod", cod);
                command.ExecuteNonQuery();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {                     
                        bool state = reader.GetBoolean(0);
                        if (state)
                        {
                            conn.Close();
                            return true;                  
                        }
                    }
                    conn.Close();
                    return false;
                }
            }          
        }      
    }
}

