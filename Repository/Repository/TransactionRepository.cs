using BusinessObjects;
using DataAccess;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        public void SaveTransaction(Transaction transaction) => TransactionDAO.SaveTransaction(transaction);
        public void UpdateTransaction(Transaction transaction) => TransactionDAO.UpdateTransaction(transaction);
        public List<Transaction> GetTransactions() => TransactionDAO.GetTransactions();
        public Transaction GetTransactionById(int id) => TransactionDAO.FindTransactionById(id);

        public void DeleteTransaction(Transaction transaction) => TransactionDAO.DeleteTransaction(transaction);
    }
}
