using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using Microsoft.SqlServer.Server;
using RabbitMQ.Client;

public partial class UserDefinedFunctions
{


    internal static readonly HelloPj _HelloPj = new HelloPj();

    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static SqlInt32 GetVowelsCount(SqlString inputString)
    {
        // list of vowels to look for 
        const string vowels = "aeiou";
        var countOfVowels = 0;
        // for each character in the given parameter 
        // for (int i = 0; i < inputString.ToString().Length; i++)
        for (int i = 0; i < _HelloPj.Name.Length; i++)
        {
            // for each character in the vowels string 
            for (int j = 0; j < vowels.Length; j++)
            {
                // convert parameter character to lowercase and compare to vowel 
                if (inputString.Value.Substring(i, 1).ToLower() == vowels.Substring(j, 1))
                {
                    // it is a vowel, increment the count
                    countOfVowels += 1;
                }
            }
        }
        return countOfVowels;
    }
}

internal class HelloPj
{
    private IConnection _rabbitConn;
    private IModel _model;
    private ConnectionFactory _connFactory;
    public string Name { get; set; }
    private RemoteEndpoint Endpoint { get; set; }
    public HelloPj()
    {
       Initialize();
    }

    private void Connect()
    {
        _connFactory = new ConnectionFactory
        {
            Uri = Endpoint.ConnectionString,
            AutomaticRecoveryEnabled = true,
            TopologyRecoveryEnabled = true
        };
        _rabbitConn = _connFactory.CreateConnection();
        _model = _rabbitConn.CreateModel();

    }

    public void PublishMessage(string msgToPost)
    {
        var msg = Encoding.UTF8.GetBytes(msgToPost);
        _model.BasicPublish(Endpoint.Exchange, Endpoint.RoutingKey, false, null, msg);
    }

    private void Initialize()
    {
        Name = "Paulus Janssen";
        using (var conn = new SqlConnection("Context Connection = true"))
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "rmq.pr_GetRabbitEndpoints";
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Endpoint = new RemoteEndpoint(reader);
                //only interested in 1 endpoint
                break;
            }
            reader.Close();
        }       
        Connect(); 
    }

    private class RemoteEndpoint
    {
        private int EndpointId { get; set; }
        private string AliasName { get; set; }
        private string ServerName { get; set; }
        private int Port { get; set; }
        private string VHost { get; set; }
        private string LoginName { get; set; }
        private string LoginPassword { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        private int ConnectionChannels { get; set; }
        private bool IsEnabled { get; set; }

        public string ConnectionString
        {
            get { return CreateConnectionString(); }
        }


        internal RemoteEndpoint()
        {

        }

        internal RemoteEndpoint(IDataRecord dr)
        {

            EndpointId = dr.GetInt32(0);
            AliasName = dr.GetString(1);
            ServerName = dr.GetString(2);
            Port = dr.GetInt32(3);
            VHost = dr.GetString(4);
            LoginName = dr.GetString(5);
            LoginPassword = dr.GetString(6);
            Exchange = dr.GetString(7);
            RoutingKey = dr[8] != DBNull.Value ? dr.GetString(8) : null;
            ConnectionChannels = dr.GetInt32(9);
            IsEnabled = dr.GetBoolean(10);

        }

        private string CreateConnectionString()
        {

            //"amqp://rabbitAdmin:rabbitAdminPwd@fpnieberg2/operator6000";
            var connBuilder = new StringBuilder();
            connBuilder.Append("amqp://");
            connBuilder.Append(LoginName);
            connBuilder.Append(":");
            connBuilder.Append(LoginPassword);
            connBuilder.Append("@");
            connBuilder.Append(ServerName);
            connBuilder.Append(":");
            connBuilder.Append(Port.ToString());
            connBuilder.Append("/");
            connBuilder.Append(VHost);

            return connBuilder.ToString();

        }

    }
}