namespace Visualizer.Model
{
    using Nancy.Json;
    using Newtonsoft.Json;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
    using System.Linq;
    using System.Net;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using Telerik.Windows.Controls.GridView;
    using Visualizer.Model;
    using Visualizer.Responses;
    using Visualizer.Settings;
	using Visualizer.Threading;
    using Visualizer.ViewModel;
	using Telerik.Windows.Controls.SplashScreen;
    using Telerik.Windows.Controls;

    public class DataManager
	{
		private string API_CALL_ADD_TAG = $"https://{ApplicationSettings.ServerUrl}/api/asset/addtag";
		private string API_CALL_ADD_DEPARTMENT = $"https://{ApplicationSettings.ServerUrl}/api/asset/adddepartment";
		private string API_CALL_GET_TENSAR_TAGS = $"https://{ApplicationSettings.ServerUrl}/api/asset/tensarreporttags";
        private string API_CALL_ALL_TAGS = $"https://{ApplicationSettings.ServerUrl}/api/asset/alltags";
		private string API_CALL_ALL_VISUALIZER_TAGS = $"https://{ApplicationSettings.ServerUrl}/api/asset/allvisualizertags";
		private string API_CALL_TAG_HISTORY = $"https://{ApplicationSettings.ServerUrl}/api/asset/tagHistory";
		private string API_CALL_ALL_CARTS = $"https://{ApplicationSettings.ServerUrl}/api/asset/allcarts";
        private string API_CALL_ALL_AUDITS = $"https://{ApplicationSettings.ServerUrl}/api/asset/allaudits";
		private string API_CALL_ALL_MICROZONES = $"https://{ApplicationSettings.ServerUrl}/api/asset/allmicrozones";
		private string API_CALL_ALL_DEPARTMENTS = $"https://{ApplicationSettings.ServerUrl}/api/asset/alldepartments";
		private string API_CALL_DELETE_TAG = $"https://{ApplicationSettings.ServerUrl}/api/asset/deletetag";
        private string API_CALL_EDIT_TAG = $"https://{ApplicationSettings.ServerUrl}/api/asset/edittag";
		private string API_CALL_EDIT_ASSET = $"https://{ApplicationSettings.ServerUrl}/api/asset/editasset";
		private string API_CALL_EDIT_MICROZONE = $"https://{ApplicationSettings.ServerUrl}/api/asset/addoreditmicrozone";
		private string API_CALL_EDIT_DEPARTMENT = $"https://{ApplicationSettings.ServerUrl}/api/asset/editdepartment";
		private string API_CALL_ADD_ASSET = $"https://{ApplicationSettings.ServerUrl}/api/asset/addasset";
		private string API_CALL_DELETE_ASSET = $"https://{ApplicationSettings.ServerUrl}/api/asset/deleteasset";
        private string API_CALL_DELETE_MICROZONE = $"https://{ApplicationSettings.ServerUrl}/api/asset/deletemicrozone"; 
        private string API_CALL_ADD_TAG_TYPE = $"https://{ApplicationSettings.ServerUrl}/api/settings/addtagtype";
        private string API_CALL_DELETE_TAG_TYPE = $"https://{ApplicationSettings.ServerUrl}/api/settings/deletetagtype";
        private string API_CALL_GET_TAG_TYPES = $"https://{ApplicationSettings.ServerUrl}/api/settings/gettagtypes";
        private string API_CALL_ADD_TAG_CATEGORY = $"https://{ApplicationSettings.ServerUrl}/api/settings/addtagcategory";
        private string API_CALL_DELETE_TAG_CATEGORY = $"https://{ApplicationSettings.ServerUrl}/api/settings/deletetagcategory";
        private string API_CALL_GET_TAG_CATEGORIES = $"https://{ApplicationSettings.ServerUrl}/api/settings/gettagcategories";

		private const string ApplicationFolder = "Visualizer";
		private const string SettingsFileName = "ApplicationSettings.json";

		private bool _isLoading = true;
		public EventHandler DataUpdated;

