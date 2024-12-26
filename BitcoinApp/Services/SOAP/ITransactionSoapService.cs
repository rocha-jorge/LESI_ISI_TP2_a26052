using CoreWCF;
using BitcoinApp.Models;
using System;

namespace BitcoinApp.Services.SOAP
{
    [ServiceContract]
    public interface ITransactionSoapService
    {
        [OperationContract]
        decimal GetBitcoinBalanceByUserId(int idUser);
    }
}
