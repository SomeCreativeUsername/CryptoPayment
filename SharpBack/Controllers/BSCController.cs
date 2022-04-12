using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nethereum.Web3;
using Nethereum.Geth;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3.Accounts;
using System.Net.Http;
using WebApplication2.Models;
using Newtonsoft.Json;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BSCController : ControllerBase
    {
        private readonly ILogger<BSCController> _logger;

        public BSCController(ILogger<BSCController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("wallet")]
        public async Task<string> GetWallet()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync("http://localhost:5000/api/createaccount", null);
                string result = response.Content.ReadAsStringAsync().Result;
                Wallet wallet = JsonConvert.DeserializeObject<Wallet>(result);
                using (var market = new MarkerContext())
                {
                    market.Wallets.Add(wallet);
                    await market.SaveChangesAsync();
                }
                return (wallet.Address);
            }
        }

        [HttpGet]
        [Route("estimateAmount")]
        public async Task<string> GetFullAmount([FromQuery] string amount, [FromQuery] string addressFrom, [FromQuery] string addressTo)
        {
            Wallet currentWallet = new Wallet();
            using (var market = new MarkerContext())
            {
                currentWallet = market.Wallets.FirstOrDefault(x => x.Address == addressFrom);

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"http://localhost:5000/api/commission/?amountTo={amount}&addressTo={addressTo}");
                    string result = response.Content.ReadAsStringAsync().Result;
                    Amount fullAmount = JsonConvert.DeserializeObject<Amount>(result);

                    currentWallet.AmountGas = fullAmount.AmountGas.Replace('.', ',');
                    currentWallet.ComissionGas = fullAmount.ComissionGas.Replace('.', ',');
                    currentWallet.InputAmount = fullAmount.InputAmount.Replace('.', ',');
                    currentWallet.Comission = fullAmount.Comission.Replace('.', ',');


                    //Wallet wallet = JsonConvert.DeserializeObject<Wallet>(result);
                    //using (var market = new MarkerContext())
                    //{
                    //    market.Wallets.Add(wallet);
                    //    await market.SaveChangesAsync();
                    //}
                    var res = Convert.ToDouble(currentWallet.InputAmount) + Convert.ToDouble(currentWallet.ComissionGas) + Convert.ToDouble(currentWallet.AmountGas) + Convert.ToDouble(currentWallet.Comission);

                    return (res.ToString());
                }
            }
        }

        [HttpGet]
        [Route("createTransaction")]
        public async Task<string> GetTransaction([FromQuery] string addressTo, [FromQuery] string addressFrom, [FromQuery] string amountTo, [FromQuery] string amountGas)
        {
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();
            //var web3 = new Web3("https://bsc.getblock.io/testnet/?api_key=741b5618-8fcb-437f-b5b5-a0a2a9bed9e5");
            //var web3 = new Web3("https://speedy-nodes-nyc.moralis.io/72470af655156fb776f17fd4/bsc/testnet");
            //var account = await web3.Personal.NewAccount.SendRequestAsync("pienusInvestigation");
            //var acc = await web3.Eth.Accounts.SendRequestAsync("create");
            //var balance = await web3.Eth.GetBalance.SendRequestAsync("0x12c5ba00E3C0634D11a7f980b17514080443B885");
            //var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            //var privateKey = ecKey.GetPrivateKeyAsBytes().ToHex();
            //var account = new Account(privateKey);
            //var acc = await web3.Personal.NewAccount.SendRequestAsync("GregorPidor");
            using (var httpClient = new HttpClient())
            {
                //var response = await httpClient.PostAsync("http://localhost:5000/api/createaccount", null);
                //string result = response.Content.ReadAsStringAsync().Result;
                //Wallet wallet = JsonConvert.DeserializeObject<Wallet>(result);
                Wallet currentWallet = new Wallet();
                using (var market = new MarkerContext())
                {
                    currentWallet = market.Wallets.FirstOrDefault(x => x.Address == addressFrom);


                    if (currentWallet == null)
                    {
                        return "Wallet not found";
                    }

                    string temp = $"http://localhost:5000/api/createtransaction/?privateKeyFrom={currentWallet.Address}&addressTo=0x24CC520B508425C1960daB7bb4F68106ba754C1c";
                    var responseMessage = await httpClient.PostAsync($"http://localhost:5000/api/createtransaction/?privateKeyFrom={currentWallet.PrivateKey}&addressTo={addressTo}&amountTo={amountTo}&gas={amountGas}", null); //addressTo=0x24CC520B508425C1960daB7bb4F68106ba754C1c
                                                                                                                                                                                          //($"http://localhost:5000/api/createtransaction/?privateKeyFrom=12caf9cc4395f278982709578cadd6907a43bb39da883ba54969f949fbb60c3f&addressTo=0x24CC520B508425C1960daB7bb4F68106ba754C1c", null);
                    string result2 = responseMessage.Content.ReadAsStringAsync().Result;
                    Transaction transaction = JsonConvert.DeserializeObject<Transaction>(result2);
                    currentWallet.TransactionHash = transaction.Hash;
                    market.Wallets.Update(currentWallet);

                    //var account = new Account("12caf9cc4395f278982709578cadd6907a43bb39da883ba54969f949fbb60c3f");//(wallet.PrivateKey);
                    ////var web3 = new Web3("https://bsc.getblock.io/testnet/?api_key=741b5618-8fcb-437f-b5b5-a0a2a9bed9e5", null, );
                    //var web3 = new Web3(account);

                    //var toAddress = "0x24CC520B508425C1960daB7bb4F68106ba754C1c";
                    //var transaction = await web3.Eth.GetEtherTransferService()
                    //                                .TransferEtherAndWaitForReceiptAsync(toAddress, 0.1m, 0.5m);
                    await market.SaveChangesAsync();
                }

            }

            return "TransactionCreated";
        }

        //[HttpGet]
        //public string GetSession()
        //{

        //}
    }
}
