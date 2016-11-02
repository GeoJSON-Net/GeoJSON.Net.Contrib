using GeoJSON.Net;
using GeoJSON.Net.Contrib.DebuggerVisualizer;
using GeoJSON.Net.Contrib.MsSqlSpatial;
using GeoJSON.Net.Geometry;
using Microsoft.SqlServer.Types;
using Microsoft.VisualStudio.DebuggerVisualizers;
using Newtonsoft.Json;
using SqlServerSpatial.Toolkit;
using SqlServerSpatial.Toolkit.Viewers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Media;

[assembly: System.Diagnostics.DebuggerVisualizer(visualizer: typeof(GeoJSON.Net.Contrib.DebuggerVisualizer.DebuggerSideGeoJSONObject),
																								visualizerObjectSource: typeof(VisualizerGeoJsonSource),
																								Target = typeof(GeoJSONObject),
																								Description = "GeoJSON Visualizer")]
namespace GeoJSON.Net.Contrib.DebuggerVisualizer
{
	public class DebuggerSideGeoJSONObject : DialogDebuggerVisualizer
	{
		public DebuggerSideGeoJSONObject()
		{
			try
			{
				SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
			}
			catch (Exception ex)
			{
				Trace.TraceError("Unable to initialize SqlServerTypes : " + ex.ToString());
			}

		}
		protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
		{
			// Initialize form
			InitializeComponent();
			try
			{

				string json = null;
				using (StreamReader reader = new StreamReader(objectProvider.GetData()))
				{
					json = reader.ReadToEnd();
				}
				JsonConverter converter = new Converters.GeoJsonConverter();
				IGeoJSONObject geoJsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<IGeoJSONObject>(value: json, converters: new JsonConverter[] { converter });
				if (geoJsonObject == null)
				{
					Trace.TraceWarning(string.Concat("GeoJSON Visualizer cannot show supplied object because it is null or is not a ", nameof(GeoJSONObject), "."));
					return;
				}

				SqlGeometry geometry = null;
				List<SqlGeometry> geometryList = null;
				switch (geoJsonObject.Type)
				{
					case GeoJSONObjectType.Feature:
						geometry = ((Feature.Feature)geoJsonObject).ToSqlGeometry(4326);
						break;
					case GeoJSONObjectType.FeatureCollection:
						geometryList = ((Feature.FeatureCollection)geoJsonObject).ToSqlGeometry(4326);
						break;
					case GeoJSONObjectType.GeometryCollection:
						geometry = ((Geometry.GeometryCollection)geoJsonObject).ToSqlGeometry(4326);
						break;
					case GeoJSONObjectType.LineString:
						geometry = ((LineString)geoJsonObject).ToSqlGeometry(4326);
						break;
					case GeoJSONObjectType.MultiLineString:
						geometry = ((MultiLineString)geoJsonObject).ToSqlGeometry(4326);
						break;
					case GeoJSONObjectType.MultiPoint:
						geometry = ((MultiPoint)geoJsonObject).ToSqlGeometry(4326);
						break;
					case GeoJSONObjectType.MultiPolygon:
						geometry = ((MultiPolygon)geoJsonObject).ToSqlGeometry(4326);
						break;
					case GeoJSONObjectType.Point:
						geometry = ((Geometry.Point)geoJsonObject).ToSqlGeometry(4326);
						break;
					case GeoJSONObjectType.Polygon:
						geometry = ((Polygon)geoJsonObject).ToSqlGeometry(4326);
						break;
				}

				bool bIsGeomOK = (geometry != null) || (geometryList != null);
				if (bIsGeomOK)
				{
					if (geometry != null)
					{
						_spatialViewerControl.SetGeometry(new SqlGeometryStyled(geometry, Color.FromArgb(200, 0, 175, 0), Colors.Black, 1f, null, true));
					}
					else if (geometryList != null)
					{
						_spatialViewerControl.SetGeometry(new SqlGeometryStyled(geometry, Color.FromArgb(200, 0, 175, 0), Colors.Black, 1f, null, true));
					}

					_form.Shown += (o, e) => _spatialViewerControl.ResetView();

					// Show the grid with the list
					windowService.ShowDialog(_form);
				}
			}
			catch (Exception e)
			{
				System.Windows.Forms.MessageBox.Show("Error: " + e.ToString());
			}
		}

		public static void TestShowVisualizer(object objectToVisualize)
		{
			VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(DebuggerSideGeoJSONObject), typeof(VisualizerGeoJsonSource));
			visualizerHost.ShowVisualizer();
		}

		#region Visualizer host Form

		protected Form _form; // Main form hosting the visualizer
		protected ElementHost _elementHost1; // SpatialToolit visualizer is WPF, so we host it in an ElementHost
		protected ISpatialViewer _spatialViewerControl; // The actual interface of the SpatialToolkit debbugerVizualiser

		/// <summary>
		/// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
		/// le contenu de cette méthode avec l'éditeur de code.
		/// </summary>
		protected void InitializeComponent()
		{
			this._elementHost1 = new System.Windows.Forms.Integration.ElementHost();
			this._spatialViewerControl = new SpatialViewer_GDIHost();
			_form = new System.Windows.Forms.Form();
			_form.SuspendLayout();
			// 
			// elementHost1
			// 
			this._elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
			this._elementHost1.Location = new System.Drawing.Point(0, 0);
			this._elementHost1.Name = "elementHost1";
			this._elementHost1.Size = new System.Drawing.Size(824, 581);
			this._elementHost1.TabIndex = 0;
			this._elementHost1.Text = "elementHost1";
			this._elementHost1.Child = (UIElement)this._spatialViewerControl;
			// 
			// Form1
			// 
			_form.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			_form.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			_form.ClientSize = new System.Drawing.Size(824, 581);
			_form.Controls.Add(this._elementHost1);
			_form.Name = "GeoJSON Visualizer";
			_form.Text = "GeoJSON Visualizer";
			_form.ResumeLayout(false);

		}

		#endregion
	}
}
