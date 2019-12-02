using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Soccer_Vision.Models
{
    public class DAL
    {
        string StrCon = null;
        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        SqlDataAdapter adapt = null;
        DataSet ds = null;
        public DAL()
        {
            Configuration config = null;
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);

            string exeConfigPath = path;
            try
            {
                config = ConfigurationManager.OpenExeConfiguration(exeConfigPath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //handle errror here.. means DLL has no sattelite configuration file.
            }

            if (config != null)
            {
                StrCon = GetAppSetting(config, "LiveDNS");
            }
            //
            // TODO: Add constructor logic here
            //
            con = new SqlConnection(StrCon);
            com = new SqlCommand();
            com.Connection = con;
            ds = new DataSet();
        }

        string GetAppSetting(Configuration config, string key)
        {
            KeyValueConfigurationElement element = config.AppSettings.Settings[key];
            if (element != null)
            {
                string value = element.Value;
                if (!string.IsNullOrEmpty(value))
                    return value;
            }
            return string.Empty;
        }

        public int CheckUser(string email, string pass)
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT User_ID FROM FinalProject_Kasem_UsersTB WHERE Email = '{email}' AND Password = '{pass}'";
                reader = com.ExecuteReader();
                if (reader.Read())
                {
                    int userID = int.Parse(reader["User_ID"].ToString());
                    reader.Close();
                    return userID;
                }
                else
                {
                    reader.Close();
                    return 0;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }

            }
        }

        public User Login(string email, string pass)
        {
            int userID = CheckUser(email, pass);
            if (userID == 0)
                return null;
            else if (userID == -1)
                return null;
            else
            {
                try
                {
                    con.Open();
                    com.CommandText = $"SELECT * FROM FinalProject_Kasem_UsersTB WHERE User_ID = {userID}";
                    reader = com.ExecuteReader();
                    if (reader.Read())
                    {
                        User user = new User()
                        {
                            UserID = userID,
                            Username = reader["Username"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString(),
                            PhotoName = reader["ProfilePIC"].ToString()
                        };
                        reader.Close();
                        return user;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (con != null)
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                            reader.Close();
                    }

                }
            }
        }

        public int Register(string username, string email, string password)
        {
            if (CheckUser(email, password) == -1)
                return -1;
            else if (CheckUser(email, password) == 0)
            {
                try
                {
                    User user = new User
                    {
                        Username = username,
                        Email = email,
                        Password = password,
                    };

                    if (user.Email == "" && user.Password == "")
                        //return "Invalid email and password"
                        return -2;
                    else if (user.Email == "")
                        //return "Invalid email"
                        return -3;
                    else if (user.Password == "")
                        //return "Invalid password"
                        return -4;

                    con.Open();
                    com.CommandText = "INSERT INTO FinalProject_Kasem_UsersTB (Username,Email,Password) VALUES (@param1,@param2,@param3) SELECT SCOPE_IDENTITY()";
                    com.Parameters.AddWithValue("param1", user.Username);
                    com.Parameters.AddWithValue("param2", user.Email);
                    com.Parameters.AddWithValue("param3", user.Password);
                    object result = com.ExecuteScalar();
                    result = (result == DBNull.Value) ? null : result;
                    int userID = Convert.ToInt32(result);
                    if (userID < 1)
                        throw new Exception("Something went wrong");

                    return userID;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    return -1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (con != null)
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                            reader.Close();
                    }

                }
            }
            else
                return -5;

        }

        public object createGroup(int userID, string groupName, int maxPlayers)
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT Accepted FROM FinalProject_Kasem_UsersInGroupsTB WHERE User_ID = {userID} AND Accepted = {1}";
                reader = com.ExecuteReader();
                if (reader.Read())
                    return "You are already in group!";
                reader.Close();
                com.CommandText = $"INSERT INTO FinalProject_Kasem_GroupsTB (Admin_ID,Group_Name,Max_Player) VALUES (@param1,@param2,@param3) SELECT SCOPE_IDENTITY()";
                com.Parameters.AddWithValue("param1", userID);
                com.Parameters.AddWithValue("param2", groupName);
                com.Parameters.AddWithValue("param3", maxPlayers);
                object result = com.ExecuteScalar();
                result = (result == DBNull.Value) ? null : result;
                int groupID = Convert.ToInt32(result);
                if (groupID < 1)
                    throw new Exception("Something went wrong");

                com.CommandText = $"INSERT INTO FinalProject_Kasem_UsersInGroupsTB (User_ID,Group_ID,Accepted) VALUES (@param1,@param2,@param3)";
                com.Parameters.AddWithValue("param1", userID);
                com.Parameters.AddWithValue("param2", groupID);
                com.Parameters.AddWithValue("param1", true);
                int res = com.ExecuteNonQuery();
                if (res != 1)
                    throw new Exception("Something went wrong");

                return new { message = "Group created!", groupID };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }

            }
        }

        public string CancelMatch(int matchID, DateTime matchDate)
        {
            if (DateTime.Today == matchDate)
                throw new Exception("You can't cancel the match now!");

            try
            {
                con.Open();
                com.CommandText = "CancelMatch";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Match_ID", matchID);
                int res = com.ExecuteNonQuery();
                if (res < 2)
                    throw new Exception("Something went wrong!");
                
                return "Done!";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }

        public int CreateMatch(int userID, string matchName, DateTime matchDate, TimeSpan matchTime, int field, int city, bool isPrivate, string matchKey, int maxPlayers, int timeToPlay)
        {
            int checkMatch = checkIfUserMadeMatchInThisDayAndTime(userID, matchDate, matchTime, timeToPlay, city, field);
            if (checkMatch == 1)
                throw new Exception("You can't make another match in this field and date!");
            else if (checkMatch == -1)
                throw new Exception("Something went wrong");
            else if (checkMatch == 2)
                throw new Exception("You are in Match in this Time/There is a match in this time and field");
            else
            {
                try
                { 

                    if (timeToPlay > 5)
                        throw new Exception("Too much play time!!");
                    else if (timeToPlay < 1)
                        throw new Exception("Little play time!!");

                    if (maxPlayers > 10)
                        throw new Exception("Too much players!");


                    Matchs match = new Matchs()
                    {
                        MatchName = matchName,
                        UserID = userID,
                        MatchDate = matchDate,
                        MatchTime = matchTime,
                        IsPrivate = isPrivate,
                        MatchKey = matchKey,
                        IsActive = true,
                        MaxPlayers = maxPlayers,
                        CityID = city,
                        FieldID = field,
                        PlayTime = timeToPlay
                    };

                    con.Open();
                    com.CommandText = "INSERT INTO FinalProject_Kasem_MatchesTB (Match_Name," +
                        "Admin_ID,Field_ID,City_ID,IsActive,Players_Joined,Max_Players,IsPrivate,Match_Key,Match_Date,Match_Time,Play_Time) VALUES " +
                        "(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10,@param11,@param12) SELECT SCOPE_IDENTITY()";
                    com.Parameters.AddWithValue("param1", match.MatchName);
                    com.Parameters.AddWithValue("param2", match.UserID);
                    com.Parameters.AddWithValue("param3", match.FieldID);
                    com.Parameters.AddWithValue("param4", match.CityID);
                    com.Parameters.AddWithValue("param5", match.IsActive);
                    com.Parameters.AddWithValue("param6", 1);
                    com.Parameters.AddWithValue("param7", match.MaxPlayers);
                    com.Parameters.AddWithValue("param8", match.IsPrivate);
                    if (String.IsNullOrEmpty(match.MatchKey))
                        com.Parameters.AddWithValue("param9", DBNull.Value);
                    else
                        com.Parameters.AddWithValue("param9", match.MatchKey);
                    com.Parameters.AddWithValue("param10", match.MatchDate);
                    com.Parameters.AddWithValue("param11", match.MatchTime);
                    com.Parameters.AddWithValue("param12", match.PlayTime);
                    object res = com.ExecuteScalar();
                    res = (res == DBNull.Value) ? null : res;
                    int matchID = Convert.ToInt32(res);
                    if (matchID < 1)
                        throw new Exception("Something went wrong");

                    com.CommandText = $"INSERT INTO FinalProject_Kasem_UsersInMatch (User_ID,Match_ID,Accepted) VALUES (@param13,@param14,@param15)";
                    com.Parameters.AddWithValue("param13", match.UserID);
                    com.Parameters.AddWithValue("param14", matchID);
                    com.Parameters.AddWithValue("param15", true);
                    int result = com.ExecuteNonQuery();
                    if (result < 1)
                    {
                        throw new Exception("Something went wrong");
                    }

                    return matchID;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    throw new Exception(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (con != null)
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                            reader.Close();
                    }

                }
            }
        }

        private int checkIfUserMadeMatchInThisDayAndTime(int userID, DateTime date, TimeSpan time, int playtime, int cityID, int fieldID)
        {
            try
            {
                con.Open();


                DataTable MatchesJoinedInThisDate = getMatchesJoinedInThisDateAndMatchesInThisField(userID, date, cityID, fieldID);
                if (MatchesJoinedInThisDate != null)
                {
                    if (MatchesJoinedInThisDate.Rows.Count != 0)
                    {
                        foreach (DataRow row in MatchesJoinedInThisDate.Rows)
                        {
                            if (time > TimeSpan.Parse(row["Match_Time"].ToString()))
                            {
                                if ((time - TimeSpan.Parse(row["Match_Time"].ToString())).Hours < int.Parse(row["Play_Time"].ToString()))
                                {
                                    return 2;
                                }
                            }
                            else if (time < TimeSpan.Parse(row["Match_Time"].ToString()))
                            {
                                if ((TimeSpan.Parse(row["Match_Time"].ToString()) - time).Hours < playtime)
                                {
                                    return 2;
                                }
                            }
                            else if (time == TimeSpan.Parse(row["Match_Time"].ToString()))
                                return 2;
                        }
                        return 0;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }

            }
        }

        private DataTable getMatchesJoinedInThisDateAndMatchesInThisField(int userID, DateTime matchDate, int cityID, int fieldID)
        {
            try
            {
                com.CommandText = $"SELECT Match_ID FROM FinalProject_Kasem_UsersInMatch WHERE User_ID = {userID} AND Accepted = {1}";
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "MatchesUserIN");

                com.CommandText = $"SELECT Match_ID FROM FinalProject_Kasem_UsersInvitedToMatch WHERE User_ID = {userID} AND Accepted = {1}";
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "MatchesUserInvitedTo");

                if (ds.Tables["MatchesUserInvitedTo"].Rows.Count == 0 && ds.Tables["MatchesUserIN"].Rows.Count == 0)
                    return ds.Tables["MatchesUserIN"];

                string commandText = $"SELECT * FROM FinalProject_Kasem_MatchesTB WHERE (Match_Date = '{matchDate.ToString("yyyy-MM-dd")}' AND Match_ID IN (";
                int tempCount = 0;
                foreach (DataRow data in ds.Tables["MatchesUserIN"].Rows)
                {
                    if (tempCount == 0)
                    {
                        tempCount++;
                        commandText += $"{data["Match_ID"]}";
                    }
                    else
                    {
                        commandText += $",{data["Match_ID"]}";
                    }
                }
                tempCount = 0;
                foreach (DataRow data in ds.Tables["MatchesUserInvitedTo"].Rows)
                {
                    if (tempCount == 0)
                    {
                        tempCount++;
                        commandText += $"{data["Match_ID"]}";
                    }
                    else
                    {
                        commandText += $",{data["Match_ID"]}";
                    }
                }
                commandText += $")) OR (Match_Date = '{matchDate.ToString("yyyy-MM-dd")}' AND City_ID = {cityID} AND Field_ID = {fieldID})";
                com.CommandText = commandText;
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "MatchesDetailed");

                foreach (DataRow data1 in ds.Tables["MatchesDetailed"].Rows)
                {
                    if (DateTime.Parse(data1["Match_Date"].ToString()) == matchDate && int.Parse(data1["City_ID"].ToString()) == cityID &&
                        int.Parse(data1["Field_ID"].ToString()) == fieldID)
                        return null;
                }

                return ds.Tables["MatchesDetailed"];

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }

        public string ExitMatch(int userID, int matchID, int adminID, DateTime matchDate, int playersJoined)
        {
            if (DateTime.Today == matchDate)
                throw new Exception("You can't exit match now!");
            else if (playersJoined == 1)
                return CancelMatch(matchID, matchDate);
            else
            {
                try
                {
                    if (userID != adminID)
                    {
                        con.Open();
                        com.CommandText = "ExitMatchUser";
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@User_ID", userID);
                        com.Parameters.AddWithValue("@Match_ID", matchID);
                        int rowsAffected = com.ExecuteNonQuery();
                        if (rowsAffected != 2)
                            throw new Exception("Something went wrong!");

                        return "Done!";
                    }
                    else
                    {
                        con.Open();
                        com.CommandText = $"SELECT TOP 1 User_ID FROM FinalProject_Kasem_UsersInMatch WHERE NOT User_ID = {userID}";
                        reader = com.ExecuteReader();
                        if(!reader.Read())
                        {
                            reader.Close();
                            com.CommandText = $"SELECT TOP 1 User_ID FROM FinalProject_Kasem_UsersInvitedToMatch WHERE NOT User_ID = {userID}";
                            reader = com.ExecuteReader();
                            if (!reader.Read())
                                throw new Exception("Something went wrong!");
                        }
                        int newAdmin = int.Parse(reader["User_ID"].ToString());
                        reader.Close();
                        com.CommandText = "ExitMatchAdmin";
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@User_ID", userID);
                        com.Parameters.AddWithValue("@Match_ID", matchID);
                        com.Parameters.AddWithValue("@NewAdmin", newAdmin);
                        int rowsAffected = com.ExecuteNonQuery();
                        if (rowsAffected != 2)
                            throw new Exception("Something went wrong!");

                        return "Done!";
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (con != null)
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                    if (reader != null)
                    {
                        if (!reader.IsClosed)
                            reader.Close();
                    }
                }
            }
        }

        public string CancelRequest(int userID, int matchID)
        {
            try
            {
                con.Open();
                com.CommandText = $"DELETE FROM FinalProject_Kasem_UsersInMatch WHERE User_ID = {userID} AND Match_ID = {matchID}";
                int res = com.ExecuteNonQuery();
                if (res < 1)
                    throw new Exception("Something went wrong!");

                return "Done!";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }

        public string CancelInvite(int userID, int matchID)
        {
            try
            {
                con.Open();
                com.CommandText = $"DELETE FROM FinalProject_Kasem_UsersInvitedToMatch WHERE User_ID = {userID} AND Match_ID = {matchID}";
                int res = com.ExecuteNonQuery();
                if (res < 1)
                    throw new Exception("Something went wrong!");

                return "Done!";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }

        public string RequestJoinMatch(int userID, int matchID, DateTime matchDate, TimeSpan matchTime, int playTime, int cityID, int fieldID, int maxPlayers)
        {
            int checkSpace = 1;
            try
            {
                con.Open();
                com.CommandText = $"SELECT Players_Joined,IsActive FROM FinalProject_Kasem_MatchesTB WHERE Match_ID = {matchID}";
                reader = com.ExecuteReader();
                if (reader.Read())
                {
                    checkSpace = int.Parse(reader["Players_Joined"].ToString()) < maxPlayers ?
                        1 : 0;

                    if (!bool.Parse(reader["IsActive"].ToString()))
                        return "Match is no longer available";
                }
                reader.Close();
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }
            if (checkSpace == 0)
                return "No space in match!";
            else
            {
                int checkIfMatchIsActive = CheckIfMatchIsActive(matchID);
                if (checkIfMatchIsActive == 1)
                    return "Match is not active!";
                else
                {
                    int checkMatch = checkIfUserMadeMatchInThisDayAndTime(userID, matchDate, matchTime, playTime, cityID, fieldID);
                    if (checkMatch == 1)
                        return "You can't join another match in this field and date!";
                    else if (checkMatch == -1)
                        return "Something went wrong";
                    else if (checkMatch == 2)
                        return "You are in Match in this Time/There is a match in this time and field";
                    else
                    {
                        try
                        {
                            con.Open();
                            com.CommandText = $"INSERT INTO FinalProject_Kasem_UsersInMatch (Match_ID,User_ID,Accepted) VALUES (@param1, @param2, @param3)";
                            com.Parameters.AddWithValue("param1", matchID);
                            com.Parameters.AddWithValue("param2", userID);
                            com.Parameters.AddWithValue("param3", false);
                            int result = com.ExecuteNonQuery();
                            if (result != 1)
                                return "something went wrong";

                            return "You have requested to join the match";
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            throw new Exception(e.Message);
                        }
                        finally
                        {
                            if (con != null)
                            {
                                if (con.State == ConnectionState.Open)
                                    con.Close();
                            }
                            if (reader != null)
                            {
                                if (!reader.IsClosed)
                                    reader.Close();
                            }
                        }
                    }
                }
            }
        }

        public string AcceptRequestJoinMatch(int userID, int matchID, DateTime matchDate, TimeSpan matchTime, int playTime, int cityID, int fieldID, int maxPlayers, bool IsInvite)
        {
            try
            {
                con.Open();
                int checkSpace = 1;
                com.CommandText = $"SELECT Players_Joined,IsActive FROM FinalProject_Kasem_MatchesTB WHERE Match_ID = {matchID}";
                reader = com.ExecuteReader();
                if (reader.Read())
                {
                    if (int.Parse(reader["Players_Joined"].ToString()) >= maxPlayers)
                        checkSpace = 0;

                    if (!bool.Parse(reader["IsActive"].ToString()))
                    {
                        reader.Close();
                        if (!IsInvite)
                        {
                            com.CommandText = $"DELETE FROM FinalProject_Kasem_UsersInMatch WHERE User_ID = {userID} AND Match_ID = {matchID}";
                            int res = com.ExecuteNonQuery();
                            if (res != 1)
                                return "Something went wrong";
                        }
                        else
                        {
                            com.CommandText = $"DELETE FROM FinalProject_Kasem_UsersInvitedToMatch WHERE User_ID = {userID} AND Match_ID = {matchID}";
                            int res = com.ExecuteNonQuery();
                            if (res != 1)
                                return "Something went wrong";
                        }
                        return "Match is no longer available";
                    }
                }
                reader.Close();

                if (checkSpace == 0)
                    return "No space in match!";
                else
                {
                    int checkIfMatchIsActive = CheckIfMatchIsActive(matchID);
                    if (checkIfMatchIsActive == 1)
                        return "Match is not active!";
                    else
                    {
                        int checkMatch = checkIfUserMadeMatchInThisDayAndTime(userID, matchDate, matchTime, playTime, cityID, fieldID);
                        if (checkMatch == 1)
                            return "This user can't join another match in this field and date!";
                        else if (checkMatch == -1)
                            return "Something went wrong";
                        else if (checkMatch == 2)
                            return "this user in Match in this Time";
                        else
                        {
                            if (!IsInvite)
                            {
                                com.CommandText = $"UPDATE FinalProject_Kasem_UsersInMatch SET Accepted = {1}";
                                int res = com.ExecuteNonQuery();
                                if (res != 1)
                                    return "Something went Wrong!";
                            }
                            else
                            {
                                com.CommandText = $"UPDATE FinalProject_Kasem_UsersInvitedToMatch SET Accepted = {1}";
                                int res = com.ExecuteNonQuery();
                                if (res != 1)
                                    return "Something went Wrong!";
                            }


                            com.CommandText = $"UPDATE FinalProject_Kasem_MatchesTB SET Players_Joined = (Players_Joined + 1)";
                            int result = com.ExecuteNonQuery();
                            if (result != 1)
                                return "Something went Wrong!";

                            return "Done!";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }

        public string inviteFriendToMatch(int matchID, int friendID, DateTime matchDate, TimeSpan matchTime, int playTime, int cityID, int fieldID, int maxPlayers)
        {
            int checkSpace = 1;
            try
            {
                con.Open();
                com.CommandText = $"SELECT Players_Joined,IsActive FROM FinalProject_Kasem_MatchesTB WHERE Match_ID = {matchID}";
                reader = com.ExecuteReader();
                if (reader.Read())
                {
                    checkSpace = int.Parse(reader["Players_Joined"].ToString()) < maxPlayers ?
                        1 : 0;

                    if (!bool.Parse(reader["IsActive"].ToString()))
                        return "Match is no longer available";
                }
                reader.Close();
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }
            if (checkSpace == 0)
                return "No space in match!";
            else
            {
                int checkMatch = checkIfUserMadeMatchInThisDayAndTime(friendID, matchDate, matchTime, playTime, cityID, fieldID);
                if (checkMatch == 1)
                    return "You can't invite this friend because he is in another match in this field and date!";
                else if (checkMatch == -1)
                    return "Something went wrong";
                else if (checkMatch == 2)
                    return "he is in Match in this Time!";
                else
                {
                    try
                    {
                        con.Open();
                        com.CommandText = $"INSERT INTO FinalProject_Kasem_UsersInvitedToMatch (Match_ID,User_ID,Accepted) VALUES (@param1,@param2,@param3)";
                        com.Parameters.AddWithValue("param1", matchID);
                        com.Parameters.AddWithValue("param2", friendID);
                        com.Parameters.AddWithValue("param3", false);
                        int result = com.ExecuteNonQuery();
                        if (result != 1)
                            return "something went wrong";

                        return "You have invited him to join your match!";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        if (con != null)
                        {
                            if (con.State == ConnectionState.Open)
                                con.Close();
                        }
                    }
                }
            }
        }

        public Matchs getMatchDetails(int matchID)
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT * FROM FinalProject_Kasem_MatchesTB WHERE Match_ID = {matchID}";
                reader = com.ExecuteReader();
                if (reader.Read())
                {
                    Matchs match = new Matchs()
                    {
                        MatchID = matchID,
                        MatchName = reader["Match_Name"].ToString(),
                        UserID = int.Parse(reader["Admin_ID"].ToString()),
                        MatchDate = DateTime.Parse(reader["Match_Date"].ToString()),
                        MatchTime = TimeSpan.Parse(reader["Match_Time"].ToString()),
                        IsPrivate = bool.Parse(reader["IsPrivate"].ToString()),
                        MatchKey = reader["Match_Key"].ToString(),
                        MaxPlayers = int.Parse(reader["Max_Players"].ToString()),
                        PlayersJoined = int.Parse(reader["Players_Joined"].ToString()),
                        CityID = int.Parse(reader["City_ID"].ToString()),
                        FieldID = int.Parse(reader["Field_ID"].ToString()),
                        IsActive = bool.Parse(reader["IsActive"].ToString()),
                        PhotoUrl = reader["Match_Picture"].ToString()
                    };
                    reader.Close();
                    return match;
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }
        }

        private int CheckIfMatchIsActive(int matchID)
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT IsActive,Match_Date,Match_Time FROM FinalProject_Kasem_MatchesTB WHERE Match_ID = {matchID}";
                reader = com.ExecuteReader();
                if (reader.Read())
                {
                    bool res = bool.Parse(reader["IsActive"].ToString());
                    if (res)
                    {
                        DateTime date = DateTime.Parse(reader["Match_Date"].ToString());
                        TimeSpan time = TimeSpan.Parse(reader["Match_Time"].ToString());
                        reader.Close();

                        if (date < DateTime.Now.Date || (date == DateTime.Now.Date && time <= DateTime.Now.TimeOfDay))
                        {
                            com.CommandText = $"UPDATE FinalProject_Kasem_MatchesTB SET IsActive = {0} WHERE Match_ID = {matchID}";
                            int result = com.ExecuteNonQuery();
                            if (result == 1)
                            {
                                com.CommandText = $"DELETE FROM FinalProject_Kasem_UsersInMatch WHERE Match_ID = {matchID}";
                                com.ExecuteNonQuery();
                                com.CommandText = $"DELETE FROM FinalProject_Kasem_UsersInvitedToMatch WHERE Match_ID = {matchID}";
                                com.ExecuteNonQuery();
                                return 1;
                            }
                            else
                                return -1;

                        }
                        else
                        { return 0; };
                    }
                    else
                    {
                        reader.Close();
                        return 0;
                    }

                }
                reader.Close();
                return -1;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }
        }

        private string CheckIfMatchesActive()
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT IsActive,Match_Date,Match_Time,Match_ID FROM FinalProject_Kasem_MatchesTB WHERE IsActive = {1}";
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "Matches");
                if (ds.Tables["Matches"].Rows.Count == 0)
                    return "No active matches";
                else
                {
                    int tempCount = 0;
                    string commandText = $"UPDATE FinalProject_Kasem_MatchesTB SET IsActive = {0} WHERE Match_ID IN (";
                    foreach (DataRow data in ds.Tables["Matches"].Rows)
                    {

                        DateTime date = DateTime.Parse(data["Match_Date"].ToString());
                        TimeSpan time = TimeSpan.Parse(data["Match_Time"].ToString());

                        if (date < DateTime.Now.Date || (date == DateTime.Now.Date && time <= DateTime.Now.TimeOfDay))
                        {
                            if (tempCount == 0)
                            {
                                commandText += $"{int.Parse(data["Match_ID"].ToString())}";
                                tempCount++;
                            }
                            else
                            {
                                commandText += $",{int.Parse(data["Match_ID"].ToString())}";
                                tempCount++;
                            }
                            com.CommandText = $"DELETE FROM FinalProject_Kasem_UsersInMatch WHERE Match_ID = {int.Parse(data["Match_ID"].ToString())}";
                            com.ExecuteNonQuery();
                            com.CommandText = $"DELETE FROM FinalProject_Kasem_UsersInvitedToMatch WHERE Match_ID = {int.Parse(data["Match_ID"].ToString())}";
                            com.ExecuteNonQuery();
                        }
                    }
                    if (tempCount == 0)
                        return "Done!";
                    commandText += ")";
                    com.CommandText = commandText;
                    int res = com.ExecuteNonQuery();
                    if (res != 1)
                        throw new Exception();

                    return "Done!";
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }
        }

        public DataTable ActiveMatchesTable()
        {
            try
            {
                if (CheckIfMatchesActive() == "Done!")
                {
                    con.Open();
                    com.CommandText = "SELECT * FROM FinalProject_Kasem_MatchesTB WHERE IsActive = 'True'";
                    adapt = new SqlDataAdapter(com);
                    adapt.Fill(ds, "ActiveMatches");

                    if (ds.Tables["ActiveMatches"].Rows.Count != 0)
                        return ds.Tables["ActiveMatches"];
                    else
                        return null;
                }
                throw new Exception("Something went wrong!");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }

        public DataTable getFriendsTable()
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT * FROM FinalProject_Kasem_FriendsTB";
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "FriendsList");

                if (ds.Tables["FriendsList"].Rows.Count != 0)
                    return ds.Tables["FriendsList"];
                else
                    return null;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }

        public DataTable FriendsList(int userID)
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT Friend_ID,User_ID FROM FinalProject_Kasem_FriendsTB WHERE (User_ID = {userID} OR Friend_ID = {userID}) AND IsFriends = 'True'";
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "FriendsList");

                if (ds.Tables["FriendsList"].Rows.Count != 0)
                    return getUsersDetailsForFriendsList(ds.Tables["FriendsList"], userID);
                else
                    return null;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }



        public DataTable FriendsRespondsList(int userID)
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT User_ID FROM FinalProject_Kasem_FriendsTB WHERE Friend_ID = {userID} AND IsFriends = 'False'";
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "FriendsList");

                if (ds.Tables["FriendsList"].Rows.Count != 0)
                    return getUsersDetails(ds.Tables["FriendsList"], "User_ID");
                else
                    return null;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }

        public DataTable FriendsRequestsList(int userID)
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT Friend_ID FROM FinalProject_Kasem_FriendsTB WHERE User_ID = {userID} AND IsFriends = 'False'";
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "FriendsList");

                if (ds.Tables["FriendsList"].Rows.Count != 0)
                    return getUsersDetails(ds.Tables["FriendsList"], "Friend_ID");
                else
                    return null;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }

        private DataTable getUsersDetails(DataTable users, string columnName)
        {
            try
            {
                string commandText = "SELECT Username,User_ID,ProfilePIC,Facebook_ID,Google_ID,Email FROM FinalProject_Kasem_UsersTB WHERE User_ID IN (";
                int tempCount = 0;
                foreach (DataRow user in users.Rows)
                {
                    if (tempCount == 0)
                    {
                        commandText += $"{int.Parse(user[columnName].ToString())}";
                        tempCount++;
                    }
                    else
                    {
                        commandText += $",{int.Parse(user[columnName].ToString())}";
                    }
                }
                commandText += ") ORDER BY Username ASC";
                com.CommandText = commandText;
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "DetailedUsersList");

                if (ds.Tables["DetailedUsersList"].Rows.Count != 0)
                    return ds.Tables["DetailedUsersList"];
                else
                    return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
        }

        private DataTable getUsersDetailsForFriendsList(DataTable users, int userID)
        {
            try
            {
                string commandText = "SELECT Username,User_ID,ProfilePIC,Facebook_ID,Google_ID,Email FROM FinalProject_Kasem_UsersTB WHERE User_ID IN (";
                int tempCount = 0;
                foreach (DataRow user in users.Rows)
                {
                    if (int.Parse(user["User_ID"].ToString()) != userID)
                    {
                        if (tempCount == 0)
                        {
                            commandText += $"{int.Parse(user["User_ID"].ToString())}";
                            tempCount++;
                        }
                        else
                        {
                            commandText += $",{int.Parse(user["User_ID"].ToString())}";
                        }
                    }
                    else if(int.Parse(user["Friend_ID"].ToString()) != userID)
                    {
                        if (tempCount == 0)
                        {
                            commandText += $"{int.Parse(user["Friend_ID"].ToString())}";
                            tempCount++;
                        }
                        else
                        {
                            commandText += $",{int.Parse(user["Friend_ID"].ToString())}";
                        }
                    }
                }
                commandText += ") ORDER BY Username ASC";
                com.CommandText = commandText;
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "DetailedUsersList");

                if (ds.Tables["DetailedUsersList"].Rows.Count != 0)
                    return ds.Tables["DetailedUsersList"];
                else
                    return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
        }

        public DataTable getUsersSortedAscending(int userID)
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT User_ID,Username,ProfilePIC,Facebook_ID,Google_ID,Email FROM FinalProject_Kasem_UsersTB WHERE NOT User_ID = {userID} ORDER BY Username ASC";
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "UsersList");

                if (ds.Tables["UsersList"].Rows.Count != 0)
                    return ds.Tables["UsersList"];
                else
                    return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }

        public DataTable getUsersInMatchList()
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT * FROM FinalProject_Kasem_UsersInMatch";
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "UsersInMatch");

                if (ds.Tables["UsersInMatch"].Rows.Count != 0)
                    return ds.Tables["UsersInMatch"];
                else
                    return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }

        public DataTable getUsersInvitedToMatchTable()
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT * FROM FinalProject_Kasem_UsersInvitedToMatch";
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "UsersInvitedToMatch");

                if (ds.Tables["UsersInvitedToMatch"].Rows.Count != 0)
                    return ds.Tables["UsersInvitedToMatch"];
                else
                    return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }

        public DataTable getUsers()
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT User_ID, Username, ProfilePIC, Facebook_ID, Google_ID, Email FROM FinalProject_Kasem_UsersTB";
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "Users");

                if (ds.Tables["Users"].Rows.Count != 0)
                    return ds.Tables["Users"];
                else
                    return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }

        public object getCitiesAndFieldsTables()
        {
            try
            {
                con.Open();
                com.CommandText = $"CitiesAndFields";
                com.CommandType = CommandType.StoredProcedure;
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds);

                if (ds.Tables[0].Rows.Count == 0 || ds.Tables[1].Rows.Count == 0)
                    return null;


                return new
                {
                    cities = ds.Tables[0],
                    fields = ds.Tables[1]
                };

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
            }
        }
    }
}