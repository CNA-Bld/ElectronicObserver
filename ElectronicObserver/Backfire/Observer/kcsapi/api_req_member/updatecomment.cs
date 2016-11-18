using ElectronicObserver.Backfire.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Backfire.Observer.kcsapi.api_req_member {

	public class updatecomment : APIBase {

		public override bool IsRequestSupported { get { return true; } }
		public override bool IsResponseSupported { get { return false; } }

		public override void OnRequestReceived( Dictionary<string, string> data ) {
			KCDatabase.Instance.Admiral.LoadFromRequest( APIName, data );

			base.OnRequestReceived( data );
		}

		public override string APIName {
			get { return "api_req_member/updatecomment"; }
		}
	}
}
