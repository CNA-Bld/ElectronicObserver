﻿using ElectronicObserver.Backfire.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Backfire.Observer.kcsapi.api_req_sortie {
	
	public class battleresult : APIBase {

		public override void OnResponseReceived( dynamic data ) {

			KCDatabase.Instance.Battle.LoadFromResponse( APIName, data );


			base.OnResponseReceived( (object)data );
		}


		public override string APIName {
			get { return "api_req_sortie/battleresult"; }
		}
	}
}
