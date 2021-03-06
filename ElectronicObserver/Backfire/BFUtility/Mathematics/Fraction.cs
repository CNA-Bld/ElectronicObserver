﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Backfire.BFUtility.Mathematics {

	public class Fraction {
		public int Current { get; set; }
		public int Max { get; set; }

		public double Rate {
			get { return (double)Current / Math.Max( Max, 1 ); }
		}


		public Fraction() {
			Current = Max = 0;
		}

		public Fraction( int current, int max ) {
			Current = current;
			Max = max;
		}

		public override string ToString() {
			return string.Format( "{0}/{1}", Current, Max );
		}
	}

}
