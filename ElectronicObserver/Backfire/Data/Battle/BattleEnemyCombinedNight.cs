﻿using ElectronicObserver.Backfire.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Backfire.Data.Battle {

	public class BattleEnemyCombinedNight : BattleNight {

		public override void LoadFromResponse( string apiname, dynamic data ) {
			base.LoadFromResponse( apiname, (object)data );

			NightBattle = new PhaseNightBattle( this, "夜戦", false );

			NightBattle.EmulateBattle( _resultHPs, _attackDamages );

		}


		public override string APIName {
			get { return "api_req_combined_battle/ec_midnight_battle"; }
		}

		public override string BattleName {
			get { return "対連合艦隊 夜戦"; }
		}

		public override BattleData.BattleTypeFlag BattleType {
			get { return BattleTypeFlag.Night | BattleTypeFlag.EnemyCombined | ( NightBattle.IsFriendEscort ? BattleTypeFlag.Combined : 0 ); }
		}


		public override IEnumerable<PhaseBase> GetPhases() {
			yield return Initial;
			yield return NightBattle;
		}
	}
}
