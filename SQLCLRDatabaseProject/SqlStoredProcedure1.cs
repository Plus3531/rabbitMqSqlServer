using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    private static readonly HelloPj HelloPj = new HelloPj();
    [SqlProcedure]
    public static void SqlStoredProcedure1 (SqlString inputString)
    {
        HelloPj.PublishMessage(inputString.Value);
       
    }
}
