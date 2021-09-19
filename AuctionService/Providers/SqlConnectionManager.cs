using System;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace AuctionService.Providers
{
    public class SqlConnectionManagerBeta
    {
        //internal string _dbName;
        private readonly int _poolCount;
        private readonly int _idleTimeAllowed;
        private readonly string _connectionString;
        private readonly ConcurrentQueue<(DateTimeOffset time, SqlConnection cn)> conList = new ConcurrentQueue<(DateTimeOffset time, SqlConnection cn)>();
        private readonly Timer timer;
        private readonly object lockObj = new object();

        public SqlConnectionManagerBeta(string connectionString, int poolCount, int purgeDuration, int idleTimeAllowed)
        {
            // _dbName = dbName;
            _poolCount = poolCount;
            _idleTimeAllowed = idleTimeAllowed;
            _connectionString = connectionString;
            InitializeConnections(connectionString);
            timer = new Timer(TimerHandler, null, 0, purgeDuration);
        }

        private void InitializeConnections(string connectionString)
        {
            //string svrName = WstConfig.CmTranLicenseDbAndSvrs.Single((x) => x.Key == _dbName).Value;
            for (int count = 0; count < _poolCount; count++)
            {
                SqlConnection con = new SqlConnection(connectionString);// new SqlConnection($"Data Source={svrName};Initial Catalog={_dbName};Integrated Security=SSPI;");
                con.Open();
                conList.Enqueue((DateTimeOffset.Now, con));
            }
        }

        private void EnqueueNewConnection()
        {
            SqlConnection con = new SqlConnection(_connectionString);// new SqlConnection($"Data Source={svrName};Initial Catalog={_dbName};Integrated Security=SSPI;");
            con.Open();
            conList.Enqueue((DateTimeOffset.Now, con));
        }

        private void TimerHandler(object state)
        {
            lock (timer)
            {
                while (conList.TryPeek(out var result))
                {
                    if (DateTimeOffset.Now.Subtract(result.time).TotalMilliseconds > _idleTimeAllowed)
                    {
                        if (conList.TryDequeue(out var leastUsedCon))
                        {
                            if (DateTimeOffset.Now.Subtract(leastUsedCon.time).TotalMilliseconds > _idleTimeAllowed)
                            {
                                leastUsedCon.cn.Close();
                                leastUsedCon.cn.DisposeAsync();
                            }
                            else
                            {
                                EnqueueConnection(leastUsedCon.cn);
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }

            }
        }
        public SqlConnection GetAvailableCon()
        {
            (DateTimeOffset timeStamp, SqlConnection connection) con = (DateTimeOffset.Now, null);//Tuple.Create(DateTimeOffset.Now, null);)
            for (int tryCount = 0; tryCount < 9999; tryCount++)
            {
                if (conList.TryDequeue(out con))
                    break;
                if (conList.Count < _poolCount)
                {
                    lock (lockObj)
                    {
                        if (conList.Count < _poolCount)
                            EnqueueNewConnection();
                    }
                }
                else
                {
                    Task.Delay(100).Wait();
                }
            }
            if (con.connection == null)
                throw new Exception("Could not get available connection");
            return con.connection;
        }

        public void EnqueueConnection(SqlConnection con)
        {
            if (con != null)
                conList.Enqueue((DateTimeOffset.Now, con));
        }
    }
}
