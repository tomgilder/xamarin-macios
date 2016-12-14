using System;
using System.IO;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Xamarin.MacDev.Tasks
{
	public abstract class SaveCodesignStateTaskBase : Task
	{
		public string SessionId { get; set; }

		[Required]
		public string CodesignAllocate { get; set; }

		public bool DisableTimestamp { get; set; }

		public string Entitlements { get; set; }

		public string Keychain { get; set; }

		public string ResourceRules { get; set; }

		[Required]
		public string SigningKey { get; set; }

		public string ExtraArgs { get; set; }

		[Output]
		[Required]
		public string SavedState { get; set; }

		public override bool Execute ()
		{
			Log.LogTaskName ("SaveCodesignState");
			Log.LogTaskProperty ("CodesignAllocate", CodesignAllocate);
			Log.LogTaskProperty ("DisableTimestamp", DisableTimestamp);
			Log.LogTaskProperty ("Entitlements", Entitlements);
			Log.LogTaskProperty ("Keychain", Keychain);
			Log.LogTaskProperty ("ResourceRules", ResourceRules);
			Log.LogTaskProperty ("SigningKey", SigningKey);
			Log.LogTaskProperty ("ExtraArgs", ExtraArgs);
			Log.LogTaskProperty ("SavedState", SavedState);

			var plist = new PDictionary ();

			plist["CodesignAllocate"] = new PString (CodesignAllocate);
			plist["DisableTimestamp"] = new PBoolean (DisableTimestamp);
			plist["Entitlements"] = new PString (!string.IsNullOrEmpty (Entitlements) ? Path.GetFullPath (Entitlements) : string.Empty);
			plist["ExtraArgs"] = new PString (ExtraArgs ?? string.Empty);
			plist["Keychain"] = new PString (Keychain ?? string.Empty);
			plist["ResourceRules"] = new PString (ResourceRules ?? string.Empty);
			plist["SigningKey"] = new PString (SigningKey);

			try {
				plist.Save (SavedState, true);
			} catch (Exception ex) {
				Log.LogError ("Could not save '{0}': {1}", SavedState, ex.Message);
				return false;
			}

			return !Log.HasLoggedErrors;
		}
	}
}
