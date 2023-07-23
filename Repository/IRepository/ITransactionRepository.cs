using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface ITransactionRepository
    {
        void SaveTransaction(Transaction transaction);
        Transaction GetTransactionById(int id);
        void DeleteTransaction(Transaction transaction);
        void UpdateTransaction(Transaction transaction);
        List<Transaction> GetTransactions();
        List<Transaction> GetMyTransactions(int userId);
    }
}
