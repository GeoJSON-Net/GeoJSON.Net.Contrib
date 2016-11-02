using GeoJSON.Net.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoJSON.Net.Contrib.DebuggerVisualizer.Test
{
	
	class Program
	{

		[STAThread()]
		static void Main(string[] args)
		{
			var point = new Point(new GeographicPosition(53.2455662, 90.65464646));

			DebuggerSideGeoJSONObject.TestShowVisualizer(point);
		}
	}
}
