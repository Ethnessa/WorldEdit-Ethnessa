using TShockAPI;

namespace WorldEdit.Extensions
{
	public static class ServerPlayerExtensions
	{
		public static PlayerInfo GetPlayerInfo(this ServerPlayer ServerPlayer)
		{
			if (!ServerPlayer.ContainsData(PlayerInfo.Key))
				ServerPlayer.SetData(PlayerInfo.Key, new PlayerInfo());

			return ServerPlayer.GetData<PlayerInfo>(PlayerInfo.Key);
		}
		
		public static DB.WorldEdit? GetDBInfo(this ServerPlayer ServerPlayer)
		{
			return DB.DB.InsertIfNotExists(ServerPlayer);
		}
	}
}
