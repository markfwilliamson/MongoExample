using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
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

        public MongoController(IMongoRepository<Entity.Book> bookRepository, 
                                IMongoRepository<Entity.BookActivity> bookActivityRepository)
        {
            _bookRepository = bookRepository;
            _bookActivityRepository = bookActivityRepository;

            #region Old Code
            //string connectionString = "mongodb://admin:MyPassword@87.15.22.12:27017/admin?connect=replicaSet&replicaSet=rs0";
            //var mongoUrl = new MongoUrlBuilder(connectionString);
            //var settings = new MongoClientSettings
            //{
            //    Server = new MongoServerAddress(mongoUrl.Server.Host, mongoUrl.Server.Port),
            //    Credential = MongoCredential.CreateCredential(mongoUrl.DatabaseName, mongoUrl.Username, mongoUrl.Password)
            //};
            //_mongoClient = new MongoClient(settings);
            #endregion
        }

        [HttpGet("Transaction")]
        public ActionResult Transaction()
        {
            using (var session = _bookRepository.MongoClient.StartSession())
            {
                session.StartTransaction();
                try
                {
                    _bookRepository.InsertOne(session, new Book { Author = "Transaction1", Title = "Transaction1", Id = new ObjectId() });
                    _bookRepository.InsertOne(session, new Book { Author = "Transaction2", Title = "Transaction2", Id = new ObjectId() });
                    _bookRepository.InsertOne(session, new Book { Author = "Transaction3", Title = "Transaction3", Id = new ObjectId() });
                    _bookRepository.InsertOne(session, new Book { Author = "Transaction4", Title = "Transaction4", Id = new ObjectId() });
                    _bookRepository.InsertOne(session, new Book { Author = "Transaction5", Title = "Transaction5", Id = new ObjectId() });

                    session.CommitTransaction();
                    
                    return Ok();
                }
                catch (Exception ex)
                {
                    session.AbortTransaction();

                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("Aborted")]
        public ActionResult Aborted()
        {
            using (var session = _bookRepository.MongoClient.StartSession())
            {
                session.StartTransaction();

                _bookRepository.InsertOne(session, new Book { Author = "Aborted1", Title = "Aborted1", Id = new ObjectId() });
                _bookRepository.InsertOne(session, new Book { Author = "Aborted2", Title = "Aborted2", Id = new ObjectId() });
                _bookRepository.InsertOne(session, new Book { Author = "Aborted3", Title = "Aborted3", Id = new ObjectId() });
                _bookRepository.InsertOne(session, new Book { Author = "Aborted4", Title = "Aborted4", Id = new ObjectId() });
                _bookRepository.InsertOne(session, new Book { Author = "Aborted5", Title = "Aborted5", Id = new ObjectId() });

                session.AbortTransaction();

                return BadRequest("Abort Transaction");
            }
        }

        [HttpGet("TransactionAsync")]
        public async Task<ActionResult> TransactionAsync()
        {
            using (var session = await _bookRepository.MongoClient.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    await _bookRepository.InsertOneAsync(session, new Book { Author = "TransactionAsync1", Title = "TransactionAsync1", Id = new ObjectId() });
                    await _bookRepository.InsertOneAsync(session, new Book { Author = "TransactionAsync2", Title = "TransactionAsync2", Id = new ObjectId() });
                    await _bookRepository.InsertOneAsync(session, new Book { Author = "TransactionAsync3", Title = "TransactionAsync3", Id = new ObjectId() });
                    await _bookRepository.InsertOneAsync(session, new Book { Author = "TransactionAsync4", Title = "TransactionAsync4", Id = new ObjectId() });
                    await _bookRepository.InsertOneAsync(session, new Book { Author = "TransactionAsync5", Title = "TransactionAsync5", Id = new ObjectId() });

                    await session.CommitTransactionAsync();

                    return Ok();
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();

                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet("AbortedAsync")]
        public async Task<ActionResult> AbortedAsync()
        {
            using (var session = await _bookRepository.MongoClient.StartSessionAsync())
            {
                session.StartTransaction();

                await _bookRepository.InsertOneAsync(session, new Book { Author = "AbortedAsync1", Title = "AbortedAsync1", Id = new ObjectId() });
                await _bookRepository.InsertOneAsync(session, new Book { Author = "AbortedAsync2", Title = "AbortedAsync2", Id = new ObjectId() });
                await _bookRepository.InsertOneAsync(session, new Book { Author = "AbortedAsync3", Title = "AbortedAsync3", Id = new ObjectId() });
                await _bookRepository.InsertOneAsync(session, new Book { Author = "AbortedAsync4", Title = "AbortedAsync4", Id = new ObjectId() });
                await _bookRepository.InsertOneAsync(session, new Book { Author = "AbortedAsync5", Title = "AbortedAsync5", Id = new ObjectId() });

                await session.AbortTransactionAsync();

                return BadRequest("Abort TransactionAsync");
            }
        }
    }
}
