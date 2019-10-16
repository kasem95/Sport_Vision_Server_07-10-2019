using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Soccer_Vision.Models
{
    public class DAL
    {
        string StrCon = ConfigurationManager.ConnectionStrings["LIVEDNS"].ConnectionString;
        SqlConnection con = null;
        SqlCommand com = null;
        SqlDataReader reader = null;
        public DAL()
        {
            //
            // TODO: Add constructor logic here
            //
            con = new SqlConnection(StrCon);
            com = new SqlCommand();
            com.Connection = con;

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
                return -1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
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
                            IsInMatch = bool.Parse(reader["IsInMatch"].ToString()),
                            MatchID = int.Parse(reader["Match_ID"].ToString()),
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
                    return null;
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

        public string Register(string username, string email, string password)
        {
            if (CheckUser(email, password) == -1)
                return "Something went wrong";
            else if (CheckUser(email, password) == 0)
            {
                try
                {
                    User user = new User
                    {
                        Username = username,
                        Email = email,
                        Password = password
                    };

                    if (user.Email == "" && user.Password == "")
                        return "Invalid email and password";
                    else if (user.Email == "")
                        return "Invalid email";
                    else if (user.Password == "")
                        return "Invalid password";

                    con.Open();
                    com.CommandText = "INSERT INTO FinalProject_Kasem_UsersTB (Username,Email,Password,IsInMatch,Match_ID) VALUES (@param1,@param2,@param3,@param4,@param5)";
                    com.Parameters.AddWithValue("param1", user.Username);
                    com.Parameters.AddWithValue("param2", user.Email);
                    com.Parameters.AddWithValue("param3", user.Password);
                    com.Parameters.AddWithValue("param4", false);
                    com.Parameters.AddWithValue("param5", -1);
                    int result = com.ExecuteNonQuery();
                    if (result != 1)
                        throw new Exception("Something went wrong");

                    return "Registration complete";
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    return "Something went wrong";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return "Something went wrong";
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
                return "User already exists";

        }

        public string CreateMatch(int userID, string matchName, DateTime matchDate, TimeSpan matchTime, int field, int city, bool isPrivate, string matchKey,int maxPlayers)
        {
            int check = checkIfInMatch(userID);
            if (check == 1)
                return "You can't create a match because you are already in match!";
            else if (check == -1)
                return "Something went wrong";
            else
            {
                try
                {
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
                        FieldID = field
                    };

                    con.Open();
                    com.CommandText = "INSERT INTO FinalProject_Kasem_MatchesTB (Match_Name," +
                        "Admin_ID,Field_ID,City_ID,IsActive,Players_Joined,Max_Players,IsPrivate,Match_Key,Match_Date,Match_Time) VALUES " +
                        "(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10,@param11)";
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
                    int result = com.ExecuteNonQuery();
                    if (result == 0)
                        return "Something went wrong";

                    com.CommandText = $"SELECT Match_ID FROM FinalProject_Kasem_MatchesTB WHERE Admin_ID = {userID}";
                    reader = com.ExecuteReader();
                    int matchID = 0;
                    if (reader.Read())
                    {
                        matchID = int.Parse(reader["Match_ID"].ToString());
                    }
                    reader.Close();

                    com.CommandText = $"UPDATE FinalProject_Kasem_UsersTB SET [IsInMatch] = {1},[Match_ID] = {matchID} WHERE User_ID = {userID}";
                    result = com.ExecuteNonQuery();
                    if (result == 0)
                    {
                        return "Something went wrong";
                    }
                    match.MatchID = matchID;

                    return "Match successfully created";
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                    return "Something went wrong";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return "Something went wrong";
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

        public int checkIfInMatch(int userID)
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT IsInMatch FROM FinalProject_Kasem_UsersTB WHERE User_ID = {userID}";
                reader = com.ExecuteReader();
                if(reader.Read())
                {
                    bool result = bool.Parse(reader["IsInMatch"].ToString());
                    reader.Close();
                    if (result == true)
                        return 1;
                    else
                        return 0;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
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

        public string JoinMatch(int userID, int matchID)
        {
            if (checkIfInMatch(userID) == 1)
                return "You can't create a match because you are already in match!";
            else if (checkIfInMatch(userID) == -1)
                return "Something went wrong!";
            else
            {
                if (CheckIfMatchIsActive(matchID) == 0)
                    return "Match is not active";
                else if (CheckIfMatchIsActive(matchID) == -1)
                    return "Something went wrong";
                else
                {
                    try
                    {
                        con.Open();
                        com.CommandText = $"UPDATE FinalProject_Kasem_UsersTB SET [IsInMatch] = {1},[Match_ID] = {matchID} WHERE User_ID = {userID}";
                        int result = com.ExecuteNonQuery();
                        if (result != 1)
                            return "something went wrong";
                        com.CommandText = $"UPDATE FinalProject_Kasem_MatchesTB SET Players_Joined = (Players_Joined + 1)";
                        result = com.ExecuteNonQuery();
                        if (result != 1)
                            return "something went wrong";
                        return "You have joiined the match";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return "something went wrong";
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

        public int CheckIfMatchIsActive(int matchID)
        {
            try
            {
                con.Open();
                com.CommandText = $"SELECT IsActive FROM FinalProject_Kasem_MatchesTB WHERE Match_ID = {matchID}";
                reader = com.ExecuteReader();
                if (reader.Read())
                {
                    bool res = bool.Parse(reader["IsActive"].ToString());
                    if (res)
                    {
                        reader.Close();
                        return 1;
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
                return -1;
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