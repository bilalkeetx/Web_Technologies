using LibraryData.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData.Repositories
{
    public class MemberRepository
    {
        private readonly string _connectionString;

        public MemberRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddMember(Member member)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Members (Name) VALUES (@Name)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", member.Name);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Member> GetAllMembers()
        {
            List<Member> members = new List<Member>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT MemberId, Name FROM Members";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        members.Add(new Member
                        {
                            MemberId = Convert.ToInt32(reader["MemberId"]),
                            Name = reader["Name"].ToString()
                        });
                    }
                }
            }
            return members;
        }
    }
}

