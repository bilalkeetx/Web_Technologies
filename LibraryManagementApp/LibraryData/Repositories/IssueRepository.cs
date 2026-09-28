using System;
using Microsoft.Data.SqlClient;

namespace LibraryData.Repositories
{
    public class IssueRepository
    {
        private readonly string _connectionString;

        public IssueRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool IssueBook(int bookId, int memberId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string checkStockQuery = "SELECT Stock FROM Books WHERE BookId = @BookId";
                    SqlCommand checkCmd = new SqlCommand(checkStockQuery, conn, transaction);
                    checkCmd.Parameters.AddWithValue("@BookId", bookId);

                    object result = checkCmd.ExecuteScalar();
                    if (result == null || Convert.ToInt32(result) <= 0)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    string issueQuery = "INSERT INTO IssuedBooks (BookId, MemberId, IssueDate, ReturnDate) " +
                                       "VALUES (@BookId, @MemberId, @IssueDate, NULL)";
                    SqlCommand issueCmd = new SqlCommand(issueQuery, conn, transaction);
                    issueCmd.Parameters.AddWithValue("@BookId", bookId);
                    issueCmd.Parameters.AddWithValue("@MemberId", memberId);
                    issueCmd.Parameters.AddWithValue("@IssueDate", DateTime.Now);
                    issueCmd.ExecuteNonQuery();

                    string updateStockQuery = "UPDATE Books SET Stock = Stock - 1 WHERE BookId = @BookId";
                    SqlCommand updateCmd = new SqlCommand(updateStockQuery, conn, transaction);
                    updateCmd.Parameters.AddWithValue("@BookId", bookId);
                    updateCmd.ExecuteNonQuery();

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public bool ReturnBook(int issueId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string checkIssueQuery = "SELECT BookId FROM IssuedBooks WHERE IssueId = @IssueId AND ReturnDate IS NULL";
                    SqlCommand checkCmd = new SqlCommand(checkIssueQuery, conn, transaction);
                    checkCmd.Parameters.AddWithValue("@IssueId", issueId);

                    object result = checkCmd.ExecuteScalar();
                    if (result == null)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    int bookId = Convert.ToInt32(result);

                    string returnQuery = "UPDATE IssuedBooks SET ReturnDate = @ReturnDate WHERE IssueId = @IssueId";
                    SqlCommand returnCmd = new SqlCommand(returnQuery, conn, transaction);
                    returnCmd.Parameters.AddWithValue("@ReturnDate", DateTime.Now);
                    returnCmd.Parameters.AddWithValue("@IssueId", issueId);
                    returnCmd.ExecuteNonQuery();

                    string updateStockQuery = "UPDATE Books SET Stock = Stock + 1 WHERE BookId = @BookId";
                    SqlCommand updateCmd = new SqlCommand(updateStockQuery, conn, transaction);
                    updateCmd.Parameters.AddWithValue("@BookId", bookId);
                    updateCmd.ExecuteNonQuery();

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}