		private List<TagResult> _mostRecentTagItems; 
		private List<TagResult> _allTagItems;
		private List<CartResult> _cartItems;
		private List<MicroZoneResult> _mZoneItems;
		private DelegateMarshaler _delegateMarshaler;
		private static DataManager instance = null;
		private static readonly object padlock = new object();

		private DataManager()
		{
			//set the default value of the  server url, in case it hasn't been set
			if(ApplicationSettings.ServerUrl == null || ApplicationSettings.ServerUrl == "")
            {
				ApplicationSettings.ServerUrl = "lrniservice-test.azurewebsites.net";
            }

			this._mostRecentTagItems = new List<TagResult>();
			this._allTagItems = new List<TagResult>();
			//this._cartItems = new List<CartResult>();
			this._mZoneItems = new List<MicroZoneResult>();
			this._delegateMarshaler = DelegateMarshaler.Create();

			//this._cartItems = GetCarts();
			this._mZoneItems = GetMicroZones();

			Task.Factory.StartNew(new Action(this.UpdateDataThread));

		}

		public void AddTag(string[] tagNameRequest)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_ADD_TAG);
				request.ContentType = "text/json";
				request.Method = "POST";
				using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
				{
					writer.Write(new JavaScriptSerializer().Serialize(tagNameRequest));
				}

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			}
			catch (Exception e)
			{
				Trace.TraceError($"Add Tag Exception: {e.ToString()}");
			}
		}

		public void DeleteTag(string[] tagNameRequest)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_DELETE_TAG);
				request.ContentType = "text/json";
				request.Method = "POST";
				using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
				{
					writer.Write(new JavaScriptSerializer().Serialize(tagNameRequest));
				}

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			}
			catch (Exception e)
			{
				Trace.TraceError($"Delete Tag Exception: {e.ToString()}");
			}
		}

		public void EditTag(string[] tagNameRequest)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_EDIT_TAG);
				request.ContentType = "text/json";
				request.Method = "POST";
				using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
				{
					writer.Write(new JavaScriptSerializer().Serialize(tagNameRequest));
				}
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			}
			catch (Exception e)
			{
				Trace.TraceError($"Edit Tag Exception: {e.ToString()}");
			}
		}

		public void EditAsset(Asset assets)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_EDIT_ASSET);
				request.ContentType = "text/json";
				request.Method = "POST";
				string json = JsonConvert.SerializeObject(assets);

				using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
				{
					writer.Write(json);
				}
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			}
			catch (Exception e)
			{
				Trace.TraceError($"Delete Asset Exception: {e.ToString()}");
			}
		}

		public void DeleteAsset(List<Asset> assets)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_DELETE_ASSET);
				request.ContentType = "text/json";
				request.Method = "POST";
				string json = JsonConvert.SerializeObject(assets);

				using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
				{
					writer.Write(json);
				}
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			}
			catch (Exception e)
			{
				Trace.TraceError($"Edit Asset Exception: {e.ToString()}");
			}
		}

        public void DeleteMicroZone(MicroZoneViewModel microZone)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_DELETE_MICROZONE);
                request.ContentType = "text/json";
                request.Method = "POST";

				MicroZone mZone = new MicroZone()
				{
					DepartmentId = microZone.DepartmentId,
					RawId = microZone.RawId,
					MicroZoneName = microZone.Name,
					MicroZoneHeight = (float)microZone.Height,
					MicroZoneWidth = (float)microZone.Width,
					MicroZoneX = (float)microZone.LocationX,
					MicroZoneY = (float)microZone.LocationY,
					IsLocked = microZone.IsLocked
				};


				string json = JsonConvert.SerializeObject(mZone);

                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(json);
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e)
            {
                Trace.TraceError($"Edit Asset Exception: {e.ToString()}");
            }
        }

        public void AddAsset(Asset asset)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_ADD_ASSET);
				request.ContentType = "text/json";
				request.Method = "POST";
				string json = JsonConvert.SerializeObject(asset);

				using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
				{
					writer.Write(json);
				}
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			}
			catch (Exception e)
			{
				Trace.TraceError($"Add Asset Exception: {e.ToString()}");
			}
		}

		public async Task<List<TagType>> GetTagTypes()
		{
			List<TagType> tagTypes = new List<TagType>();

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_GET_TAG_TYPES);
				request.ContentType = "text/json";
				request.Method = "GET";
				var responseString = String.Empty;

				using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
				using (Stream stream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(stream))
				{
					responseString = await reader.ReadToEndAsync();
				}

				tagTypes = JsonConvert.DeserializeObject<List<TagType>>(responseString);
			}
			catch (Exception e)
			{
				Trace.TraceError($"Get Tag Types Exception: {e.ToString()}");
			}

			return tagTypes;
		}

		public async Task<string> AddTagType(TagType tagType)
		{
			var responseString = String.Empty;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_ADD_TAG_TYPE);
				request.ContentType = "text/json";
				request.Method = "POST";

				using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
				{
					await writer.WriteAsync(new JavaScriptSerializer().Serialize(tagType));
				}

				using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
				using (Stream stream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(stream))
				{
					responseString = await reader.ReadToEndAsync();
				}
			}
			catch (Exception e
			)
			{
				Trace.TraceError($"Add Tag Type Exception: {e.ToString()}");
			}

			return responseString;
		}

		public void DeleteTagType(string tagType)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_DELETE_TAG_TYPE);
				request.ContentType = "text/json";
				request.Method = "DELETE";
				using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
				{
					writer.Write(tagType);
				}

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			}
			catch (Exception e)
			{
				Trace.TraceError($"Delete Tag Type Exception: {e.ToString()}");
			}
		}

		public void DeleteTagCategory(string tagCategory)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_DELETE_TAG_CATEGORY);
				request.ContentType = "text/json";
				request.Method = "DELETE";
				using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
				{
					writer.Write(tagCategory);
				}

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			}
			catch (Exception e)
			{
				Trace.TraceError($"Delete Tag Category Exception: {e.ToString()}");
			}
		}

		public async Task<List<TagCategory>> GetTagCategories()
		{
			List<TagCategory> tagCategories = new List<TagCategory>();

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_GET_TAG_CATEGORIES);
				request.ContentType = "text/json";
				request.Method = "GET";
				var responseString = String.Empty;

				using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
				using (Stream stream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(stream))
				{
					responseString = await reader.ReadToEndAsync();
				}

				tagCategories = JsonConvert.DeserializeObject<List<TagCategory>>(responseString);
			}
			catch (Exception e)
			{
				Trace.TraceError($"Get Tag Categories Exception: {e.ToString()}");
			}

			return tagCategories;
		}

		public async Task<string> AddTagCategory(TagCategory tagCategory)
		{
			var responseString = String.Empty;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_ADD_TAG_CATEGORY);
				request.ContentType = "text/json";
				request.Method = "POST";

				using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
				{
					await writer.WriteAsync(new JavaScriptSerializer().Serialize(tagCategory));
				}

				using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
				using (Stream stream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(stream))
				{
					responseString = await reader.ReadToEndAsync();
				}
			}
			catch (Exception e
			)
			{
				Trace.TraceError($"Add Tag Category Exception: {e.ToString()}");
			}

			return responseString;
		}

		protected virtual void OnDataUpdated(EventArgs e)
		{
			if (this.DataUpdated != null)
			{
				this.DataUpdated(this, e);
			}
		}

		public List<TagResult> GetTensarReportTags()
		{
			RootObject root = new RootObject();
			List<TagResult> tagResults = new List<TagResult>();

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_GET_TENSAR_TAGS);
				request.ContentType = "text/json";
				request.Method = "GET";
				string str = string.Empty;
				using (Stream stream = ((HttpWebResponse)request.GetResponse()).GetResponseStream())
				{
					str = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
				}

				root = JsonConvert.DeserializeObject<RootObject>(str);

				if (root != null && root.TagResults != null)
				{
					tagResults = root.TagResults;
				}
			}
			catch (Exception e)
			{
				string fileName = "\\logs.txt";
				string mydocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				string fullpath = mydocsPath + fileName;
				File.WriteAllText(fullpath, e.Message);
			}

			return tagResults;
		}

		public List<CartResult> GetCarts()
		{
			RootObject root = new RootObject();
			List<CartResult> cartResults = new List<CartResult>();

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_ALL_CARTS);
				request.ContentType = "text/json";
				request.Method = "GET";
				string str = string.Empty;
				using (Stream stream = ((HttpWebResponse)request.GetResponse()).GetResponseStream())
				{
					str = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
				}

				root = JsonConvert.DeserializeObject<RootObject>(str);

				if (root != null && root.CartResults != null)
				{
					cartResults = root.CartResults;
				}
			}
			catch (Exception e)
			{
				string fileName = "\\logs.txt";
				string mydocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				string fullpath = mydocsPath + fileName;
				File.WriteAllText(fullpath, e.Message);
			}

			return cartResults;
		}

		public List<MicroZoneResult> GetMicroZones()
		{
			RootObject root = new RootObject();
			List<MicroZoneResult> mZoneResults = new List<MicroZoneResult>();

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_ALL_MICROZONES);
				request.ContentType = "text/json";
				request.Method = "GET";
				string str = string.Empty;
				using (Stream stream = ((HttpWebResponse)request.GetResponse()).GetResponseStream())
				{
					str = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
				}

				root = JsonConvert.DeserializeObject<RootObject>(str);

				if (root != null && root.MicroZoneResults != null)
				{
					mZoneResults = root.MicroZoneResults;
				}

				foreach(MicroZoneResult result in mZoneResults)
                {
					//result.TagsPreviouslyInZone = new List<string>();
					//result.TagsCurrentlyInZone = new List<string>();

					TagResult tagResult = this.MostRecentTags.Where(x => x.Id == result.RawId).FirstOrDefault();

					//if(tagResult != null)
					//               {
					//	result.Battery = tagResult.Battery;
					//	result.LastUpdatedOnServer = tagResult.LastUpdatedOnServer;
					//	result.ReaderId = tagResult.ReaderId;
					//	result.Rssi = tagResult.Rssi;
					//	result.StatusCode = tagResult.StatusCode;
					//	result.Latitude = tagResult.Latitude;
					//	result.Longitude = tagResult.Longitude;
					//               }
					//               else
					//               {
					//	result.Battery = "N/A";
					//	result.ReaderId = "N/A";
					//	result.Rssi = "N/A";
					//	result.StatusCode = "N/A";
					//}
					result.TagsCurrentlyInZone = new List<TagResult>();
					result.TagsPreviouslyInZone = new List<TagResult>();
					tagResult = null;
                }
			}
			catch (Exception e)
			{
				string fileName = "\\logs.txt";
				string mydocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				string fullpath = mydocsPath + fileName;
				File.WriteAllText(fullpath, e.Message);
			}

			return mZoneResults;
		}

		public List<DepartmentResult> GetDepartments()
		{
			RootObject root = new RootObject();
			List<DepartmentResult> deptResults = new List<DepartmentResult>();

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_ALL_DEPARTMENTS);
				request.ContentType = "text/json";
				request.Method = "GET";
				string str = string.Empty;
				using (Stream stream = ((HttpWebResponse)request.GetResponse()).GetResponseStream())
				{
					str = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
				}

				root = JsonConvert.DeserializeObject<RootObject>(str);

				if (root != null && root.DepartmentResults != null)
				{
					deptResults = root.DepartmentResults;
				}
			}
			catch (Exception e)
			{
				string fileName = "\\logs.txt";
				string mydocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				string fullpath = mydocsPath + fileName;
				File.WriteAllText(fullpath, e.Message);
			}

			return deptResults;
		}

		public List<TagAuditResult> GetTagAudits()
		{
			RootObject root = new RootObject();
			List<TagAuditResult> auditResults = new List<TagAuditResult>();

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_ALL_AUDITS);
				request.ContentType = "text/json";
				request.Method = "GET";
				string str = string.Empty;
				using (Stream stream = ((HttpWebResponse)request.GetResponse()).GetResponseStream())
				{
					str = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
				}

				root = JsonConvert.DeserializeObject<RootObject>(str);

				if (root != null && root.TagAuditResults != null)
				{
					auditResults = root.TagAuditResults;
				}
			}
			catch (Exception e)
			{
				string fileName = "\\logs.txt";
				string mydocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				string fullpath = mydocsPath + fileName;
				File.WriteAllText(fullpath, e.Message);
			}

			return auditResults;
		}

		public void UpdateData()
		{
			RootObject root = new RootObject();

			if(_isLoading)
            {
				var dataContext = (SplashScreenDataContext)RadSplashScreenManager.SplashScreenDataContext;
				dataContext.ImagePath = "";
				dataContext.Content = "Retrieving recent tag data from server...";
				dataContext.Footer = "Copyright ©2022, RTLS";
			}

			//get most recent tags first for the mapping screen
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_ALL_VISUALIZER_TAGS);
				request.ContentType = "text/json";
				request.Method = "GET";
				string str = string.Empty;
				using (Stream stream = ((HttpWebResponse)request.GetResponse()).GetResponseStream())
				{
					str = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
				}

				root = JsonConvert.DeserializeObject<RootObject>(str);
			}
			catch (Exception e)
			{
				//Application.Current.MainWindow.Cursor = Cursors.Arrow;
				string fileName = "\\logs.txt";
				string mydocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				string fullpath = mydocsPath + fileName;
				File.WriteAllText(fullpath, e.Message);
			}

			if (root != null && root.TagResults != null)
			{
				//clear out tag list before updating 
				this._mostRecentTagItems.Clear();

				TimeZoneInfo destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
				foreach (TagResult result in root.TagResults)
				{
					DateTime dateTime = Convert.ToDateTime(result.LastUpdatedOnServer);
					result.Id = result?.Id.ToUpper() ?? "";

					if(result.Name != null)
						result.Name = result?.Name.ToUpper() ?? "";

					if (result.TagType != null)
						result.TagType = result.TagType.ToUpper();

					result.LastUpdatedOnServer = TimeZoneInfo.ConvertTimeFromUtc(dateTime, destinationTimeZone);
					result.MZone1Rssi = result.MZone1Rssi.ToString();
					result.Battery = result.Battery.ToString();
					result.MZone1Name = GetMicroZoneNameForTag(result.MZone1);
					result.MZone1Number = GetMicroZoneNumberForTag(result.MZone1);
					result.MZone2Name = GetMicroZoneNameForTag(result.MZone2);
					result.MZone2Number = GetMicroZoneNumberForTag(result.MZone2);
					result.SequenceNumber = HexToInt(result.SequenceNumber).ToString();

					string tempInHex = HexToDecimal(result.Temperature).ToString();
					string tempInFahrenheit = ConvertTempToDoubleString(tempInHex);

					result.Temperature = tempInFahrenheit;
					result.Humidity = ConvertHumidityToString(result.Humidity);
					this._mostRecentTagItems.Add(result);
				}

				//this._mostRecentTagItems.Sort((tagA, tagB) => tagB.LastUpdatedOnServer.CompareTo(tagA.LastUpdatedOnServer));
			}

			this._mZoneItems = GetMicroZones();

			if (_isLoading)
			{
				var dataContext = (SplashScreenDataContext)RadSplashScreenManager.SplashScreenDataContext;
				dataContext.ImagePath = "";
				dataContext.Content = "Retrieving tag history from server...";
				dataContext.Footer = "Copyright ©2022, RTLS";
			}

			//now get the history of all tags for the Dashboard screen
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_TAG_HISTORY);
				request.ContentType = "text/json";
				request.Method = "GET";
				string str = string.Empty;
				using (Stream stream = ((HttpWebResponse)request.GetResponse()).GetResponseStream())
				{
					str = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
				}

				root = JsonConvert.DeserializeObject<RootObject>(str);
			}
			catch (Exception e)
			{
				//Application.Current.MainWindow.Cursor = Cursors.Arrow;
				string fileName = "\\logs.txt";
				string mydocsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				string fullpath = mydocsPath + fileName;
				File.WriteAllText(fullpath, e.Message);
			}
			if (root != null && root.TagResults != null)
			{
				//clear out tag list before updating 
				this._allTagItems.Clear();

				TimeZoneInfo destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
				foreach (TagResult result in root.TagResults)
				{
					DateTime dateTime = Convert.ToDateTime(result.LastUpdatedOnServer);
					result.Id = result?.Id.ToUpper() ?? "";

					if (result.Name != null)
						result.Name = result?.Name.ToUpper() ?? "";

					if (result.TagType != null)
						result.TagType = result.TagType.ToUpper();

					result.LastUpdatedOnServer = TimeZoneInfo.ConvertTimeFromUtc(dateTime, destinationTimeZone);
					result.MZone1Rssi = result.MZone1Rssi.ToString();
					result.Battery = result.Battery.ToString();
					result.MZone1Name = GetMicroZoneNameForTag(result.MZone1);
					result.MZone1Number = GetMicroZoneNumberForTag(result.MZone1);
					result.MZone2Name = GetMicroZoneNameForTag(result.MZone2);
					result.MZone2Number = GetMicroZoneNumberForTag(result.MZone2);
					result.SequenceNumber = HexToInt(result.SequenceNumber).ToString();

					string tempInHex = HexToDecimal(result.Temperature).ToString();
					string tempInFahrenheit = ConvertTempToDoubleString(tempInHex);

					result.Temperature = tempInFahrenheit;
					result.Humidity = ConvertHumidityToString(result.Humidity);

					this._allTagItems.Add(result);
				}

				if (_isLoading)
				{
					var dataContext = (SplashScreenDataContext)RadSplashScreenManager.SplashScreenDataContext;
					dataContext.ImagePath = "";
					dataContext.Content = "Finalizing...";
					dataContext.Footer = "Copyright ©2022, RTLS";

					Thread.Sleep(1000);
					_isLoading = false;
					RadSplashScreenManager.Close();
				}

				//this._allTagItems.Sort((tagA, tagB) => tagB.LastUpdatedOnServer.CompareTo(tagA.LastUpdatedOnServer));
			}
		}

        private string ConvertTempToDoubleString(string temp)
        {
			double unconvertedTemp;

			if(Double.TryParse(temp, out unconvertedTemp))
			{
				double tempInCelsius = unconvertedTemp / 100;
				double tempInFahrenheit = (tempInCelsius * 1.8) + 32;
				return String.Format("{0:0.##}", tempInFahrenheit);
            }
            else
            {
				return "";
            }
        }

		private string ConvertHumidityToString(string temp)
		{
			int unconvertedTemp;
			
			if (int.TryParse(temp, System.Globalization.NumberStyles.HexNumber, null, out unconvertedTemp))
			{
				return unconvertedTemp.ToString();
			}
			else
			{
				return "";
			}
		}

		public async Task<string> AddDepartment(DepartmentViewModel departmentViewModel)
		{
			var responseString = String.Empty;

			DepartmentResult result = new DepartmentResult()
			{
				DepartmentId = departmentViewModel.DepartmentId,
				IsLastLoaded = departmentViewModel.IsLastLoaded,
				Name = departmentViewModel.Name,
				FilePath = departmentViewModel.FilePath,
				ScreenHeight= departmentViewModel.ScreenHeight,
				ScreenWidth= departmentViewModel.ScreenWidth
			};

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_ADD_DEPARTMENT);
				request.ContentType = "text/json";
				request.Method = "POST";

				using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
				{
					await writer.WriteAsync(new JavaScriptSerializer().Serialize(result));
				}

				using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
				using (Stream stream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(stream))
				{
					responseString = await reader.ReadToEndAsync();
				}
			}
			catch (Exception e
			)
			{
				Trace.TraceError($"Add Department Exception: {e.ToString()}");
			}

			return responseString;
		}

        private string GetMicroZoneNameForTag(string tagNumber)
        {
			MicroZoneResult zone = this.MicroZones.Where(zone => zone.TagAssociationNumber == tagNumber).FirstOrDefault();

			if (zone != null)
				return zone.MicroZoneName;
			else
				return "";
        }

		private int GetMicroZoneNumberForTag(string tagNumber)
		{
			MicroZoneResult zone = this.MicroZones.Where(zone => zone.TagAssociationNumber == tagNumber).FirstOrDefault();

			if (zone != null)
				return zone.MicroZoneId;
			else
				return 0;
		}

		private void UpdateDataThread()
		{
			while (true)
			{
				UpdateData();
				this._delegateMarshaler.BeginInvoke(this.OnDataUpdated, EventArgs.Empty);
				Thread.Sleep(ApplicationSettings.SyncIntervalSeconds * 1000);
			}
		}

		private double HexToDecimal(string hexData)
        {
			double convertedValue = 0;

			if (String.IsNullOrEmpty(hexData))
			{
				return 0;
			}

			try
			{
				convertedValue = Convert.ToInt64(hexData, 16);
			}
			catch (Exception e)
			{
				return 0;
			}

			return convertedValue;
		}

		private int HexToInt(string hexData)
		{
			int convertedValue = 0;

			if (String.IsNullOrEmpty(hexData))
			{
				return 0;
			}

			try
			{
				convertedValue = Convert.ToSByte(hexData, 16);
				if (convertedValue >= 0x80)
				{
					return -(convertedValue & 0x7F);
				}
			}
			catch (Exception e)
			{
				return 0;
			}

			return convertedValue;
		}

		public static DataManager Instance
		{
			get
			{
				object padlock = DataManager.padlock;
				lock (padlock)
				{
					if (ReferenceEquals(instance, null))
					{
						instance = new DataManager();
					}
					return instance;
				}
			}
		}

        internal List<AssetViewModel> GetAssets()
        {
			List<AssetViewModel> assets = new List<AssetViewModel>();
			return assets;
		}

		public List<TagResult> AllTags
		{
			get => this._allTagItems;
		}

		public List<TagResult> MostRecentTags
		{
			get => this._mostRecentTagItems;
		}

		public List<CartResult> Carts
		{
			get => this._cartItems;
		}

		public List<MicroZoneResult> MicroZones
        {
			get => this._mZoneItems;
        }

		public async Task<string> AddOrEditMicroZone(MicroZoneViewModel microzone)
		{
			var responseString = String.Empty;

			MicroZoneResult result = new MicroZoneResult()
			{
				MicroZoneName = microzone.Name,
				RawId = microzone.RawId,
				MicroZoneX = (float)microzone.LocationX,
				MicroZoneY = (float)microzone.LocationY,
				MicroZoneHeight = (float)microzone.Height,
				MicroZoneWidth = (float)microzone.Width,
				DepartmentId = microzone.DepartmentId,
				TagAssociationNumber = microzone.TagAssociationNumber,
				IsLocked = microzone.IsLocked
			};


			try
			 {
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_EDIT_MICROZONE);
				request.ContentType = "text/json";
				request.Method = "POST";

				using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
				{
					await writer.WriteAsync(new JavaScriptSerializer().Serialize(result));
				}

				using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
				using (Stream stream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(stream))
				{
					responseString = await reader.ReadToEndAsync();
				}
			}
			catch (Exception e
			)
			{
				Trace.TraceError($"Edit mZone Exception: {e.ToString()}");
			}

			return responseString;
		}

		public async Task<string> UpdateDepartment(DepartmentViewModel dept)
		{
			var responseString = String.Empty;

			DepartmentResult result = new DepartmentResult()
			{
				DepartmentId = dept.DepartmentId,
				IsLastLoaded = dept.IsLastLoaded,
				Name = dept.Name,
				FilePath = dept.FilePath,
				ScreenWidth= dept.ScreenWidth,
				ScreenHeight= dept.ScreenHeight
			};

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_CALL_EDIT_DEPARTMENT);
				request.ContentType = "text/json";
				request.Method = "POST";

				using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
				{
					await writer.WriteAsync(new JavaScriptSerializer().Serialize(result));
				}

				using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
				using (Stream stream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(stream))
				{
					responseString = await reader.ReadToEndAsync();
				}
			}
			catch (Exception e
			)
			{
				Trace.TraceError($"Edit mZone Exception: {e.ToString()}");
			}

			return responseString;
		}
	}
}

