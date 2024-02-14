using System.IO;
using Terraria;
using TShockAPI;
using WorldEdit.DB;
using WorldEdit.Extensions;

namespace WorldEdit.Commands
{
	public class Cut : WECommand
	{
		public Cut(int x, int y, int x2, int y2, ServerPlayer plr)
			: base(x, y, x2, y2, plr)
		{
		}

		public override void Execute()
		{
			foreach (string fileName in Directory.EnumerateFiles("worldedit", string.Format("redo-{0}-{1}-*.dat", Main.worldID, plr.Account.AccountId)))
				File.Delete(fileName);

			var dbData = plr.GetDBInfo();

			var undoLevel = dbData.UndoLevel + 1;
			DB.DB.SetRedoLevel(plr, -1);
			DB.DB.SetUndoLevel(plr, undoLevel);
			
			
			string clipboard = Tools.GetClipboardPath(plr.Account.AccountId);

			string undoPath = Path.Combine("worldedit", string.Format("undo-{0}-{1}-{2}.dat", Main.worldID, plr.Account.AccountId, undoLevel));

			Tools.SaveWorldSection(x, y, x2, y2, undoPath);
            Tools.ClearObjects(x, y, x2, y2);

			for (int i = x; i <= x2; i++)
			{
				for (int j = y; j <= y2; j++)
				{ Main.tile[i, j] = new Tile(); }
			}

			if (File.Exists(clipboard)) File.Delete(clipboard);
			File.Copy(undoPath, clipboard);

			ResetSection();
			plr.SendSuccessMessage("Cut selection. ({0})", (x2 - x + 1) * (y2 - y + 1));
		}
	}
}
