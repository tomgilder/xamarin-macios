using System;
using System.IO;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Xamarin.MacDev.Tasks
{
	public abstract class LoadCodesignStateTaskBase : Task
	{
		public string SessionId { get; set; }

		[Required]
		public string AppBundleDir { get; set; }

		[Required]
		public string SavedState { get; set; }

		[Output]
		public ITaskItem LoadedItem { get; set; }

		public override bool Execute ()
		{
			Log.LogTaskName ("LoadCodesignState");
			Log.LogTaskProperty ("AppBundleDir", AppBundleDir);
			Log.LogTaskProperty ("SavedState", SavedState);

			var item = new TaskItem (AppBundleDir);
			PDictionary plist;

			try {
				plist = PDictionary.FromFile (SavedState);
			} catch (Exception ex) {
				Log.LogError ("Failed to load saved state '{0}': {1}", SavedState, ex.Message);
				return false;
			}

			foreach (var kvp in plist) {
				string value;

				if (kvp.Value.Type == PObjectType.Boolean)
					value = ((PBoolean) kvp.Value).Value.ToString ();
				else if (kvp.Value.Type == PObjectType.String)
					value = ((PString) kvp.Value).Value;
				else
					continue;

				item.SetMetadata (kvp.Key, value);
			}

			LoadedItem = item;

			return !Log.HasLoggedErrors;
		}
	}
}
