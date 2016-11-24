﻿using ElectronicObserver.Backfire.Data.Battle.Phase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Backfire.Data.Battle.Detail {

	public static class BattleDetailDescriptor {

		public static string GetBattleDetail( BattleManager bm ) {
			var sb = new StringBuilder();

			if ( bm.StartsFromDayBattle ) {
				sb.AppendLine( "◆ 昼戦 ◆" ).AppendLine( GetBattleDetail( bm.BattleDay ) );
				if ( bm.BattleNight != null )
					sb.AppendLine( "◆ 夜戦 ◆" ).AppendLine( GetBattleDetail( bm.BattleNight ) );

			} else {
				sb.AppendLine( "◆ 夜戦 ◆" ).AppendLine( GetBattleDetail( bm.BattleNight ) );
				if ( bm.BattleDay != null )
					sb.AppendLine( "◆ 昼戦 ◆" ).AppendLine( GetBattleDetail( bm.BattleDay ) );
			}

			return sb.ToString();
		}


		public static string GetBattleDetail( BattleData battle ) {

			var sbmaster = new StringBuilder();
			bool isBaseAirRaid = ( battle.BattleType & BattleData.BattleTypeFlag.BaseAirRaid ) != 0;


			foreach ( var phase in battle.GetPhases() ) {

				var sb = new StringBuilder();

				if ( phase is PhaseAirBattle ) {
					var p = phase as PhaseAirBattle;

					GetBattleDetailPhaseAirBattle( sb, p );


				} else if ( phase is PhaseBaseAirAttack ) {
					var p = phase as PhaseBaseAirAttack;

					foreach ( var a in p.AirAttackUnits ) {
						sb.AppendFormat( "〈第{0}波〉\r\n", a.AirAttackIndex + 1 );
						GetBattleDetailPhaseAirBattle( sb, a );
						sb.Append( a.GetBattleDetail() );
					}


				} else if ( phase is PhaseInitial ) {
					var p = phase as PhaseInitial;

					if ( p.FriendFleetEscort != null )
						sb.AppendLine( "〈味方主力艦隊〉" );
					else
						sb.AppendLine( "〈味方艦隊〉" );

					if ( isBaseAirRaid )
						OutputFriendBase( sb, p.InitialHPs.Take( 6 ).ToArray(), p.MaxHPs.Take( 6 ).ToArray() );
					else
						OutputFriendData( sb, p.FriendFleet, p.InitialHPs.Take( 6 ).ToArray(), p.MaxHPs.Take( 6 ).ToArray() );

					if ( p.FriendFleetEscort != null ) {
						sb.AppendLine();
						sb.AppendLine( "〈味方随伴艦隊〉" );

						OutputFriendData( sb, p.FriendFleetEscort, p.InitialHPs.Skip( 12 ).Take( 6 ).ToArray(), p.MaxHPs.Skip( 12 ).Take( 6 ).ToArray() );
					}

					sb.AppendLine();

					if ( p.EnemyMembersEscort != null )
						sb.Append( "〈敵主力艦隊〉" );
					else
						sb.Append( "〈敵艦隊〉" );

					if ( p.IsBossDamaged )
						sb.Append( " : 装甲破壊" );
					sb.AppendLine();

					OutputEnemyData( sb, p.EnemyMembersInstance, p.EnemyLevels, p.InitialHPs.Skip( 6 ).Take( 6 ).ToArray(), p.MaxHPs.Skip( 6 ).Take( 6 ).ToArray(), p.EnemySlotsInstance, p.EnemyParameters );


					if ( p.EnemyMembersEscort != null ) {
						sb.AppendLine();
						sb.AppendLine( "〈敵随伴艦隊〉" );

						OutputEnemyData( sb, p.EnemyMembersEscortInstance, p.EnemyLevelsEscort, p.InitialHPs.Skip( 18 ).Take( 6 ).ToArray(), p.MaxHPs.Skip( 18 ).Take( 6 ).ToArray(), p.EnemySlotsEscortInstance, p.EnemyParametersEscort );
					}

					sb.AppendLine();

					if ( p.RationIndexes.Length > 0 ) {
						sb.AppendLine( "〈戦闘糧食補給〉" );
						foreach ( var index in p.RationIndexes ) {
							ShipData ship;

							if ( index < 6 )
								ship = p.FriendFleet.MembersInstance[index];
							else
								ship = p.FriendFleetEscort.MembersInstance[index - 6];

							if ( ship != null ) {
								sb.AppendFormat( "　{0} #{1}\r\n", ship.NameWithLevel, index );
							}
						}
						sb.AppendLine();
					}


				} else if ( phase is PhaseNightBattle ) {
					var p = phase as PhaseNightBattle;
					int length = sb.Length;

					{
						var eq = KCDatabase.Instance.MasterEquipments[p.TouchAircraftFriend];
						if ( eq != null ) {
							sb.Append( "自軍夜間触接: " ).AppendLine( eq.Name );
						}
						eq = KCDatabase.Instance.MasterEquipments[p.TouchAircraftEnemy];
						if ( eq != null ) {
							sb.Append( "敵軍夜間触接: " ).AppendLine( eq.Name );
						}
					}

					{
						int searchlightIndex = p.SearchlightIndexFriend;
						if ( searchlightIndex != -1 ) {
							sb.AppendFormat( "自軍探照灯照射: {0} #{1}\r\n", p.FriendFleet.MembersInstance[searchlightIndex].Name, searchlightIndex + 1 );
						}
						searchlightIndex = p.SearchlightIndexEnemy;
						if ( searchlightIndex != -1 ) {
							sb.AppendFormat( "敵軍探照灯照射: {0} #{1}\r\n", p.EnemyMembersInstance[searchlightIndex].NameWithClass, searchlightIndex + 1 );
						}
					}

					if ( p.FlareIndexFriend != -1 ) {
						sb.AppendFormat( "自軍照明弾投射: {0} #{1}\r\n", p.FriendFleet.MembersInstance[p.FlareIndexFriend].Name, p.FlareIndexFriend + 1 );
					}
					if ( p.FlareIndexEnemy != -1 ) {
						sb.AppendFormat( "敵軍照明弾投射: {0} #{1}\r\n", p.FlareEnemyInstance.NameWithClass, p.FlareIndexEnemy + 1 );
					}

					if ( sb.Length > length )		// 追加行があった場合
						sb.AppendLine();


				} else if ( phase is PhaseSearching ) {
					var p = phase as PhaseSearching;

					sb.Append( "自軍陣形: " ).Append( Constants.GetFormation( p.FormationFriend ) );
					sb.Append( " / 敵軍陣形: " ).AppendLine( Constants.GetFormation( p.FormationEnemy ) );
					sb.Append( "交戦形態: " ).AppendLine( Constants.GetEngagementForm( p.EngagementForm ) );
					sb.Append( "自軍索敵: " ).Append( Constants.GetSearchingResult( p.SearchingFriend ) );
					sb.Append( " / 敵軍索敵: " ).AppendLine( Constants.GetSearchingResult( p.SearchingEnemy ) );

					sb.AppendLine();
				}


				if ( !( phase is PhaseBaseAirAttack ) )		// 通常出力と重複するため
					sb.Append( phase.GetBattleDetail() );

				if ( sb.Length > 0 ) {
					sbmaster.AppendFormat( "《{0}》\r\n", phase.Title ).Append( sb );
				}
			}


			{
				sbmaster.AppendLine( "《戦闘終了》" );

				var friend = battle.Initial.FriendFleet;
				var friendescort = battle.Initial.FriendFleetEscort;
				var enemy = battle.Initial.EnemyMembersInstance;
				var enemyescort = battle.Initial.EnemyMembersEscortInstance;

				if ( friendescort != null )
					sbmaster.AppendLine( "〈味方主力艦隊〉" );
				else
					sbmaster.AppendLine( "〈味方艦隊〉" );

				if ( isBaseAirRaid ) {

					for ( int i = 0; i < 6; i++ ) {
						if ( battle.Initial.MaxHPs[i] <= 0 )
							continue;

						OutputResultData( sbmaster, i, string.Format( "第{0}基地", i + 1 ),
							battle.Initial.InitialHPs[i], battle.ResultHPs[i], battle.Initial.MaxHPs[i] );
					}

				} else {
					for ( int i = 0; i < friend.Members.Count(); i++ ) {
						var ship = friend.MembersInstance[i];
						if ( ship == null )
							continue;

						OutputResultData( sbmaster, i, ship.Name,
							battle.Initial.InitialHPs[i], battle.ResultHPs[i], battle.Initial.MaxHPs[i] );
					}
				}

				if ( friendescort != null ) {
					sbmaster.AppendLine().AppendLine( "〈味方随伴艦隊〉" );

					for ( int i = 0; i < friendescort.Members.Count(); i++ ) {
						var ship = friendescort.MembersInstance[i];
						if ( ship == null )
							continue;

						OutputResultData( sbmaster, i + 6, ship.Name,
							battle.Initial.InitialHPs[i + 12], battle.ResultHPs[i + 12], battle.Initial.MaxHPs[i + 12] );
					}

				}


				sbmaster.AppendLine();
				if ( enemyescort != null )
					sbmaster.AppendLine( "〈敵主力艦隊〉" );
				else
					sbmaster.AppendLine( "〈敵艦隊〉" );

				for ( int i = 0; i < enemy.Length; i++ ) {
					var ship = enemy[i];
					if ( ship == null )
						continue;

					OutputResultData( sbmaster, i,
						ship.NameWithClass,
						battle.Initial.InitialHPs[i + 6], battle.ResultHPs[i + 6], battle.Initial.MaxHPs[i + 6] );
				}

				if ( enemyescort != null ) {
					sbmaster.AppendLine().AppendLine( "〈敵随伴艦隊〉" );

					for ( int i = 0; i < enemyescort.Length; i++ ) {
						var ship = enemyescort[i];
						if ( ship == null )
							continue;

						OutputResultData( sbmaster, i + 6, ship.NameWithClass,
							battle.Initial.InitialHPs[i + 18], battle.ResultHPs[i + 18], battle.Initial.MaxHPs[i + 18] );
					}
				}

				sbmaster.AppendLine();
			}

			return sbmaster.ToString();
		}


		private static void GetBattleDetailPhaseAirBattle( StringBuilder sb, PhaseAirBattle p ) {

			if ( p.IsStage1Available ) {
				sb.Append( "Stage1: " ).AppendLine( Constants.GetAirSuperiority( p.AirSuperiority ) );
				sb.AppendFormat( "　自軍: -{0}/{1}\r\n　敵軍: -{2}/{3}\r\n",
					p.AircraftLostStage1Friend, p.AircraftTotalStage1Friend,
					p.AircraftLostStage1Enemy, p.AircraftTotalStage1Enemy );
				if ( p.TouchAircraftFriend > 0 )
					sb.AppendFormat( "　自軍触接: {0}\r\n", KCDatabase.Instance.MasterEquipments[p.TouchAircraftFriend].Name );
				if ( p.TouchAircraftEnemy > 0 )
					sb.AppendFormat( "　敵軍触接: {0}\r\n", KCDatabase.Instance.MasterEquipments[p.TouchAircraftEnemy].Name );
			}
			if ( p.IsStage2Available ) {
				sb.Append( "Stage2: " );
				if ( p.IsAACutinAvailable ) {
					sb.AppendFormat( "対空カットイン( {0}, {1}({2}) )", p.AACutInShip.NameWithLevel, Constants.GetAACutinKind( p.AACutInKind ), p.AACutInKind );
				}
				sb.AppendLine();
				sb.AppendFormat( "　自軍: -{0}/{1}\r\n　敵軍: -{2}/{3}\r\n",
					p.AircraftLostStage2Friend, p.AircraftTotalStage2Friend,
					p.AircraftLostStage2Enemy, p.AircraftTotalStage2Enemy );
			}
			sb.AppendLine();
		}


		private static void OutputFriendData( StringBuilder sb, FleetData fleet, int[] initialHPs, int[] maxHPs ) {

			for ( int i = 0; i < fleet.MembersInstance.Count; i++ ) {
				var ship = fleet.MembersInstance[i];

				if ( ship == null )
					continue;

				sb.AppendFormat( "#{0}: {1} {2} HP: {3} / {4} - 火力{5}, 雷装{6}, 対空{7}, 装甲{8}\r\n",
					i + 1,
					ship.MasterShip.ShipTypeName, ship.NameWithLevel,
					initialHPs[i], maxHPs[i],
					ship.FirepowerBase, ship.TorpedoBase, ship.AABase, ship.ArmorBase );

				sb.Append( "　" );
				for ( int k = 0; k < ship.SlotInstance.Count; k++ ) {
					var eq = ship.SlotInstance[k];
					if ( eq != null ) {
						if ( k > 0 )
							sb.Append( ", " );
						sb.Append( eq.ToString() );
					}
				}
				sb.AppendLine();
			}
		}

		private static void OutputFriendBase( StringBuilder sb, int[] initialHPs, int[] maxHPs ) {

			for ( int i = 0; i < initialHPs.Length; i++ ) {
				if ( maxHPs[i] <= 0 )
					continue;

				sb.AppendFormat( "#{0}: 陸上施設 第{1}基地 HP: {2} / {3}\r\n\r\n",
					i + 1,
					i + 1,
					initialHPs[i], maxHPs[i] );
			}

		}

		private static void OutputEnemyData( StringBuilder sb, ShipDataMaster[] members, int[] levels, int[] initialHPs, int[] maxHPs, EquipmentDataMaster[][] slots, int[][] parameters ) {

			for ( int i = 0; i < members.Length; i++ ) {
				if ( members[i] == null )
					continue;

				sb.AppendFormat( "#{0}: {1} {2} Lv. {3} HP: {4} / {5}",
					i + 1,
					members[i].ShipTypeName, members[i].NameWithClass,
					levels[i],
					initialHPs[i], maxHPs[i] );

				if ( parameters != null ) {
					sb.AppendFormat( " - 火力{0}, 雷装{1}, 対空{2}, 装甲{3}",
					parameters[i][0], parameters[i][1], parameters[i][2], parameters[i][3] );
				}

				sb.AppendLine().Append( "　" );
				for ( int k = 0; k < slots[i].Length; k++ ) {
					var eq = slots[i][k];
					if ( eq != null ) {
						if ( k > 0 )
							sb.Append( ", " );
						sb.Append( eq.ToString() );
					}
				}
				sb.AppendLine();
			}
		}


		private static void OutputResultData( StringBuilder sb, int index, string name, int initialHP, int resultHP, int maxHP ) {
			sb.AppendFormat( "#{0}: {1} HP: ({2} → {3})/{4} ({5})\r\n",
				index + 1, name,
				Math.Max( initialHP, 0 ),
				Math.Max( resultHP, 0 ),
				Math.Max( maxHP, 0 ),
				resultHP - initialHP );
		}


	}
}
