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
        private int _numberOfTimesToExecute = 5;
        private int _numberOfDocumentsToInsert = 100;

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
            try
            {
                for (int a = 0; a < _numberOfTimesToExecute; a++)
                {
                    _clientSessionHandle.StartTransaction();
                    for (int i = 0; i < _numberOfDocumentsToInsert; i++)
                    {
                        _bookRepository.InsertOne(_clientSessionHandle, new Book { Author = $"Transaction{i}", Title = $"Transaction{i}", Id = new ObjectId() });
                        _bookActivityRepository.InsertOne(_clientSessionHandle, new BookActivity { Message = $"TransactionMessage{i}", Id = new ObjectId() });
                    }
                    _clientSessionHandle.CommitTransaction();
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
            for (int a = 0; a < _numberOfTimesToExecute; a++)
            {
                _clientSessionHandle.StartTransaction();
                for (int i = 0; i < _numberOfDocumentsToInsert; i++)
                {
                    _bookRepository.InsertOne(_clientSessionHandle, new Book { Author = $"Aborted{i}", Title = $"Aborted{i}", Id = new ObjectId() });
                    _bookActivityRepository.InsertOne(_clientSessionHandle, new BookActivity { Message = $"AbortedMessage{i}", Id = new ObjectId() });
                }
                _clientSessionHandle.AbortTransaction();
            }
            return BadRequest($"Abort Transaction");
        }

        [HttpGet("TransactionAsync")]
        public async Task<ActionResult> TransactionAsync()
        {
            try
            {
                for (int a = 0; a < _numberOfTimesToExecute; a++)
                {
                    _clientSessionHandle.StartTransaction();
                    for (int i = 0; i < _numberOfDocumentsToInsert; i++)
                    {
                        await _bookRepository.InsertOneAsync(_clientSessionHandle, new Book { Author = $"Transaction{i}", Title = $"Transaction{i}", Id = new ObjectId() });
                        await _bookActivityRepository.InsertOneAsync(_clientSessionHandle, new BookActivity { Message = $"TransactionMessage{i}", Id = new ObjectId() });
                    }
                    await _clientSessionHandle.CommitTransactionAsync();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                await _clientSessionHandle.AbortTransactionAsync();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("AbortedAsync")]
        public async Task<ActionResult> AbortedAsync()
        {
            for (int a = 0; a < _numberOfTimesToExecute; a++)
            {
                _clientSessionHandle.StartTransaction();
                for (int i = 0; i < _numberOfDocumentsToInsert; i++)
                {
                    await _bookRepository.InsertOneAsync(_clientSessionHandle, new Book { Author = $"Aborted{i}", Title = $"Aborted{i}", Id = new ObjectId() });
                    await _bookActivityRepository.InsertOneAsync(_clientSessionHandle, new BookActivity { Message = $"AbortedMessage{i}", Id = new ObjectId() });
                }
                await _clientSessionHandle.AbortTransactionAsync();
            }
            return BadRequest($"Abort Transaction");
        }
    }
}
