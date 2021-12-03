using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpGet("NewTransaction")]
        public ActionResult NewTransaction()
        {
            _clientSessionHandle.StartTransaction();
            try
            {
                _bookRepository.InsertOne(_clientSessionHandle, new Book { Author = "NewTransaction1", Title = "NewTransaction1", Id = new ObjectId() });
                _bookRepository.InsertOne(_clientSessionHandle, new Book { Author = "NewTransaction2", Title = "NewTransaction2", Id = new ObjectId() });
                _bookRepository.InsertOne(_clientSessionHandle, new Book { Author = "NewTransaction3", Title = "NewTransaction3", Id = new ObjectId() });
                _bookRepository.InsertOne(_clientSessionHandle, new Book { Author = "NewTransaction4", Title = "NewTransaction4", Id = new ObjectId() });
                _bookRepository.InsertOne(_clientSessionHandle, new Book { Author = "NewTransaction5", Title = "NewTransaction5", Id = new ObjectId() });
                _bookActivityRepository.InsertOne(_clientSessionHandle, new BookActivity { Message = "NewTransactionMessage1", Id = new ObjectId() });
                _bookActivityRepository.InsertOne(_clientSessionHandle, new BookActivity { Message = "NewTransactionMessage2", Id = new ObjectId() });
                _bookActivityRepository.InsertOne(_clientSessionHandle, new BookActivity { Message = "NewTransactionMessage3", Id = new ObjectId() });
                _bookActivityRepository.InsertOne(_clientSessionHandle, new BookActivity { Message = "NewTransactionMessage4", Id = new ObjectId() });
                _bookActivityRepository.InsertOne(_clientSessionHandle, new BookActivity { Message = "NewTransactionMessage5", Id = new ObjectId() });
                _clientSessionHandle.CommitTransaction();

                return Ok();
            }
            catch (Exception ex)
            {
                _clientSessionHandle.AbortTransaction();

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("NewAborted")]
        public ActionResult NewAborted()
        {
            _clientSessionHandle.StartTransaction();

            _bookRepository.InsertOne(_clientSessionHandle, new Book { Author = "NewAborted1", Title = "NewAborted1", Id = new ObjectId() });
            _bookRepository.InsertOne(_clientSessionHandle, new Book { Author = "NewAborted2", Title = "NewAborted2", Id = new ObjectId() });
            _bookRepository.InsertOne(_clientSessionHandle, new Book { Author = "NewAborted3", Title = "NewAborted3", Id = new ObjectId() });
            _bookRepository.InsertOne(_clientSessionHandle, new Book { Author = "NewAborted4", Title = "NewAborted4", Id = new ObjectId() });
            _bookRepository.InsertOne(_clientSessionHandle, new Book { Author = "NewAborted5", Title = "NewAborted5", Id = new ObjectId() });
            _bookActivityRepository.InsertOne(_clientSessionHandle, new BookActivity { Message = "NewAbortedMessage1", Id = new ObjectId() });
            _bookActivityRepository.InsertOne(_clientSessionHandle, new BookActivity { Message = "NewAbortedMessage2", Id = new ObjectId() });
            _bookActivityRepository.InsertOne(_clientSessionHandle, new BookActivity { Message = "NewAbortedMessage3", Id = new ObjectId() });
            _bookActivityRepository.InsertOne(_clientSessionHandle, new BookActivity { Message = "NewAbortedMessage4", Id = new ObjectId() });
            _bookActivityRepository.InsertOne(_clientSessionHandle, new BookActivity { Message = "NewAbortedMessage5", Id = new ObjectId() });

            _clientSessionHandle.AbortTransaction();

            return BadRequest("Abort Transaction");
        }
    }
}
