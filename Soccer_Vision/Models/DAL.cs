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
                            PhotoName = reader["ProfilePIC"].ToString(),
                            Google_ID = reader["Google_ID"].ToString(),
                            Facebook_ID = reader["Facebook_ID"].ToString()
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

        public string changeUsername(int userID, string newUserName)
        {
            try
            {
                con.Open();
                com.CommandText = $"UPDATE FinalProject_Kasem_UsersTB SET Username = '{newUserName}' WHERE User_ID = {userID}";
                int res = com.ExecuteNonQuery();
                if (res != 1)
                    throw new Exception("Something went Wrong!");

                return "ok";
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

        public string changePassword(int userID, string newPassword)
        {
            try
            {
                con.Open();
                User objForPasswordValidationCheck = new User() { Password = newPassword };
                if (objForPasswordValidationCheck.Password == "" || objForPasswordValidationCheck == null)
                    throw new Exception("Invalid password (password should be at least 8 characters and at least 1 number and 1 capital letter and 1 small letter)");

                com.CommandText = $"UPDATE FinalProject_Kasem_UsersTB SET Password = '{newPassword}' WHERE User_ID = {userID}";
                int res = com.ExecuteNonQuery();
                if (res != 1)
                    throw new Exception("Something went Wrong!");

                return "ok";
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

        public User getUserAfterChanges(int userID)
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
                        PhotoName = reader["ProfilePIC"].ToString(),
                        Google_ID = reader["Google_ID"].ToString(),
                        Facebook_ID = reader["Facebook_ID"].ToString()
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

        public string CancelGroup(int groupID)
        {
            try
            {
                con.Open();
                com.CommandText = "CancelGroup";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Group_ID", groupID);
                int res = com.ExecuteNonQuery();
                if (res < 0)
                    throw new Exception("Something went wrong!");
                else if (res == 0)
                    throw new Exception("You have already canceled the group");
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

        public int createGroup(int userID, string groupName, int maxPlayers)
        {
            try
            {
                con.Open();
                com.CommandText = $"INSERT INTO FinalProject_Kasem_GroupsTB (Admin_ID,Group_Name,Max_Players,Users_Joined) VALUES (@param1,@param2,@param3,@param4) SELECT SCOPE_IDENTITY()";
                com.Parameters.AddWithValue("param1", userID);
                com.Parameters.AddWithValue("param2", groupName);
                com.Parameters.AddWithValue("param3", maxPlayers);
                com.Parameters.AddWithValue("param4", 1);
                object result = com.ExecuteScalar();
                result = (result == DBNull.Value) ? null : result;
                int groupID = Convert.ToInt32(result);
                if (groupID < 1)
                    throw new Exception("Something went wrong");

                com.CommandText = $"INSERT INTO FinalProject_Kasem_UsersInGroupsTB (User_ID,Group_ID,Accepted) VALUES (@param5,@param6,@param7)";
                com.Parameters.AddWithValue("param5", userID);
                com.Parameters.AddWithValue("param6", groupID);
                com.Parameters.AddWithValue("param7", true);
                int res = com.ExecuteNonQuery();
                if (res != 1)
                    throw new Exception("Something went wrong");

                return groupID;
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
                if (res < 0)
                    throw new Exception("Something went wrong!");
                else if (res == 0)
                    throw new Exception("You have already canceled the match");

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
            int checkMatch = checkIfUserCanMakeOrJoinMatchInThisDayAndTimeAndField(userID, matchDate, matchTime, timeToPlay, field, true);
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

        public int createMatchWithGroup(int userID, string matchName, DateTime matchDate, TimeSpan matchTime, int field, int city, bool isPrivate, string matchKey, int maxPlayers, int timeToPlay, List<int> usersIDs)
        {
            try
            {


                List<int> usersThatCanJoin = new List<int>();
                foreach (int user in usersIDs)
                {
                    if (userID == user)
                    {
                        int check = checkIfUserCanMakeOrJoinMatchInThisDayAndTimeAndField(user, matchDate, matchTime, timeToPlay, field, true);
                        if (check == 1)
                            throw new Exception("You can't make another match in this field and date!");
                        else if (check == -1)
                            throw new Exception("Something went wrong");
                        else if (check == 2)
                            throw new Exception("You are in Match in this Time/There is a match in this time and field");
                    }
                    else
                    {
                        int check = checkIfUserCanMakeOrJoinMatchInThisDayAndTimeAndField(user, matchDate, matchTime, timeToPlay, field, false);
                        if (check == 0)
                            usersThatCanJoin.Add(user);
                    }
                }
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
                SqlTransaction trans = con.BeginTransaction();
                com.Transaction = trans;
                com.CommandText = $"INSERT INTO FinalProject_Kasem_UsersInvitedToMatch (User_ID,Match_ID,Accepted) VALUES (@param16,@param17,@param18)";
                com.Parameters.Add(new SqlParameter("@param16", SqlDbType.Int));
                com.Parameters.Add(new SqlParameter("@param17", SqlDbType.Int));
                com.Parameters.Add(new SqlParameter("@param18", SqlDbType.Bit));
                foreach (int user in usersThatCanJoin)
                {
                    com.Parameters[15].Value = user;
                    com.Parameters[16].Value = matchID;
                    com.Parameters[17].Value = false;
                    result = com.ExecuteNonQuery();
                    if (result < 1)
                    {
                        throw new Exception("Something went wrong");
                    }
                }
                trans.Commit();
                return matchID;
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

        private int checkIfUserCanMakeOrJoinMatchInThisDayAndTimeAndField(int userID, DateTime date, TimeSpan time, int playtime, int fieldID, bool isCreateMatch)
        {
            try
            {
                con.Open();


                DataTable MatchesInThisDate = getMatchesInThisDateForMatchMakingOrJoining(userID, date, fieldID, isCreateMatch);
                if (MatchesInThisDate != null)
                {
                    if (MatchesInThisDate.Rows.Count != 0)
                    {
                        foreach (DataRow row in MatchesInThisDate.Rows)
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

        private DataTable getMatchesInThisDateForMatchMakingOrJoining(int userID, DateTime matchDate, int fieldID, bool isCreateMatch)
        {
            try
            {
                com.CommandText = $"SELECT Match_ID FROM FinalProject_Kasem_UsersInMatch WHERE User_ID = {userID} AND Accepted = {1}";
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "MatchesUserIN");

                com.CommandText = $"SELECT Match_ID FROM FinalProject_Kasem_UsersInvitedToMatch WHERE User_ID = {userID} AND Accepted = {1}";
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "MatchesUserInvitedTo");

                if (!isCreateMatch)
                {
                    if (ds.Tables["MatchesUserIN"].Rows.Count != 0 || ds.Tables["MatchesUserInvitedTo"].Rows.Count != 0)
                    {
                        string commandText = $"SELECT * FROM FinalProject_Kasem_MatchesTB WHERE Match_Date = '{matchDate.ToString("yyyy-MM-dd")}' AND Match_ID IN (";
                        if (ds.Tables["MatchesUserIN"].Rows.Count != 0)
                        {
                            int count = 0;
                            foreach (DataRow data in ds.Tables["MatchesUserIN"].Rows)
                            {
                                if (count == 0)
                                    commandText += $"{int.Parse(data["Match_ID"].ToString())}";
                                else
                                    commandText += $", {int.Parse(data["Match_ID"].ToString())}";
                                count++;
                            }
                        }
                        if (ds.Tables["MatchesUserInvitedTo"].Rows.Count != 0)
                        {
                            int count = 0;
                            foreach (DataRow data in ds.Tables["MatchesUserIN"].Rows)
                            {
                                if (count == 0)
                                    commandText += $"{int.Parse(data["Match_ID"].ToString())}";
                                else
                                    commandText += $", {int.Parse(data["Match_ID"].ToString())}";
                                count++;
                            }
                        }
                        commandText += ')';
                        com.CommandText = commandText;
                        adapt = new SqlDataAdapter(com);
                        adapt.Fill(ds, "MatchesDetailed");
                        return ds.Tables["MatchesDetailed"];
                    }
                    return ds.Tables["MatchesUserIN"];
                }
                else
                {
                    string commandText = $"SELECT * FROM FinalProject_Kasem_MatchesTB WHERE Match_Date = '{matchDate.ToString("yyyy-MM-dd")}' AND (Field_ID = ${fieldID}";
                    if (ds.Tables["MatchesUserIN"].Rows.Count != 0 || ds.Tables["MatchesUserInvitedTo"].Rows.Count != 0)
                    {
                        commandText += $" OR (NOT Field_ID = ${fieldID} AND Match_ID IN (";
                        if (ds.Tables["MatchesUserIN"].Rows.Count != 0)
                        {
                            int count = 0;
                            foreach (DataRow data in ds.Tables["MatchesUserIN"].Rows)
                            {
                                if (count == 0)
                                    commandText += $"{int.Parse(data["Match_ID"].ToString())}";
                                else
                                    commandText += $", {int.Parse(data["Match_ID"].ToString())}";
                                count++;
                            }
                        }
                        if (ds.Tables["MatchesUserInvitedTo"].Rows.Count != 0)
                        {
                            int count = 0;
                            foreach (DataRow data in ds.Tables["MatchesUserIN"].Rows)
                            {
                                if (count == 0)
                                    commandText += $"{int.Parse(data["Match_ID"].ToString())}";
                                else
                                    commandText += $", {int.Parse(data["Match_ID"].ToString())}";
                                count++;
                            }
                        }
                        commandText += ")))";
                    }
                    else
                        commandText += ')';
                    com.CommandText = commandText;
                    adapt = new SqlDataAdapter(com);
                    adapt.Fill(ds, "MatchesDetailed");
                    foreach (DataRow data1 in ds.Tables["MatchesDetailed"].Rows)
                    {
                        if (int.Parse(data1["Admin_ID"].ToString()) == userID
                            && int.Parse(data1["Field_ID"].ToString()) == fieldID)
                            return null;
                    }

                    return ds.Tables["MatchesDetailed"];
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
                    con.Open();
                    com.CommandText = $"SELECT * FROM FinalProject_Kasem_UsersInMatch WHERE User_ID = {userID} AND Match_ID = {matchID} AND Accepted = {1}";
                    reader = com.ExecuteReader();
                    if (!reader.Read())
                    {
                        reader.Close();
                        com.CommandText = $"SELECT * FROM FinalProject_Kasem_UsersInvitedToMatch WHERE User_ID = {userID} AND Match_ID = {matchID} AND Accepted = {1}";
                        reader = com.ExecuteReader();
                        if (!reader.Read())
                        {
                            reader.Close();
                            return "User already exited the match";
                        }
                    }
                    reader.Close();
                    if (userID != adminID)
                    {
                        com.CommandText = "ExitMatchUser";
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@User_ID", userID);
                        com.Parameters.AddWithValue("@Match_ID", matchID);
                        int rowsAffected = com.ExecuteNonQuery();
                        if (rowsAffected < 0)
                            throw new Exception("Something went wrong!");

                        return "Done!";
                    }
                    else
                    {
                        com.CommandText = $"SELECT TOP 1 User_ID FROM FinalProject_Kasem_UsersInMatch WHERE NOT User_ID = {userID} AND Accepted = {1} AND Match_ID = {matchID}";
                        reader = com.ExecuteReader();
                        if (!reader.Read())
                        {
                            reader.Close();
                            com.CommandText = $"SELECT TOP 1 User_ID FROM FinalProject_Kasem_UsersInvitedToMatch WHERE NOT User_ID = {userID} AND Accepted = {1} AND Match_ID = {matchID}";
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
                        if (rowsAffected < 0)
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

        public string ExitGroup(int userID, int groupID, int adminID, int playersJoined)
        {
            if (playersJoined == 1)
                return CancelGroup(groupID);
            else
            {
                try
                {
                    con.Open();
                    com.CommandText = $"SELECT * FROM FinalProject_Kasem_UsersInGroupsTB WHERE User_ID = {userID} AND Group_ID = {groupID} AND Accepted = {1}";
                    reader = com.ExecuteReader();
                    if (!reader.Read())
                    {
                        reader.Close();
                        com.CommandText = $"SELECT * FROM FinalProject_Kasem_UsersInvitedToGroup WHERE User_ID = {userID} AND Group_ID = {groupID} AND Accepted = {1}";
                        reader = com.ExecuteReader();
                        if (!reader.Read())
                        {
                            reader.Close();
                            return "User already exited the group";
                        }
                    }
                    reader.Close();
                    if (userID != adminID)
                    {
                        com.CommandText = "ExitGroupUser";
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@User_ID", userID);
                        com.Parameters.AddWithValue("@Group_ID", groupID);
                        int rowsAffected = com.ExecuteNonQuery();
                        if (rowsAffected < 0)
                            throw new Exception("Something went wrong!");

                        return "Done!";
                    }
                    else
                    {
                        com.CommandText = $"SELECT TOP 1 User_ID FROM FinalProject_Kasem_UsersInGroupsTB WHERE NOT User_ID = {userID} AND Accepted = {1} AND Group_ID = {groupID}";
                        reader = com.ExecuteReader();
                        if (!reader.Read())
                        {
                            reader.Close();
                            com.CommandText = $"SELECT TOP 1 User_ID FROM FinalProject_Kasem_UsersInvitedToGroup WHERE NOT User_ID = {userID} AND Accepted = {1} AND Group_ID = {groupID}";
                            reader = com.ExecuteReader();
                            if (!reader.Read())
                                throw new Exception("Something went wrong!");
                        }
                        int newAdmin = int.Parse(reader["User_ID"].ToString());
                        reader.Close();
                        com.CommandText = "ExitGroupAdmin";
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@User_ID", userID);
                        com.Parameters.AddWithValue("@Group_ID", groupID);
                        com.Parameters.AddWithValue("@NewAdmin", newAdmin);
                        int rowsAffected = com.ExecuteNonQuery();
                        if (rowsAffected < 0)
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
                if (res < 0)
                    throw new Exception("Something went wrong!");
                else if (res == 0)
                    throw new Exception("You have already canceled the request");

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

        public string CancelGroupRequest(int userID, int groupID)
        {
            try
            {
                con.Open();
                com.CommandText = $"DELETE FROM FinalProject_Kasem_UsersInGroupsTB WHERE User_ID = {userID} AND Group_ID = {groupID}";
                int res = com.ExecuteNonQuery();
                if (res < 0)
                    throw new Exception("Something went wrong!");
                else if (res == 0)
                    throw new Exception("You have already canceled the request");

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
                if (res < 0)
                    throw new Exception("Something went wrong!");
                else if (res == 0)
                    throw new Exception("You have already canceled the invite");

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

        public string CancelGroupInvite(int userID, int groupID)
        {
            try
            {
                con.Open();
                com.CommandText = $"DELETE FROM FinalProject_Kasem_UsersInvitedToGroup WHERE User_ID = {userID} AND Group_ID = {groupID}";
                int res = com.ExecuteNonQuery();
                if (res < 0)
                    throw new Exception("Something went wrong!");
                else if (res == 0)
                    throw new Exception("You have already canceled the invite");

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
                if (checkSpace == 0)
                    return "No space in match!";
                else
                {

                    con.Close();


                    int checkIfMatchIsActive = CheckIfMatchIsActive(matchID);
                    if (checkIfMatchIsActive == 1)
                        return "Match is not active!";
                    else
                    {
                        int checkMatch = checkIfUserCanMakeOrJoinMatchInThisDayAndTimeAndField(userID, matchDate, matchTime, playTime, fieldID, false);
                        if (checkMatch == 1)
                            return "You can't join another match in this field and date!";
                        else if (checkMatch == -1)
                            return "Something went wrong";
                        else if (checkMatch == 2)
                            return "You are in Match in this Time/There is a match in this time and field";
                        else
                        {
                            con.Open();
                            com.CommandText = $"SELECT Accepted FROM FinalProject_Kasem_UsersInvitedToMatch WHERE User_ID = {userID} AND Match_ID = {matchID}";
                            reader = com.ExecuteReader();
                            if (reader.Read())
                            {
                                if (!bool.Parse(reader["Accepted"].ToString()))
                                {
                                    reader.Close();
                                    com.CommandText = $"UPDATE FinalProject_Kasem_UsersInvitedToMatch SET Accepted = {1} WHERE User_ID = {userID} AND Match_ID = {matchID}";
                                    int res = com.ExecuteNonQuery();
                                    if (res < 1)
                                        return "Something went wrong!";
                                    com.CommandText = $"UPDATE FinalProject_Kasem_MatchesTB SET Players_Joined = (Players_Joined + 1) WHERE Match_ID = {matchID}";
                                    res = com.ExecuteNonQuery();
                                    if (res > 0)
                                        return "You have joined the match because you were invited!";
                                    return "Something went wrong!";
                                }
                                return "You are in this match!";
                            }
                            reader.Close();
                            com.CommandText = $"SELECT User_ID FROM FinalProject_Kasem_UsersInMatch WHERE User_ID = {userID} AND Match_ID = {matchID}";
                            reader = com.ExecuteReader();
                            if (reader.Read())
                            {
                                return "You have already requested to join this match!";
                            }
                            reader.Close();
                            com.CommandText = $"INSERT INTO FinalProject_Kasem_UsersInMatch (Match_ID,User_ID,Accepted) VALUES (@param1, @param2, @param3)";
                            com.Parameters.AddWithValue("param1", matchID);
                            com.Parameters.AddWithValue("param2", userID);
                            com.Parameters.AddWithValue("param3", false);
                            int result = com.ExecuteNonQuery();
                            if (result != 1)
                                return "something went wrong";

                            return "You have requested to join the match";
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
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }
        }

        public string RequestJoinGroup(int userID, int groupID, int maxPlayers)
        {
            int checkSpace = 1;
            try
            {
                con.Open();
                com.CommandText = $"SELECT Users_Joined FROM FinalProject_Kasem_GroupsTB WHERE Group_ID = {groupID}";
                reader = com.ExecuteReader();
                if (reader.Read())
                {
                    checkSpace = int.Parse(reader["Users_Joined"].ToString()) < maxPlayers ?
                        1 : 0;
                }
                reader.Close();
                if (checkSpace == 0)
                    return "No space in group!";
                else
                {
                    com.CommandText = $"SELECT Accepted FROM FinalProject_Kasem_UsersInvitedToGroup WHERE User_ID = {userID} AND Group_ID = {groupID}";
                    reader = com.ExecuteReader();
                    if (reader.Read())
                    {
                        if (!bool.Parse(reader["Accepted"].ToString()))
                        {
                            reader.Close();
                            com.CommandText = $"UPDATE FinalProject_Kasem_UsersInvitedToGroup SET Accepted = {1} WHERE User_ID = {userID} AND Group_ID = {groupID}";
                            int res = com.ExecuteNonQuery();
                            if (res < 1)
                                return "Something went wrong!";
                            com.CommandText = $"UPDATE FinalProject_Kasem_GroupsTB SET Users_Joined = (Users_Joined + 1) WHERE Group_ID = {groupID}";
                            res = com.ExecuteNonQuery();
                            if (res > 0)
                                return "You have joined the group because you were invited!";
                            return "Something went wrong!";
                        }
                        return "You are in this group!";
                    }
                    reader.Close();
                    com.CommandText = $"SELECT User_ID FROM FinalProject_Kasem_UsersInGroupsTB WHERE User_ID = {userID} AND Group_ID = {groupID}";
                    reader = com.ExecuteReader();
                    if (reader.Read())
                    {
                        return "You have already requested to join this group!";
                    }
                    reader.Close();
                    com.CommandText = $"INSERT INTO FinalProject_Kasem_UsersInGroupsTB (Group_ID,User_ID,Accepted) VALUES (@param1, @param2, @param3)";
                    com.Parameters.AddWithValue("param1", groupID);
                    com.Parameters.AddWithValue("param2", userID);
                    com.Parameters.AddWithValue("param3", false);
                    int result = com.ExecuteNonQuery();
                    if (result != 1)
                        return "something went wrong";

                    return "You have requested to join the group";
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
                        com.CommandText = $"DELETE FROM FinalProject_Kasem_UsersInMatch WHERE Match_ID = {matchID}";
                        com.ExecuteNonQuery();

                        com.CommandText = $"DELETE FROM FinalProject_Kasem_UsersInvitedToMatch WHERE Match_ID = {matchID}";
                        com.ExecuteNonQuery();

                        return "Match is no longer available";
                    }
                }
                reader.Close();

                if (checkSpace == 0)
                    return "No space in match!";
                else
                {

                    if (con != null)
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                    int checkIfMatchIsActive = CheckIfMatchIsActive(matchID);
                    if (checkIfMatchIsActive == 1)
                        return "Match is not active!";
                    else
                    {
                        int checkMatch = checkIfUserCanMakeOrJoinMatchInThisDayAndTimeAndField(userID, matchDate, matchTime, playTime, fieldID, false);
                        if (checkMatch == 1)
                            return "This user can't join another match in this field and date!";
                        else if (checkMatch == -1)
                            return "Something went wrong";
                        else if (checkMatch == 2)
                            return "this user in Match in this Time";
                        else
                        {
                            con.Open();
                            if (!IsInvite)
                            {
                                com.CommandText = $"SELECT Accepted FROM FinalProject_Kasem_UsersInMatch WHERE User_ID = {userID} AND Match_ID = {matchID}";
                                reader = com.ExecuteReader();
                                if (reader.Read())
                                {
                                    if (bool.Parse(reader["Accepted"].ToString()))
                                    {
                                        reader.Close();
                                        return "You have already accepted to let him join!";
                                    }
                                }
                                reader.Close();
                                com.CommandText = $"UPDATE FinalProject_Kasem_UsersInMatch SET Accepted = {1} WHERE User_ID = {userID} AND Match_ID = {matchID}";
                                int res = com.ExecuteNonQuery();
                                if (res < 0)
                                    return "Something went Wrong!";
                            }
                            else
                            {
                                com.CommandText = $"SELECT Accepted FROM FinalProject_Kasem_UsersInvitedToMatch WHERE User_ID = {userID} AND Match_ID = {matchID}";
                                reader = com.ExecuteReader();
                                if (reader.Read())
                                {
                                    if (bool.Parse(reader["Accepted"].ToString()))
                                    {
                                        reader.Close();
                                        return "You have already accepted to join the match!";
                                    }
                                }
                                reader.Close();
                                com.CommandText = $"UPDATE FinalProject_Kasem_UsersInvitedToMatch SET Accepted = {1} WHERE User_ID = {userID} AND Match_ID = {matchID}";
                                int res = com.ExecuteNonQuery();
                                if (res < 0)
                                    return "Something went Wrong!";
                            }


                            com.CommandText = $"UPDATE FinalProject_Kasem_MatchesTB SET Players_Joined = (Players_Joined + 1) WHERE Match_ID = {matchID}";
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
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }
        }

        public string AcceptRequestJoinGroup(int userID, int groupID, int maxPlayers, bool IsInvite)
        {
            try
            {
                con.Open();
                int checkSpace = 1;
                com.CommandText = $"SELECT Users_Joined FROM FinalProject_Kasem_GroupsTB WHERE Group_ID = {groupID}";
                reader = com.ExecuteReader();
                if (reader.Read())
                {
                    if (int.Parse(reader["Users_Joined"].ToString()) >= maxPlayers)
                        checkSpace = 0;
                }
                reader.Close();

                if (checkSpace == 0)
                    return "No space in group!";
                else
                {
                    if (!IsInvite)
                    {
                        com.CommandText = $"SELECT Accepted FROM FinalProject_Kasem_UsersInGroupsTB WHERE User_ID = {userID} AND Group_ID = {groupID}";
                        reader = com.ExecuteReader();
                        if (reader.Read())
                        {
                            if (bool.Parse(reader["Accepted"].ToString()))
                            {
                                reader.Close();
                                return "You have already accepted to let him join!";
                            }
                        }
                        reader.Close();
                        com.CommandText = $"UPDATE FinalProject_Kasem_UsersInGroupsTB SET Accepted = {1} WHERE User_ID = {userID} AND Group_ID = {groupID}";
                        int res = com.ExecuteNonQuery();
                        if (res < 0)
                            return "Something went Wrong!";
                    }
                    else
                    {
                        com.CommandText = $"SELECT Accepted FROM FinalProject_Kasem_UsersInvitedToGroup WHERE User_ID = {userID} AND Group_ID = {groupID}";
                        reader = com.ExecuteReader();
                        if (reader.Read())
                        {
                            if (bool.Parse(reader["Accepted"].ToString()))
                            {
                                reader.Close();
                                return "You have already accepted to join the group!";
                            }
                        }
                        reader.Close();
                        com.CommandText = $"UPDATE FinalProject_Kasem_UsersInvitedToGroup SET Accepted = {1} WHERE User_ID = {userID} AND Group_ID = {groupID}";
                        int res = com.ExecuteNonQuery();
                        if (res < 0)
                            return "Something went Wrong!";
                    }


                    com.CommandText = $"UPDATE FinalProject_Kasem_GroupsTB SET Users_Joined = (Users_Joined + 1) WHERE Group_ID = {groupID}";
                    int result = com.ExecuteNonQuery();
                    if (result != 1)
                        return "Something went Wrong!";

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


                if (checkSpace == 0)
                    return "No space in match!";
                else
                {
                    int checkMatch = checkIfUserCanMakeOrJoinMatchInThisDayAndTimeAndField(friendID, matchDate, matchTime, playTime, fieldID, false);
                    if (checkMatch == 1)
                        return "You can't invite this friend because he is in another match in this field and date!";
                    else if (checkMatch == -1)
                        return "Something went wrong";
                    else if (checkMatch == 2)
                        return "he is in Match in this Time!";
                    else
                    {
                        con.Open();
                        com.CommandText = $"SELECT Accepted FROM FinalProject_Kasem_UsersInMatch WHERE User_ID = {friendID} AND Match_ID = {matchID}";
                        reader = com.ExecuteReader();
                        if (reader.Read())
                        {
                            if (!bool.Parse(reader["Accepted"].ToString()))
                            {
                                reader.Close();
                                com.CommandText = $"UPDATE FinalProject_Kasem_UsersInMatch SET Accepted = {1} WHERE User_ID = {friendID} AND Match_ID = {matchID}";
                                int res = com.ExecuteNonQuery();
                                if (res < 1)
                                    return "Something went wrong!";
                                com.CommandText = $"UPDATE FinalProject_Kasem_MatchesTB SET Players_Joined = (Players_Joined + 1) WHERE Match_ID = {matchID}";
                                res = com.ExecuteNonQuery();
                                if (res > 0)
                                    return "the player requested to join before you invite him so he will join immediately!";
                                return "Something went wrong!";
                            }
                            return "he is in the match!";
                        }
                        reader.Close();
                        com.CommandText = $"SELECT User_ID FROM FinalProject_Kasem_UsersInvitedToMatch WHERE User_ID = {friendID} AND Match_ID = {matchID}";
                        reader = com.ExecuteReader();
                        if (reader.Read())
                        {
                            reader.Close();
                            return "You have already invited him to join this match!";
                        }
                        reader.Close();
                        com.CommandText = $"INSERT INTO FinalProject_Kasem_UsersInvitedToMatch (Match_ID,User_ID,Accepted) VALUES (@param1,@param2,@param3)";
                        com.Parameters.AddWithValue("param1", matchID);
                        com.Parameters.AddWithValue("param2", friendID);
                        com.Parameters.AddWithValue("param3", false);
                        int result = com.ExecuteNonQuery();
                        if (result != 1)
                            return "something went wrong";

                        return "You have invited him to join your match!";
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
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }
        }

        public string inviteFriendToGroup(int groupID, int friendID, int maxPlayers)
        {
            int checkSpace = 1;
            try
            {
                con.Open();
                com.CommandText = $"SELECT Users_Joined FROM FinalProject_Kasem_GroupsTB WHERE Group_ID = {groupID}";
                reader = com.ExecuteReader();
                if (reader.Read())
                {
                    checkSpace = int.Parse(reader["Users_Joined"].ToString()) < maxPlayers ?
                        1 : 0;
                }
                reader.Close();

                if (checkSpace == 0)
                    return "No space in group!";
                else
                {
                    com.CommandText = $"SELECT Accepted FROM FinalProject_Kasem_UsersInGroupsTB WHERE User_ID = {friendID} AND Group_ID = {groupID}";
                    reader = com.ExecuteReader();
                    if (reader.Read())
                    {
                        if (!bool.Parse(reader["Accepted"].ToString()))
                        {
                            reader.Close();
                            com.CommandText = $"UPDATE FinalProject_Kasem_UsersInGroupsTB SET Accepted = {1} WHERE User_ID = {friendID} AND Group_ID = {groupID}";
                            int res = com.ExecuteNonQuery();
                            if (res < 1)
                                return "Something went wrong!";
                            com.CommandText = $"UPDATE FinalProject_Kasem_GroupsTB SET Users_Joined = (Users_Joined + 1) WHERE Group_ID = {groupID}";
                            res = com.ExecuteNonQuery();
                            if (res > 0)
                                return "the player requested to join before you invite him so he will join immediately!";
                            return "Something went wrong!";
                        }
                        return "he is in the group!";
                    }
                    reader.Close();
                    com.CommandText = $"SELECT User_ID FROM FinalProject_Kasem_UsersInvitedToGroup WHERE User_ID = {friendID} AND Group_ID = {groupID}";
                    reader = com.ExecuteReader();
                    if (reader.Read())
                    {
                        reader.Close();
                        return "You have already invited him to join this group!";
                    }
                    reader.Close();
                    com.CommandText = $"INSERT INTO FinalProject_Kasem_UsersInvitedToGroup (Group_ID,User_ID,Accepted) VALUES (@param1,@param2,@param3)";
                    com.Parameters.AddWithValue("param1", groupID);
                    com.Parameters.AddWithValue("param2", friendID);
                    com.Parameters.AddWithValue("param3", false);
                    int result = com.ExecuteNonQuery();
                    if (result != 1)
                        return "something went wrong";

                    return "You have invited him to join your group!";
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
                    if (res < 1)
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
                string check = CheckIfMatchesActive();
                if (check == "Done!")
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
                throw new Exception(check);

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
                    else if (int.Parse(user["Friend_ID"].ToString()) != userID)
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

        public DataTable getUsersInGroupList()
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT * FROM FinalProject_Kasem_UsersInGroupsTB";
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "UsersInGroups");

                if (ds.Tables["UsersInGroups"].Rows.Count != 0)
                    return ds.Tables["UsersInGroups"];
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

        public DataTable getUsersInvitedToGroupTable()
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT * FROM FinalProject_Kasem_UsersInvitedToGroup";
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "UsersInvitedToGroup");

                if (ds.Tables["UsersInvitedToGroup"].Rows.Count != 0)
                    return ds.Tables["UsersInvitedToGroup"];
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

        public DataTable getGroupsTable()
        {
            try
            {
                con.Open();
                com.CommandText = "SELECT * FROM FinalProject_Kasem_GroupsTB";
                adapt = new SqlDataAdapter(com);
                adapt.Fill(ds, "Groups");

                if (ds.Tables["Groups"].Rows.Count != 0)
                    return ds.Tables["Groups"];
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
    }
}