using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GeoJSON.Net.Contrib.DebuggerVisualizer
{
	public class VisualizerGeoJsonSource : VisualizerObjectSource
	{
		public override void GetData(object target, Stream outgoingData)
		{
			var writer = new StreamWriter(outgoingData);
			var json = Newtonsoft.Json.JsonConvert.SerializeObject(target);
			writer.WriteLine(json);
			writer.Flush();
		}
	}
}
