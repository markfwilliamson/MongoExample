using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApi.Entity;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MongoController : ControllerBase
    {
        private readonly IMongoRepository<Entity.Book> _bookRepository;
        private readonly IMongoRepository<BookActivity> _bookActivityRepository;
        private readonly IClientSessionHandle _clientSessionHandle;

        public MongoController(IMongoRepository<Entity.Book> bookRepository,
                                IMongoRepository<Entity.BookActivity> bookActivityRepository,
                                IClientSessionHandle clientSessionHandle)
        {
            _bookRepository = bookRepository;
            _bookActivityRepository = bookActivityRepository;
            _clientSessionHandle = clientSessionHandle;
        }

        [HttpGet("Transaction")]
        public ActionResult Transaction()
        {
            var sleepTimeInMilliseconds = 20;
            var options = new TransactionOptions(ReadConcern.Snapshot, ReadPreference.Primary, WriteConcern.WMajority);
            try
            {
                for (int a = 0; a < 100; a++)
                {
                    _clientSessionHandle.StartTransaction(options);
                    for (int i = 0; i < 10; i++)
                    {
                        _bookRepository.InsertOne(_clientSessionHandle, new Book { Author = $"NewTransaction{i}", Title = $"NewTransaction{i}", Id = new ObjectId() });
                        System.Threading.Thread.Sleep(sleepTimeInMilliseconds);
                        _bookActivityRepository.InsertOne(_clientSessionHandle, new BookActivity { Message = $"NewTransactionMessage{i}", Id = new ObjectId() });
                        System.Threading.Thread.Sleep(sleepTimeInMilliseconds);
                    }
                    _clientSessionHandle.CommitTransaction(CancellationToken.None);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _clientSessionHandle.AbortTransaction();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Aborted")]
        public ActionResult Aborted()
        {
            int counter = 0;
            var sleepTimeInMilliseconds = 20;

            for (int a = 0; a < 100; a++)
            {
                //var options = new TransactionOptions(ReadConcern.Snapshot, ReadPreference.Primary, WriteConcern.WMajority);
                _clientSessionHandle.StartTransaction();
                for (int i = 0; i < 10; i++)
                {
                    _bookRepository.InsertOne(_clientSessionHandle, new Book { Author = $"NewAborted{i}", Title = $"NewAborted{i}", Id = new ObjectId() });
                    System.Threading.Thread.Sleep(sleepTimeInMilliseconds);
                    _bookActivityRepository.InsertOne(_clientSessionHandle, new BookActivity { Message = $"NewAbortedMessage{i}", Id = new ObjectId() });
                    System.Threading.Thread.Sleep(sleepTimeInMilliseconds);

                    counter = counter + 1;
                }
                _clientSessionHandle.AbortTransaction();
            }
            return BadRequest($"Abort Transaction {counter}");
        }
    }
}
