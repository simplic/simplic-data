using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Simplic.Configuration;

namespace Simplic.Data.MongoDB
{
    /// <summary>
    /// Mongo context
    /// </summary>
    public class MongoContext : IMongoContext
    {
        private IMongoDatabase database;
        private IConnectionConfigurationService configurationService;
        private readonly List<Func<Task>> commands;
        private IDictionary<string, string> connectionStringCache = new Dictionary<string, string>();

        public MongoContext(IConnectionConfigurationService configurationService)
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;

            // Every command will be stored and it'll be processed at SaveChanges
            commands = new List<Func<Task>>();
            this.configurationService = configurationService;

            SetConfiguration("MongoDB");
        }

        /// <summary>
        /// Initialize context
        /// </summary>
        private void Initialize()
        {
            if (MongoClient != null)
                return;

            string cstr;
            if (connectionStringCache.ContainsKey("MongoDB"))
                cstr = connectionStringCache["MongoDB"];
            else
            {
                cstr = configurationService.GetByName("MongoDB")?.ConnectionString ?? "";
                connectionStringCache["MongoDB"] = cstr;
            }

            MongoClient = new MongoClient(cstr);
            database = MongoClient.GetDatabase("simplic");
        }

        /// <summary>
        /// Dispose context
        /// </summary>
        public void Dispose()
        {
            while (Session != null && Session.IsInTransaction)
                Thread.Sleep(TimeSpan.FromMilliseconds(100));

            GC.SuppressFinalize(this);
        }

        public void AddCommand(Func<Task> func)
        {
            commands.Add(func);
        }

        public async Task<int> SaveChangesAsync()
        {
            Initialize();

            if (EnableTransactions)
            {
                using (Session = await MongoClient.StartSessionAsync())
                {
                    Session.StartTransaction();

                    var commandTasks = commands.Select(c => c());

                    await Task.WhenAll(commandTasks);

                    await Session.CommitTransactionAsync();
                }
            }
            else
            {
                var commandTasks = commands.Select(c => c());
                await Task.WhenAll(commandTasks);
            }

            var count = commands.Count;
            commands.Clear();

            return commands.Count;
        }

        /// <summary>
        /// Gets or sets whether transactions are allowed or not
        /// </summary>
        public bool EnableTransactions { get; private set; } = true;

        /// <summary>
        /// Get mongodb collection
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="name">Collection name</param>
        /// <returns>Collection instance</returns>
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            Initialize();
            return database.GetCollection<T>(name);
        }

        /// <summary>
        /// Set database configuration by name
        /// </summary>
        public void SetConfiguration(string configurationName)
        {
            MongoClient = null;
            Initialize();
        }

        /// <summary>
        /// Gets the mongo client instance
        /// </summary>
        public MongoClient MongoClient { get; private set; }

        /// <summary>
        /// Gets or sets the client session handle
        /// </summary>
        public IClientSessionHandle Session { get; set; }
    }
}
