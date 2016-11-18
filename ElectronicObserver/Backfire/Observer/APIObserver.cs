using Codeplex.Data;
using ElectronicObserver.Backfire.Observer.kcsapi;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace ElectronicObserver.Backfire.Observer {


    public sealed class APIObserver
    {


        #region Singleton

        private static readonly APIObserver instance = new APIObserver();

        public static APIObserver Instance
        {
            get { return instance; }
        }

        #endregion



        public APIDictionary APIList { get; private set; }

        public event APIReceivedEventHandler RequestReceived = delegate { };
        public event APIReceivedEventHandler ResponseReceived = delegate { };


        private APIObserver()
        {

            APIList = new APIDictionary();
            APIList.Add(new kcsapi.api_start2());
            APIList.Add(new kcsapi.api_get_member.basic());
            APIList.Add(new kcsapi.api_get_member.slot_item());
            APIList.Add(new kcsapi.api_get_member.useitem());
            APIList.Add(new kcsapi.api_get_member.kdock());
            APIList.Add(new kcsapi.api_port.port());
            APIList.Add(new kcsapi.api_get_member.ship2());
            APIList.Add(new kcsapi.api_get_member.questlist());
            APIList.Add(new kcsapi.api_get_member.ndock());
            APIList.Add(new kcsapi.api_req_kousyou.getship());
            APIList.Add(new kcsapi.api_req_hokyu.charge());
            APIList.Add(new kcsapi.api_req_kousyou.destroyship());
            APIList.Add(new kcsapi.api_req_kousyou.destroyitem2());
            APIList.Add(new kcsapi.api_req_member.get_practice_enemyinfo());
            APIList.Add(new kcsapi.api_get_member.picture_book());
            APIList.Add(new kcsapi.api_req_mission.start());
            APIList.Add(new kcsapi.api_get_member.ship3());
            APIList.Add(new kcsapi.api_req_kaisou.powerup());
            APIList.Add(new kcsapi.api_req_map.start());
            APIList.Add(new kcsapi.api_req_map.next());
            APIList.Add(new kcsapi.api_req_kousyou.createitem());
            APIList.Add(new kcsapi.api_req_sortie.battle());
            APIList.Add(new kcsapi.api_req_sortie.battleresult());
            APIList.Add(new kcsapi.api_req_battle_midnight.battle());
            APIList.Add(new kcsapi.api_req_battle_midnight.sp_midnight());
            APIList.Add(new kcsapi.api_req_combined_battle.battle());
            APIList.Add(new kcsapi.api_req_combined_battle.midnight_battle());
            APIList.Add(new kcsapi.api_req_combined_battle.sp_midnight());
            APIList.Add(new kcsapi.api_req_combined_battle.airbattle());
            APIList.Add(new kcsapi.api_req_combined_battle.battleresult());
            APIList.Add(new kcsapi.api_req_practice.battle());
            APIList.Add(new kcsapi.api_req_practice.midnight_battle());
            APIList.Add(new kcsapi.api_req_practice.battle_result());
            APIList.Add(new kcsapi.api_get_member.deck());
            APIList.Add(new kcsapi.api_get_member.mapinfo());
            APIList.Add(new kcsapi.api_req_combined_battle.battle_water());
            APIList.Add(new kcsapi.api_req_combined_battle.goback_port());
            APIList.Add(new kcsapi.api_req_kousyou.remodel_slot());
            APIList.Add(new kcsapi.api_get_member.material());
            APIList.Add(new kcsapi.api_req_mission.result());
            APIList.Add(new kcsapi.api_req_ranking.getlist());
            APIList.Add(new kcsapi.api_req_sortie.airbattle());
            APIList.Add(new kcsapi.api_get_member.ship_deck());
            APIList.Add(new kcsapi.api_req_kaisou.marriage());
            APIList.Add(new kcsapi.api_req_hensei.preset_select());
            APIList.Add(new kcsapi.api_req_kaisou.slot_exchange_index());
            APIList.Add(new kcsapi.api_get_member.record());
            APIList.Add(new kcsapi.api_get_member.payitem());
            APIList.Add(new kcsapi.api_req_kousyou.remodel_slotlist());
            APIList.Add(new kcsapi.api_req_sortie.ld_airbattle());
            APIList.Add(new kcsapi.api_req_combined_battle.ld_airbattle());
            APIList.Add(new kcsapi.api_get_member.require_info());
            APIList.Add(new kcsapi.api_get_member.base_air_corps());
            APIList.Add(new kcsapi.api_req_air_corps.set_plane());
            APIList.Add(new kcsapi.api_req_air_corps.set_action());
            APIList.Add(new kcsapi.api_req_air_corps.supply());
            APIList.Add(new kcsapi.api_req_kaisou.slot_deprive());
            APIList.Add(new kcsapi.api_req_air_corps.expand_base());
            APIList.Add(new kcsapi.api_req_combined_battle.ec_battle());
            APIList.Add(new kcsapi.api_req_combined_battle.ec_midnight_battle());

            APIList.Add(new kcsapi.api_req_quest.clearitemget());
            APIList.Add(new kcsapi.api_req_nyukyo.start());
            APIList.Add(new kcsapi.api_req_nyukyo.speedchange());
            APIList.Add(new kcsapi.api_req_kousyou.createship());
            APIList.Add(new kcsapi.api_req_kousyou.createship_speedchange());
            APIList.Add(new kcsapi.api_req_hensei.change());
            APIList.Add(new kcsapi.api_req_member.updatedeckname());
            APIList.Add(new kcsapi.api_req_kaisou.remodeling());
            APIList.Add(new kcsapi.api_req_kaisou.open_exslot());
            APIList.Add(new kcsapi.api_req_map.select_eventmap_rank());
            APIList.Add(new kcsapi.api_req_hensei.combined());
            APIList.Add(new kcsapi.api_req_member.updatecomment());
            APIList.Add(new kcsapi.api_req_air_corps.change_name());
            APIList.Add(new kcsapi.api_req_quest.stop());

        }


        public APIBase this[string key]
        {
            get
            {
                if (APIList.ContainsKey(key)) return APIList[key];
                else return null;
            }
        }

        public void OnRequestReceived(string shortpath, Dictionary<string, string> parsedData)
        {
            try
            {
                APIList.OnRequestReceived(shortpath, parsedData);
                RequestReceived(shortpath, parsedData);
            }
            catch (Exception ex)
            {
                ErrorReporter.SendErrorReport(ex, "[Backfire] Request の受信中にエラーが発生しました。", shortpath);
            }
        }

        public void OnResponseReceived(string shortpath, Dictionary<string, string> parsedData)
        {
            try
            {
                APIList.OnResponseReceived(shortpath, parsedData);
                ResponseReceived(shortpath, parsedData);
            }
            catch (Exception ex)
            {
                ErrorReporter.SendErrorReport(ex, "[Backfire] Responseの受信中にエラーが発生しました。", shortpath);
            }
        }

    }
}
