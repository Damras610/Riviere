/* Generated by MyraPad at 07/11/2020 01:51:28 */
using Microsoft.Xna.Framework;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rivière.Screen.UI
{
	public partial class IngameScreenUI
	{
		public ScoreGrid score_grid;

		public IngameScreenUI()
		{
			BuildUI();
			score_grid = new ScoreGrid();
			Widgets.Insert(0, score_grid);
		}
	}
}