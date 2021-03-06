//
// Unit tests for CIImage
//
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright 2012 Xamarin Inc. All rights reserved.
//

#if !__WATCHOS__

using System;
using System.IO;

#if XAMCORE_2_0
using Foundation;
using UIKit;
using CoreImage;
using CoreGraphics;
using ObjCRuntime;
#else
using MonoTouch.CoreImage;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;
#endif
using NUnit.Framework;

#if XAMCORE_2_0
using RectangleF=CoreGraphics.CGRect;
using SizeF=CoreGraphics.CGSize;
using PointF=CoreGraphics.CGPoint;
#else
using nfloat=global::System.Single;
using nint=global::System.Int32;
using nuint=global::System.UInt32;
#endif

namespace MonoTouchFixtures.CoreImage {
	
	[TestFixture]
	[Preserve (AllMembers = true)]
	public class ImageTest {
		
		[Test]
		public void EmptyImage ()
		{
			Assert.IsNull (CIImage.EmptyImage.Properties);
		}

		[Test]
		public void InitializationWithCustomMetadata ()
		{
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");
			using (var dp = new CGDataProvider (file)) {
				using (var img = CGImage.FromPNG (dp, null, false, CGColorRenderingIntent.Default)) {

					var opt = new CIImageInitializationOptionsWithMetadata () {
						Properties = new CGImageProperties () {
							ProfileName = "test profile name"
						}
					};

					using (var ci = new CIImage (img, opt)) {
						Assert.AreEqual ("test profile name", ci.Properties.ProfileName);
					}
				}
			}
		}

		[Test]
		public void UIImageInterop ()
		{
			// to validate http://stackoverflow.com/q/14464154/220643
			string file = Path.Combine (NSBundle.MainBundle.ResourcePath, "basn3p08.png");
			using (var url = NSUrl.FromFilename (file))
			using (var ci = CIImage.FromUrl (url))
			using (var ui = new UIImage (ci, 1.0f, UIImageOrientation.Up)) {
				Assert.IsNotNull (ui.CIImage, "CIImage");
			}
		}

		[Test]
		public void AreaHistogram ()
		{
			if (!TestRuntime.CheckSystemAndSDKVersion (8, 0))
				Assert.Inconclusive ("requires iOS8+");

			// validate that a null NSDictionary is correct (i.e. uses filter defaults)
			using (var h = CIImage.EmptyImage.CreateByFiltering ("CIAreaHistogram", null)) {
				// broken on simulator/64 bits on iOS9 beta 2 - radar 21564256 -> fixed in beta 4
				Assert.That (h.Extent.Height, Is.EqualTo ((nfloat) 1), "Height");
			}
		}

		[Test]
		public void CIImageColorSpaceTest ()
		{
			if (!TestRuntime.CheckXcodeVersion (7, 0))
				Assert.Inconclusive ("requires iOS9+");

			using (var cgimage = new CIImage (NSUrl.FromFilename ("xamarin1.png")))
			using (var cs = cgimage.ColorSpace) {
				Assert.NotNull (cs, "ColorSpace should not be null");
				if (TestRuntime.CheckXcodeVersion (8, 0)) {
					Assert.That (cs.Name, Is.EqualTo ("kCGColorSpaceSRGB"));
				} else {
					Assert.IsNull (cs.Name);
				}
			}
		}
	}
}

#endif // !__WATCHOS__
