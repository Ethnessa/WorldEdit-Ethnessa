using TShockAPI;
using TShockAPI.Database;

namespace WorldEdit.Commands
{
	public class Redo : WECommand
	{
		private ServerPlayer plr;
		private int steps;

		public Redo(ServerPlayer plr, int steps)
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
			while (++i < steps && Tools.Redo(plr)) ;
			if (i == 0)
				plr.SendErrorMessage("Failed to redo any actions.");
			else
				plr.SendSuccessMessage("Redid {0}'s last {1}action{2}.", ((plr.Account.AccountId == 0) ? "ServerConsole" : plr.Name), i == 1 ? "" : i + " ", i == 1 ? "" : "s");
		}
	}
}
