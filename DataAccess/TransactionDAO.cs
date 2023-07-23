using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TransactionDAO
    {
        public static List<Transaction> GetTransactions()
        {
            var listTransactions = new List<Transaction>();
            try
            {
                using (var context = new MyDbContext())
                {
                    listTransactions = context.Transactions
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listTransactions;
        }        
        public static List<Transaction> GetMyTransactions(int userId)
        {
            var listTransactions = new List<Transaction>();
            try
            {
                using (var context = new MyDbContext())
                {
                    listTransactions = context.Transactions
                        .Where(x => x.UserId == userId)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listTransactions;
        }
        public static Transaction FindTransactionById(int prodId)
        {
            Transaction transaction = null;
            try
            {
                using (var context = new MyDbContext())
                {
                    transaction = context.Transactions
                        .SingleOrDefault(x => x.TransactionId == prodId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return transaction;
        }
        public static void SaveTransaction(Transaction transaction)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    transaction.TransactionDate = DateTime.Now;
                    context.Transactions.Add(transaction);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void UpdateTransaction(Transaction transaction)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    context.Entry<Transaction>(transaction).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void DeleteTransaction(Transaction transaction)
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var p1 = context.Transactions.SingleOrDefault(x => x.TransactionId == transaction.TransactionId);
                    context.Transactions.Remove(p1);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
