using TShockAPI;
using TShockAPI.Database;

namespace WorldEdit.Commands
{
	public class Undo : WECommand
	{
		private ServerPlayer plr;
		private int steps;

		public Undo(ServerPlayer plr, int steps)
			: base(0, 0, 0, 0, plr)
		{
			this.plr = plr;
			this.steps = steps;
		}

		public override void Execute()
		{
            if (WorldEdit.Config.DisableUndoSystemForUnrealPlayers
                && (!plr.RealPlayer || (plr.Account.AccountId == 0)))
            {
                plr.SendErrorMessage("Undo system is disabled for unreal players.");
                return;
            }

			int i = -1;
			while (++i < steps && Tools.Undo(plr)) ;
			if (i == 0)
				plr.SendErrorMessage("Failed to undo any actions.");
			else
				plr.SendSuccessMessage("Undid {0}'s last {1}action{2}.", ((plr.Account.AccountId == 0) ? "ServerConsole" : UserAccountManager.GetUserAccountById(plr.Account.AccountId).Name), i == 1 ? "" : i + " ", i == 1 ? "" : "s");
		}
	}
}