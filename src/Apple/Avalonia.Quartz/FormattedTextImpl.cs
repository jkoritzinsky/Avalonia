using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using Avalonia.Platform;
using CoreText;
using Foundation;

namespace Avalonia.Quartz
{
	class FormattedTextImpl : IFormattedTextImpl
	{
		private NSMutableAttributedString text;

		public FormattedTextImpl(string rawText, string fontFamilyName, double fontSize, FontStyle fontStyle, TextAlignment textAlignment, FontWeight fontWeight, TextWrapping wrapping)
		{
			CTTextAlignment alignment = textAlignment.ToNative();

			var paragraphStyle = new CTParagraphStyle(new CTParagraphStyleSettings{ Alignment = alignment, LineBreakMode = wrapping.ToNative() });
			var descriptorAttributes = new Dictionary<NSString, object>
			{
				{CTFontDescriptorAttributeKey.FamilyName, fontFamilyName},
				{CTFontDescriptorAttributeKey.StyleName, fontStyle.ToString()},
				{CTFontDescriptorAttributeKey.Traits, new NSDictionary(CTFontTraitKey.Weight, fontWeight.ToNative())},

			};
			var descriptor = new CTFontDescriptor(new CTFontDescriptorAttributes(NSDictionary.FromObjectsAndKeys(descriptorAttributes.Values.ToArray(), descriptorAttributes.Keys.ToArray())));

			var font = new CTFont(descriptor, (nfloat)fontSize);

			var stringAttributes = new Dictionary<NSString, object>
			{
				{CTStringAttributeKey.ParagraphStyle, paragraphStyle},
				{CTStringAttributeKey.Font, font}
			};

			text = new NSMutableAttributedString(rawText, new CTStringAttributes(NSDictionary.FromObjectsAndKeys(stringAttributes.Values.ToArray(), stringAttributes.Keys.ToArray())));
			Framesetter = new CTFramesetter(text);
		}

		public CTFramesetter Framesetter { get; set; }

		public Size Constraint { get; set; }

		public void Dispose()
		{
			text.Dispose();
			Framesetter.Dispose();
		}

		public IEnumerable<FormattedTextLine> GetLines()
		{
			throw new NotImplementedException();
		}

		public TextHitTestResult HitTestPoint(Point point)
		{
			throw new NotImplementedException();
		}

		public Rect HitTestTextPosition(int index)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Rect> HitTestTextRange(int index, int length)
		{
			throw new NotImplementedException();
		}

		public Size Measure()
		{
			NSRange includedTextRange;
			return Framesetter.SuggestFrameSize(new NSRange(0, text.Length), new CTFrameAttributes(), Constraint.ToNative(), out includedTextRange).ToAvalonia();
		}

		public void SetForegroundBrush(IBrush brush, int startIndex, int length)
		{
			var solidColor = brush as ISolidColorBrush;
			if (solidColor != null)
			{
				text.AddAttribute(CTStringAttributeKey.ForegroundColor, NSObject.FromObject(solidColor.Color.ToNative()), new NSRange(0, text.Length));
			}
		}
	}
}
