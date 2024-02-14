using MongoDB.Bson;
using MongoDB.Driver;
using TShockAPI;

namespace WorldEdit.DB;

public static class DB
{
    internal static string CollectionName => "WorldEdit";

    private static IMongoCollection<WorldEdit> _collection =
        global::WorldEdit.WorldEdit.Database.GetCollection<WorldEdit>(CollectionName);

    public static WorldEdit? InsertIfNotExists(ServerPlayer plr)
    {
        if (plr.IsLoggedIn is false)
        {
            return null;
        }
        
        var data = FindOne(plr.Account.AccountId);
        if (data is null)
        {
            return Insert(new WorldEdit { AccountId = plr.Account.AccountId, UndoLevel = 0, RedoLevel = 0 });
        }
        else
        {
            return data;
        }
    }

    public static void SetUndoLevel(ServerPlayer plr, int level)
    {
        var we = InsertIfNotExists(plr);
        if (we != null)
        {
            we.UndoLevel = level;
            Update(we);
        }
    }
    
    public static void SetRedoLevel(ServerPlayer plr, int level)
    {
        var we = InsertIfNotExists(plr);
        if (we != null)
        {
            we.RedoLevel = level;
            Update(we);
        }
    }

    public static WorldEdit Insert(WorldEdit we)
    {
        _collection.InsertOne(we);
        return we;
    }

    public static void Update(WorldEdit we)
    {
        _collection.ReplaceOne<WorldEdit>(x => x.AccountId == we.AccountId, we);
    }
    
    public static void Delete(int accId) => _collection.DeleteOne<WorldEdit>(x=>x.AccountId==accId);
    public static WorldEdit? FindOne(int accId) => _collection.Find(x=>x.AccountId==accId).FirstOrDefault();
}

public class WorldEdit
{
    private ObjectId Id { get; set; }
    public int AccountId { get; set; }
    public int UndoLevel { get; set; }
    public int RedoLevel { get; set; }
}