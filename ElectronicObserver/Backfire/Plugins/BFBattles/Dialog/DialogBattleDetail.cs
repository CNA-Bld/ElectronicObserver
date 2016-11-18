﻿using ElectronicObserver.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BFBattles.Dialog {
	public partial class DialogBattleDetail : Form {

		public string BattleDetailText {
			get { return TextBattleDetail.Text; }
			set { TextBattleDetail.Text = value; }
		}

		public DialogBattleDetail() {
			InitializeComponent();

			Font = ElectronicObserver.Utility.Configuration.Config.UI.MainFont;
		}

		private void DialogBattleDetail_Load( object sender, EventArgs e ) {
			this.Icon = ResourceManager.ImageToIcon( ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBattle] );
		}

		private void DialogBattleDetail_FormClosed( object sender, FormClosedEventArgs e ) {
			ResourceManager.DestroyIcon( Icon );
		}


		private void DialogBattleDetail_Shown( object sender, EventArgs e ) {
			ClientSize = new Size(
				Math.Min( TextBattleDetail.Location.X * 2 + TextBattleDetail.Width + TextBattleDetail.Margin.Horizontal, 800 ),
				Math.Min( TextBattleDetail.Location.Y * 2 + TextBattleDetail.Height + TextBattleDetail.Margin.Vertical, 600 ) );

			//Location -= new Size( Width / 2, Height / 2 );
		}
	}
}
