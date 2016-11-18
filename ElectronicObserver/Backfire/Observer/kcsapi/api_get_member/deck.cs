﻿using ElectronicObserver.Backfire.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Backfire.Observer.kcsapi.api_get_member {

	public class deck : APIBase {

		public override void OnResponseReceived( dynamic data ) {

			KCDatabase.Instance.Fleet.LoadFromResponse( APIName, data );

			base.OnResponseReceived( (object)data );
		}


		public override string APIName {
			get { return "api_get_member/deck"; }
		}

	}
}
